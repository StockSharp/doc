# Perfil de Volume

O script "Volume Profile" serve como uma ferramenta para analisar a distribuição do volume de negociação por níveis de preço ao longo de um período selecionado. Permite que traders e analistas quantitativos visualizem e examinem onde a principal atividade de negociação esteve concentrada em termos de níveis de preço.

![hydra_analytics_volume_profile](../../../../images/hydra_analytics_volume_profile.png)

## Descrição da Funcionalidade

O script agrega dados de transações para formar um perfil que apresenta os volumes executados em vários níveis de preço. Esta informação pode ser representada num gráfico, ilustrando a densidade de negócios em diferentes pontos de preço.

## Importância Prática

A análise do perfil de volume ajuda a identificar zonas chave de procura e oferta e pode ser usada para:

- Identificar níveis de suporte e resistência onde o instrumento encontra interesse significativo por parte dos participantes do mercado.
- Avaliar a força da tendência atual ou o seu potencial enfraquecimento, com base na alteração da distribuição de volume.
- Planear pontos de entrada e saída do mercado, tendo em conta níveis com máxima liquidez acumulada.

## Aplicação em Negociação e Análise Quantitativa

- **Negociação**: O perfil de volume pode ser utilizado para desenvolver estratégias baseadas em análise de volume, fornecendo uma visão clara de onde ocorrem as principais operações de negociação.
- **Análise Quantitativa**: Dados sobre a distribuição de volume podem servir como entrada para modelos quantitativos que preveem a probabilidade de movimentos de preço com base no volume acumulado num nível.

## Implementação do Script

O script "Volume Profile" executa os seguintes passos:

1. **Recolha de Dados**: O script agrega dados de transações para o período especificado.
2. **Formação do Perfil**: Com base nos dados recolhidos, o script forma um perfil de volume que reflete a atividade de negociação em cada nível de preço.
3. **Visualização**: Os resultados do funcionamento do script são visualizados como gráfico ou histograma, onde cada barra corresponde a um nível de preço específico e ao seu volume de negociação.

A utilização do script "Volume Profile" na plataforma StockSharp permite uma análise abrangente do mercado, a construção de hipóteses de negociação fundamentadas e a melhoria da qualidade das decisões de negociação tomadas.

## Código do Script em C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// O script analítico, calcula a distribuição do volume por níveis de preço.
	/// </summary>
	public class PriceVolumeScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			// o script pode processar apenas 1 instrumento
			var security = securities.First();

			// obter o armazenamento de candles
			var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

			// obter datas disponíveis para o período especificado
			var dates = candleStorage.GetDates(from, to).ToArray();

			if (dates.Length == 0)
			{
				logs.LogWarning("no data");
				return Task.CompletedTask;
			}

			// agrupar candles pelo preço médio
			var rows = candleStorage.Load(from, to)
				.GroupBy(c => c.LowPrice + c.GetLength() / 2)
				.ToDictionary(g => g.Key, g => g.Sum(c => c.TotalVolume));

			// desenhar no gráfico
			panel.CreateChart<decimal, decimal>()
				.Append(security.ToStringId(), rows.Keys, rows.Values, DrawStyles.Histogram);

			return Task.CompletedTask;
		}
	}
}

```

## Código do Script em Python

```python
import clr

# Adicionar referências .NET
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# O script analítico, calcula a distribuição do volume por níveis de preço.
class price_volume_script(IAnalyticsScript):
	def Run(
		self,
		logs,
		panel,
		securities,
		from_date,
		to_date,
		storage,
		drive,
		format,
		data_type,
		cancellation_token
	):
		# Verificar se não existem instrumentos
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		# O script pode processar apenas 1 instrumento
		security = securities[0]

		if data_type is None:
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		# Obter o armazenamento de candles
		candle_storage = get_candle_storage(storage, security, data_type, drive, format)

		# Obter datas disponíveis para o período especificado
		dates = get_dates(candle_storage, from_date, to_date)

		if len(dates) == 0:
			logs.LogWarning("no data")
			return Task.CompletedTask

		# Agrupar candles pelo preço médio e somar os seus volumes
		candles = load_range(candle_storage, message_type, from_date, to_date)
		rows_dict = {}
		for candle in candles:
			# Calcular preço médio da candle
			key = candle.LowPrice + get_length(candle) / 2
			# Somar volumes para o mesmo nível de preço
			rows_dict[key] = rows_dict.get(key, 0) + candle.TotalVolume

		# Desenhar no gráfico
		chart = create_chart(panel, float, float)
		chart.Append(to_string_id(security), list(rows_dict.keys()), list(rows_dict.values()), DrawStyles.Histogram)

		return Task.CompletedTask

```

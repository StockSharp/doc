# Normalização do Preço de Fecho

O script "Closing Price Normalization" foi concebido para normalizar os preços de fecho de instrumentos financeiros, permitindo a comparação e análise de diferentes ativos numa escala unificada. Isto é particularmente útil ao comparar instrumentos com custos e volatilidade variados.

![Normalização do Preço de Fecho](../../../../images/hydra_analytics_normalize.png)

## Descrição do Funcionamento do Script

O script adapta os dados de preços de fecho escalando-os ou transformando-os de acordo com o método de normalização selecionado. O resultado é um conjunto de valores normalizados que pode ser usado para comparação quantitativa e análise multi-instrumento.

## Aplicação da Normalização

- **Unificação da Escala de Preços**: A normalização ajuda a colocar dados de vários instrumentos numa única escala, simplificando a comparação visual e a avaliação analítica.
- **Análise de Correlação**: Dados normalizados permitem identificar relações de correlação entre ativos e formar carteiras diversificadas.
- **Criação de Índices e Modelos**: Preços normalizados são usados para criar índices compostos, modelos de precificação e outros estudos quantitativos.

## Metodologia de Normalização

A normalização pode incluir os seguintes métodos:

1. **Escalonamento**: Ajustar preços de fecho a um determinado intervalo de valores, como de 0 a 1.
2. **Z-score**: Transformar preços de fecho usando o Z-score, que indica quantos desvios-padrão um valor se afasta da média.
3. **Logaritmização**: Aplicar transformação logarítmica para suavizar a dispersão dos dados e reduzir o impacto de valores extremos.

## Implementação do Script

O processo de normalização inclui normalmente os seguintes passos:

1. **Seleção da Normalização**: Determinar o método de normalização com base nos objetivos da análise e nas características dos dados.
2. **Processamento de Dados**: Aplicar o método de normalização escolhido aos preços de fecho de cada instrumento.
3. **Análise dos Resultados**: Usar dados normalizados para posterior análise e comparação de instrumentos.

O script "Closing Price Normalization" é uma ferramenta crucial para preparar dados para negociação e análise quantitativa, permitindo que traders e analistas comparem e avaliem ativos financeiros com maior precisão em várias estratégias e estudos.

## Código do Script em C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// O script analítico, normaliza os preços de fecho das securities e mostra-os no mesmo gráfico.
	/// </summary>
	public class NormalizePriceScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var chart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// parar o cálculo se o utilizador cancelar a execução do script
				if (cancellationToken.IsCancellationRequested)
					break;

				var series = new Dictionary<DateTimeOffset, decimal>();

				// obter o armazenamento de candles
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				decimal? firstClose = null;

				foreach (var candle in candleStorage.Load(from, to))
				{
					firstClose ??= candle.ClosePrice;

					// normalizar preços de fecho dividindo pelo primeiro fecho
					series[candle.OpenTime] = candle.ClosePrice / firstClose.Value;
				}

				// desenhar séries no gráfico
				chart.Append(security.ToStringId(), series.Keys, series.Values);
			}

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

# O script analítico, normaliza os preços de fecho das securities e mostra-os no mesmo gráfico.
class normalize_price_script(IAnalyticsScript):
	def Run(self, logs, panel, securities, from_date, to_date, storage, drive, format, data_type, cancellation_token):
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		chart = create_chart(panel, datetime, float)

		if data_type is None:
			logs.LogWarning(f"Tipo de dados não suportado {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# parar o cálculo se o utilizador cancelar a execução do script
			if cancellation_token.IsCancellationRequested:
				break

			series = {}

			# obter o armazenamento de candles
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			first_close = None

			for candle in load_range(candle_storage, message_type, from_date, to_date):
				if first_close is None:
					first_close = candle.ClosePrice

				# normalizar preços de fecho dividindo pelo primeiro fecho
				series[candle.OpenTime] = candle.ClosePrice / first_close

			# desenhar séries no gráfico
			chart.Append(to_string_id(security), list(series.keys()), list(series.values()))

		return Task.CompletedTask

```

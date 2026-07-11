# Correlação de Pearson

A correlação de Pearson é um método estatístico usado para medir o grau de relação linear entre duas variáveis quantitativas. Na análise financeira, este método é amplamente utilizado para explorar as relações entre diferentes ativos, como ações ou pares de moedas.

![Correlação de Pearson](../../../../images/hydra_analytics_pearson_correlation.png)

## Descrição do Método

O coeficiente de correlação de Pearson varia de -1 a 1, onde:

- **1** indica uma correlação positiva perfeita,
- **0** sugere ausência de relação linear,
- **-1** significa uma correlação negativa perfeita.

O valor do coeficiente mostra quão estreitamente duas variáveis estão linearmente relacionadas.

## Aplicação Prática

- **Gestão de Carteiras**: A avaliação das correlações entre ativos ajuda a construir carteiras diversificadas para minimizar o risco.
- **Estratégias de Cobertura**: A identificação de ativos com elevada correlação negativa pode ser usada para desenvolver estratégias de cobertura.
- **Análise de Tendências de Mercado**: O estudo das correlações entre diferentes mercados e instrumentos fornece informações sobre interligações económicas globais.

## Cálculo da Correlação de Pearson

O coeficiente de correlação de Pearson é calculado usando a seguinte fórmula:

\[ r = \frac{n(\sum xy) - (\sum x)(\sum y)}{\sqrt{[n\sum x^2 - (\sum x)^2][n\sum y^2 - (\sum y)^2]}} \]

onde:
- \(n\) é o número de observações,
- \(x\) e \(y\) são os valores das variáveis,
- \(\sum\) denota somatório.

## Implementação do Script

Um script para calcular a correlação de Pearson deve incluir os seguintes passos:

1. **Recolha de Dados**: Carregar séries temporais para as duas variáveis em análise.
2. **Processamento Preliminar**: Alinhar séries temporais por datas e remover valores em falta.
3. **Cálculo**: Aplicar a fórmula da correlação de Pearson aos dados processados.
4. **Análise dos Resultados**: Interpretar o coeficiente de correlação resultante para a tomada de decisões.

O cálculo da correlação de Pearson fornece informação crucial para análise de mercado e otimização de estratégias de investimento, permitindo avaliar o grau de influência mútua entre ativos financeiros.

## Código do Script em C#

```cs
namespace StockSharp.Algo.Analytics
{
	using MathNet.Numerics.Statistics;

	/// <summary>
	/// O script analítico, calcula a correlação de Pearson por securities especificadas.
	/// </summary>
	public class PearsonCorrelationScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var closes = new List<double[]>();

			foreach (var security in securities)
			{
				// parar o cálculo se o utilizador cancelar a execução do script
				if (cancellationToken.IsCancellationRequested)
					break;

				// obter o armazenamento de candles
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// obter preços de fecho
				var prices = candleStorage.Load(from, to).Select(c => (double)c.ClosePrice).ToArray();

				if (prices.Length == 0)
				{
					logs.LogWarning("Sem dados para {0}", security);
					return Task.CompletedTask;
				}

				closes.Add(prices);
			}

			// todas as matrizes devem ter o mesmo comprimento, por isso truncar as mais longas
			var min = closes.Select(arr => arr.Length).Min();

			for (var i = 0; i < closes.Count; i++)
			{
				var arr = closes[i];

				if (arr.Length > min)
					closes[i] = arr.Take(min).ToArray();
			}

			// calcular correlação
			var matrix = Correlation.PearsonMatrix(closes);

			// apresentar o resultado num heatmap
			var ids = securities.Select(s => s.ToStringId());
			panel.DrawHeatmap(ids, ids, matrix.ToArray());

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
from numpy_extensions import nx

clr.AddReference("NumpyDotNet")
from NumpyDotNet import np

# O script analítico, calcula a correlação de Pearson por securities especificadas.
class pearson_correlation_script(IAnalyticsScript):
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
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		closes = []

		if data_type is None:
			logs.LogWarning(f"Tipo de dados não suportado {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# parar o cálculo se o utilizador cancelar a execução do script
			if cancellation_token.IsCancellationRequested:
				break

			# obter o armazenamento de candles
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# obter preços de fecho
			prices = [float(c.ClosePrice) for c in load_range(candle_storage, message_type, from_date, to_date)]

			if len(prices) == 0:
				logs.LogWarning("Sem dados para {0}", security)
				return Task.CompletedTask

			closes.append(prices)

		# todas as matrizes devem ter o mesmo comprimento, por isso truncar as mais longas
		min_length = min(len(arr) for arr in closes)
		closes = [arr[:min_length] for arr in closes]

		# converter lista ou matriz numa matriz 2D
		array2d = nx.to2darray(closes)

		# calcular correlação usando NumSharp
		np_array = np.array(array2d)
		matrix = np.corrcoef(np_array)

		# apresentar o resultado num heatmap
		ids = [to_string_id(s) for s in securities]
		panel.DrawHeatmap(ids, ids, nx.tosystemarray(matrix))

		return Task.CompletedTask

```

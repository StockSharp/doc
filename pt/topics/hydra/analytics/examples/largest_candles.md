# Maiores velas

O script "Maiores velas" foi concebido para identificar velas com o volume máximo e o maior comprimento de corpo nos gráficos dos instrumentos financeiros selecionados ao longo de um determinado período. Esta ferramenta permite que traders e analistas identifiquem eventos significativos do mercado e a reação dos participantes do mercado.

![hydra_analytics_big_candle](../../../../images/hydra_analytics_big_candle.png)

## Funcionalidades Principais

O script analisa um conjunto de instrumentos especificados, procura entre eles candles com o maior volume e comprimento de corpo, e apresenta estes dados em dois gráficos:

- **Gráfico do Comprimento do Corpo da Candle**: Mostra candles com a maior diferença entre o preço de abertura e o preço de fecho.
- **Gráfico do Volume de Negociação**: Demonstra candles com o volume máximo de negociação durante o tempo de existência da candle.

## Fluxo de Trabalho

1. **Seleção de Instrumentos e Período de Análise**: Determina a lista de instrumentos e o intervalo temporal para análise.
2. **Análise de Dados**: Envolve o carregamento e a análise de dados históricos de candles para identificar candles com os maiores indicadores.
3. **Visualização dos Resultados**: As candles encontradas são apresentadas em gráficos na interface do painel de analytics.

## Aplicação

- **Análise da Atividade do Mercado**: Ajuda a determinar momentos de maior atividade dos traders e potenciais inversões do mercado.
- **Identificação de níveis-chave**: velas com volume e comprimento de corpo significativos formam-se frequentemente em torno de níveis-chave de suporte e resistência.
- **Planeamento Estratégico**: A informação sobre as maiores candles pode ser usada para planear pontos de entrada e saída do mercado, tendo em conta a volatilidade potencial.

## Código do Script em C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// O script analítico, mostra a maior candle (por volume e por comprimento) para securities especificadas.
	/// </summary>
	public class BiggestCandleScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var priceChart = panel.CreateChart<DateTimeOffset, decimal, decimal>();
			var volChart = panel.CreateChart<DateTimeOffset, decimal, decimal>();

			var bigPriceCandles = new List<CandleMessage>();
			var bigVolCandles = new List<CandleMessage>();

			foreach (var security in securities)
			{
				// parar o cálculo se o utilizador cancelar a execução do script
				if (cancellationToken.IsCancellationRequested)
					break;

				// obter o armazenamento de candles
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				var allCandles = candleStorage.Load(from, to).ToArray();

				// a primeira ordenação por volume descendente será a nossa maior candle
				var bigPriceCandle = allCandles.OrderByDescending(c => c.GetLength()).FirstOrDefault();
				var bigVolCandle = allCandles.OrderByDescending(c => c.TotalVolume).FirstOrDefault();

				if (bigPriceCandle != null)
					bigPriceCandles.Add(bigPriceCandle);

				if (bigVolCandle != null)
					bigVolCandles.Add(bigVolCandle);
			}

			// desenhar séries no gráfico
			priceChart.Append("prices", bigPriceCandles.Select(c => c.OpenTime), bigPriceCandles.Select(c => c.GetMiddlePrice(null)), bigPriceCandles.Select(c => c.GetLength()));
			volChart.Append("prices", bigVolCandles.Select(c => c.OpenTime), bigPriceCandles.Select(c => c.GetMiddlePrice(null)), bigVolCandles.Select(c => c.TotalVolume));

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

# O script analítico, mostra a maior candle (por volume e por comprimento) para securities especificadas.
class biggest_candle_script(IAnalyticsScript):
	def Run(self, logs, panel, securities, from_date, to_date, storage, drive, format, data_type, cancellation_token):
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		price_chart = create_3d_chart(panel, datetime, float, float)
		vol_chart = create_3d_chart(panel, datetime, float, float)

		big_price_candles = []
		big_vol_candles = []

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
			all_candles = load_range(candle_storage, message_type, from_date, to_date)

			if len(all_candles) > 0:
				# a primeira ordenação por volume descendente será a nossa maior candle
				big_price_candle = max(all_candles, key=lambda c: get_length(c))
				big_vol_candle = max(all_candles, key=lambda c: c.TotalVolume)

				if big_price_candle is not None:
					big_price_candles.append(big_price_candle)

				if big_vol_candle is not None:
					big_vol_candles.append(big_vol_candle)

		# desenhar séries no gráfico
		price_chart.Append(
			"prices",
			[c.OpenTime for c in big_price_candles],
			[get_middle_price(c) for c in big_price_candles],
			[get_length(c) for c in big_price_candles]
		)

		vol_chart.Append(
			"prices",
			[c.OpenTime for c in big_vol_candles],
			[get_middle_price(c) for c in big_price_candles],
			[c.TotalVolume for c in big_vol_candles]
		)

		return Task.CompletedTask

```

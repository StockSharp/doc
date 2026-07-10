# Grosste Kerzen

Das Skript "Largest Candles" dient dazu, Kerzen mit maximalem Volumen und der größten Körperlänge in den Charts ausgewahlter Finanzinstrumente über einen bestimmten Zeitraum zu identifizieren. Dieses Werkzeug hilft Tradern und Analysten, wichtige Marktereignisse und die Reaktion der Marktteilnehmer zu erkennen.

![hydra_analytics_big_candle](../../../../images/hydra_analytics_big_candle.png)

## Hauptfunktionen

Das Skript analysiert eine Gruppe angegebener Instrumente, sucht darin nach Kerzen mit dem größten Volumen und der größten Körperlänge und zeigt diese Daten in zwei Diagrammen an:

- **Chart der Kerzenkorperlange**: Zeigt Kerzen mit der größten Differenz zwischen Eroffnungs- und Schlusskurs.
- **Chart des Handelsvolumens**: Zeigt Kerzen mit dem maximalen Handelsvolumen für die Existenzdauer der Kerze.

## Ablauf

1. **Auswahl von Instrumenten und Analysezeitraum**: Legt die Liste der Instrumente und das Zeitintervall für die Analyse fest.
2. **Datenanalyse**: Umfasst das Laden und Analysieren historischer Kerzendaten, um Kerzen mit den größten Kennzahlen zu identifizieren.
3. **Visualisierung der Ergebnisse**: Gefundene Kerzen werden in Charts in der Oberfläche des Analysepanels angezeigt.

## Anwendung

- **Analyse der Marktaktivitat**: Hilft, Momente der größten Traderaktivitat und potenzielle Marktumkehrungen zu bestimmen.
- **Identifikation wichtiger Niveaus**: Kerzen mit signifikantem Volumen und größer Körperlänge entstehen häufig an wichtigen Unterstutzungs- und Widerstandsniveaus.
- **Strategische Planung**: Informationen über die größten Kerzen können für die Planung von Marktein- und -ausstiegen unter Berucksichtigung potenzieller Volatilität verwendet werden.

## Skriptcode in C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Das Analyseskript zeigt die größte Kerze (nach Volumen und nach Länge) für angegebene Instrumente.
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
				// Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
				if (cancellationToken.IsCancellationRequested)
					break;

				// Kerzenspeicher abrufen
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				var allCandles = candleStorage.Load(from, to).ToArray();

				// Die zuerst nach Volumen absteigend sortierte Kerze ist unsere größte Kerze
				var bigPriceCandle = allCandles.OrderByDescending(c => c.GetLength()).FirstOrDefault();
				var bigVolCandle = allCandles.OrderByDescending(c => c.TotalVolume).FirstOrDefault();

				if (bigPriceCandle != null)
					bigPriceCandles.Add(bigPriceCandle);

				if (bigVolCandle != null)
					bigVolCandles.Add(bigVolCandle);
			}

			// Reihen im Chart zeichnen
			priceChart.Append("prices", bigPriceCandles.Select(c => c.OpenTime), bigPriceCandles.Select(c => c.GetMiddlePrice(null)), bigPriceCandles.Select(c => c.GetLength()));
			volChart.Append("prices", bigVolCandles.Select(c => c.OpenTime), bigPriceCandles.Select(c => c.GetMiddlePrice(null)), bigVolCandles.Select(c => c.TotalVolume));

			return Task.CompletedTask;
		}
	}
}

```

## Skriptcode in Python

```python
import clr

# .NET-Referenzen hinzufugen
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

# Das Analyseskript zeigt die größte Kerze (nach Volumen und nach Länge) für angegebene Instrumente.
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
			logs.LogWarning(f"Nicht unterstützter Datentyp {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
			if cancellation_token.IsCancellationRequested:
				break

			# Kerzenspeicher abrufen
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)
			all_candles = load_range(candle_storage, message_type, from_date, to_date)

			if len(all_candles) > 0:
				# Die zuerst nach Volumen absteigend sortierte Kerze ist unsere größte Kerze
				big_price_candle = max(all_candles, key=lambda c: get_length(c))
				big_vol_candle = max(all_candles, key=lambda c: c.TotalVolume)

				if big_price_candle is not None:
					big_price_candles.append(big_price_candle)

				if big_vol_candle is not None:
					big_vol_candles.append(big_vol_candle)

		# Reihen im Chart zeichnen
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


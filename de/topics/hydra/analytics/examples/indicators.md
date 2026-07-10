# Indikatoren

Das Skript "Indicator" demonstriert die Arbeit mit technischen Analyseindikatoren innerhalb der StockSharp-Plattform. Es ermöglicht Benutzern, historische Daten zu laden, verschiedene Indikatoren darauf anzuwenden und die Ergebnisse in einem Chart anzuzeigen. Dieser Ansatz unterstützt die Analyse von Markttrends und fundierte Handelsentscheidungen.

![hydra_analytics_indicator](../../../../images/hydra_analytics_indicator.png)

## Funktionen

Das Skript bietet die folgende Funktionalitat:

- **Laden historischer Daten**: Auswahl interessanter Finanzinstrumente und Laden ihrer historischen Daten für einen angegebenen Zeitraum.
- **Anwenden von Indikatoren**: Anwendung eines oder mehrerer technischer Analyseindikatoren auf die geladenen Daten.
- **Visualisierung**: Anzeige von Daten und Analyseergebnissen mithilfe von Indikatoren in einem Chart, um einen klaren Blick auf die Marktdynamik zu bieten.

## Beispiele für Indikatoren

Das Skript kann mit einer großen Auswahl von Indikatoren arbeiten, darunter unter anderem:

- **Gleitende Durchschnitte (MA)**: Stellen den Durchschnittspreis über einen bestimmten Zeitraum dar und helfen, Trends zu identifizieren.
- **Relative Strength Index (RSI)**: Bewertet Ausmass und Geschwindigkeit von Preisanderungen und hilft, uberkaufte oder uberverkaufte Bedingungen zu erkennen.
- **Bollinger Bands (BB)**: Zeigen Preisspanne und Volatilität auf Basis gleitender Durchschnitte und Standardabweichungen.

## Anwendung im Handel und in der Analyse

Die Verwendung technischer Analyseindikatoren über dieses Skript ermöglicht:

- **Trenderkennung**: Erkennen der Bewegungsrichtung des Marktes zur Planung von Einstiegs- und Ausstiegsstrategien.
- **Erkennung von Umkehrpunkten**: Bestimmen von Momenten, in denen der Markttrend seine Richtung ändern kann.
- **Volatilitätsanalyse**: Bewertung der Preisinstabilität, um Strategien an Marktbedingungen anzupassen.

## Implementierung im Skript

Für die Arbeit mit dem Skript sind die folgenden Schritte erforderlich:

1. **Auswahl eines Instruments und Zeitraums**: Festlegen der Wertpapiere und des Zeitrahmens für die Analyse.
2. **Anwenden von Indikatoren**: Auswahl und Parametrierung der Indikatoren, die auf die Daten angewendet werden sollen.
3. **Anzeige der Ergebnisse**: Visualisierung historischer Daten und Indikatoren in einem Chart zur Analyse.

Das Skript "Indicator" stellt ein leistungsfahiges Werkzeug für die detaillierte Analyse von Finanzmarkten bereit und ermöglicht Tradern und Analysten, diese Indikatoren zur Entwicklung wirksamer Handelsstrategien zu nutzen.

## Skriptcode in C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Das Analyseskript verwendet den Indikator ROC.
	/// </summary>
	public class IndicatorScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			// 2 Bereiche für Kerzen und Indikatorreihen erstellen
			var candleChart = panel.CreateChart<DateTimeOffset, decimal>();
			var indicatorChart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
				if (cancellationToken.IsCancellationRequested)
					break;

				var candlesSeries = new Dictionary<DateTimeOffset, decimal>();
				var indicatorSeries = new Dictionary<DateTimeOffset, decimal>();

				// ROC erstellen
				var roc = new RateOfChange();

				// Kerzenspeicher abrufen
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				foreach (var candle in candleStorage.Load(from, to))
				{
					// Reihen befullen
					candlesSeries[candle.OpenTime] = candle.ClosePrice;
					indicatorSeries[candle.OpenTime] = roc.Process(candle).ToDecimal();
				}

				// Reihen im Chart zeichnen
				candleChart.Append($"{security} (close)", candlesSeries.Keys, candlesSeries.Values);
				indicatorChart.Append($"{security} (ROC)", indicatorSeries.Keys, indicatorSeries.Values);
			}

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
from StockSharp.Algo.Indicators import ROC
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# Das Analyseskript verwendet den Indikator ROC.
class indicator_script(IAnalyticsScript):
	def Run(self, logs, panel, securities, from_date, to_date, storage, drive, format, data_type, cancellation_token):
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		# 2 Bereiche für Kerzen und Indikatorreihen erstellen
		candle_chart = create_chart(panel, datetime, float)
		indicator_chart = create_chart(panel, datetime, float)

		if data_type is None:
			logs.LogWarning(f"Nicht unterstützter Datentyp {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
			if cancellation_token.IsCancellationRequested:
				break

			candles_series = {}
			indicator_series = {}

			# ROC erstellen
			roc = ROC()

			# Kerzenspeicher abrufen
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			for candle in load_range(candle_storage, message_type, from_date, to_date):
				# Reihen befullen
				candles_series[candle.OpenTime] = candle.ClosePrice
				indicator_series[candle.OpenTime] = to_decimal(process_candle(roc, candle))

			# Reihen im Chart zeichnen
			candle_chart.Append(
				f"{security} (close)",
				list(candles_series.keys()),
				list(candles_series.values())
			)
			indicator_chart.Append(
				f"{security} (ROC)",
				list(indicator_series.keys()),
				list(indicator_series.values())
			)

		return Task.CompletedTask

```


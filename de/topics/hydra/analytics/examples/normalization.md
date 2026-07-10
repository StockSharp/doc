# Normalisierung des Schlusskurses

Das Skript "Closing Price Normalization" dient zur Standardisierung der Schlusskurse von Finanzinstrumenten und ermöglicht den Vergleich und die Analyse unterschiedlicher Assets auf einer einheitlichen Skala. Dies ist besonders nutzlich beim Vergleich von Instrumenten mit unterschiedlichen Preisen und unterschiedlicher Volatilität.

![hydra_analytics_normalize](../../../../images/hydra_analytics_normalize.png)

## Beschreibung der Skriptausführung

Das Skript passt Schlusskursdaten an, indem es sie entsprechend der gewahlten Normalisierungsmethode skaliert oder transformiert. Das Ergebnis ist ein Satz standardisierter Werte, der für quantitative Vergleiche und Multi-Instrument-Analysen verwendet werden kann.

## Anwendung der Normalisierung

- **Vereinheitlichung der Preisskala**: Die Normalisierung bringt Daten verschiedener Instrumente auf eine gemeinsame Skala und erleichtert dadurch den visuellen Vergleich und die analytische Bewertung.
- **Korrelationsanalyse**: Standardisierte Daten ermöglichen das Erkennen korrelativer Beziehungen zwischen Assets und die Bildung diversifizierter Portfolios.
- **Index- und Modellerstellung**: Normalisierte Preise werden zur Erstellung zusammengesetzter Indizes, Preisbildungsmodelle und anderer quantitativer Untersuchungen verwendet.

## Normalisierungsmethodik

Die Normalisierung kann folgende Methoden umfassen:

1. **Skalierung**: Anpassung der Schlusskurse an einen bestimmten Wertebereich, zum Beispiel von 0 bis 1.
2. **Z-Score**: Transformation der Schlusskurse mit dem Z-Score, der angibt, wie viele Standardabweichungen ein Wert vom Mittelwert entfernt ist.
3. **Logarithmierung**: Anwendung einer logarithmischen Transformation, um die Datenstreuung zu glatten und den Einfluss extremer Werte zu reduzieren.

## Skriptimplementierung

Der Normalisierungsprozess umfasst typischerweise die folgenden Schritte:

1. **Auswahl der Normalisierung**: Festlegen der Normalisierungsmethode anhand der Analyseziele und Dateneigenschaften.
2. **Datenverarbeitung**: Anwenden der gewahlten Normalisierungsmethode auf die Schlusskurse jedes Instruments.
3. **Analyse der Ergebnisse**: Verwenden normalisierter Daten für die anschliessende Analyse und den Vergleich von Instrumenten.

Das Skript "Closing Price Normalization" ist ein wichtiges Werkzeug zur Vorbereitung von Daten für Handel und quantitative Analyse. Es ermöglicht Tradern und Analysten, Finanzassets innerhalb verschiedener Strategien und Studien genauer zu vergleichen und zu bewerten.

## Skriptcode in C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Das Analyseskript normalisiert Schlusskurse von Instrumenten und zeigt sie im selben Chart an.
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
				// Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
				if (cancellationToken.IsCancellationRequested)
					break;

				var series = new Dictionary<DateTimeOffset, decimal>();

				// Kerzenspeicher abrufen
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				decimal? firstClose = null;

				foreach (var candle in candleStorage.Load(from, to))
				{
					firstClose ??= candle.ClosePrice;

					// Schlusskurse durch Division durch den ersten Schlusskurs normalisieren
					series[candle.OpenTime] = candle.ClosePrice / firstClose.Value;
				}

				// Reihen im Chart zeichnen
				chart.Append(security.ToStringId(), series.Keys, series.Values);
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
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# Das Analyseskript normalisiert Schlusskurse von Instrumenten und zeigt sie im selben Chart an.
class normalize_price_script(IAnalyticsScript):
	def Run(self, logs, panel, securities, from_date, to_date, storage, drive, format, data_type, cancellation_token):
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		chart = create_chart(panel, datetime, float)

		if data_type is None:
			logs.LogWarning(f"Nicht unterstützter Datentyp {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
			if cancellation_token.IsCancellationRequested:
				break

			series = {}

			# Kerzenspeicher abrufen
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			first_close = None

			for candle in load_range(candle_storage, message_type, from_date, to_date):
				if first_close is None:
					first_close = candle.ClosePrice

				# Schlusskurse durch Division durch den ersten Schlusskurs normalisieren
				series[candle.OpenTime] = candle.ClosePrice / first_close

			# Reihen im Chart zeichnen
			chart.Append(to_string_id(security), list(series.keys()), list(series.values()))

		return Task.CompletedTask

```


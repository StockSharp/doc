# Pearson-Korrelation

Die Pearson-Korrelation ist eine statistische Methode zur Messung des Grades einer linearen Beziehung zwischen zwei quantitativen Variablen. In der Finanzanalyse wird diese Methode häufig verwendet, um Beziehungen zwischen verschiedenen Assets wie Aktien oder Wahrungspaaren zu untersuchen.

![hydra_analytics_pearson_correlation](../../../../images/hydra_analytics_pearson_correlation.png)

## Methodenbeschreibung

Der Pearson-Korrelationskoeffizient liegt zwischen -1 und 1, wobei:

- **1** eine perfekte positive Korrelation anzeigt,
- **0** auf keine lineare Beziehung hinweist,
- **-1** eine perfekte negative Korrelation bedeutet.

Der Wert des Koeffizienten zeigt, wie eng zwei Variablen linear miteinander verbunden sind.

## Praktische Anwendung

- **Portfoliomanagement**: Die Bewertung von Korrelationen zwischen Assets hilft beim Aufbau diversifizierter Portfolios zur Risikominimierung.
- **Hedging-Strategien**: Das Erkennen von Assets mit hoher negativer Korrelation kann zur Entwicklung von Hedging-Strategien genutzt werden.
- **Analyse von Markttrends**: Die Untersuchung von Korrelationen zwischen verschiedenen Markten und Instrumenten liefert Einblicke in globale wirtschaftliche Zusammenhange.

## Berechnung der Pearson-Korrelation

Der Pearson-Korrelationskoeffizient wird mit folgender Formel berechnet:

\[ r = \frac{n(\sum xy) - (\sum x)(\sum y)}{\sqrt{[n\sum x^2 - (\sum x)^2][n\sum y^2 - (\sum y)^2]}} \]

wobei:
- \(n\) die Anzahl der Beobachtungen ist,
- \(x\) und \(y\) die Werte der Variablen sind,
- \(\sum\) die Summation bezeichnet.

## Skriptimplementierung

Ein Skript zur Berechnung der Pearson-Korrelation sollte die folgenden Schritte enthalten:

1. **Datenerfassung**: Laden der Zeitreihen für die beiden analysierten Variablen.
2. **Vorverarbeitung**: Ausrichten der Zeitreihen nach Daten und Entfernen fehlender Werte.
3. **Berechnung**: Anwenden der Pearson-Korrelationsformel auf die verarbeiteten Daten.
4. **Analyse der Ergebnisse**: Interpretieren des resultierenden Korrelationskoeffizienten für die Entscheidungsfindung.

Die Berechnung der Pearson-Korrelation liefert wichtige Informationen für Marktanalyse und Optimierung von Anlagestrategien, da sie die Einschatzung des gegenseitigen Einflusses zwischen Finanzassets ermöglicht.

## Skriptcode in C#

```cs
namespace StockSharp.Algo.Analytics
{
	using MathNet.Numerics.Statistics;

	/// <summary>
	/// Das Analyseskript berechnet die Pearson-Korrelation für angegebene Instrumente.
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
				// Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
				if (cancellationToken.IsCancellationRequested)
					break;

				// Kerzenspeicher abrufen
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// Schlusskurse abrufen
				var prices = candleStorage.Load(from, to).Select(c => (double)c.ClosePrice).ToArray();

				if (prices.Length == 0)
				{
					logs.LogWarning("Keine Daten für {0}", security);
					return Task.CompletedTask;
				}

				closes.Add(prices);
			}

			// Alle Arrays müssen die gleiche Länge haben, daher längere abschneiden
			var min = closes.Select(arr => arr.Length).Min();

			for (var i = 0; i < closes.Count; i++)
			{
				var arr = closes[i];

				if (arr.Length > min)
					closes[i] = arr.Take(min).ToArray();
			}

			// Korrelation berechnen
			var matrix = Correlation.PearsonMatrix(closes);

			// Ergebnis in einer Heatmap anzeigen
			var ids = securities.Select(s => s.ToStringId());
			panel.DrawHeatmap(ids, ids, matrix.ToArray());

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
from numpy_extensions import nx

clr.AddReference("NumpyDotNet")
from NumpyDotNet import np

# Das Analyseskript berechnet die Pearson-Korrelation für angegebene Instrumente.
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
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
			if cancellation_token.IsCancellationRequested:
				break

			# Kerzenspeicher abrufen
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# Schlusskurse abrufen
			prices = [float(c.ClosePrice) for c in load_range(candle_storage, message_type, from_date, to_date)]

			if len(prices) == 0:
				logs.LogWarning("Keine Daten für {0}", security)
				return Task.CompletedTask

			closes.append(prices)

		# Alle Arrays müssen die gleiche Länge haben, daher längere abschneiden
		min_length = min(len(arr) for arr in closes)
		closes = [arr[:min_length] for arr in closes]

		# Liste oder Array in ein 2D-Array konvertieren
		array2d = nx.to2darray(closes)

		# Korrelation mit NumSharp berechnen
		np_array = np.array(array2d)
		matrix = np.corrcoef(np_array)

		# Ergebnis in einer Heatmap anzeigen
		ids = [to_string_id(s) for s in securities]
		panel.DrawHeatmap(ids, ids, nx.tosystemarray(matrix))

		return Task.CompletedTask

```


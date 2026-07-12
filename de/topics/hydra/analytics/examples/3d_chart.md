# 3D-Chart

Das Skript `Chart3DScript` demonstriert die Erstellung eines 3D-Charts zur Visualisierung der Verteilung von Handelsvolumina nach Stunden für verschiedene Finanzinstrumente. Diese Visualisierungsmethode ermöglicht eine klare Darstellung der Handelsdynamik und das Erkennen von Spitzen der Marktaktivität.

![3D-Chart](../../../../images/hydra_analytics_chart3d.png)

## Beschreibung der Skriptausführung

Das Skript analysiert Kerzendaten für den angegebenen Zeitraum, gruppiert sie nach Stunden und berechnet das gesamte Handelsvolumen für jede Stunde. Die Ergebnisse werden in einem 3D-Chart dargestellt, dessen Achsen Folgendes repräsentieren:

- **X-Achse**: Finanzinstrumente.
- **Y-Achse**: Stunden der Handelssitzung (von 0 bis 23).
- **Z-Achse**: Handelsvolumina.

## Nutzen eines 3D-Charts

### Analyse der Marktaktivität

Der 3D-Chart ermöglicht die Einschätzung, wann bei mehreren Instrumenten gleichzeitig die größte Aktivität auftritt. Dies kann nützlich sein, um optimale Handelsfenster zu identifizieren oder den Einfluss globaler Ereignisse auf den Markt zu untersuchen.

### Vergleich von Instrumenten

Durch die Visualisierung der Handelsvolumina nach Stunden im dreidimensionalen Raum können Trader Instrumente hinsichtlich Aktivität und bevorzugter Handelszeiten miteinander vergleichen. Dies kann bei der Auswahl der liquidesten Instrumente zu bestimmten Stunden oder bei der Suche nach Instrumenten mit ähnlichen Aktivitätsmustern zur Portfoliodiversifikation helfen.

### Strategieoptimierung

Die Analyse der Verteilung von Handelsvolumina kann als Grundlage für die Optimierung von Handelsstrategien dienen, da sie eine Anpassung an Zeiträume mit der höchsten Marktaktivität ermöglicht. Dies ist besonders für algorithmischen und Hochfrequenzhandel relevant.

## Skriptimplementierung

Das Skript führt die folgenden Aktionen aus:

1. Prüfen, ob Finanzinstrumente für die Analyse vorhanden sind.
2. Bilden der Beschriftungen für die Achsen X (Instrumente) und Y (Stunden).
3. Laden und Gruppieren der Kerzendaten.
4. Berechnen der gesamten Handelsvolumina nach Stunden und Befüllen der Daten für die Z-Achse.
5. Zeichnen des 3D-Charts mit der Methode `panel.Draw3D`.

## Skriptcode in C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Das Analyseskript berechnet die Verteilung des größten Volumens nach Stunden
	/// und zeigt sie in einem 3D-Chart an.
	/// </summary>
	public class Chart3DScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("Keine Instrumente.");
				return Task.CompletedTask;
			}

			var x = new List<string>();
			var y = new List<string>();

			// Y-Beschriftungen befüllen
			for (var h = 0; h < 24; h++)
				y.Add(h.ToString());

			var z = new double[securities.Length, y.Count];

			for (var i = 0; i < securities.Length; i++)
			{
				// Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
				if (cancellationToken.IsCancellationRequested)
					break;

				var security = securities[i];

				// X-Beschriftungen befüllen
				x.Add(security.ToStringId());

				// Kerzenspeicher abrufen
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// Verfügbare Daten für den angegebenen Zeitraum abrufen
				var dates = candleStorage.GetDates(from, to).ToArray();

				if (dates.Length == 0)
				{
					logs.LogWarning("Keine Daten.");
					return Task.CompletedTask;
				}

				// Kerzen nach Eröffnungszeit gruppieren (nur Zeitanteil) mit Kürzung auf 1 Stunde
				var byHours = candleStorage.Load(from, to)
					.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
					.ToDictionary(g => g.Key.Hours, g => g.Sum(c => c.TotalVolume));

				// Z-Werte befüllen
				foreach (var pair in byHours)
					z[i, pair.Key] = (double)pair.Value;
			}

			panel.Draw3D(x, y, z, "Instrumente", "Stunden", "Volumen");

			return Task.CompletedTask;
		}
	}
}

```

## Skriptcode in Python

```python
import clr

# .NET-Referenzen hinzufügen
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from numpy_extensions import nx

# Das Analyseskript berechnet die Verteilung des größten Volumens nach Stunden und zeigt sie in einem 3D-Chart an.
class chart3d_script(IAnalyticsScript):
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
		# Prüfen, ob keine Instrumente vorhanden sind
		if not securities:
			logs.LogWarning("Keine Instrumente.")
			return Task.CompletedTask

		x = []  # X-Beschriftungen für Instrumente
		y = []  # Y-Beschriftungen für Stunden

		# Y-Beschriftungen mit Stunden von 0 bis 23 befüllen
		for h in range(24):
			y.append(str(h))

		# Ein 2D-Array für Z-Werte mit Dimensionen erstellen: (Anzahl der Instrumente) x (Anzahl der Stunden)
		z = [[0.0 for _ in range(len(y))] for _ in range(len(securities))]

		if data_type is None:
			logs.LogWarning(f"Nicht unterstützter Datentyp {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for i, security in enumerate(securities):
			# Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
			if cancellation_token.IsCancellationRequested:
				break

			# X-Beschriftungen mit Instrumentkennungen befüllen
			x.append(to_string_id(security))

			# Kerzenspeicher für das aktuelle Instrument abrufen
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# Verfügbare Daten für den angegebenen Zeitraum abrufen
			dates = get_dates(candle_storage, from_date, to_date)

			if len(dates) == 0:
				logs.LogWarning("Keine Daten.")
				return Task.CompletedTask

			# Kerzen nach Eröffnungszeit gruppieren (auf die nächste Stunde gekürzt) und Volumina summieren
			candles = load_range(candle_storage, message_type, from_date, to_date)
			by_hours = {}
			for candle in candles:
				hour = int(candle.OpenTime.TimeOfDay.TotalHours)
				by_hours[hour] = by_hours.get(hour, 0) + candle.TotalVolume

			# Z-Werte für das aktuelle Instrument befüllen
			for hour, volume in by_hours.items():
				if hour < len(y):
					z[i][hour] = float(volume)

		# 3D-Chart über panel zeichnen
		panel.Draw3D(x, y, nx.to2darray(z), "Instrumente", "Stunden", "Volumen")

		return Task.CompletedTask

```


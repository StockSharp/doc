# 3D-Chart

Das Skript `Chart3DScript` demonstriert die Erstellung eines 3D-Charts zur Visualisierung der Verteilung von Handelsvolumina nach Stunden fur verschiedene Finanzinstrumente. Diese Visualisierungsmethode ermoglicht eine klare Darstellung der Handelsdynamik und das Erkennen von Spitzen der Marktaktivitat.

![hydra_analytics_chart3d](../../../../images/hydra_analytics_chart3d.png)

## Beschreibung der Skriptausfuhrung

Das Skript analysiert Kerzendaten fur den angegebenen Zeitraum, gruppiert sie nach Stunden und berechnet das gesamte Handelsvolumen fur jede Stunde. Die Ergebnisse werden in einem 3D-Chart dargestellt, dessen Achsen Folgendes reprasentieren:

- **X-Achse**: Finanzinstrumente.
- **Y-Achse**: Stunden der Handelssitzung (von 0 bis 23).
- **Z-Achse**: Handelsvolumina.

## Nutzen eines 3D-Charts

### Analyse der Marktaktivitat

Der 3D-Chart ermoglicht die Einschatzung, wann bei mehreren Instrumenten gleichzeitig die grosste Aktivitat auftritt. Dies kann nutzlich sein, um optimale Handelsfenster zu identifizieren oder den Einfluss globaler Ereignisse auf den Markt zu untersuchen.

### Vergleich von Instrumenten

Durch die Visualisierung der Handelsvolumina nach Stunden im dreidimensionalen Raum konnen Trader Instrumente hinsichtlich Aktivitat und bevorzugter Handelszeiten miteinander vergleichen. Dies kann bei der Auswahl der liquidesten Instrumente zu bestimmten Stunden oder bei der Suche nach Instrumenten mit ahnlichen Aktivitätsmustern zur Portfoliodiversifikation helfen.

### Strategieoptimierung

Die Analyse der Verteilung von Handelsvolumina kann als Grundlage fur die Optimierung von Handelsstrategien dienen, da sie eine Anpassung an Zeitraume mit der hochsten Marktaktivitat ermoglicht. Dies ist besonders fur algorithmischen und Hochfrequenzhandel relevant.

## Skriptimplementierung

Das Skript fuhrt die folgenden Aktionen aus:

1. Prufen, ob Finanzinstrumente fur die Analyse vorhanden sind.
2. Bilden der Beschriftungen fur die Achsen X (Instrumente) und Y (Stunden).
3. Laden und Gruppieren der Kerzendaten.
4. Berechnen der gesamten Handelsvolumina nach Stunden und Befullen der Daten fur die Z-Achse.
5. Zeichnen des 3D-Charts mit der Methode `panel.Draw3D`.

## Skriptcode in C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Das Analyseskript berechnet die Verteilung des grossten Volumens nach Stunden
	/// und zeigt sie in einem 3D-Chart an.
	/// </summary>
	public class Chart3DScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var x = new List<string>();
			var y = new List<string>();

			// Y-Beschriftungen befullen
			for (var h = 0; h < 24; h++)
				y.Add(h.ToString());

			var z = new double[securities.Length, y.Count];

			for (var i = 0; i < securities.Length; i++)
			{
				// Berechnung stoppen, wenn der Benutzer die Skriptausfuhrung abbricht
				if (cancellationToken.IsCancellationRequested)
					break;

				var security = securities[i];

				// X-Beschriftungen befullen
				x.Add(security.ToStringId());

				// Kerzenspeicher abrufen
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// Verfugbare Daten fur den angegebenen Zeitraum abrufen
				var dates = candleStorage.GetDates(from, to).ToArray();

				if (dates.Length == 0)
				{
					logs.LogWarning("no data");
					return Task.CompletedTask;
				}

				// Kerzen nach Eroffnungszeit gruppieren (nur Zeitanteil) mit Kurzung auf 1 Stunde
				var byHours = candleStorage.Load(from, to)
					.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
					.ToDictionary(g => g.Key.Hours, g => g.Sum(c => c.TotalVolume));

				// Z-Werte befullen
				foreach (var pair in byHours)
					z[i, pair.Key] = (double)pair.Value;
			}

			panel.Draw3D(x, y, z, "Instruments", "Hours", "Volume");

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
from numpy_extensions import nx

# Das Analyseskript berechnet die Verteilung des grossten Volumens nach Stunden und zeigt sie in einem 3D-Chart an.
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
		# Prufen, ob keine Instrumente vorhanden sind
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		x = []  # X-Beschriftungen fur Instrumente
		y = []  # Y-Beschriftungen fur Stunden

		# Y-Beschriftungen mit Stunden von 0 bis 23 befullen
		for h in range(24):
			y.append(str(h))

		# Ein 2D-Array fur Z-Werte mit Dimensionen erstellen: (Anzahl der Instrumente) x (Anzahl der Stunden)
		z = [[0.0 for _ in range(len(y))] for _ in range(len(securities))]

		if data_type is None:
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for i, security in enumerate(securities):
			# Berechnung stoppen, wenn der Benutzer die Skriptausfuhrung abbricht
			if cancellation_token.IsCancellationRequested:
				break

			# X-Beschriftungen mit Instrumentkennungen befullen
			x.append(to_string_id(security))

			# Kerzenspeicher fur das aktuelle Instrument abrufen
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# Verfugbare Daten fur den angegebenen Zeitraum abrufen
			dates = get_dates(candle_storage, from_date, to_date)

			if len(dates) == 0:
				logs.LogWarning("no data")
				return Task.CompletedTask

			# Kerzen nach Eroffnungszeit gruppieren (auf die nachste Stunde gekurzt) und Volumina summieren
			candles = load_range(candle_storage, message_type, from_date, to_date)
			by_hours = {}
			for candle in candles:
				hour = int(candle.OpenTime.TimeOfDay.TotalHours)
				by_hours[hour] = by_hours.get(hour, 0) + candle.TotalVolume

			# Z-Werte fur das aktuelle Instrument befullen
			for hour, volume in by_hours.items():
				if hour < len(y):
					z[i][hour] = float(volume)

		# 3D-Chart uber panel zeichnen
		panel.Draw3D(x, y, nx.to2darray(z), "Instruments", "Hours", "Volume")

		return Task.CompletedTask

```


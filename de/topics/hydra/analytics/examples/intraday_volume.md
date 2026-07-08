# Intraday-Volumen

Das Skript "Intraday Volume" ist ein Werkzeug zur Analyse der Verteilung des Handelsvolumens von Wertpapieren nach Stunden innerhalb einer einzelnen Handelssitzung. Es ist für die Verwendung innerhalb der StockSharp-Plattform konzipiert und richtet sich an Trader und quantitative Analysten, die Marktverhalten detailliert untersuchen und Handelsstrategien optimieren möchten.

![hydra_analytics_intraday_volume](../../../../images/hydra_analytics_intraday_volume.png)

## Funktionsbeschreibung

Das Skript sammelt Daten zu Handelsoperationen für einen ausgewählten Zeitraum und stellt sie in Diagrammform dar. Dadurch können Benutzer visualisieren, wie sich das Handelsvolumen nach Stunden verandert. So lasst sich einschätzen, zu welchen Tageszeiten die Handelsaktivitat zunimmt oder abnimmt.

## Praktische Bedeutung

- **Für den Handel**: Das Verstandnis von Spitzen- und Nebenzeiten hilft, die aktivsten Marktphasen zu erkennen, und beeinflusst Entscheidungen daruber, wann Positionen eroffnet oder geschlossen werden.
- **Für quantitative Analyse**: Quantitative Analysten können Intraday-Volumendaten verwenden, um mathematische Modelle und Algorithmen zu erstellen, die Marktverhalten anhand von Volumenindikatoren prognostizieren.

## Stundenverteilung

Die Verteilung des Handelsvolumens nach Stunden verdeutlicht die Marktdynamik und hebt Zeitintervalle mit der wichtigsten Handelsaktivitat hervor. Dies kann auf Trendanderungen, Unterstutzungs- und Widerstandsniveaus sowie potenzielle Momente erhohter oder knapper Liquiditat hinweisen.

## Datenanwendung

Das Skript "Intraday Volume" kann in ein breiteres Marktanalysesystem integriert werden und Daten liefern, die für Folgendes genutzt werden können:

- **Strategieanpassung**: Anpassung der Parameter von Handelsalgorithmen an das Niveau der Marktaktivitat.
- **Risikobewertung**: Berechnung der Wahrscheinlichkeit signifikanter Preisbewegungen in Abhangigkeit von der Tageszeit.

Die Verwendung des Skripts "Intraday Volume" innerhalb der StockSharp-Handelsplattform ermöglicht Tradern und Analysten, ihre Entscheidungen auf konkrete Daten zur Marktaktivitat zu stutzen und Strategien optimal an die aktuellen Handelsbedingungen anzupassen.

## Skriptcode in C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Das Analyseskript berechnet die Verteilung des größten Volumens nach Stunden.
	/// </summary>
	public class TimeVolumeScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			// Skript kann nur 1 Instrument verarbeiten
			var security = securities.First();

			// Kerzenspeicher abrufen
			var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

			// Verfügbare Daten für den angegebenen Zeitraum abrufen
			var dates = candleStorage.GetDates(from, to).ToArray();

			if (dates.Length == 0)
			{
				logs.LogWarning("no data");
				return Task.CompletedTask;
			}

			// Kerzen nach Eroffnungszeit gruppieren (nur Zeitanteil) mit Kurzung auf 1 Stunde
			var rows = candleStorage.Load(from, to)
				.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
				.ToDictionary(g => g.Key, g => g.Sum(c => c.TotalVolume));

			// Unsere Berechnungen in das Grid einfugen
			var grid = panel.CreateGrid("Time", "Volume");

			foreach (var row in rows)
				grid.SetRow(row.Key, row.Value);

			// Nach Volumenspalte sortieren (absteigend)
			grid.SetSort("Volume", false);

			return Task.CompletedTask;
		}
	}
}

```

## Skriptcode in Python

```python
import clr

# .NET-Referenzen hinzufugen
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("StockSharp.Messages")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System import TimeSpan
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# Das Analyseskript berechnet die Verteilung des größten Volumens nach Stunden.
class time_volume_script(IAnalyticsScript):
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

		# Skript kann nur 1 Instrument verarbeiten
		security = securities[0]

		if data_type is None:
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		# Kerzenspeicher abrufen
		candle_storage = get_candle_storage(storage, security, data_type, drive, format)

		# Verfügbare Daten für den angegebenen Zeitraum abrufen
		dates = get_dates(candle_storage, from_date, to_date)

		if len(dates) == 0:
			logs.LogWarning("no data")
			return Task.CompletedTask

		# Kerzen nach Eroffnungszeit gruppieren (stundliche Kurzung) und ihre Volumina summieren
		candles = load_range(candle_storage, message_type, from_date, to_date)
		rows = {}
		for candle in candles:
			time_of_day = candle.OpenTime.TimeOfDay
			truncated = TimeSpan.FromHours(int(time_of_day.TotalHours))
			rows[truncated] = rows.get(truncated, 0) + candle.TotalVolume

		# Unsere Berechnungen in das Grid einfugen
		grid = panel.CreateGrid("Time", "Volume")

		for key, value in rows.items():
			grid.SetRow(key, value)

		# Nach der Volume-Spalte absteigend sortieren
		grid.SetSort("Volume", False)

		return Task.CompletedTask

```


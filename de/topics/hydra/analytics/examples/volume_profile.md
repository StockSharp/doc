# Volumenprofil

Das Skript "Volumenprofil" dient als Werkzeug zur Analyse der Verteilung des Handelsvolumens über Preisniveaus in einem ausgewählten Zeitraum. Es ermöglicht Tradern und quantitativen Analysten, zu visualisieren und zu untersuchen, wo die wichtigste Handelsaktivitat bezogen auf Preisniveaus konzentriert war.

![Volumenprofil](../../../../images/hydra_analytics_volume_profile.png)

## Funktionsbeschreibung

Das Skript aggregiert Transaktionsdaten, um ein Profil zu bilden, das die ausgefuhrten Volumina auf verschiedenen Preisniveaus anzeigt. Diese Informationen können in einem Chart dargestellt werden, der die Dichte der Trades über verschiedene Preisbereiche hinweg verdeutlicht.

## Praktische Bedeutung

Die Analyse des Volumenprofils hilft beim Erkennen wichtiger Nachfrage- und Angebotszonen und kann für Folgendes genutzt werden:

- Identifikation von Unterstutzungs- und Widerstandsniveaus, an denen das Instrument erhebliches Interesse von Marktteilnehmern findet.
- Einschatzung der Starke des aktuellen Trends oder einer möglichen Abschwachung anhand der Anderung der Volumenverteilung.
- Planung von Marktein- und -ausstiegen unter Berucksichtigung von Niveaus mit maximal angesammelter Liquiditat.

## Anwendung im Handel und in der quantitativen Analyse

- **Handel**: Das Volumenprofil kann zur Entwicklung von Strategien auf Basis der Volumenanalyse verwendet werden und zeigt klar, wo die wichtigsten Handelsoperationen stattfinden.
- **Quantitative Analyse**: Daten zur Volumenverteilung können als Eingangsdaten für quantitative Modelle dienen, die die Wahrscheinlichkeit von Preisbewegungen auf Basis des auf einem Niveau angesammelten Volumens prognostizieren.

## Skriptimplementierung

Das Skript "Volumenprofil" führt die folgenden Schritte aus:

1. **Datenerfassung**: Das Skript aggregiert Transaktionsdaten für den angegebenen Zeitraum.
2. **Profilbildung**: Auf Grundlage der gesammelten Daten erstellt das Skript ein Volumenprofil, das die Handelsaktivitat auf jedem Preisniveau widerspiegelt.
3. **Visualisierung**: Die Ergebnisse der Skriptausführung werden als Chart oder Histogramm visualisiert, wobei jeder Balken einem bestimmten Preisniveau und dessen Handelsvolumen entspricht.

Die Verwendung des Skripts "Volumenprofil" innerhalb der StockSharp-Plattform ermöglicht eine umfassende Marktanalyse, den Aufbau fundierter Handelshypothesen und eine Verbesserung der Qualitat getroffener Handelsentscheidungen.

## Skriptcode in C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Das Analyseskript berechnet die Verteilung des Volumens nach Preisniveaus.
	/// </summary>
	public class PriceVolumeScript : IAnalyticsScript
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

			// Kerzen nach mittlerem Preis gruppieren
			var rows = candleStorage.Load(from, to)
				.GroupBy(c => c.LowPrice + c.GetLength() / 2)
				.ToDictionary(g => g.Key, g => g.Sum(c => c.TotalVolume));

			// Im Chart zeichnen
			panel.CreateChart<decimal, decimal>()
				.Append(security.ToStringId(), rows.Keys, rows.Values, DrawStyles.Histogram);

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

# Das Analyseskript berechnet die Verteilung des Volumens nach Preisniveaus.
class price_volume_script(IAnalyticsScript):
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
			logs.LogWarning(f"Nicht unterstützter Datentyp {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		# Kerzenspeicher abrufen
		candle_storage = get_candle_storage(storage, security, data_type, drive, format)

		# Verfügbare Daten für den angegebenen Zeitraum abrufen
		dates = get_dates(candle_storage, from_date, to_date)

		if len(dates) == 0:
			logs.LogWarning("no data")
			return Task.CompletedTask

		# Kerzen nach mittlerem Preis gruppieren und ihre Volumina summieren
		candles = load_range(candle_storage, message_type, from_date, to_date)
		rows_dict = {}
		for candle in candles:
			# Mittleren Preis der Kerze berechnen
			key = candle.LowPrice + get_length(candle) / 2
			# Volumina für dasselbe Preisniveau summieren
			rows_dict[key] = rows_dict.get(key, 0) + candle.TotalVolume

		# Im Chart zeichnen
		chart = create_chart(panel, float, float)
		chart.Append(to_string_id(security), list(rows_dict.keys()), list(rows_dict.values()), DrawStyles.Histogram)

		return Task.CompletedTask

```

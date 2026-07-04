# Skript erstellen

Mit **Analytics** konnen Sie eigene Skripte erstellen. Als Beispiel betrachten wir **ChartDrawScript**, das die Moglichkeiten zum Zeichnen von Charts demonstriert:

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Das Analyseskript zeigt die Moglichkeiten zum Zeichnen von Charts.
	/// </summary>
	public class ChartDrawScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var lineChart = panel.CreateChart<DateTimeOffset, decimal>();
			var histogramChart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// Berechnung stoppen, wenn der Benutzer die Skriptausfuhrung abbricht
				if (cancellationToken.IsCancellationRequested)
					break;

				var candlesSeries = new Dictionary<DateTimeOffset, decimal>();
				var volsSeries = new Dictionary<DateTimeOffset, decimal>();

				// Kerzenspeicher abrufen
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				foreach (var candle in candleStorage.Load(from, to))
				{
					// Reihen befullen
					candlesSeries[candle.OpenTime] = candle.ClosePrice;
					volsSeries[candle.OpenTime] = candle.TotalVolume;
				}

				// Reihen im Chart als Linie und Histogramm zeichnen
				lineChart.Append($"{security} (close)", candlesSeries.Keys, candlesSeries.Values, DrawStyles.DashedLine);
				histogramChart.Append($"{security} (vol)", volsSeries.Keys, volsSeries.Values, DrawStyles.Histogram);
			}

			return Task.CompletedTask;
		}
	}
}

```

## Uberblick

Dieses Skript zeichnet Charts auf Basis der Preis- und Volumendaten von Finanzinstrumenten uber einen bestimmten Zeitraum. Es implementiert die Schnittstelle [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript), die den Vertrag fur jedes Analyseskript definiert, das im Programm **Hydra** ausgefuhrt werden kann.

## Schnittstelle `IAnalyticsScript`

Die Schnittstelle [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) stellt sicher, dass jedes implementierende Analyseskript die Methode [Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)) besitzt, die fur die analytischen Operationen des Skripts erforderlich ist.

### Methode `Run`

Die Methode [Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)) ist der Einstiegspunkt eines Analyseskripts, an dem die eigentliche Datenverarbeitung und Analyse ausgefuhrt werden.

#### Parameter:

- `logs`: Ubernimmt eine Instanz von [ILogReceiver](xref:Ecng.Logging.ILogReceiver) fur das Logging innerhalb des Skripts.
- `panel`: Stellt [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel) bereit, ein Benutzeroberflachenelement zum Zeichnen von Charts und Anzeigen von Ergebnissen.
- `securities`: Ein Array von [SecurityId](xref:StockSharp.Messages.SecurityId), das die Finanzinstrumente fur die Analyse identifiziert.
- `from`: Das Startdatum des Datenbereichs fur die Analyse.
- `to`: Das Enddatum des Datenbereichs fur die Analyse.
- `storage`: Eine Instanz von [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry), die Zugriff auf den Marktdatenspeicher ermoglicht.
- `drive`: Stellt [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) dar und gibt den Speicherort der Marktdaten an.
- `format`: Ein Wert von [StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats), der das Marktdatenformat angibt.
- `dataType`: [DataType](xref:StockSharp.Messages.DataType), der den angeforderten Marktdatentyp und dessen Parameter beschreibt (zum Beispiel den Candle-Timeframe).
- `cancellationToken`: [CancellationToken](xref:System.Threading.CancellationToken) zur Uberwachung von Abbruchanforderungen.

#### Ruckgabe:

- [Task](xref:System.Threading.Tasks.Task), der die asynchrone Operation des Analyseskripts darstellt.

## Implementierungsdetails

Die Klasse `ChartDrawScript` verarbeitet gezielt Marktdaten fur jedes ubergebene Instrument. Sie erstellt zwei Charttypen: einen Linienchart fur Schlusskurse und ein Histogramm fur Volumendaten.

### Hauptverarbeitungsschritte:

1. Prufen, ob Instrumente zur Verarbeitung vorhanden sind. Wenn keine vorhanden sind, wird eine Warnung protokolliert und die Aufgabe beendet.
2. Erstellen eines Liniencharts und eines Histogramms mit der Methode [IAnalyticsPanel.CreateChart](xref:StockSharp.Algo.Analytics.IAnalyticsPanel.CreateChart``2).
3. Durchlaufen jedes Instruments und Prufen auf Abbruchanforderungen.
4. Abrufen des Kerzenspeichers mit der Methode `storage.GetCandleMessageStorage`.
5. Laden der Kerzendaten innerhalb des angegebenen Datumsbereichs.
6. Befullen von Dictionaries mit Zeitreihendaten der Eroffnungszeit, den entsprechenden Schlusskursen und Gesamtvolumina.
7. Zeichnen der Reihendaten in Charts mit den Methoden `lineChart.Append` und `histogramChart.Append`.

Das Skript verwendet Stile wie [DrawStyles.DashedLine](xref:Ecng.Drawing.DrawStyles.DashedLine) fur den Linienchart und [DrawStyles.Histogram](xref:Ecng.Drawing.DrawStyles.Histogram) fur das Histogramm, um unterschiedliche Datendarstellungen visuell zu unterscheiden.

Durch die Implementierung von [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) ermoglicht die Klasse `ChartDrawScript` die Integration eines Ansatzes zur Ausfuhrung anpassbarer Analyseskripte und wird damit zu einem vielseitigen Werkzeug fur Trader und Analysten auf der StockSharp-Plattform.

## Ausfuhrungsergebnis

![hydra_analytics_chart](../../../images/hydra_analytics_chart.png)


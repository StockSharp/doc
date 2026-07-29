# Skript erstellen

Mit **Analytik** können Sie eigene Skripte erstellen. Als Beispiel betrachten wir **ChartDrawScript**, das die Möglichkeiten zum Zeichnen von Charts demonstriert:

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// Das Analyseskript zeigt die Möglichkeiten zum Zeichnen von Charts.
	/// </summary>
	public class ChartDrawScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("Keine Instrumente.");
				return Task.CompletedTask;
			}

			var lineChart = panel.CreateChart<DateTimeOffset, decimal>();
			var histogramChart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// Berechnung stoppen, wenn der Benutzer die Skriptausführung abbricht
				if (cancellationToken.IsCancellationRequested)
					break;

				var candlesSeries = new Dictionary<DateTimeOffset, decimal>();
				var volsSeries = new Dictionary<DateTimeOffset, decimal>();

				// Kerzenspeicher abrufen
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				foreach (var candle in candleStorage.Load(from, to))
				{
					// Reihen befüllen
					candlesSeries[candle.OpenTime] = candle.ClosePrice;
					volsSeries[candle.OpenTime] = candle.TotalVolume;
				}

				// Reihen im Chart als Linie und Histogramm zeichnen
				lineChart.Append($"{security} (Schlusskurs)", candlesSeries.Keys, candlesSeries.Values, DrawStyles.DashedLine);
				histogramChart.Append($"{security} (Volumen)", volsSeries.Keys, volsSeries.Values, DrawStyles.Histogram);
			}

			return Task.CompletedTask;
		}
	}
}

```

## Überblick

Dieses Skript zeichnet Charts auf Basis der Preis- und Volumendaten von Finanzinstrumenten über einen bestimmten Zeitraum. Es implementiert die Schnittstelle [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript), die den Vertrag für jedes Analyseskript definiert, das im Programm **Hydra** ausgeführt werden kann.

## Schnittstelle `IAnalyticsScript`

Die Schnittstelle [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) stellt sicher, dass jedes implementierende Analyseskript die Methode [Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)) besitzt, die für die analytischen Operationen des Skripts erforderlich ist.

### Methode `Run`

Die Methode [Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)) ist der Einstiegspunkt eines Analyseskripts, an dem die eigentliche Datenverarbeitung und Analyse ausgeführt werden.

#### Parameter:

- `logs`: Übernimmt eine Instanz von [ILogReceiver](xref:Ecng.Logging.ILogReceiver) für die Protokollierung innerhalb des Skripts.
- `panel`: Stellt [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel) bereit, ein Benutzeroberflächenelement zum Zeichnen von Charts und Anzeigen von Ergebnissen.
- `securities`: Ein Array von [SecurityId](xref:StockSharp.Messages.SecurityId), das die Finanzinstrumente für die Analyse identifiziert.
- `from`: Das Startdatum des Datenbereichs für die Analyse.
- `to`: Das Enddatum des Datenbereichs für die Analyse.
- `storage`: Eine Instanz von [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry), die Zugriff auf den Marktdatenspeicher ermöglicht.
- `drive`: Stellt [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) dar und gibt den Speicherort der Marktdaten an.
- `format`: Ein Wert von [StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats), der das Marktdatenformat angibt.
- `dataType`: [DataType](xref:StockSharp.Messages.DataType), der den angeforderten Marktdatentyp und dessen Parameter beschreibt (zum Beispiel den Kerzenzeitrahmen).
- `cancellationToken`: [CancellationToken](xref:System.Threading.CancellationToken) zur Überwachung von Abbruchanforderungen.

#### Rückgabe:

- [Task](xref:System.Threading.Tasks.Task), der die asynchrone Operation des Analyseskripts darstellt.

## Implementierungsdetails

Die Klasse `ChartDrawScript` verarbeitet gezielt Marktdaten für jedes übergebene Instrument. Sie erstellt zwei Charttypen: einen Linienchart für Schlusskurse und ein Histogramm für Volumendaten.

### Hauptverarbeitungsschritte:

1. Prüfen, ob Instrumente zur Verarbeitung vorhanden sind. Wenn keine vorhanden sind, wird eine Warnung protokolliert und die Aufgabe beendet.
2. Erstellen eines Liniencharts und eines Histogramms mit der Methode [IAnalyticsPanel.CreateChart](xref:StockSharp.Algo.Analytics.IAnalyticsPanel.CreateChart``2).
3. Durchlaufen jedes Instruments und Prüfen auf Abbruchanforderungen.
4. Abrufen des Kerzenspeichers mit der Methode `storage.GetCandleMessageStorage`.
5. Laden der Kerzendaten innerhalb des angegebenen Datumsbereichs.
6. Befüllen von Dictionaries mit Zeitreihendaten der Eröffnungszeit, den entsprechenden Schlusskursen und Gesamtvolumina.
7. Zeichnen der Reihendaten in Charts mit den Methoden `lineChart.Append` und `histogramChart.Append`.

Das Skript verwendet Stile wie [DrawStyles.DashedLine](xref:Ecng.Drawing.DrawStyles.DashedLine) für den Linienchart und [DrawStyles.Histogram](xref:Ecng.Drawing.DrawStyles.Histogram) für das Histogramm, um unterschiedliche Datendarstellungen visuell zu unterscheiden.

Durch die Implementierung von [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) ermöglicht die Klasse `ChartDrawScript` die Integration eines Ansatzes zur Ausführung anpassbarer Analyseskripte und wird damit zu einem vielseitigen Werkzeug für Trader und Analysten auf der StockSharp-Plattform.

## Ausführungsergebnis

![Skript erstellen](../../../images/hydra_analytics_chart.png)


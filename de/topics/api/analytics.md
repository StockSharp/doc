# Analyseskripte

[S#](../api.md) implementiert ein Subsystem für Analyseskripte, das eine beliebige Analyse von Marktdaten mit Visualisierung der Ergebnisse ermöglicht. Die Klassen befinden sich im Namespace `StockSharp.Algo.Analytics`.

## IAnalyticsScript — Hauptschnittstelle

Die Schnittstelle [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) definiert eine einzelne Methode:

```cs
Task Run(
    ILogReceiver logs,
    IAnalyticsPanel panel,
    SecurityId[] securities,
    DateTime from,
    DateTime to,
    IStorageRegistry storage,
    IMarketDataDrive drive,
    StorageFormats format,
    DataType dataType,
    CancellationToken cancellationToken);
```

Parameter:

- **logs** — Log-Empfänger zur Ausgabe von Diagnosemeldungen.
- **panel** — Panel zur Anzeige der Analyseergebnisse.
- **securities** — Array der zu analysierenden Instrumente.
- **from** / **to** — Zeitbereich.
- **storage** — Registry für die Marktdatenspeicherung.
- **drive** — Datenquelle.
- **format** — Format der Datenspeicherung.
- **dataType** — Typ der zu analysierenden Daten.
- **cancellationToken** — Abbruch-Token.

## IAnalyticsPanel — Ergebnis-Panel

Die Schnittstelle [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel) stellt Methoden zur Erstellung verschiedener Visualisierungen bereit:

- **CreateGrid(params string[] columns)** — erstellt eine Tabelle [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) mit den angegebenen Spalten.
- **CreateChart\<X, Y\>()** — erstellt ein zweidimensionales Diagramm [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2).
- **CreateChart\<X, Y, Z\>()** — erstellt ein dreidimensionales Diagramm [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`3).
- **DrawHeatmap(string[] xTitles, string[] yTitles, double[,] data)** — zeichnet eine Heatmap.
- **Draw3D(string[] xTitles, string[] yTitles, data, xTitle, yTitle, zTitle)** — zeichnet eine 3D-Visualisierung.

## IAnalyticsChart — Diagramme

Die Schnittstelle [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2) stellt eine Methode zum Hinzufügen von Datenserien bereit:

```cs
void Append(string title, IEnumerable<X> xValues, IEnumerable<Y> yValues,
    DrawStyles style, Color? color = null);
```

Verfügbare Zeichenstile ([DrawStyles](xref:StockSharp.Algo.Analytics.DrawStyles)):

- **Line** — Liniendiagramm.
- **DashedLine** — gestrichelte Linie.
- **Histogram** — Histogramm.
- **Bubble** — Blasendiagramm.

## IAnalyticsGrid — Tabellen

Die Schnittstelle [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) ermöglicht die Anzeige tabellarischer Daten:

- **SetSort(string column, bool ascending)** — Sortierung nach Spalte festlegen.
- **SetRow(params object[] values)** — eine Datenzeile hinzufügen.

## Integrierte Skripte

Das Paket `StockSharp.Algo.Analytics.CSharp` enthält fertige Skripte:

- **IndicatorScript** — berechnet und visualisiert Indikatoren in einem Diagramm.
- **ChartDrawScript** — demonstriert die Erstellung verschiedener Diagrammtypen.
- **PriceVolumeScript** — analysiert die Volumenverteilung über Preisniveaus.

## Beispiel: Benutzerdefiniertes Analyseskript

Nachfolgend ein Beispielskript, das Kerzen für eine Liste von Instrumenten lädt und Schlusskurse in einem Liniendiagramm anzeigt:

```cs
public class MyAnalyticsScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        // zweidimensionales Diagramm erstellen
        var chart = panel.CreateChart<DateTime, decimal>();

        foreach (var secId in securities)
        {
            // Kerzenspeicher abrufen
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            // Daten für den Zeitraum laden
            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
            {
                logs.AddWarningLog($"Keine Daten für {secId}");
                continue;
            }

            // Serie zum Diagramm hinzufügen
            chart.Append(secId.ToString(),
                candles.Select(c => c.OpenTime.UtcDateTime),
                candles.Select(c => c.ClosePrice),
                DrawStyles.Line);

            logs.AddInfoLog($"{secId}: {candles.Length} Kerzen geladen");
        }
    }
}
```

## Beispiel: Volumen-Tabelle

```cs
public class VolumeTableScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        var grid = panel.CreateGrid("Instrument", "Kerzen gesamt",
            "Gesamtvolumen", "Durchschnittsvolumen");
        grid.SetSort("Gesamtvolumen", false);

        foreach (var secId in securities)
        {
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
                continue;

            var totalVolume = candles.Sum(c => c.TotalVolume);
            var avgVolume = totalVolume / candles.Length;

            grid.SetRow(secId.ToString(), candles.Length,
                totalVolume, avgVolume);
        }
    }
}
```

## Siehe auch

[Indikatoren](indicators.md)

[Datenspeicherung](market_data_storage.md)

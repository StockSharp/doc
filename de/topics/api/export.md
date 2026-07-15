# Datenexport

[S#](../api.md) implementiert ein Subsystem für den Export von Marktdaten in verschiedene Formate. Alle Exporter erben von der Basisklasse [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) und unterstützen eine einheitliche asynchrone Schnittstelle.

## BaseExporter

Die abstrakte Basisklasse [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) definiert den gemeinsamen Vertrag für alle Exporter:

- **Datentyp** - Typ der exportierten Daten (Ticks, Kerzen, Orderbuch usw.).
- **Codierung** - Kodierung (standardmäßig UTF-8).
- **Export\<T\>(IAsyncEnumerable\<T\>, CancellationToken)** - die zentrale Exportmethode. Gibt `Task<(int count, DateTime? lastTime)>` zurück - die Anzahl der exportierten Datensätze und die Zeit des letzten Datensatzes.

Die Methode leitet Daten automatisch an typspezifische Handler weiter für: [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage), [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage), [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) (Ticks, Orderprotokoll, Transaktionen), [CandleMessage](xref:StockSharp.Messages.CandleMessage), [NewsMessage](xref:StockSharp.Messages.NewsMessage), [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage), [IndicatorValue](xref:StockSharp.Messages.IndicatorValue) und [BoardStateMessage](xref:StockSharp.Messages.BoardStateMessage).

## Exporter-Typen

### 1. TextExporter - CSV-/Textexport

[TextExporter](xref:StockSharp.Algo.Export.TextExporter) exportiert Daten mit SmartFormat-Vorlagen in ein Textformat.

- **Konstruktor**: `(DataType dataType, Stream stream, string template, string header)`
- Vorlagen verwenden die SmartFormat-Syntax, zum Beispiel: `{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}`

```cs
await using var stream = File.Create("trades.csv");
var exporter = new TextExporter(DataType.Ticks, stream,
    "{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}",
    "Datum;Preis;Volumen");

var (count, lastTime) = await exporter.Export(tickMessages, token);
```

### 2. JsonExporter - JSON-Export

[JsonExporter](xref:StockSharp.Algo.Export.JsonExporter) speichert Daten im JSON-Format.

- **Konstruktor**: `(DataType dataType, Stream stream)`
- **Einrückung** - eingerückte Formatierung (standardmäßig `true`).

```cs
await using var stream = File.Create("candles.json");
var exporter = new JsonExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 3. XmlExporter - XML-Export

[XmlExporter](xref:StockSharp.Algo.Export.XmlExporter) speichert Daten im XML-Format.

- **Konstruktor**: `(DataType dataType, Stream stream)`
- **Einrückung** - eingerückte Formatierung (standardmäßig `true`).

```cs
await using var stream = File.Create("candles.xml");
var exporter = new XmlExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 4. ExcelExporter - Excel-Export

[ExcelExporter](xref:StockSharp.Algo.Export.ExcelExporter) exportiert Daten in Excel-Tabellen.

- **Konstruktor**: `(IExcelWorkerProvider provider, DataType dataType, Stream stream, Action breaked)`
- Maximale Zeilenanzahl: 1.048.576 (Beschränkung des Excel-Formats).

```cs
await using var stream = File.Create("data.xlsx");
var exporter = new ExcelExporter(excelProvider, DataType.Ticks, stream,
    () => { /* Unterbrechung behandeln */ });
await exporter.Export(tickMessages, token);
```

### 5. DatabaseExporter - Datenbankexport

[DatabaseExporter](xref:StockSharp.Algo.Export.DatabaseExporter) speichert Daten über LinqToDB in einer Datenbank.

- **Konstruktor**: `(IDatabaseProvider dbProvider, DataType dataType, DatabaseConnectionPair connection, decimal? priceStep, decimal? volumeStep)`
- **Stapelgröße** - Batchgröße für Datensätze (standardmäßig 50).
- **CheckUnique** - Datensatz-Eindeutigkeit prüfen (standardmäßig `false`).
- **DropExisting** - vorhandene Daten vor dem Export löschen (standardmäßig `false`).

```cs
var exporter = new DatabaseExporter(
    dbProvider, DataType.Ticks, dbConnection)
{
    BatchSize = 100,
    CheckUnique = true
};
await exporter.Export(tickMessages, token);
```

### 6. StockSharpExporter - natives StockSharp-Format

[StockSharpExporter](xref:StockSharp.Algo.Export.StockSharpExporter) speichert Daten im internen StockSharp-Speicherformat.

- **Konstruktor**: `(DataType dataType, IStorageRegistry storageRegistry, IMarketDataDrive drive, StorageFormats format)`
- **Stapelgröße** - Batchgröße für Datensätze (standardmäßig 50).

```cs
var exporter = new StockSharpExporter(
    DataType.Ticks, storageRegistry, drive, StorageFormats.Binary);
await exporter.Export(tickMessages, token);
```

## TemplateTxtRegistry - Registry für Textvorlagen

Die Klasse [TemplateTxtRegistry](xref:StockSharp.Algo.Export.TemplateTxtRegistry) enthält vordefinierte Vorlagen für den Export verschiedener Datentypen über [TextExporter](xref:StockSharp.Algo.Export.TextExporter):

- **TemplateTxtTick** - Vorlage für Tick-Daten.
- **TemplateTxtDepth** - Vorlage für Orderbücher.
- **TemplateTxtCandle** - Vorlage für Kerzen.
- **TemplateTxtLevel1** - Vorlage für Level1-Daten.
- **TemplateTxtOrderLog** - Vorlage für Orderprotokoll.
- **TemplateTxtTransaction** - Vorlage für Transaktionen.
- **Instrumentvorlage** - Vorlage für Instrumente.
- **TemplateTxtNews** - Vorlage für Nachrichten.

Vorlagen können bei Bedarf angepasst oder ersetzt werden. Die Registry implementiert [IPersistable](xref:Ecng.Serialization.IPersistable) und kann aus Einstellungen gespeichert bzw. geladen werden.

## Siehe auch

[Datenimport](import.md)

[Datenspeicherung](market_data_storage.md)

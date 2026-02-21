# Data Export

[S\#](../api.md) implements a market data export subsystem supporting various formats. All exporters inherit from the base class [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) and support a unified asynchronous interface.

## BaseExporter

The base abstract class [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) defines the common contract for all exporters:

- **DataType** — the type of data being exported (ticks, candles, order book, etc.).
- **Encoding** — encoding (UTF-8 by default).
- **Export\<T\>(IAsyncEnumerable\<T\>, CancellationToken)** — the main export method. Returns `Task<(int count, DateTime? lastTime)>` — the number of exported records and the time of the last record.

The method automatically routes data to type-specific handlers for: [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage), [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage), [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) (ticks, order log, transactions), [CandleMessage](xref:StockSharp.Messages.CandleMessage), [NewsMessage](xref:StockSharp.Messages.NewsMessage), [SecurityMessage](xref:StockSharp.Messages.SecurityMessage), [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage), [IndicatorValue](xref:StockSharp.Messages.IndicatorValue), and [BoardStateMessage](xref:StockSharp.Messages.BoardStateMessage).

## Exporter Types

### 1. TextExporter — CSV/Text Export

[TextExporter](xref:StockSharp.Algo.Export.TextExporter) exports data to text format using SmartFormat templates.

- **Constructor**: `(DataType dataType, Stream stream, string template, string header)`
- Templates use SmartFormat syntax, for example: `{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}`

```cs
await using var stream = File.Create("trades.csv");
var exporter = new TextExporter(DataType.Ticks, stream,
    "{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}",
    "Date;Price;Volume");

var (count, lastTime) = await exporter.Export(tickMessages, token);
```

### 2. JsonExporter — JSON Export

[JsonExporter](xref:StockSharp.Algo.Export.JsonExporter) saves data in JSON format.

- **Constructor**: `(DataType dataType, Stream stream)`
- **Indent** — indented formatting (default `true`).

```cs
await using var stream = File.Create("candles.json");
var exporter = new JsonExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 3. XmlExporter — XML Export

[XmlExporter](xref:StockSharp.Algo.Export.XmlExporter) saves data in XML format.

- **Constructor**: `(DataType dataType, Stream stream)`
- **Indent** — indented formatting (default `true`).

```cs
await using var stream = File.Create("candles.xml");
var exporter = new XmlExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 4. ExcelExporter — Excel Export

[ExcelExporter](xref:StockSharp.Algo.Export.ExcelExporter) exports data to Excel spreadsheets.

- **Constructor**: `(IExcelWorkerProvider provider, DataType dataType, Stream stream, Action breaked)`
- Maximum row count: 1,048,576 (Excel format limitation).

```cs
await using var stream = File.Create("data.xlsx");
var exporter = new ExcelExporter(excelProvider, DataType.Ticks, stream,
    () => { /* handle interruption */ });
await exporter.Export(tickMessages, token);
```

### 5. DatabaseExporter — Database Export

[DatabaseExporter](xref:StockSharp.Algo.Export.DatabaseExporter) saves data to a database via LinqToDB.

- **Constructor**: `(IDatabaseProvider dbProvider, DataType dataType, DatabaseConnectionPair connection, decimal? priceStep, decimal? volumeStep)`
- **BatchSize** — batch size for records (default 50).
- **CheckUnique** — check record uniqueness (default `false`).
- **DropExisting** — delete existing data before export (default `false`).

```cs
var exporter = new DatabaseExporter(
    dbProvider, DataType.Ticks, dbConnection)
{
    BatchSize = 100,
    CheckUnique = true
};
await exporter.Export(tickMessages, token);
```

### 6. StockSharpExporter — StockSharp Native Format

[StockSharpExporter](xref:StockSharp.Algo.Export.StockSharpExporter) saves data in the internal StockSharp storage format.

- **Constructor**: `(DataType dataType, IStorageRegistry storageRegistry, IMarketDataDrive drive, StorageFormats format)`
- **BatchSize** — batch size for records (default 50).

```cs
var exporter = new StockSharpExporter(
    DataType.Ticks, storageRegistry, drive, StorageFormats.Binary);
await exporter.Export(tickMessages, token);
```

## TemplateTxtRegistry — Text Template Registry

The [TemplateTxtRegistry](xref:StockSharp.Algo.Export.TemplateTxtRegistry) class contains predefined templates for exporting various data types via [TextExporter](xref:StockSharp.Algo.Export.TextExporter):

- **TemplateTxtTick** — template for tick data.
- **TemplateTxtDepth** — template for order books.
- **TemplateTxtCandle** — template for candles.
- **TemplateTxtLevel1** — template for Level1 data.
- **TemplateTxtOrderLog** — template for order log.
- **TemplateTxtTransaction** — template for transactions.
- **TemplateTxtSecurity** — template for instruments.
- **TemplateTxtNews** — template for news.

Templates can be customized or replaced as needed. The registry implements [IPersistable](xref:Ecng.Serialization.IPersistable) and can be saved/loaded from settings.

## See Also

[Data Import](import.md)

[Data Storage](market_data_storage.md)

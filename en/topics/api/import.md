# Data Import

[S\#](../api.md) implements a market data import subsystem from CSV files. The main classes are located in the `StockSharp.Algo.Import` namespace.

## CsvParser — Base Parser

The [CsvParser](xref:StockSharp.Algo.Import.CsvParser) class performs CSV file parsing and converts rows into [S\#](../api.md) messages.

- **Constructor**: `(DataType dataType, IEnumerable<FieldMapping> fields)`
- **ColumnSeparator** — column separator (default `","`).
- **LineSeparator** — line separator (default CRLF).
- **SkipFromHeader** — number of rows to skip from the beginning of the file (default `0`).
- **IgnoreNonIdSecurities** — ignore rows with unrecognized instruments (default `true`).
- **Parse(Stream)** — parsing method, returns `IAsyncEnumerable<Message>`.

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);
var parser = new CsvParser(DataType.Ticks, fields)
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");

await foreach (var msg in parser.Parse(stream))
{
    // process each message
}
```

## CsvImporter — Import with Storage Saving

The [CsvImporter](xref:StockSharp.Algo.Import.CsvImporter) class extends [CsvParser](xref:StockSharp.Algo.Import.CsvParser), adding the ability to automatically save data to a market data storage.

- **Constructor**: `(DataType dataType, IEnumerable<FieldMapping> fields, ISecurityStorage securityStorage, IExchangeInfoProvider exchangeInfoProvider, Func<SecurityId, IMarketDataStorage> getStorage)`
- **Import(Stream, Action\<int\> progress, CancellationToken)** — performs the import and returns `ValueTask<(int count, DateTime? lastTime)>`.
- **UpdateDuplicateSecurities** — whether to update duplicate instruments (default `false`).
- **SecurityUpdated** — event raised when an instrument is updated.

```cs
var fields = FieldMappingRegistry.CreateFields(DataType.Ticks);

var importer = new CsvImporter(
    DataType.Ticks,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, DataType.Ticks))
{
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

await using var stream = File.OpenRead("trades.csv");
var (count, lastTime) = await importer.Import(
    stream,
    p => Console.WriteLine($"Progress: {p}%"),
    token);

Console.WriteLine($"Imported {count} records, last: {lastTime}");
```

## FieldMapping — Field Descriptions

The [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) class describes the mapping between CSV file columns and message properties.

Main properties:

- **Name** — field name in the message.
- **DisplayName** — display name.
- **Type** — value type.
- **Order** — column index in the file (starting from 0).
- **IsRequired** — whether the field is required.
- **Format** — parsing format (e.g., date format).
- **DefaultValue** — default value.
- **ZeroAsNull** — whether to interpret zero values as `null`.

For custom value transformations, use [FieldMappingValue](xref:StockSharp.Algo.Import.FieldMappingValue). For example, you can define a mapping of text values to enumerations:

```cs
var sideField = fields.First(f => f.Name == "Side");
sideField.Order = 3;
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "B",
    ValueTo = Sides.Buy
});
sideField.Values.Add(new FieldMappingValue
{
    ValueFrom = "S",
    ValueTo = Sides.Sell
});
```

## FieldMappingRegistry — Standard Fields Registry

The static class [FieldMappingRegistry](xref:StockSharp.Algo.Import.FieldMappingRegistry) provides a method for creating a standard set of fields:

- **CreateFields(DataType)** — returns a list of [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) for the specified data type.

Supported data types: ticks, candles, order books, Level1, order log, transactions, instruments, news, and positions.

## ImportSettings — Import Settings

The [ImportSettings](xref:StockSharp.Algo.Import.ImportSettings) class combines all import parameters into a single configuration object:

- **DataType** — the type of data being imported.
- **FileName** — file path.
- **Directory** — directory for file search.
- **FileMask** — file search mask (e.g., `*.csv`).
- **ColumnSeparator** — column separator.
- **SkipFromHeader** — number of rows to skip.
- **SelectedFields** — fields selected for import.
- **UpdateDuplicateSecurities** — whether to update duplicate instruments.

Helper methods:

- **GetFiles(IFileSystem)** — get a list of files matching the mask.
- **FillParser(CsvParser)** — populate parser settings.
- **FillImporter(CsvImporter)** — populate importer settings.

```cs
var settings = new ImportSettings
{
    DataType = DataType.Ticks,
    FileName = "trades.csv",
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

var fields = FieldMappingRegistry.CreateFields(settings.DataType);
// configure column order
fields[0].Order = 0; // SecurityId
fields[1].Order = 1; // Date
fields[2].Order = 2; // Price
fields[3].Order = 3; // Volume

var importer = new CsvImporter(
    settings.DataType,
    fields,
    securityStorage,
    exchangeInfoProvider,
    secId => storageRegistry.GetStorage(secId, settings.DataType));

settings.FillImporter(importer);

await using var stream = File.OpenRead(settings.FileName);
var (count, lastTime) = await importer.Import(stream, p => { }, token);
```

## See Also

[Data Export](export.md)

[Data Storage](market_data_storage.md)

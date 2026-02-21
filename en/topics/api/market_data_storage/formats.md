# Storage Formats

StockSharp supports two market data storage formats defined by the [StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) enumeration: **Binary** and **CSV**. Each format has its own advantages and is suitable for different use cases.

## Binary Format

The binary format is the primary high-performance data storage format in StockSharp. The implementation is located in the `BinaryMarketDataSerializer` class within `Algo/Storages/Binary/`.

### Features

- **Compactness** -- data is serialized in binary form with compression, ensuring minimal file sizes.
- **Performance** -- reading and writing are significantly faster compared to text formats.
- **Metadata** -- the `BinaryMetaInfo` class stores auxiliary information: first and last prices, fractional values, timestamps. This allows quickly retrieving general data information without fully reading the file.
- **Compression-ready architecture** -- the format is designed from the ground up for efficient streaming compression.

Binary files have the `.bin` extension.

### Supported Data Types

The binary format supports serialization of all major market data types: candles, ticks (trades), order books (Level2), Level1 data, orders, and own trades. Each type has a specialized serializer optimized for the structure of the specific data.

## CSV Format

The CSV (Comma-Separated Values) text format is implemented in the `CsvMarketDataSerializer` class located in `Algo/Storages/Csv/`.

### Features

- **Readability** -- files can be opened in any text editor or in Excel for visual data analysis.
- **Editability** -- data can be manually corrected when necessary.
- **Metadata** -- the `CsvMetaInfo` class provides metadata storage with encoding support. It contains the `IncrementalOnly` property (incremental data support) and `LastId` (last record identifier).
- **Larger file size** -- text representation takes significantly more disk space.
- **Slower processing** -- parsing text data requires additional computational resources.

CSV files have the `.csv` extension.

## When to Use Which Format

| Scenario | Recommended Format |
|----------|-------------------|
| Production use | Binary |
| Large data volumes | Binary |
| Performance-critical | Binary |
| Debugging and diagnostics | CSV |
| Visual data analysis | CSV |
| Integration with external tools | CSV |
| Manual data correction | CSV |

## File Organization on Disk

Data on disk is organized according to the following path structure:

```
{root_folder}/{first_letter}/{instrument_identifier}/{yyyy_MM_dd}/{file_name}.{extension}
```

Where the extension is `.bin` for binary format or `.csv` for text format. This hierarchical organization ensures fast data lookup by instrument and date.

For example, 5-minute candles for the SBER@TQBR instrument on April 1, 2024 in binary format would be located at a path like:

```
Storage/S/SBER@TQBR/2024_04_01/candles_5m.bin
```

## Converting Between Formats

StockSharp allows loading data in one format and saving it in another. This can be useful, for example, for exporting binary data to CSV for analysis in external tools:

```cs
// Load from binary storage
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Binary);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();

// Save to CSV storage
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId, TimeSpan.FromMinutes(5), StorageFormats.Csv);
await csvStorage.SaveAsync(candles);
```

## Code Example

The format is selected when creating a storage via [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry):

```cs
var storageRegistry = new StorageRegistry();

// Create candle storage in binary format
var binaryStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Binary);

// Create candle storage in CSV format
var csvStorage = storageRegistry.GetTimeFrameCandleMessageStorage(
    securityId,
    TimeSpan.FromMinutes(5),
    StorageFormats.Csv);

// Load data
var from = new DateTime(2024, 1, 1);
var to = new DateTime(2024, 1, 31);
var candles = await binaryStorage.LoadAsync(from, to).ToArrayAsync();
```

The storage format can also be specified when working with tick data and order books:

```cs
// Ticks in binary format
var tickStorage = storageRegistry.GetTickMessageStorage(
    securityId, StorageFormats.Binary);

// Order books in CSV format
var depthStorage = storageRegistry.GetQuoteMessageStorage(
    securityId, StorageFormats.Csv);
```

## See Also

- [Working with the API](api.md)
- [Storage Drives](drives.md)

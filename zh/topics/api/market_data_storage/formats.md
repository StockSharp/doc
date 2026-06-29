# 存储格式

StockSharp 支持由 [StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) 枚举定义的两种市场数据存储格式：**二进制** 和 **CSV**。每种格式都有其自身的优点，并适用于不同的使用场景。

## 二进制格式

二进制格式是 StockSharp 中主要的高性能数据存储格式。其实现位于 `Algo/Storages/Binary/` 中的 `BinaryMarketDataSerializer` 类。

### 特征

- **紧凑性** —— 数据以二进制形式序列化并压缩，确保文件体积最小。
- **性能** -- 与文本格式相比，读写速度显著更快。
- **元数据** -- `BinaryMetaInfo` 类存储辅助信息：首尾价格、分数值、时间戳。这允许在不完全读取文件的情况下快速获取一般数据信息。
- **可压缩架构** —— 该格式从零开始设计，以实现高效的流式压缩。

二进制文件的扩展名为 `.bin`。

### 支持的数据类型

二进制格式支持对所有主要市场数据类型的序列化：K线、逐笔（交易）、订单簿（Level2）、一级数据、订单以及自有交易。每种类型都有针对特定数据结构优化的专用序列化器。

## CSV格式

CSV（逗号分隔值）文本格式在位于 `Algo/Storages/Csv/` 的 `CsvMarketDataSerializer` 类中实现。

### 特征

- **可读性** —— 文件可以在任何文本编辑器中打开，或在 Excel 中进行可视化数据分析。
- **可编辑性** -- 数据在必要时可以手动更正。
- **元数据** —— `CsvMetaInfo` 类提供支持编码的元数据存储。它包含 `IncrementalOnly` 属性（增量数据支持）和 `LastId`（最后记录标识）。
- **更大的文件大小** —— 文本表示占用的磁盘空间显著更多。
- **处理速度较慢**——解析文本数据需要额外的计算资源。

CSV 文件的扩展名是 `.csv`。

## 何时使用哪种格式

| 场景 | 推荐格式 |
|----------|-------------------|
| 生产使用 | 二进制 |
| 大数据量 | 二进制 |
| 性能关键 | 二进制 |
| 调试和诊断 | CSV |
| 可视化数据分析 | CSV |
| 与外部工具的集成 | CSV |
| 手动数据更正 | CSV |

## 磁盘上的文件组织

磁盘上的数据按照以下路径结构组织：

```
{root_folder}/{first_letter}/{instrument_identifier}/{yyyy_MM_dd}/{file_name}.{extension}
```

扩展名为二进制格式时是 `.bin`，文本格式时是 `.csv`。这种分层组织确保按工具和日期快速查找数据。例如，SBER@TQBR 工具在 2024 年 4 月 1 日的 5 分钟 K 线数据的二进制格式将位于如下路径：



```
Storage/S/SBER@TQBR/2024_04_01/candles_5m.bin
```

## 格式转换

StockSharp 允许以一种格式加载数据并以另一种格式保存数据。例如，这在将二进制数据导出为 CSV 以便在外部工具中进行分析时非常有用：

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

## 代码示例

在通过 [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) 创建存储时选择格式：

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

在处理行情数据和订单簿时，也可以指定存储格式：

```cs
// Ticks in binary format
var tickStorage = storageRegistry.GetTickMessageStorage(
    securityId, StorageFormats.Binary);

// Order books in CSV format
var depthStorage = storageRegistry.GetQuoteMessageStorage(
    securityId, StorageFormats.Csv);
```

## 另请参阅

- [使用 API](api.md)
- [存储驱动器](drives.md)

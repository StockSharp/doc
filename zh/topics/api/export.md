# 数据导出

[S\#](../api.md) 实现了一个支持多种格式的市场数据导出子系统。所有导出器都继承自基类 [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter)，并支持统一的异步接口。

## 基础导出器

基础抽象类 [BaseExporter](xref:StockSharp.Algo.Export.BaseExporter) 定义了所有导出器的通用契约：

- **数据类型** — 被导出的数据类型（逐笔数据、K线、订单簿等）。
- **编码** — 编码（默认 UTF-8）。
- **Export\<T\>(IAsyncEnumerable\<T\>, CancellationToken)** — 主要的导出方法。返回 `Task<(int count, DateTime? lastTime)>` — 导出记录的数量以及最后一条记录的时间。

该方法会自动将数据路由到特定类型的处理程序，适用于：[QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage)、[Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage)、[ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)（行情、订单日志、交易）、[CandleMessage](xref:StockSharp.Messages.CandleMessage)、[NewsMessage](xref:StockSharp.Messages.NewsMessage)、[SecurityMessage](xref:StockSharp.Messages.SecurityMessage)、[PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage)、[IndicatorValue](xref:StockSharp.Messages.IndicatorValue) 和 [BoardStateMessage](xref:StockSharp.Messages.BoardStateMessage)。

## 出口商类型

### 1. 文本导出器 — CSV/文本导出

[TextExporter](xref:StockSharp.Algo.Export.TextExporter) 使用 SmartFormat 模板将数据导出为文本格式。

- **构造函数**: `(DataType dataType, Stream stream, string template, string header)`
- 模板使用 SmartFormat 语法，例如：`{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}`

```cs
await using var stream = File.Create("trades.csv");
var exporter = new TextExporter(DataType.Ticks, stream,
    "{ServerTime:default:yyyyMMdd};{TradePrice};{TradeVolume}",
    "Date;Price;Volume");

var (count, lastTime) = await exporter.Export(tickMessages, token);
```

### 2. JsonExporter — JSON 导出

[JsonExporter](xref:StockSharp.Algo.Export.JsonExporter) 以 JSON 格式保存数据。

- **构造函数**: `(DataType dataType, Stream stream)`
- **缩进** — 缩进格式（默认 `true`）。

```cs
await using var stream = File.Create("candles.json");
var exporter = new JsonExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 3. XmlExporter — XML 导出

[XmlExporter](xref:StockSharp.Algo.Export.XmlExporter) 以 XML 格式保存数据。

- **构造函数**: `(DataType dataType, Stream stream)`
- **缩进** — 缩进格式（默认 `true`）。

```cs
await using var stream = File.Create("candles.xml");
var exporter = new XmlExporter(
    DataType.CandleTimeFrame(TimeSpan.FromMinutes(5)), stream);
await exporter.Export(candleMessages, token);
```

### 4. ExcelExporter — Excel 导出

[ExcelExporter](xref:StockSharp.Algo.Export.ExcelExporter) 将数据导出到 Excel 电子表格。

- **构造函数**: `(IExcelWorkerProvider provider, DataType dataType, Stream stream, Action breaked)`
- 最大行数：1,048,576（Excel 格式限制）。

```cs
await using var stream = File.Create("data.xlsx");
var exporter = new ExcelExporter(excelProvider, DataType.Ticks, stream,
    () => { /* handle interruption */ });
await exporter.Export(tickMessages, token);
```

### 5. DatabaseExporter — 数据库导出

[DatabaseExporter](xref:StockSharp.Algo.Export.DatabaseExporter) 通过 LinqToDB 将数据保存到数据库。

- **构造函数**: `(IDatabaseProvider dbProvider, DataType dataType, DatabaseConnectionPair connection, decimal? priceStep, decimal? volumeStep)`
- **BatchSize** — 记录的批量大小（默认 50）。
- **CheckUnique** — 检查记录唯一性（默认 `false`）。
- **DropExisting** — 在导出前删除现有数据（默认 `false`）。

```cs
var exporter = new DatabaseExporter(
    dbProvider, DataType.Ticks, dbConnection)
{
    BatchSize = 100,
    CheckUnique = true
};
await exporter.Export(tickMessages, token);
```

### 6. StockSharpExporter — StockSharp 原生格式

[StockSharpExporter](xref:StockSharp.Algo.Export.StockSharpExporter) 将数据保存为内部 StockSharp 存储格式。

- **构造函数**: `(DataType dataType, IStorageRegistry storageRegistry, IMarketDataDrive drive, StorageFormats format)`
- **BatchSize** — 记录的批量大小（默认 50）。

```cs
var exporter = new StockSharpExporter(
    DataType.Ticks, storageRegistry, drive, StorageFormats.Binary);
await exporter.Export(tickMessages, token);
```

## TemplateTxtRegistry — 文本模板注册表

[TemplateTxtRegistry](xref:StockSharp.Algo.Export.TemplateTxtRegistry) 类包含用于通过 [TextExporter](xref:StockSharp.Algo.Export.TextExporter) 导出各种数据类型的预定义模板：

- **TemplateTxtTick** — 针对逐笔数据的模板。
- **TemplateTxtDepth** — 订单簿模板。
- **TemplateTxtCandle** — 蜡烛模板。
- **TemplateTxtLevel1** — Level1 数据的模板。
- **TemplateTxtOrderLog** — 订单日志模板。
- **TemplateTxtTransaction** — 交易模板。
- **TemplateTxtSecurity** — 乐器模板。
- **TemplateTxtNews** — 新闻模板。

模板可以根据需要进行自定义或替换。注册表实现了 [IPersistable](xref:Ecng.Serialization.IPersistable)，可以从设置中保存/加载。

## 另请参阅

[数据导入](import.md)

[数据存储](market_data_storage.md)

# 数据导入

[S#](../api.md) 实现了一个从 CSV 文件导入市场数据的子系统。主要类位于 `StockSharp.Algo.Import` 命名空间中。

## CsvParser — 基础解析器

[CsvParser](xref:StockSharp.Algo.Import.CsvParser) 类执行 CSV 文件解析并将行转换为 [S#](../api.md) 消息。

- **构造函数**: `(DataType dataType, IEnumerable<FieldMapping> fields)`
- **ColumnSeparator** — 列分隔符（默认 `","`）。
- **LineSeparator** — 行分隔符（默认 CRLF）。
- **SkipFromHeader** — 从文件开头跳过的行数（默认 `0`）。
- **IgnoreNonIdSecurities** — 忽略具有无法识别工具的行（默认 `true`）。
- **Parse(Stream)** — 解析方法，返回 `IAsyncEnumerable<Message>`。

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
    // 处理每条消息
}
```

## CsvImporter — 带存储节省的导入

[CsvImporter](xref:StockSharp.Algo.Import.CsvImporter) 类继承自 [CsvParser](xref:StockSharp.Algo.Import.CsvParser)，增加了自动将数据保存到市场数据存储的功能。

- **构造函数**: `(DataType dataType, IEnumerable<FieldMapping> fields, ISecurityStorage securityStorage, IExchangeInfoProvider exchangeInfoProvider, Func<SecurityId, IMarketDataStorage> getStorage)`
- **Import(Stream, Action\<int\> progress, CancellationToken)** — 执行导入并返回 `ValueTask<(int count, DateTime? lastTime)>`。
- **UpdateDuplicateSecurities** — 是否更新重复的交易品种（默认 `false`）。
- **SecurityUpdated** — 当工具被更新时触发的事件。

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
    p => Console.WriteLine($"进度: {p}%"),
    token);

Console.WriteLine($"已导入 {count} 条记录，最后一条: {lastTime}");
```

## 字段映射 — 字段描述

[FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) 类描述了 CSV 文件列与消息属性之间的映射。

主要属性：

- **名称** — 消息中的字段名称。
- **DisplayName** — 显示名称。
- **类型** — 值类型。
- **顺序** — 文件中的列索引（从 0 开始）。
- **IsRequired** — 该字段是否为必填项。
- **格式** — 解析格式 (例如， 日期格式)。
- **DefaultValue** — 默认值。
- **ZeroAsNull** — 是否将零值解释为 `null`。

对于自定义值转换，请使用 [FieldMappingValue](xref:StockSharp.Algo.Import.FieldMappingValue)。例如，您可以定义文本值到枚举的映射：

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

## FieldMappingRegistry — 标准字段注册表

静态类 [FieldMappingRegistry](xref:StockSharp.Algo.Import.FieldMappingRegistry) 提供了一种创建标准字段集的方法：

- **CreateFields(DataType)** — 返回指定数据类型的 [FieldMapping](xref:StockSharp.Algo.Import.FieldMapping) 列表。

支持的数据类型：ticks（逐笔成交）、candles（K线）、order books（订单簿）、Level1（Level1 数据）、order log（订单日志）、transactions（交易）、instruments（交易品种）、news（新闻）和 positions（持仓）。

## 导入设置 — 导入设置

[ImportSettings](xref:StockSharp.Algo.Import.ImportSettings) 类将所有导入参数组合到一个配置对象中：

- **数据类型** — 被导入的数据类型。
- **文件名** — 文件路径。
- **目录** — 用于文件搜索的目录。
- **FileMask** — 文件搜索掩码 (例如， `*.csv`)。
- **ColumnSeparator** — 列分隔符。
- **SkipFromHeader** — 要跳过的行数。
- **SelectedFields** — 选择导入的字段。
- **UpdateDuplicateSecurities** — 是否更新重复的交易品种。

辅助方法：

- **GetFiles(IFileSystem)** — 获取与掩码匹配的文件列表。
- **FillParser(CsvParser)** — 填充解析器设置。
- **FillImporter(CsvImporter)** — 填充导入器设置。

```cs
var settings = new ImportSettings
{
    DataType = DataType.Ticks,
    FileName = "trades.csv",
    ColumnSeparator = ";",
    SkipFromHeader = 1
};

var fields = FieldMappingRegistry.CreateFields(settings.DataType);
// 配置列顺序
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

## 另请参阅

[数据导出](export.md)

[数据存储](market_data_storage.md)

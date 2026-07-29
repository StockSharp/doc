# 分析脚本

[S#](../api.md) 提供分析脚本子系统，可对市场数据执行任意分析并将结果可视化。相关类位于 `StockSharp.Algo.Analytics` 命名空间。

## IAnalyticsScript — 主接口

[IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) 接口定义了一个方法：

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

参数：

- **logs** — 用于输出诊断消息的日志接收器。
- **panel** — 用于显示分析结果的面板。
- **securities** — 要分析的交易品种数组。
- **from** / **to** — 时间范围。
- **storage** — 市场数据存储注册表。
- **drive** — 数据源。
- **format** — 数据存储格式。
- `dataType` — 要分析的数据类型。
- **cancellationToken** — 取消令牌。

## IAnalyticsPanel — 结果面板

[IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel) 接口提供创建各种可视化对象的方法：

- **CreateGrid(params string[] columns)** — 创建具有指定列的表格 [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid)。
- **CreateChart\<X, Y\>()** — 创建二维图表 [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2)。
- **CreateChart\<X, Y, Z\>()** — 创建三维图表 [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`3)。
- **DrawHeatmap(string[] xTitles, string[] yTitles, double[,] data)** — 绘制热力图。
- **Draw3D(string[] xTitles, string[] yTitles, data, xTitle, yTitle, zTitle)** — 绘制三维可视化图形。

## IAnalyticsChart — 图表

[IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2) 接口提供添加数据序列的方法：

```cs
void Append(string title, IEnumerable<X> xValues, IEnumerable<Y> yValues,
    DrawStyles style, Color? color = null);
```

可用绘图样式（[DrawStyles](xref:StockSharp.Algo.Analytics.DrawStyles)）：

- `Line` — 折线图。
- `DashedLine` — 虚线图。
- `Histogram` — 直方图。
- `Bubble` — 气泡图。

## IAnalyticsGrid — 表格

[IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) 接口用于显示表格数据：

- **SetSort(string column, bool ascending)** — 设置按指定列排序。
- **SetRow(params object[] values)** — 添加一行数据。

## 内置脚本

`StockSharp.Algo.Analytics.CSharp` 包含以下现成脚本：

- **IndicatorScript** — 计算指标并将其显示在图表上。
- **ChartDrawScript** — 演示如何构建不同类型的图表。
- **PriceVolumeScript** — 分析各价格档位的成交量分布。

## 示例：自定义分析脚本

以下示例脚本会加载一组交易品种的K线，并在折线图中显示收盘价：

```cs
public class MyAnalyticsScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        // 创建二维图表
        var chart = panel.CreateChart<DateTime, decimal>();

        foreach (var secId in securities)
        {
            // 获取 K线存储
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            // 加载指定期间的数据
            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
            {
                logs.AddWarningLog($"没有 {secId} 的数据");
                continue;
            }

            // 向图表添加序列
            chart.Append(secId.ToString(),
                candles.Select(c => c.OpenTime.UtcDateTime),
                candles.Select(c => c.ClosePrice),
                DrawStyles.Line);

            logs.AddInfoLog($"{secId}: 已加载 {candles.Length} 根K线");
        }
    }
}
```

## 示例：成交量表

```cs
public class VolumeTableScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        var grid = panel.CreateGrid("Instrument", "K线总数",
            "总成交量", "平均成交量");
        grid.SetSort("总成交量", false);

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

## 另请参阅

[指标](indicators.md)

[数据存储](market_data_storage.md)

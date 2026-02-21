# Analytics Scripts

[S\#](../api.md) implements an analytics scripts subsystem that allows you to perform arbitrary analysis of market data with result visualization. The classes are located in the `StockSharp.Algo.Analytics` namespace.

## IAnalyticsScript — Main Interface

The [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) interface defines a single method:

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

Parameters:

- **logs** — log receiver for outputting diagnostic messages.
- **panel** — panel for displaying analysis results.
- **securities** — array of instruments to analyze.
- **from** / **to** — time range.
- **storage** — market data storage registry.
- **drive** — data source.
- **format** — data storage format.
- **dataType** — type of data to analyze.
- **cancellationToken** — cancellation token.

## IAnalyticsPanel — Results Panel

The [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel) interface provides methods for creating various visualizations:

- **CreateGrid(params string[] columns)** — creates a table [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) with the specified columns.
- **CreateChart\<X, Y\>()** — creates a two-dimensional chart [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2).
- **CreateChart\<X, Y, Z\>()** — creates a three-dimensional chart [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`3).
- **DrawHeatmap(string[] xTitles, string[] yTitles, double[,] data)** — draws a heatmap.
- **Draw3D(string[] xTitles, string[] yTitles, data, xTitle, yTitle, zTitle)** — draws a 3D visualization.

## IAnalyticsChart — Charts

The [IAnalyticsChart](xref:StockSharp.Algo.Analytics.IAnalyticsChart`2) interface provides a method for adding data series:

```cs
void Append(string title, IEnumerable<X> xValues, IEnumerable<Y> yValues,
    DrawStyles style, Color? color = null);
```

Available drawing styles ([DrawStyles](xref:StockSharp.Algo.Analytics.DrawStyles)):

- **Line** — line chart.
- **DashedLine** — dashed line.
- **Histogram** — histogram.
- **Bubble** — bubble chart.

## IAnalyticsGrid — Tables

The [IAnalyticsGrid](xref:StockSharp.Algo.Analytics.IAnalyticsGrid) interface allows displaying tabular data:

- **SetSort(string column, bool ascending)** — set sorting by column.
- **SetRow(params object[] values)** — add a data row.

## Built-in Scripts

The `StockSharp.Algo.Analytics.CSharp` package includes ready-made scripts:

- **IndicatorScript** — calculates and visualizes indicators on a chart.
- **ChartDrawScript** — demonstrates building various chart types.
- **PriceVolumeScript** — analyzes volume distribution across price levels.

## Example: Custom Analytics Script

Below is an example script that loads candles for a list of instruments and displays close prices on a line chart:

```cs
public class MyAnalyticsScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        // create a two-dimensional chart
        var chart = panel.CreateChart<DateTime, decimal>();

        foreach (var secId in securities)
        {
            // get candle storage
            var candleStorage = storage.GetCandleMessageStorage(
                secId, dataType, drive, format);

            // load data for the period
            var candles = await candleStorage
                .LoadAsync(from, to)
                .WithCancellation(cancellationToken)
                .ToArrayAsync(cancellationToken);

            if (candles.Length == 0)
            {
                logs.AddWarningLog($"No data for {secId}");
                continue;
            }

            // add series to the chart
            chart.Append(secId.ToString(),
                candles.Select(c => c.OpenTime.UtcDateTime),
                candles.Select(c => c.ClosePrice),
                DrawStyles.Line);

            logs.AddInfoLog($"{secId}: loaded {candles.Length} candles");
        }
    }
}
```

## Example: Volume Table

```cs
public class VolumeTableScript : IAnalyticsScript
{
    public async Task Run(ILogReceiver logs, IAnalyticsPanel panel,
        SecurityId[] securities, DateTime from, DateTime to,
        IStorageRegistry storage, IMarketDataDrive drive,
        StorageFormats format, DataType dataType,
        CancellationToken cancellationToken)
    {
        var grid = panel.CreateGrid("Instrument", "Total Candles",
            "Total Volume", "Average Volume");
        grid.SetSort("Total Volume", false);

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

## See Also

[Indicators](indicators.md)

[Data Storage](market_data_storage.md)

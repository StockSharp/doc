# 创建脚本

**Analytics** 支持创建自定义脚本。下面以 **ChartDrawScript** 为例，介绍图表绘制功能：

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// The analytic script, shows chart drawing possibilities.
	/// </summary>
	public class ChartDrawScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var lineChart = panel.CreateChart<DateTimeOffset, decimal>();
			var histogramChart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// stop calculation if user cancel script execution
				if (cancellationToken.IsCancellationRequested)
					break;

				var candlesSeries = new Dictionary<DateTimeOffset, decimal>();
				var volsSeries = new Dictionary<DateTimeOffset, decimal>();

				// get candle storage
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				foreach (var candle in candleStorage.Load(from, to))
				{
					// fill series
					candlesSeries[candle.OpenTime] = candle.ClosePrice;
					volsSeries[candle.OpenTime] = candle.TotalVolume;
				}

				// draw series on chart as line and histogram
				lineChart.Append($"{security} (close)", candlesSeries.Keys, candlesSeries.Values, DrawStyles.DashedLine);
				histogramChart.Append($"{security} (vol)", volsSeries.Keys, volsSeries.Values, DrawStyles.Histogram);
			}

			return Task.CompletedTask;
		}
	}
}

```

## 概述

该脚本根据金融交易品种在指定时间段内的价格和成交量数据绘制图表。它实现了 [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) 接口。此接口为所有可在 **Hydra** 中执行的分析脚本定义了统一约定。

## `IAnalyticsScript` 接口

[IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript) 接口要求每个实现它的分析脚本都提供 [Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)) 方法，该方法用于执行脚本的分析操作。

### `Run` 方法

[Run](xref:StockSharp.Algo.Analytics.IAnalyticsScript.Run(Ecng.Logging.ILogReceiver,StockSharp.Algo.Analytics.IAnalyticsPanel,StockSharp.Messages.SecurityId[],System.DateTime,System.DateTime,StockSharp.Algo.Storages.IStorageRegistry,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats,StockSharp.Messages.DataType,System.Threading.CancellationToken)) 方法是分析脚本的入口点，实际的数据处理和分析操作都在此执行。

#### 参数：

- `logs`：用于脚本内部日志记录的 [ILogReceiver](xref:Ecng.Logging.ILogReceiver) 实例。
- `panel`：提供 [IAnalyticsPanel](xref:StockSharp.Algo.Analytics.IAnalyticsPanel)，即用于绘制图表和显示结果的用户界面元素。
- `securities`：用于标识待分析金融交易品种的 [SecurityId](xref:StockSharp.Messages.SecurityId) 数组。
- `from`：分析数据范围的开始日期。
- `to`：分析数据范围的结束日期。
- `storage`：用于访问市场数据存储的 [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) 实例。
- `drive`：表示 [IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive)，用于指定市场数据存储位置。
- `format`：[StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) 值，表示市场数据格式。
- `dataType`：[DataType](xref:StockSharp.Messages.DataType)，描述请求的市场数据类型及其参数，例如K线时间周期。
- `cancellationToken`：[CancellationToken](xref:System.Threading.CancellationToken)，用于监视取消请求。

#### 返回值：

- [Task](xref:System.Threading.Tasks.Task)，表示分析脚本的异步操作。

## 实现细节

`ChartDrawScript` 类分别处理每个传入交易品种的市场数据。它会创建两种图表：用于显示收盘价的折线图，以及用于显示成交量数据的直方图。

### 主要处理阶段：

1. 检查是否存在待处理的交易品种。如果没有交易品种，则记录警告并结束任务。
2. 使用 [IAnalyticsPanel.CreateChart](xref:StockSharp.Algo.Analytics.IAnalyticsPanel.CreateChart``2) 方法创建折线图和直方图。
3. 遍历各个交易品种，并检查是否收到取消请求。
4. 使用 `storage.GetCandleMessageStorage` 方法获取K线数据存储。
5. 加载指定日期范围内的K线数据。
6. 以开盘时间为键，将对应的收盘价和总成交量填入字典。
7. 使用 `lineChart.Append` 和 `histogramChart.Append` 方法在图表中绘制数据序列。

脚本使用不同样式直观地区分数据的显示方式：折线图使用 [DrawStyles.DashedLine](xref:Ecng.Drawing.DrawStyles.DashedLine)，直方图使用 [DrawStyles.Histogram](xref:Ecng.Drawing.DrawStyles.Histogram)。

通过实现 [IAnalyticsScript](xref:StockSharp.Algo.Analytics.IAnalyticsScript)，`ChartDrawScript` 类可集成到可自定义分析脚本的执行机制中，为使用 StockSharp 平台的交易者和分析人员提供灵活的分析工具。

## 执行结果

![hydra_analytics_chart](../../../images/hydra_analytics_chart.png)

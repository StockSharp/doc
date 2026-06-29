# 指标

`Indicator` 脚本用于演示如何在 StockSharp 平台中使用技术分析指标。用户可以加载历史数据、对其应用各种指标，并在图表中显示结果。这种方式有助于分析市场趋势并作出有依据的交易决策。

![hydra_analytics_indicator](../../../../images/hydra_analytics_indicator.png)

## 功能

脚本提供以下功能：

- **加载历史数据**：选择需要分析的证券，并加载其指定时间段内的历史数据。
- **应用指标**：对加载的数据应用一个或多个技术分析指标。
- **可视化**：在图表中显示数据及指标分析结果，以直观呈现市场动态。

## 指标示例

脚本支持多种指标，包括但不限于：

- **移动平均线（MA）**：表示特定时间段内的平均价格，有助于识别趋势。
- **相对强弱指数（RSI）**：评估价格变化的幅度和速度，有助于识别超买或超卖状态。
- **布林带（BB）**：基于移动平均线和标准差显示价格区间及波动性。

## 在交易和分析中的应用

通过该脚本使用技术分析指标，可以：

- **识别趋势**：判断市场运行方向，以规划入场和出场策略。
- **识别反转点**：确定市场趋势可能改变方向的时点。
- **分析波动性**：评估价格波动程度，使策略适应市场状况。

## 脚本实现

使用该脚本需要执行以下步骤：

1. **选择证券和时间段**：确定要分析的证券及时间范围。
2. **应用指标**：选择指标并设置其参数，然后将其应用于数据。
3. **显示结果**：在图表中显示历史数据和指标，以便进行分析。

`Indicator` 脚本为深入分析金融市场提供了有力工具，使交易者和分析人员能够利用这些指标制定有效的交易策略。

## C# 脚本代码

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// The analytic script, using indicator ROC.
	/// </summary>
	public class IndicatorScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			// creating 2 panes for candles and indicator series
			var candleChart = panel.CreateChart<DateTimeOffset, decimal>();
			var indicatorChart = panel.CreateChart<DateTimeOffset, decimal>();

			foreach (var security in securities)
			{
				// stop calculation if user cancel script execution
				if (cancellationToken.IsCancellationRequested)
					break;

				var candlesSeries = new Dictionary<DateTimeOffset, decimal>();
				var indicatorSeries = new Dictionary<DateTimeOffset, decimal>();

				// creating ROC
				var roc = new RateOfChange();

				// get candle storage
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				foreach (var candle in candleStorage.Load(from, to))
				{
					// fill series
					candlesSeries[candle.OpenTime] = candle.ClosePrice;
					indicatorSeries[candle.OpenTime] = roc.Process(candle).ToDecimal();
				}

				// draw series on chart
				candleChart.Append($"{security} (close)", candlesSeries.Keys, candlesSeries.Values);
				indicatorChart.Append($"{security} (ROC)", indicatorSeries.Keys, indicatorSeries.Values);
			}

			return Task.CompletedTask;
		}
	}
}

```

## Python 脚本代码

```python
import clr

# Add .NET references
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from StockSharp.Algo.Indicators import ROC
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# The analytic script, using indicator ROC.
class indicator_script(IAnalyticsScript):
	def Run(self, logs, panel, securities, from_date, to_date, storage, drive, format, data_type, cancellation_token):
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		# creating 2 panes for candles and indicator series
		candle_chart = create_chart(panel, datetime, float)
		indicator_chart = create_chart(panel, datetime, float)

		if data_type is None:
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# stop calculation if user cancel script execution
			if cancellation_token.IsCancellationRequested:
				break

			candles_series = {}
			indicator_series = {}

			# creating ROC
			roc = ROC()

			# get candle storage
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			for candle in load_range(candle_storage, message_type, from_date, to_date):
				# fill series
				candles_series[candle.OpenTime] = candle.ClosePrice
				indicator_series[candle.OpenTime] = to_decimal(process_candle(roc, candle))

			# draw series on chart
			candle_chart.Append(
				f"{security} (close)",
				list(candles_series.keys()),
				list(candles_series.values())
			)
			indicator_chart.Append(
				f"{security} (ROC)",
				list(indicator_series.keys()),
				list(indicator_series.values())
			)

		return Task.CompletedTask

```

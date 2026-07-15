# 日内成交量

“日内成交量”脚本用于分析单个交易时段内交易品种成交量的逐小时分布。该脚本面向使用 StockSharp 平台的交易者和量化分析人员，可帮助他们深入研究市场行为并优化交易策略。

![日内成交量](../../../../images/hydra_analytics_intraday_volume.png)

## 功能描述

脚本收集所选时间段内的交易数据，并以图表形式显示，使用户能够直观看到每小时成交量的变化，由此判断一天中哪些时段的交易活动较强或较弱。

## 实际意义

- **交易方面**：了解高峰和低谷时段有助于确定市场最活跃的时间，从而为入场和出场决策提供依据。
- **量化分析方面**：量化分析人员可以利用日内成交量数据建立数学模型和算法，根据成交量指标预测市场行为。

## 每小时分布

逐小时成交量分布可以揭示市场动态，并突出主要交易活动集中的时间区间。这些信息可能反映趋势变化、支撑位和阻力位，以及流动性可能增加或不足的时点。

## 数据应用

“日内成交量”脚本可以集成到更完整的市场分析系统中，提供可用于以下方面的数据：

- **策略调整**：根据市场活跃程度调整交易算法的参数。
- **风险评估**：根据一天中的不同时间计算价格大幅波动的概率。

在 StockSharp 交易平台中使用“日内成交量”脚本，交易者和分析人员可以依据具体的市场活跃度数据作出决策，并调整策略，使其更好地适应当前交易环境。

## C# 脚本代码

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// 按小时计算最大成交量分布的分析脚本。
	/// </summary>
	public class TimeVolumeScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("没有交易品种。");
				return Task.CompletedTask;
			}

			// 脚本只能处理 1 个交易品种
			var security = securities.First();

			// 获取 K线存储
			var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

			// 获取指定期间内的可用日期
			var dates = candleStorage.GetDates(from, to).ToArray();

			if (dates.Length == 0)
			{
				logs.LogWarning("没有数据。");
				return Task.CompletedTask;
			}

			// 按开盘时间对 K线分组（仅时间部分，截断到 1 小时）
			var rows = candleStorage.Load(from, to)
				.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
				.ToDictionary(g => g.Key, g => g.Sum(c => c.TotalVolume));

			// 将计算结果放入表格
			var grid = panel.CreateGrid("时间", "成交量");

			foreach (var row in rows)
				grid.SetRow(row.Key, row.Value);

			// 按成交量列降序排序
			grid.SetSort("成交量", false);

			return Task.CompletedTask;
		}
	}
}

```

## Python 脚本代码

```python
import clr

# 添加 .NET 引用
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("StockSharp.Messages")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System import TimeSpan
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *

# 按小时计算最大成交量分布的分析脚本。
class time_volume_script(IAnalyticsScript):
	def Run(
		self,
		logs,
		panel,
		securities,
		from_date,
		to_date,
		storage,
		drive,
		format,
		data_type,
		cancellation_token
	):
		# 检查是否 没有交易品种
		if not securities:
			logs.LogWarning("没有交易品种。")
			return Task.CompletedTask

		# 脚本只能处理 1 个交易品种
		security = securities[0]

		if data_type is None:
			logs.LogWarning(f"不支持的数据类型 {data_type}。")
			return Task.CompletedTask

		message_type = data_type.MessageType

		# 获取K线存储
		candle_storage = get_candle_storage(storage, security, data_type, drive, format)

		# 获取指定期间内的可用日期
		dates = get_dates(candle_storage, from_date, to_date)

		if len(dates) == 0:
			logs.LogWarning("没有数据。")
			return Task.CompletedTask

		# 按开盘时间对 K线分组（按小时截断）并汇总成交量
		candles = load_range(candle_storage, message_type, from_date, to_date)
		rows = {}
		for candle in candles:
			time_of_day = candle.OpenTime.TimeOfDay
			truncated = TimeSpan.FromHours(int(time_of_day.TotalHours))
			rows[truncated] = rows.get(truncated, 0) + candle.TotalVolume

		# 将计算结果放入表格
		grid = panel.CreateGrid("时间", "成交量")

		for key, value in rows.items():
			grid.SetRow(key, value)

		# 按 Volume 列降序排序
		grid.SetSort("成交量", False)

		return Task.CompletedTask

```

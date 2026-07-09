# 3D 图表

`Chart3DScript` 脚本演示如何创建 3D 图表，以可视化不同交易品种按小时统计的成交量分布。这种可视化方式可以清楚地呈现交易动态，并识别市场活跃度的高峰。

![hydra_analytics_chart3d](../../../../images/hydra_analytics_chart3d.png)

## 脚本运行说明

脚本分析指定时间段内的K线数据，按小时分组，并计算每小时的总成交量。结果以 3D 图表显示，各坐标轴分别表示：

- **X 轴**：交易品种。
- **Y 轴**：交易时段中的小时（0 至 23）。
- **Z 轴**：成交量。

## 3D 图表的用途

### 市场活动分析

3D 图表可以评估多个交易品种在什么时段同时最为活跃。这有助于确定最佳交易时段，或研究全球事件对市场的影响。

### 交易品种比较

通过在三维空间中按小时显示成交量，交易者可以比较各交易品种的活跃程度和主要交易时段。这有助于选择特定时段内流动性最高的交易品种，或寻找活跃模式相似的交易品种，以实现投资组合多元化。

### 策略优化

分析成交量分布可以为优化交易策略提供依据，使策略适应市场最活跃的时段。这对算法交易和高频交易尤其重要。

## 脚本实现

脚本执行以下操作：

1. 检查是否存在用于分析的交易品种。
2. 生成 X 轴（交易品种）和 Y 轴（小时）的标签。
3. 加载K线数据并进行分组。
4. 按小时计算总成交量，并填充 Z 轴数据。
5. 使用 `panel.Draw3D` 方法绘制 3D 图表。

## C# 脚本代码

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// 该分析脚本按小时计算最大成交量分布
	/// 并在 3D 图表中显示。
	/// </summary>
	public class Chart3DScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var x = new List<string>();
			var y = new List<string>();

			// 填充 Y 标签
			for (var h = 0; h < 24; h++)
				y.Add(h.ToString());

			var z = new double[securities.Length, y.Count];

			for (var i = 0; i < securities.Length; i++)
			{
				// 如果用户取消脚本执行，则停止计算
				if (cancellationToken.IsCancellationRequested)
					break;

				var security = securities[i];

				// 填充 X 标签
				x.Add(security.ToStringId());

				// 获取 K线存储
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// 获取指定期间内的可用日期
				var dates = candleStorage.GetDates(from, to).ToArray();

				if (dates.Length == 0)
				{
					logs.LogWarning("no data");
					return Task.CompletedTask;
				}

				// grouping candles by opening time (time part only) with 1 hour truncating
				var byHours = candleStorage.Load(from, to)
					.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
					.ToDictionary(g => g.Key.Hours, g => g.Sum(c => c.TotalVolume));

				// 填充 Z 值
				foreach (var pair in byHours)
					z[i, pair.Key] = (double)pair.Value;
			}

			panel.Draw3D(x, y, z, "Instruments", "Hours", "Volume");

			return Task.CompletedTask;
		}
	}
}

```

## Python 脚本代码

```python
import clr

# 添加 .NET 引用
clr.AddReference("StockSharp.Messages")
clr.AddReference("StockSharp.Algo.Analytics")
clr.AddReference("Ecng.Drawing")

from Ecng.Drawing import DrawStyles
from System.Threading.Tasks import Task
from StockSharp.Algo.Analytics import IAnalyticsScript
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from numpy_extensions import nx

# 该分析脚本按小时计算最大成交量分布，并在 3D 图表中显示。
class chart3d_script(IAnalyticsScript):
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
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		x = []  # X labels for instruments
		y = []  # Y labels for hours

		# 用 0 到 23 小时填充 Y 标签
		for h in range(24):
			y.append(str(h))

		# 为 Z 值创建二维数组，维度为：（交易品种数量）x（小时数）
		z = [[0.0 for _ in range(len(y))] for _ in range(len(securities))]

		if data_type is None:
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for i, security in enumerate(securities):
			# 如果用户取消脚本执行，则停止计算
			if cancellation_token.IsCancellationRequested:
				break

			# 用工具标识符填充 X 标签
			x.append(to_string_id(security))

			# 获取当前工具的K线存储
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# 获取指定期间内的可用日期
			dates = get_dates(candle_storage, from_date, to_date)

			if len(dates) == 0:
				logs.LogWarning("no data")
				return Task.CompletedTask

			# Grouping candles by opening time (truncated to the nearest hour) and summing volumes
			candles = load_range(candle_storage, message_type, from_date, to_date)
			by_hours = {}
			for candle in candles:
				hour = int(candle.OpenTime.TimeOfDay.TotalHours)
				by_hours[hour] = by_hours.get(hour, 0) + candle.TotalVolume

			# 填充当前工具的 Z 值
			for hour, volume in by_hours.items():
				if hour < len(y):
					z[i][hour] = float(volume)

		# 使用面板绘制 3D 图表
		panel.Draw3D(x, y, nx.to2darray(z), "Instruments", "Hours", "Volume")

		return Task.CompletedTask

```

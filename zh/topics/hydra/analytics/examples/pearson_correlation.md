# 皮尔逊相关系数

皮尔逊相关系数是一种衡量两个定量变量之间线性关系程度的统计方法。在金融分析中，该方法广泛用于研究股票、货币对等不同资产之间的关系。

![hydra_analytics_pearson_correlation](../../../../images/hydra_analytics_pearson_correlation.png)

## 方法描述

皮尔逊相关系数的取值范围为 -1 到 1，其中：

- **1** 表示完全正相关，
- **0** 表示没有线性关系，
- **-1** 表示完全负相关。

系数值表示两个变量之间线性关系的紧密程度。

## 实际应用

- **投资组合管理**：评估资产之间的相关性有助于构建多元化投资组合，从而降低风险。
- **对冲策略**：识别高度负相关的资产，可用于制定对冲策略。
- **市场趋势分析**：研究不同市场和证券之间的相关性，有助于了解全球经济之间的相互联系。

## 计算皮尔逊相关系数

皮尔逊相关系数的计算公式如下：

\[ r = \frac{n(\sum xy) - (\sum x)(\sum y)}{\sqrt{[n\sum x^2 - (\sum x)^2][n\sum y^2 - (\sum y)^2]}} \]

其中：
- \(n\) 是观测值数量，
- \(x\) 和 \(y\) 是变量值，
- \(\sum\) 表示求和。

## 脚本实现

计算皮尔逊相关系数的脚本应包含以下步骤：

1. **收集数据**：加载待分析的两个变量的时间序列。
2. **预处理**：按日期对齐时间序列，并删除缺失值。
3. **计算**：对处理后的数据应用皮尔逊相关系数公式。
4. **分析结果**：解释得到的相关系数，为决策提供依据。

皮尔逊相关系数为市场分析和投资策略优化提供重要信息，使分析人员能够评估金融资产之间相互影响的程度。

## C# 脚本代码

```cs
namespace StockSharp.Algo.Analytics
{
	using MathNet.Numerics.Statistics;

	/// <summary>
	/// The analytic script, calculating Pearson correlation by specified securities.
	/// </summary>
	public class PearsonCorrelationScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			var closes = new List<double[]>();

			foreach (var security in securities)
			{
				// stop calculation if user cancel script execution
				if (cancellationToken.IsCancellationRequested)
					break;

				// get candle storage
				var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

				// get closing prices
				var prices = candleStorage.Load(from, to).Select(c => (double)c.ClosePrice).ToArray();

				if (prices.Length == 0)
				{
					logs.LogWarning("No data for {0}", security);
					return Task.CompletedTask;
				}

				closes.Add(prices);
			}

			// all array must be same length, so truncate longer
			var min = closes.Select(arr => arr.Length).Min();

			for (var i = 0; i < closes.Count; i++)
			{
				var arr = closes[i];

				if (arr.Length > min)
					closes[i] = arr.Take(min).ToArray();
			}

			// calculating correlation
			var matrix = Correlation.PearsonMatrix(closes);

			// displaying result into heatmap
			var ids = securities.Select(s => s.ToStringId());
			panel.DrawHeatmap(ids, ids, matrix.ToArray());

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
from storage_extensions import *
from candle_extensions import *
from chart_extensions import *
from indicator_extensions import *
from numpy_extensions import nx

clr.AddReference("NumpyDotNet")
from NumpyDotNet import np

# The analytic script, calculating Pearson correlation by specified securities.
class pearson_correlation_script(IAnalyticsScript):
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
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		closes = []

		if data_type is None:
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		for security in securities:
			# stop calculation if user cancel script execution
			if cancellation_token.IsCancellationRequested:
				break

			# get candle storage
			candle_storage = get_candle_storage(storage, security, data_type, drive, format)

			# get closing prices
			prices = [float(c.ClosePrice) for c in load_range(candle_storage, message_type, from_date, to_date)]

			if len(prices) == 0:
				logs.LogWarning("No data for {0}", security)
				return Task.CompletedTask

			closes.append(prices)

		# all arrays must be the same length, so truncate longer ones
		min_length = min(len(arr) for arr in closes)
		closes = [arr[:min_length] for arr in closes]

		# convert list or array into 2D array
		array2d = nx.to2darray(closes)

		# calculating correlation using NumSharp
		np_array = np.array(array2d)
		matrix = np.corrcoef(np_array)

		# displaying result into heatmap
		ids = [to_string_id(s) for s in securities]
		panel.DrawHeatmap(ids, ids, nx.tosystemarray(matrix))

		return Task.CompletedTask

```

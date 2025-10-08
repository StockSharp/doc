# Intraday Volume

The "Intraday Volume" script is a tool for analyzing the distribution of trading volume of securities by hours within a single trading session. Designed for use within the StockSharp platform, it targets traders and quantitative analysts aiming for an in-depth study of market behavior and trading strategy optimization.

![hydra_analytics_intraday_volume](../../../../images/hydra_analytics_intraday_volume.png)

## Functional Description

The script collects data on trading operations for a selected time period and presents it in a graph format, allowing users to visualize changes in trading volume by hour. This provides the ability to assess which hours of the day witness increased or decreased trading activity.

## Practical Significance

- **For Trading**: Understanding peak and off-peak hours helps identify the most active market periods, influencing decisions on when to enter or exit positions.
- **For Quantitative Analysis**: Quant analysts can use intraday volume data to create mathematical models and algorithms predicting market behavior based on volume indicators.

## Hourly Distribution

The distribution of trading volume by hour sheds light on market dynamics, highlighting the time intervals with the main trading activity. This may indicate changes in trends, support and resistance levels, as well as potential moments of increased liquidity or its shortage.

## Data Application

The "Intraday Volume" script can be integrated into a broader market analysis system, providing data that can be used for:

- **Strategy Adaptation**: Adjusting trading algorithm parameters according to market activity levels.
- **Risk Assessment**: Calculating the probability of significant price movements depending on the time of day.

Using the "Intraday Volume" script within the StockSharp trading platform allows traders and analysts to base their decisions on specific data regarding market activity and adapt their strategies to match current trading conditions optimally.

## Script Code on C#

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// The analytic script, calculating distribution of the biggest volume by hours.
	/// </summary>
	public class TimeVolumeScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, DataType dataType, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.LogWarning("No instruments.");
				return Task.CompletedTask;
			}

			// script can process only 1 instrument
			var security = securities.First();

			// get candle storage
			var candleStorage = storage.GetCandleMessageStorage(security, dataType, drive, format);

			// get available dates for the specified period
			var dates = candleStorage.GetDates(from, to).ToArray();

			if (dates.Length == 0)
			{
				logs.LogWarning("no data");
				return Task.CompletedTask;
			}

			// grouping candles by opening time (time part only) with 1 hour truncating
			var rows = candleStorage.Load(from, to)
				.GroupBy(c => c.OpenTime.TimeOfDay.Truncate(TimeSpan.FromHours(1)))
				.ToDictionary(g => g.Key, g => g.Sum(c => c.TotalVolume));

			// put our calculations into grid
			var grid = panel.CreateGrid("Time", "Volume");

			foreach (var row in rows)
				grid.SetRow(row.Key, row.Value);

			// sorting by volume column (descending)
			grid.SetSort("Volume", false);

			return Task.CompletedTask;
		}
	}
}

```

## Script Code on Python

```python
import clr

# Add .NET references
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

# The analytic script, calculating distribution of the biggest volume by hours.
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
		# Check if there are no instruments
		if not securities:
			logs.LogWarning("No instruments.")
			return Task.CompletedTask

		# Script can process only 1 instrument
		security = securities[0]

		if data_type is None:
			logs.LogWarning(f"Unsupported data type {data_type}.")
			return Task.CompletedTask

		message_type = data_type.MessageType

		# Get candle storage
		candle_storage = get_candle_storage(storage, security, data_type, drive, format)

		# Get available dates for the specified period
		dates = get_dates(candle_storage, from_date, to_date)

		if len(dates) == 0:
			logs.LogWarning("no data")
			return Task.CompletedTask

		# Grouping candles by opening time (hourly truncation) and summing their volumes
		candles = load_range(candle_storage, message_type, from_date, to_date)
		rows = {}
		for candle in candles:
			time_of_day = candle.OpenTime.TimeOfDay
			truncated = TimeSpan.FromHours(int(time_of_day.TotalHours))
			rows[truncated] = rows.get(truncated, 0) + candle.TotalVolume

		# Put our calculations into grid
		grid = panel.CreateGrid("Time", "Volume")

		for key, value in rows.items():
			grid.SetRow(key, value)

		# Sorting by Volume column in descending order
		grid.SetSort("Volume", False)

		return Task.CompletedTask

```

# Largest Candles

The "Largest Candles" script is designed to identify candles with the maximum volume and the greatest body length on the charts of selected financial instruments over a given period. This tool allows traders and analysts to identify significant market events and the reaction of market participants.

![hydra_analitics_big_candle](../../../../images/hydra_analitics_big_candle.png)

## Key Features

The script analyzes a set of specified instruments, searches among them for candles with the greatest volume and body length, and displays this data on two graphs:

- **Candle Body Length Chart**: Shows candles with the greatest difference between the opening and closing price.
- **Trading Volume Chart**: Demonstrates candles with the maximum trading volume for the existence time of the candle.

## Workflow

1. **Selection of Instruments and Analysis Period**: Determines the list of instruments and the time interval for analysis.
2. **Data Analysis**: Involves loading and analyzing historical candle data to identify candles with the greatest indicators.
3. **Visualization of Results**: Found candles are displayed on charts in the analytics panel interface.

## Application

- **Market Activity Analysis**: Helps to determine moments of the greatest trader activity and potential market reversals.
- **Key Levels Identification**: Candles with significant volume and body length often form around key support and resistance levels.
- **Strategic Planning**: Information about the largest candles can be used for planning entry and exit points from the market, considering potential volatility.

## Script Code

```cs
namespace StockSharp.Algo.Analytics
{
	/// <summary>
	/// The analytic script, shows biggest candle (by volume and by length) for specified securities.
	/// </summary>
	public class BiggestCandleScript : IAnalyticsScript
	{
		Task IAnalyticsScript.Run(ILogReceiver logs, IAnalyticsPanel panel, SecurityId[] securities, DateTime from, DateTime to, IStorageRegistry storage, IMarketDataDrive drive, StorageFormats format, TimeSpan timeFrame, CancellationToken cancellationToken)
		{
			if (securities.Length == 0)
			{
				logs.AddWarningLog("No instruments.");
				return Task.CompletedTask;
			}

			var priceChart = panel.CreateChart<DateTimeOffset, decimal, decimal>();
			var volChart = panel.CreateChart<DateTimeOffset, decimal, decimal>();

			var bigPriceCandles = new List<CandleMessage>();
			var bigVolCandles = new List<CandleMessage>();

			foreach (var security in securities)
			{
				// stop calculation if user cancel script execution
				if (cancellationToken.IsCancellationRequested)
					break;

				// get candle storage
				var candleStorage = storage.GetTimeFrameCandleMessageStorage(security, timeFrame, drive, format);

				var allCandles = candleStorage.Load(from, to).ToArray();

				// first orders by volume desc will be our biggest candle
				var bigPriceCandle = allCandles.OrderByDescending(c => c.GetLength()).FirstOrDefault();
				var bigVolCandle = allCandles.OrderByDescending(c => c.TotalVolume).FirstOrDefault();

				if (bigPriceCandle != null)
					bigPriceCandles.Add(bigPriceCandle);

				if (bigVolCandle != null)
					bigVolCandles.Add(bigVolCandle);
			}

			// draw series on chart
			priceChart.Append("prices", bigPriceCandles.Select(c => c.OpenTime), bigPriceCandles.Select(c => c.GetMiddlePrice(null)), bigPriceCandles.Select(c => c.GetLength()));
			volChart.Append("prices", bigVolCandles.Select(c => c.OpenTime), bigPriceCandles.Select(c => c.GetMiddlePrice(null)), bigVolCandles.Select(c => c.TotalVolume));

			return Task.CompletedTask;
		}
	}
}
```
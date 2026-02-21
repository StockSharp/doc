# Rule for a Single Candle

## Overview

`SimpleCandleRulesStrategy` is a strategy that demonstrates the use of rules for candles in StockSharp. It tracks candle volumes and logs information when certain conditions are met.

## Main Components

```cs
// Main components
public class SimpleCandleRulesStrategy : Strategy
{
}
```

## OnStarted Method

Called when the strategy starts:

- Initializes a subscription to 5-minute candles
- Establishes rules for processing candles

```cs
// OnStarted method
protected override void OnStarted2(DateTime time)
{
	var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security)
	{
		// ready-to-use candles much faster than compression on fly mode
		// turn off compression to boost optimizer (!!! make sure you have candles)

		//MarketData =
		//{
		//    BuildMode = MarketDataBuildModes.Build,
		//    BuildFrom = DataType.Ticks,
		//}
	};
	Subscribe(subscription);

	var i = 0;
	var diff = "10%".ToUnit();

	this.WhenCandlesStarted(subscription)
		.Do((candle) =>
		{
			i++;

			this
				.WhenTotalVolumeMore(candle, diff)
				.Do((candle1) =>
				{
					LogInfo($"The rule WhenCandlesStarted and WhenTotalVolumeMore candle={candle1}");
					LogInfo($"The rule WhenCandlesStarted and WhenTotalVolumeMore i={i}");
				})
				.Once().Apply(this);

		}).Apply(this);

	base.OnStarted2(time);
}
```

## Logic

- The strategy subscribes to 5-minute candles
- When each candle begins forming, a rule is established
- The rule triggers when the total volume of the candle exceeds 10% (using a percentage value)
- When the rule is triggered, information about the candle and counter is added to the log
- After the first trigger, the rule stops working thanks to the `Once()` method

## Features

- Demonstrates the use of the `WhenCandlesStarted` and `WhenTotalVolumeMore` rules
- Uses the candle subscription mechanism
- Shows an example of creating a percentage value through `"10%".ToUnit()`
- Shows an example of logging information in a strategy using the `LogInfo` method
- Contains commented code for configuring candle building from ticks
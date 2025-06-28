# Rules for Tick Trades

## Overview

`SimpleTradeRulesStrategy` is a strategy that demonstrates the use of combined rules for analyzing trade prices in StockSharp. It subscribes to trades and creates a rule that triggers under certain price conditions.

## Main Components

```cs
// Main components
public class SimpleTradeRulesStrategy : Strategy
{
}
```

## OnStarted Method

Called when the strategy starts:

- Creates a subscription to ticks
- Creates a combined rule for analyzing trade prices

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
	var sub = new Subscription(DataType.Ticks, Security);

	sub.WhenTickTradeReceived(this).Do(t =>
	{
		sub
			.WhenLastTradePriceMore(this, t.Price + 2)
			.Or(sub.WhenLastTradePriceLess(this, t.Price - 2))
			.Do(t =>
			{
				LogInfo($"The rule WhenLastTradePriceMore Or WhenLastTradePriceLess tick={t}");
			})
			.Apply(this);
	})
	.Once() // call this rule only once
	.Apply(this);

	// Sending request for subscribe to market data.
	Subscribe(sub);

	base.OnStarted(time);
}
```

## Logic

- When the first tick is received, a combined rule is created
- It is based on the price of the received tick: creates a rule that triggers when the price changes by +/- 2
- The rule triggers when the price of the last trade becomes more than current + 2 or less than current - 2
- When the rule triggers, information about the tick is added to the log
- The outer rule triggers only once (`Once()`)

## Features

- Demonstrates creating combined rules using `Or()`
- Uses `WhenLastTradePriceMore` and `WhenLastTradePriceLess` for price analysis
- Shows an example of logging information about trades using the `LogInfo` method
- Illustrates the use of `Once()` to limit rule triggering
- Passes the tick parameter to the event handler (unlike the example in the documentation)
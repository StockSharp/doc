# Until Rule

## Overview

`SimpleRulesUntilStrategy` is a strategy that demonstrates the use of a rule with a termination condition (`Until`) in StockSharp. It subscribes to trades and order books, and then establishes a rule that executes until a certain condition is met.

## Main Components

```cs
// Main components
public class SimpleRulesUntilStrategy : Strategy
{
}
```

## OnStarted Method

Called when the strategy starts:

- Creates subscriptions to ticks and order books
- Creates a rule that executes when order book data is received until a certain condition is met

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	var i = 0;
	mdSub.WhenOrderBookReceived(this).Do(depth =>
	{
		i++;
		LogInfo($"The rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
		LogInfo($"The rule WhenOrderBookReceived i={i}");
	})
	.Until(() => i >= 10)
	.Apply(this);

	// Sending requests for subscribe to market data.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted(time);
}
```

## Logic

- When launched, the strategy creates subscriptions to ticks and order books
- A rule is created that triggers each time order book data is received
- When the rule triggers:
  - The counter `i` is incremented
  - Information about the best bid and ask prices is added to the log
  - The current value of the counter `i` is added to the log
- The rule executes until the value of the counter `i` reaches or exceeds 10
- After the condition is met, the rule automatically stops working

## Features

- Demonstrates the use of the `Until()` method to limit rule execution
- Uses subscription to trades and order books
- Shows an example of logging information about the order book and counter state using the `LogInfo` method
- Illustrates how to limit the number of rule executions based on a specific condition
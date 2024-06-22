# Until Rule

## Overview

`SimpleRulesUntilStrategy` is a strategy that demonstrates the use of a rule with a termination condition (`Until`) in StockSharp. It subscribes to trades and order book, and then sets up a rule that executes until a certain condition is met.

## Main Components

```cs
// Main components
public class SimpleRulesUntilStrategy : Strategy
{
}
```

## OnStarted Method

Called when the strategy starts:

- Subscribes to trades and order book
- Creates a rule that executes when order book data is received until a certain condition is met

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    var tickSub = this.SubscribeTrades(Security);

    var mdSub = this.SubscribeMarketDepth(Security);

    var i = 0;
    mdSub.WhenOrderBookReceived(this).Do(depth =>
    {
        i++;
        this.AddInfoLog($"The rule WhenOrderBookReceived BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
        this.AddInfoLog($"The rule WhenOrderBookReceived i={i}");
    })
    .Until(() => i >= 10)
    .Apply(this);

    base.OnStarted(time);
}
```

## Working Logic

- When launched, the strategy subscribes to trades and order book
- A rule is created that triggers each time order book data is received
- When the rule triggers:
  - The counter `i` is incremented
  - Information about the best bid and ask prices is added to the log
  - The current value of the counter `i` is added to the log
- The rule executes until the value of the counter `i` reaches or exceeds 10

## Features

- Demonstrates the use of the `Until()` method to limit rule execution
- Uses subscription to trades and order book
- Shows an example of logging information about the order book and counter state
- Illustrates how to limit the number of rule executions based on a specific condition
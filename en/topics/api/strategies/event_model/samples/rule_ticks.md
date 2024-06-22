# Tick Trade Rules Strategy

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

- Subscribes to trades
- Creates a combined rule for analyzing trade prices

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    var sub = this.SubscribeTrades(Security);

    sub.WhenTickTradeReceived(this).Do(() =>
    {
        new IMarketRule[] { Security.WhenLastTradePriceMore(this, 2), Security.WhenLastTradePriceLess(this, 2) }
            .Or() // or conditions (WhenLastTradePriceMore or WhenLastTradePriceLess)
            .Do(() =>
            {
                this.AddInfoLog($"The rule WhenLastTradePriceMore Or WhenLastTradePriceLess candle={Security.LastTick}");
            })
            .Apply(this);
    })
    .Once() // call this rule only once
    .Apply(this);

    base.OnStarted(time);
}
```

## Working Logic

- A combined rule is created when the first tick is received
- The rule triggers when the last trade price becomes greater than 2 or less than 2
- When the rule triggers, information about the last tick is added to the log
- The outer rule triggers only once (`Once()`)

## Features

- Demonstrates the creation of combined rules using `Or()`
- Uses `WhenLastTradePriceMore` and `WhenLastTradePriceLess` for price analysis
- Shows an example of logging information about trades
- Illustrates the use of `Once()` to limit rule triggering
- The rule is created only once when the first tick is received
# Order Book and Trade Rules Strategy

## Overview

`SimpleRulesStrategy` is a strategy that demonstrates various ways of creating and applying rules in StockSharp. It subscribes to trades and order book, and then sets up various rules for processing the received data.

## Main Components

```cs
// Main components
public class SimpleRulesStrategy : Strategy
{
}
```

## OnStarted Method

Called when the strategy starts:

- Subscribes to trades and order book
- Demonstrates various ways of creating and applying rules

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    var tickSub = this.SubscribeTrades(Security);

    var mdSub = this.SubscribeMarketDepth(Security);

    //-----------------------Create a rule. Method №1-----------------------------------
    mdSub.WhenOrderBookReceived(this).Do((depth) =>
    {
        this.AddInfoLog($"The rule WhenOrderBookReceived №1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
    }).Once().Apply(this);

    //-----------------------Create a rule. Method №2-----------------------------------
    var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

    whenMarketDepthChanged.Do((depth) =>
    {
        this.AddInfoLog($"The rule WhenOrderBookReceived №2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
    }).Once().Apply(this);

    //----------------------Rule inside rule-----------------------------------
    mdSub.WhenOrderBookReceived(this).Do((depth) =>
    {
        this.AddInfoLog($"The rule WhenOrderBookReceived №3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

        //----------------------not a Once rule-----------------------------------
        mdSub.WhenOrderBookReceived(this).Do((depth1) =>
        {
            this.AddInfoLog($"The rule WhenOrderBookReceived №4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
        }).Apply(this);
    }).Once().Apply(this);

    base.OnStarted(time);
}
```

## Working Logic

### Method №1: Creating a Rule

- Creates a rule that triggers when an order book is received
- Logs the best bid and ask
- The rule triggers only once (`Once()`)

### Method №2: Creating a Rule

- Demonstrates an alternative way of creating a rule
- Functionally identical to Method №1

### Rule Inside Rule

- Creates a rule that triggers when an order book is received
- Inside this rule, another rule is created
- The outer rule triggers once, the inner rule triggers every time an order book is received

## Features

- Demonstrates various ways of creating and applying rules in StockSharp
- Uses subscription to trades and order book
- Shows an example of logging information in the strategy
- Illustrates the use of `Once()` to limit rule triggering
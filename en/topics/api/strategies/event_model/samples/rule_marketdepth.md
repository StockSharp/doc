# Rules for Order Books and Trades

## Overview

`SimpleRulesStrategy` is a strategy that demonstrates various ways to create and apply rules in StockSharp. It subscribes to trades and order books, and then establishes various rules for processing the received data.

## Main Components

```cs
// Main components
public class SimpleRulesStrategy : Strategy
{
}
```

## OnStarted Method

Called when the strategy starts:

- Creates subscriptions to trades and order books
- Demonstrates various ways to create and apply rules

```cs
// OnStarted method
protected override void OnStarted2(DateTime time)
{
	var tickSub = new Subscription(DataType.Ticks, Security);
	var mdSub = new Subscription(DataType.MarketDepth, Security);

	//-----------------------Create a rule. Method №1-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №1 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//-----------------------Create a rule. Method №2-----------------------------------
	var whenMarketDepthChanged = mdSub.WhenOrderBookReceived(this);

	whenMarketDepthChanged.Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №2 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");
	}).Once().Apply(this);

	//----------------------Rule inside rule-----------------------------------
	mdSub.WhenOrderBookReceived(this).Do((depth) =>
	{
		LogInfo($"The rule WhenOrderBookReceived №3 BestBid={depth.GetBestBid()}, BestAsk={depth.GetBestAsk()}");

		//----------------------not a Once rule-----------------------------------
		mdSub.WhenOrderBookReceived(this).Do((depth1) =>
		{
			LogInfo($"The rule WhenOrderBookReceived №4 BestBid={depth1.GetBestBid()}, BestAsk={depth1.GetBestAsk()}");
		}).Apply(this);
	}).Once().Apply(this);

	// Sending requests for subscribe to market data.
	Subscribe(tickSub);
	Subscribe(mdSub);

	base.OnStarted2(time);
}
```

## Logic

### Method #1: Creating a Rule

- Creates a rule that triggers when an order book is received
- Logs the best bid and ask
- The rule triggers only once (`Once()`)

### Method #2: Creating a Rule

- Demonstrates an alternative way to create a rule
- Functionally identical to Method #1

### Rule Inside a Rule

- Creates a rule that triggers when an order book is received
- Inside this rule, another rule is created
- The outer rule triggers once, the inner rule - every time an order book is received

## Features

- Demonstrates various ways to create and apply rules in StockSharp
- Uses subscription to trades and order books
- Shows an example of logging information in a strategy using the `LogInfo` method
- Illustrates the use of `Once()` to limit rule triggering
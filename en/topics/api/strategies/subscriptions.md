# Market Data Subscriptions in Strategies

In StockSharp, strategies use a subscription mechanism to receive market data. This approach is the primary and preferred method of obtaining data in trading strategies.

## Subscription Basics

Subscriptions in strategies are based on the general [StockSharp subscription mechanism](../market_data/subscriptions.md). They provide a centralized and unified way of obtaining all types of market data.

## Creating a Subscription in a Strategy

In the [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) method of the strategy, you can create and start a subscription for the required data:

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);
    
    // Creating a subscription for 5-minute candles directly through DataType
    var subscription = new Subscription(
        DataType.TimeFrame(TimeSpan.FromMinutes(5)),
        Security);
    
    // If additional parameters are required, you can configure the subscription
    subscription.From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(7));
    
    // Creating a rule to process incoming candles
    Connector
        .WhenCandlesFinished(subscription)
        .Do(ProcessCandle)
        .Apply(this);
    
    // Starting the subscription
    Connector.Subscribe(subscription);
}
```

In this example, a subscription is created for 5-minute candles using a convenient constructor that accepts `DataType` and `Security`. If necessary, you can additionally configure subscription parameters, such as the history period.

## Advantages of Subscriptions in Strategies

Using subscriptions in strategies has several advantages compared to direct subscription to [Strategy.Connector](xref:StockSharp.Algo.Strategies.Strategy.Connector) events:

1. **Isolation** - each subscription works independently, allowing different types of data to be received for different instruments without mutual interference. This also protects the strategy from receiving data intended for other strategies running in parallel. With direct subscription to connector events, you would have to additionally filter data to exclude information from other strategies.

2. **State Management** - subscriptions have clear states ([SubscriptionStates](xref:StockSharp.Messages.SubscriptionStates)), which allow precise determination of whether historical data is currently being received or the subscription has already transitioned to online mode.

3. **Automatic Strategy State Control** - the strategy automatically tracks the state of all its subscriptions and transitions to online mode ([IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline)) only when all subscriptions have gone online.

4. **Code Uniformity** - subscriptions use a unified approach, independent of the type of requested data.

5. **Integration with Rules** - subscriptions easily integrate with strategy [event model](event_model.md) through rules.

6. **Automatic Subscription Management** - when the strategy stops, all its subscriptions are automatically canceled, freeing up resources.

7. **Historical Data Support** - ability to load historical data before transitioning to real-time data.

## Monitoring Subscription States

The strategy automatically tracks the state of all subscriptions to control its operating mode:

```cs
private void CheckRefreshOnlineState()
{
    bool nowOnline = ProcessState == ProcessStates.Started;

    if (nowOnline)
        nowOnline = _subscriptions.CachedKeys
            .Where(s => !s.SubscriptionMessage.IsHistoryOnly())
            .All(s => s.State == SubscriptionStates.Online);
    
    // Update strategy's IsOnline state
    IsOnline = nowOnline;
}
```

The [Strategy.IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) property will be `true` only when all strategy subscriptions have transitioned to the [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) state. This allows the strategy to understand the moment when it is working with current market data.

## Types of Subscriptions

In strategies, you can use subscriptions to various types of market data:

```cs
// Subscription to candles
var candleSubscription = new Subscription(
    DataType.TimeFrame(TimeSpan.FromMinutes(1)),
    Security);

// Subscription to market depth
var depthSubscription = new Subscription(
    DataType.MarketDepth,
    Security);

// Subscription to tick trades
var tickSubscription = new Subscription(
    DataType.Ticks,
    Security);

// Subscription to Level1 (best bid/ask and other basic information)
var level1Subscription = new Subscription(
    DataType.Level1,
    Security);
```

## Processing Subscription Data Through Rules

To process data coming through a subscription, it is recommended to use [rules](event_model.md):

```cs
// Subscription to candles
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), Security);

// Creating a rule for processing incoming candles
Connector
    .WhenCandlesFinished(subscription)  // Rule activation when a completed candle is received
    .Do(ProcessCandle)                   // Call processing method
    .Apply(this);                        // Apply rule to strategy

// Start subscription
Connector.Subscribe(subscription);
```

In the example above, a rule is created that will call the `ProcessCandle` method when each completed candle is received.

## Requesting Historical Data

The strategy automatically sets the history load period through the [Strategy.HistorySize](xref:StockSharp.Algo.Strategies.Strategy.HistorySize) property:

```cs
// Set history load period to 30 days
strategy.HistorySize = TimeSpan.FromDays(30);
```

When creating a subscription, the strategy automatically sets the `From` parameter to load history if it was not explicitly specified.

## Canceling Subscriptions

Subscriptions can be manually canceled by calling the [UnSubscribe](xref:StockSharp.BusinessEntities.ISubscriptionProvider.UnSubscribe(StockSharp.BusinessEntities.Subscription)) method:

```cs
// Cancel subscription
Connector.UnSubscribe(subscription);
```

When stopping the strategy, if the [UnsubscribeOnStop](xref:StockSharp.Algo.Strategies.Strategy.UnsubscribeOnStop) parameter is set to `true` (default), all subscriptions will be automatically canceled.

## See Also

- [Market Data Subscriptions](../market_data/subscriptions.md)
- [Event Model](event_model.md)
- [Strategy Platform Compatibility](compatibility.md)

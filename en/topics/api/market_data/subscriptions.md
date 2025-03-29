# Subscriptions

**StockSharp API** offers a data acquisition model based on subscriptions. This is a universal mechanism for receiving both market data and transaction information. This approach has significant advantages:

- **Subscription isolation** — each subscription works independently, allowing any number of subscriptions with different parameters to be run in parallel (with or without history request).
- **State tracking** — subscriptions have specific states that allow you to control whether historical data is currently flowing or the subscription has switched to real-time mode.
- **Universality** — the code for working with subscriptions is the same regardless of the types of data requested, making development more efficient.

To work with subscriptions, you need to use the [Subscription](xref:StockSharp.BusinessEntities.Subscription) class. Let's look at examples of using subscriptions to obtain various types of data.

## Example of Candle Subscription

```cs
// Create a subscription for 5-minute candles
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
    // Configure subscription parameters via the MarketData property
    MarketData =
    {
        // Request data for the last 30 days
        From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
        // null means the subscription will switch to real-time mode after receiving history
        To = null
    }
};

// Processing received candles
_connector.CandleReceived += (sub, candle) =>
{
    if (sub != subscription)
        return;
        
    // Process the candle
    Console.WriteLine($"Candle: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
};

// Handling the subscription's transition to online mode
_connector.SubscriptionOnline += (sub) =>
{
    if (sub != subscription)
        return;
        
    Console.WriteLine("Subscription switched to real-time mode");
};

// Handling subscription errors
_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
{
    if (sub != subscription)
        return;
        
    Console.WriteLine($"Subscription error: {error}");
};

// Starting the subscription
_connector.Subscribe(subscription);
```

## Example of Order Book Subscription

```cs
// Create a subscription to the order book for the selected instrument
var depthSubscription = new Subscription(DataType.MarketDepth, security);

// Processing received order books
_connector.OrderBookReceived += (sub, depth) =>
{
    if (sub != depthSubscription)
        return;
        
    // Process the order book
    Console.WriteLine($"Order book: {depth.SecurityId}, Time: {depth.ServerTime}");
    Console.WriteLine($"Bids: {depth.Bids.Count}, Asks: {depth.Asks.Count}");
};

// Starting the subscription
_connector.Subscribe(depthSubscription);
```

## Example of Tick Trades Subscription

```cs
// Create a subscription to tick trades for the selected instrument
var tickSubscription = new Subscription(DataType.Ticks, security);

// Processing received ticks
_connector.TickTradeReceived += (sub, tick) =>
{
    if (sub != tickSubscription)
        return;
        
    // Process the tick
    Console.WriteLine($"Tick: {tick.SecurityId}, Time: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
};

// Starting the subscription
_connector.Subscribe(tickSubscription);
```

## Example of Subscription with Candle Building Mode Configuration

```cs
// Subscription to 5-minute candles that will be built from ticks
var candleSubscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
    MarketData =
    {
        // Specify the building mode and data source
        BuildMode = MarketDataBuildModes.Build,
        BuildFrom = DataType.Ticks,
        // You can also enable volume profile building
        IsCalcVolumeProfile = true,
    }
};

_connector.Subscribe(candleSubscription);
```

## Example of Level1 Subscription (Basic Instrument Information)

```cs
// Creating a subscription for basic instrument information
var level1Subscription = new Subscription(DataType.Level1, security);

// Processing received Level1 data
_connector.Level1Received += (sub, level1) =>
{
    if (sub != level1Subscription)
        return;
    
    Console.WriteLine($"Level1: {level1.SecurityId}, Time: {level1.ServerTime}");
    
    // Output Level1 field values
    foreach (var pair in level1.Changes)
    {
        Console.WriteLine($"Field: {pair.Key}, Value: {pair.Value}");
    }
};

// Starting the subscription
_connector.Subscribe(level1Subscription);
```

## Unsubscribing from Data

To stop receiving data, use the `UnSubscribe` method:

```cs
// Unsubscribe from a specific subscription
_connector.UnSubscribe(subscription);

// Or you can unsubscribe from all subscriptions
foreach (var sub in _connector.Subscriptions)
{
    _connector.UnSubscribe(sub);
}
```

## Subscription States

Subscriptions can be in the following states:

- [SubscriptionStates.Stopped](xref:StockSharp.Messages.SubscriptionStates.Stopped) — the subscription is inactive (stopped or not started).
- [SubscriptionStates.Active](xref:StockSharp.Messages.SubscriptionStates.Active) — the subscription is active and may transmit historical data until switching to real-time mode or completion.
- [SubscriptionStates.Error](xref:StockSharp.Messages.SubscriptionStates.Error) — the subscription is inactive and in an error state.
- [SubscriptionStates.Finished](xref:StockSharp.Messages.SubscriptionStates.Finished) — the subscription has completed its work (all data received).
- [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) — the subscription has switched to real-time mode and only transmits current data.
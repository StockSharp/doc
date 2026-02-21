# High-Level Subscriptions

## Overview

The `Strategy` class provides a set of high-level market data subscription methods: `SubscribeCandles`, `SubscribeTicks`, `SubscribeLevel1`, and `SubscribeOrderBook`. These methods return an `ISubscriptionHandler<T>` object that allows binding data handlers and indicators in a convenient fluent style.

Unlike manually creating a `Subscription` object and calling `Subscribe()`, the high-level methods:

- Automatically create a subscription with the correct parameters
- Provide a typed `ISubscriptionHandler<T>` for binding handlers
- Integrate with the indicator system via `Bind` methods
- Support automatic chart rendering
- Properly manage the subscription lifecycle

## Subscription Methods

### SubscribeCandles

Subscribes to candles. Accepts a timeframe or `DataType`:

```csharp
// Subscribe by timeframe
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    TimeSpan tf,
    bool isFinishedOnly = true,
    Security security = default);

// Subscribe by DataType (supports all candle types)
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    DataType dt,
    bool isFinishedOnly = true,
    Security security = default);

// Subscribe with a ready-made Subscription object
ISubscriptionHandler<ICandleMessage> SubscribeCandles(Subscription subscription);
```

The `isFinishedOnly` parameter is `true` by default -- the handler receives only completed candles.

### SubscribeTicks

Subscribes to tick trades:

```csharp
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Security security = null);
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Subscription subscription);
```

### SubscribeLevel1

Subscribes to Level1 data (best bid/ask prices, last trade, and other fields):

```csharp
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Security security = null);
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Subscription subscription);
```

### SubscribeOrderBook

Subscribes to the order book:

```csharp
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Security security = null);
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Subscription subscription);
```

If the `security` parameter is not specified, the strategy's `Security` is used.

## ISubscriptionHandler Interface

The `ISubscriptionHandler<T>` object provides the following methods:

### Start / Stop

Starting and stopping the subscription:

```csharp
handler.Start();   // calls Subscribe
handler.Stop();    // calls UnSubscribe
```

### Bind (without indicators)

Binding a simple data handler:

```csharp
handler.Bind(Action<T> callback);
```

### Bind (with indicators)

Binding a handler with one or more indicators. The indicator automatically processes incoming data, and the handler receives the already-calculated value:

```csharp
// One indicator -- decimal value
handler.Bind(IIndicator indicator, Action<T, decimal> callback);

// Two indicators
handler.Bind(IIndicator ind1, IIndicator ind2, Action<T, decimal, decimal> callback);

// Up to eight indicators
handler.Bind(ind1, ind2, ind3, ..., callback);

// Array of indicators
handler.Bind(IIndicator[] indicators, Action<T, decimal[]> callback);
```

The handler with `Bind` is called only when all indicators have returned a non-empty value.

### BindWithEmpty

Similar to `Bind`, but the handler is called even if the indicator returned an empty value. Values are represented as `decimal?`:

```csharp
handler.BindWithEmpty(IIndicator indicator, Action<T, decimal?> callback);
```

### BindEx

Provides access to the full `IIndicatorValue` object instead of the extracted `decimal`:

```csharp
handler.BindEx(IIndicator indicator, Action<T, IIndicatorValue> callback, bool allowEmpty = false);
```

## Example: Strategy with Indicators

```csharp
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<int> _shortPeriod;
    private readonly StrategyParam<int> _longPeriod;
    private readonly StrategyParam<DataType> _candleType;

    public int ShortPeriod
    {
        get => _shortPeriod.Value;
        set => _shortPeriod.Value = value;
    }

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public SmaStrategy()
    {
        _shortPeriod = Param(nameof(ShortPeriod), 10);
        _longPeriod = Param(nameof(LongPeriod), 20);
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var shortSma = new SimpleMovingAverage { Length = ShortPeriod };
        var longSma = new SimpleMovingAverage { Length = LongPeriod };

        var subscription = SubscribeCandles(CandleType);

        // Binding two indicators -- handler is called
        // when both indicators are formed
        subscription
            .Bind(shortSma, longSma, (candle, shortValue, longValue) =>
            {
                if (!IsFormedAndOnlineAndAllowTrading())
                    return;

                if (shortValue > longValue && Position <= 0)
                    BuyMarket(Volume + Math.Abs(Position));
                else if (shortValue < longValue && Position >= 0)
                    SellMarket(Volume + Math.Abs(Position));
            })
            .Start();

        // Chart setup
        var area = CreateChartArea();
        if (area != null)
        {
            DrawCandles(area, subscription);
            DrawIndicator(area, shortSma);
            DrawIndicator(area, longSma);
            DrawOwnTrades(area);
        }
    }
}
```

## Example: Tick Subscription

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeTicks()
        .Bind(tick =>
        {
            if (!IsFormedAndOnlineAndAllowTrading())
                return;

            this.AddInfoLog("Tick: price={0}, volume={1}", tick.Price, tick.Volume);
        })
        .Start();
}
```

## Example: Order Book Subscription

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeOrderBook()
        .Bind(book =>
        {
            var bestBid = book.GetBestBid();
            var bestAsk = book.GetBestAsk();

            if (bestBid != null && bestAsk != null)
            {
                var spread = bestAsk.Price - bestBid.Price;
                this.AddInfoLog("Spread: {0}", spread);
            }
        })
        .Start();
}
```

## Differences from Manual Subscription Creation

| Aspect | Manual subscription | High-level method |
|--------|-------------------|-------------------|
| Creation | `new Subscription(DataType, Security)` | `SubscribeCandles(tf)` |
| Data handling | Subscribing to connector events | `Bind(callback)` |
| Indicators | Manual `indicator.Process()` call | Automatically via `Bind(indicator, callback)` |
| Indicator registration | Manual addition to `Indicators` | Automatically on `Bind` |
| Chart rendering | Manual integration with `IChart` | `DrawCandles`, `DrawIndicator` |

High-level methods are recommended for use in most strategies, as they significantly reduce the amount of code and decrease the likelihood of errors.

# Moving Averages with Martingale

## Overview

`SmaStrategyMartingaleStrategy` is a trading strategy based on the intersection of two simple moving averages (SMA) with martingale elements. The strategy uses long and short SMAs to determine market entry and exit signals, increasing the position size with each new trade.

## Main Components

```cs
// Main components
internal class SmaStrategyMartingaleStrategy : Strategy
{
    private readonly Subscription _subscription;

    public SimpleMovingAverage LongSma { get; set; }
    public SimpleMovingAverage ShortSma { get; set; }
}
```

## Constructor

The constructor takes a `CandleSeries` and initializes the subscription to this candle series.

```cs
// Constructor
public SmaStrategyMartingaleStrategy(CandleSeries series)
{
    _subscription = new(series);
}
```

## Methods

### IsRealTime

Determines if a candle is "real" (recently closed):

- Checks if less than 10 seconds have passed since the candle's closing time to the current time
- Used to filter out outdated data in real-time mode

```cs
// IsRealTime method
private bool IsRealTime(ICandleMessage candle)
{
    return (CurrentTime - candle.CloseTime).TotalSeconds < 10;
}
```

### OnStarted

Called when the strategy starts:

- Subscribes to candle completion
- Binds candle processing to the `ProcessCandle` method
- Starts the subscription to the candle series

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    this.WhenCandlesFinished(_subscription).Do(ProcessCandle).Apply(this);
    Subscribe(_subscription);
    base.OnStarted(time);
}
```

### ProcessCandle

Main method for processing each completed candle:

1. Updates the values of long and short SMAs
2. Checks if the indicators are formed
3. In real-time mode, checks the relevance of the candle
4. Determines if an SMA intersection has occurred
5. On intersection:
   - Cancels active orders
   - Determines the direction of the trade (buy or sell)
   - Calculates the position volume considering the current position (martingale element)
   - Registers a new order

```cs
// ProcessCandle method
private void ProcessCandle(ICandleMessage candle)
{
    var longSmaIsFormedPrev = LongSma.IsFormed;
    LongSma.Process(candle);
    ShortSma.Process(candle);

    if (!LongSma.IsFormed || !longSmaIsFormedPrev) return;
    if (!IsBacktesting && !IsRealTime(candle)) return;

    var isShortLessThenLongCurrent = ShortSma.GetCurrentValue() < LongSma.GetCurrentValue();
    var isShortLessThenLongPrevios = ShortSma.GetValue(1) < LongSma.GetValue(1);

    if (isShortLessThenLongPrevios == isShortLessThenLongCurrent) return;

    CancelActiveOrders();

    var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

    var volume = Volume + Math.Abs(Position);

    var price = Security.ShrinkPrice(ShortSma.GetCurrentValue());
    RegisterOrder(this.CreateOrder(direction, price, volume));
}
```

## Trading Logic

- Buy signal: short SMA crosses long SMA from below
- Sell signal: short SMA crosses long SMA from above
- With each new trade, the volume increases by the current position size
- The order price is set to the current value of the short SMA

## Features

- The strategy works with both historical data and in real-time mode
- Uses a candle subscription mechanism for data processing
- Applies martingale elements, increasing the position size with each new trade
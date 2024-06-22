# Stairs Countertrend Strategy

## Overview

`StairsCountertrendStrategy` is a countertrend trading strategy that opens positions against an established trend of a certain length.

## Main Components

```cs
// Main components
public class StairsCountertrendStrategy : Strategy
{
    private readonly CandleSeries _candleSeries;
    private Subscription _subscription;

    private int _bullLength;
    private int _bearLength;
}
```

## Constructor

The constructor takes a `CandleSeries` to initialize the strategy.

```cs
// Constructor
public StairsCountertrendStrategy(CandleSeries candleSeries)
{
    _candleSeries = candleSeries;
}
```

## Properties

### Length

Defines the minimum number of consecutive candles in one direction to determine a trend.

```cs
// Length property
private int Length { get; set; } = 3;
```

## Methods

### OnStarted

Called when the strategy starts:

- Subscribes to receive candles
- Initializes subscription to the candle series

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    CandleReceived += OnCandleReceived;
    _subscription = this.SubscribeCandles(_candleSeries);

    base.OnStarted(time);
}
```

### OnStopped

Called when the strategy stops:

- Cancels the subscription to candles

```cs
// OnStopped method
protected override void OnStopped()
{
    if (_subscription != null)
    {
        UnSubscribe(_subscription);
        _subscription = null;
    }

    base.OnStopped();
}
```

### OnCandleReceived

Main method for processing each completed candle:

1. Checks if the candle belongs to the required subscription
2. Analyzes the direction of the candle (bullish or bearish)
3. Updates counters of consecutive bullish and bearish candles
4. Makes a decision to open a position against the trend

```cs
// OnCandleReceived method
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
    if (subscription != _subscription)
        return;

    if (candle.State != CandleStates.Finished) return;

    if (candle.OpenPrice < candle.ClosePrice)
    {
        _bullLength++;
        _bearLength = 0;
    }
    else if (candle.OpenPrice > candle.ClosePrice)
    {
        _bullLength = 0;
        _bearLength++;
    }

    if (_bullLength >= Length && Position >= 0)
    {
        RegisterOrder(this.SellAtMarket(Volume + Math.Abs(Position)));
    }
    else if (_bearLength >= Length && Position <= 0)
    {
        RegisterOrder(this.BuyAtMarket(Volume + Math.Abs(Position)));
    }
}
```

## Trading Logic

- Sell signal: `Length` consecutive bullish candles with no short position
- Buy signal: `Length` consecutive bearish candles with no long position
- Position volume increases by the current position size with each new trade

## Features

- The strategy works with completed candles
- Uses market orders for position entry
- Applies a countertrend approach, opening positions against the established trend
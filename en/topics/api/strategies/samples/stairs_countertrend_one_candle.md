# One Candle Countertrend Strategy

## Overview

`OneCandleCountertrendStrategy` is a simple countertrend strategy that makes decisions based on the analysis of a single candle.

## Main Components

// Main components
public class OneCandleCountertrendStrategy : Strategy
{
    private readonly CandleSeries _candleSeries;
    private Subscription _subscription;
}

## Constructor

The constructor takes a [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) to initialize the strategy.

// Constructor
public OneCandleCountertrendStrategy(CandleSeries candleSeries)
{
    _candleSeries = candleSeries;
}

## Methods

### OnStarted

Called when the strategy starts:

- Subscribes to receive candles
- Initializes subscription to the candle series

// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    CandleReceived += OnCandleReceived;
    _subscription = this.SubscribeCandles(_candleSeries);

    base.OnStarted(time);
}

### OnStopped

Called when the strategy stops:

- Cancels the subscription to candles

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

### OnCandleReceived

Main method for processing each completed candle:

1. Checks if the candle belongs to the required subscription
2. Analyzes the direction of the candle (bullish or bearish)
3. Makes a decision to open a position against the trend based on the current candle and position

// OnCandleReceived method
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
    if (subscription != _subscription)
        return;

    if (candle.State != CandleStates.Finished) return;

    if (candle.OpenPrice < candle.ClosePrice && Position >= 0)
    {
        RegisterOrder(this.SellAtMarket(Volume + Math.Abs(Position)));
    }
    else if (candle.OpenPrice > candle.ClosePrice && Position <= 0)
    {
        RegisterOrder(this.BuyAtMarket(Volume + Math.Abs(Position)));
    }
}

## Trading Logic

- Sell signal: bullish candle (closing price higher than opening price) with no short position
- Buy signal: bearish candle (closing price lower than opening price) with no long position
- Position volume increases by the current position size with each new trade

## Features

- The strategy works with completed candles
- Uses market orders for position entry
- Applies simple countertrend logic based on a single candle
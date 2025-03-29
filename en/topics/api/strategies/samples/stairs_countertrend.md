# Stairs Countertrend Strategy

## Overview

`StairsCountertrendStrategy` is a countertrend trading strategy that opens positions against an established trend of a specific length.

## Main Components

```cs
public class StairsCountertrendStrategy : Strategy
{
    private readonly StrategyParam<int> _length;
    private readonly StrategyParam<DataType> _candleType;
    
    private int _bullLength;
    private int _bearLength;
}
```

## Strategy Parameters

The strategy allows customizing the following parameters:

- **Length** - number of consecutive candles in one direction to identify a trend (default 3)
- **CandleType** - candle type to work with (default 5-minute)

The Length parameter is available for optimization in the range from 2 to 10 with a step of 1.

## Strategy Initialization

In the [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) method, counters are reset, candle subscription is created, and visualization is prepared:

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);
    
    // Reset counters
    _bullLength = 0;
    _bearLength = 0;

    // Create subscription
    var subscription = SubscribeCandles(CandleType);
    
    subscription
        .Bind(ProcessCandle)
        .Start();

    // Set up visualization on the chart
    var area = CreateChartArea();
    if (area != null)
    {
        DrawCandles(area, subscription);
        DrawOwnTrades(area);
    }
}
```

## Processing Candles

The `ProcessCandle` method is called for each completed candle and implements the trading logic:

```cs
private void ProcessCandle(ICandleMessage candle)
{
    // Check if the candle is finished
    if (candle.State != CandleStates.Finished)
        return;

    // Check if the strategy is ready for trading
    if (!IsFormedAndOnlineAndAllowTrading())
        return;

    // Update counters based on candle direction
    if (candle.OpenPrice < candle.ClosePrice)
    {
        // Bullish candle
        _bullLength++;
        _bearLength = 0;
    }
    else if (candle.OpenPrice > candle.ClosePrice)
    {
        // Bearish candle
        _bullLength = 0;
        _bearLength++;
    }

    // Countertrend strategy: 
    // Sell after Length consecutive bullish candles
    if (_bullLength >= Length && Position >= 0)
    {
        SellMarket(Volume + Math.Abs(Position));
    }
    // Buy after Length consecutive bearish candles
    else if (_bearLength >= Length && Position <= 0)
    {
        BuyMarket(Volume + Math.Abs(Position));
    }
}
```

## Trading Logic

- **Sell signal**: `Length` consecutive bullish candles (close price above open price) when there is no short position
- **Buy signal**: `Length` consecutive bearish candles (close price below open price) when there is no long position
- Position volume increases by the current position amount with each new trade

## Features

- The strategy automatically determines instruments to work with via the `GetWorkingSecurities()` method
- The strategy only works with completed candles
- The strategy uses market orders for position entry
- The strategy applies a countertrend approach, opening positions against the established trend
- Candle counters are reset when a candle in the opposite direction appears
- Candles and trades are visualized on the chart when a graphic area is available
- The sequence length optimization is supported to find optimal strategy settings
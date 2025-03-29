# One Candle Trend Strategy

## Overview

`OneCandleTrendStrategy` is a simple trend strategy that makes decisions based on the analysis of a single candle.

## Main Components

```cs
public class OneCandleTrendStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
}
```

## Strategy Parameters

The strategy allows customizing the following parameters:

- **CandleType** - candle type to work with (default 5-minute)

## Strategy Initialization

In the [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) method, candle subscription is created and visualization is prepared:

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);

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

    // Trend strategy: buy on bullish candle, sell on bearish candle
    if (candle.OpenPrice < candle.ClosePrice && Position <= 0)
    {
        // Bullish candle - buy
        BuyMarket(Volume + Math.Abs(Position));
    }
    else if (candle.OpenPrice > candle.ClosePrice && Position >= 0)
    {
        // Bearish candle - sell
        SellMarket(Volume + Math.Abs(Position));
    }
}
```

## Trading Logic

- **Buy signal**: bullish candle (close price above open price) when there is no long position
- **Sell signal**: bearish candle (close price below open price) when there is no short position
- Position volume increases by the current position amount with each new trade

## Features

- The strategy automatically determines instruments to work with via the `GetWorkingSecurities()` method
- The strategy only works with completed candles
- The strategy uses market orders for position entry
- The strategy applies a simple trend detection logic based on a single candle
- Candles and trades are visualized on the chart when a graphic area is available
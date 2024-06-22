# Countertrend Strategy with Quoting

## Overview

`StairsCountertrendStrategy` is a countertrend strategy based on the analysis of a sequence of candles. It uses a "stairs" approach to open positions against the current trend.

## Main Components

```cs
// Main components
public class StairsCountertrendStrategy : Strategy
{
    private readonly Subscription _subscription;
    private int _bullLength;
    private int _bearLength;
}
```

## Constructor

The constructor takes a [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) to initialize the strategy.

```cs
// Constructor
public StairsCountertrendStrategy(CandleSeries candleSeries)
{
    _subscription = new(candleSeries);
}
```

## Properties

### Length

Defines the minimum number of consecutive candles in one direction to determine a trend.

public int Length { get; set; } = 3;

## Methods

### OnStarted

Called when the strategy starts:

- Sets up support for filtered order book
- Subscribes to candle completion
- Initializes candle processing

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
    Connector.SupportFilteredMarketDepth = true;

    this
        .WhenCandlesFinished(_subscription)
        .Do(CandleManager_Processing)
        .Apply(this);
    
    Subscribe(_subscription);

    base.OnStarted(time);
}
```

### CandleManager_Processing

Main method for processing each completed candle:

1. Checks the candle state
2. Analyzes the direction of the candle (bullish or bearish)
3. Updates counters of consecutive bullish and bearish candles
4. Makes a decision to open a position against the trend
5. Creates and adds a child quoting strategy

```cs
// CandleManager_Processing method
private void CandleManager_Processing(ICandleMessage candle)
{
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
        ChildStrategies.ToList().ForEach(s => s.Stop());
        var strategy = new MarketQuotingStrategy(Sides.Sell, 1)
        {
            WaitAllTrades = true
        };
        ChildStrategies.Add(strategy);
    }
    else if (_bearLength >= Length && Position <= 0)
    {
        ChildStrategies.ToList().ForEach(s => s.Stop());
        var strategy = new MarketQuotingStrategy(Sides.Buy, 1)
        {
            WaitAllTrades = true
        };
        ChildStrategies.Add(strategy);
    }
}
```

## Trading Logic

- Sell signal: `Length` consecutive bullish candles with no short position
- Buy signal: `Length` consecutive bearish candles with no long position
- When a signal is received, a child quoting strategy is created ([MarketQuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingStrategy))

## Features

- The strategy works with completed candles
- Uses child quoting strategies for market entry
- Applies a countertrend approach, opening positions against the established trend
- Supports filtered order book for improved performance
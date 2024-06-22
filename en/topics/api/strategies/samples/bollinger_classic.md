# Classic Bollinger Strategy

## Overview

`BollingerStrategyClassicStrategy` is a strategy based on the Bollinger Bands indicator. It opens positions when the price reaches the upper or lower Bollinger Bands.

## Main Components

// Main components
internal class BollingerStrategyClassicStrategy : Strategy
{
   private readonly Subscription _subscription;

   public BollingerBands BollingerBands { get; set; }
}

## Constructor

The constructor takes a [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) to initialize the strategy.

// Constructor
public BollingerStrategyClassicStrategy(CandleSeries series)
{
   _subscription = new(series);
}

## Methods

### OnStarted

Called when the strategy starts:

- Subscribes to candle completion
- Initializes candle processing

// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
   this.WhenCandlesFinished(_subscription).Do(ProcessCandle).Apply(this);
   Subscribe(_subscription);
   base.OnStarted(time);
}

### IsRealTime

Checks if the candle is "real" (recently closed):

// IsRealTime method
private bool IsRealTime(ICandleMessage candle)
{
   return (CurrentTime - candle.CloseTime).TotalSeconds < 10;
}

### ProcessCandle

Main method for processing each completed candle:

1. Processes the candle with the Bollinger Bands indicator
2. Checks if the indicator is formed
3. Checks the operating mode (backtesting or real-time)
4. Makes a decision to open a position based on the closing price position relative to the Bollinger Bands

// ProcessCandle method
private void ProcessCandle(ICandleMessage candle)
{
   BollingerBands.Process(candle);

   if (!BollingerBands.IsFormed) return;
   if (!IsBacktesting && !IsRealTime(candle)) return;

   if (candle.ClosePrice >= BollingerBands.UpBand.GetCurrentValue() && Position >= 0)
   {
       RegisterOrder(this.SellAtMarket(Volume + Math.Abs(Position)));
   }
   else if (candle.ClosePrice <= BollingerBands.LowBand.GetCurrentValue() && Position <= 0)
   {
       RegisterOrder(this.BuyAtMarket(Volume + Math.Abs(Position)));
   }
}

## Trading Logic

- Sell signal: candle closing price reaches or exceeds the upper Bollinger Band when there is no short position
- Buy signal: candle closing price reaches or falls below the lower Bollinger Band when there is no long position
- Position volume increases by the current position size with each new trade

## Features

- The strategy works with both historical data and in real-time mode
- Uses the Bollinger Bands indicator to determine market entry points
- Applies a "reality" check on the candle in real-time mode
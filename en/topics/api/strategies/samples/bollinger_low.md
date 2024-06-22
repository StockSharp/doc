# Bollinger Strategy with Focus on Lower Band

## Overview

`BollingerStrategyLowBandStrategy` is a strategy based on the Bollinger Bands indicator. It opens a short position when the price reaches the lower Bollinger Band and closes it when reaching the middle line.

## Main Components

// Main components
internal class BollingerStrategyLowBandStrategy : Strategy
{
   private readonly Subscription _subscription;

   public BollingerBands BollingerBands { get; set; }
}

## Constructor

The constructor takes a [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) to initialize the strategy.

// Constructor
public BollingerStrategyLowBandStrategy(CandleSeries series)
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
   this
       .WhenCandlesFinished(_subscription)
       .Do(ProcessCandle)
       .Apply(this);

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
4. Makes a decision to open a short position when reaching the lower Bollinger Band
5. Makes a decision to close the short position when reaching the middle Bollinger Band

// ProcessCandle method
private void ProcessCandle(ICandleMessage candle)
{
   BollingerBands.Process(candle);

   if (!BollingerBands.IsFormed) return;
   if (!IsBacktesting && !IsRealTime(candle)) return;

   if (candle.ClosePrice <= BollingerBands.LowBand.GetCurrentValue() && Position == 0)
   {
       RegisterOrder(this.SellAtMarket(Volume));
   }
   else if (candle.ClosePrice >= BollingerBands.MovingAverage.GetCurrentValue() && Position < 0)
   {
       RegisterOrder(this.BuyAtMarket(Math.Abs(Position)));
   }
}

## Trading Logic

- Sell signal: candle closing price reaches or falls below the lower Bollinger Band when there is no open position
- Buy signal (closing short position): candle closing price reaches or exceeds the middle Bollinger Band when there is a short position
- Position volume is fixed when entering the market

## Features

- The strategy works with both historical data and in real-time mode
- Uses only the lower band and middle line of the Bollinger Bands indicator
- Opens only short positions
- Applies a "reality" check on the candle in real-time mode
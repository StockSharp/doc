# Bollinger Strategy with Focus on Upper Band

## Overview

`BollingerStrategyUpBandStrategy` is a strategy based on the Bollinger Bands indicator. It opens a long position when the price reaches the upper Bollinger Band and closes it when reaching the middle line.

## Main Components

```cs
// Main components
internal class BollingerStrategyUpBandStrategy : Strategy
{
   private readonly Subscription _subscription;

   public BollingerBands BollingerBands { get; set; }
}
```

## Constructor

The constructor takes a [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) to initialize the strategy.

```cs
// Constructor
public BollingerStrategyUpBandStrategy(CandleSeries series)
{
   _subscription = new(series);
}
```

## Methods

### OnStarted

Called when the strategy starts:

- Subscribes to candle completion
- Initializes candle processing

```cs
// OnStarted method
protected override void OnStarted(DateTimeOffset time)
{
   this.WhenCandlesFinished(_subscription).Do(ProcessCandle).Apply(this);
   Subscribe(_subscription);
   base.OnStarted(time);
}
```

### IsRealTime

Checks if the candle is "real" (recently closed):

```cs
// IsRealTime method
private bool IsRealTime(ICandleMessage candle)
{
   return (CurrentTime - candle.CloseTime).TotalSeconds < 10;
}
```

### ProcessCandle

Main method for processing each completed candle:

1. Processes the candle with the Bollinger Bands indicator
2. Checks if the indicator is formed
3. Checks the operating mode (backtesting or real-time)
4. Makes a decision to open a long position when reaching the upper Bollinger Band
5. Makes a decision to close the long position when reaching the middle Bollinger Band

```cs
// ProcessCandle method
private void ProcessCandle(ICandleMessage candle)
{
   BollingerBands.Process(candle);

   if (!BollingerBands.IsFormed) return;
   if (!IsBacktesting && !IsRealTime(candle)) return;

   if (candle.ClosePrice >= BollingerBands.UpBand.GetCurrentValue() && Position == 0)
   {
       RegisterOrder(this.BuyAtMarket(Volume));
   }
   else if (candle.ClosePrice <= BollingerBands.MovingAverage.GetCurrentValue() && Position > 0)
   {
       RegisterOrder(this.SellAtMarket(Math.Abs(Position)));
   }
}
```

## Trading Logic

- Buy signal: candle closing price reaches or exceeds the upper Bollinger Band when there is no open position
- Sell signal (closing long position): candle closing price reaches or falls below the middle Bollinger Band when there is a long position
- Position volume is fixed when entering the market

## Features

- The strategy works with both historical data and in real-time mode
- Uses only the upper band and middle line of the Bollinger Bands indicator
- Opens only long positions
- Applies a "reality" check on the candle in real-time mode
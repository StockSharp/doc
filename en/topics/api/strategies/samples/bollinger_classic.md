# Classic Bollinger Strategy

## Overview

`BollingerStrategyClassicStrategy` is a strategy based on the [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) indicator. It opens positions when the price reaches the upper or lower boundary of the Bollinger Bands.

## Main Components

The strategy inherits from [Strategy](xref:StockSharp.Algo.Strategies.Strategy) and uses parameters for configuration:

```cs
public class BollingerStrategyClassicStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## Strategy Parameters

The strategy allows customizing the following parameters:

- **BollingerLength** - Bollinger Bands indicator period (default 20)
- **BollingerDeviation** - standard deviation multiplier (default 2.0)
- **CandleType** - candle type to work with (default 5-minute)

All parameters are available for optimization with specified value ranges.

## Strategy Initialization

In the [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) method, the Bollinger Bands indicator is created, candle subscription is set up, and visualization is prepared:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Create indicator
	_bollingerBands = new BollingerBands
	{
		Length = BollingerLength,
		Width = BollingerDeviation
	};

	// Create subscription and bind indicator
	var subscription = SubscribeCandles(CandleType);
	subscription
		.BindEx(_bollingerBands, ProcessCandle)
		.Start();

	// Set up visualization on the chart
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, _bollingerBands, System.Drawing.Color.Purple);
		DrawOwnTrades(area);
	}
}
```

## Processing Candles

The `ProcessCandle` method is called for each completed candle and implements the trading logic:

```cs
private void ProcessCandle(ICandleMessage candle, IIndicatorValue bollingerValue)
{
	// Skip incomplete candles
	if (candle.State != CandleStates.Finished)
		return;

	// Check if the strategy is ready for trading
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var typed = (BollingerBandsValue)bollingerValue;

	// Trading logic:
	// Sell when price reaches or exceeds the upper band
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Buy when price reaches or falls below the lower band
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## Trading Logic

- **Sell signal**: candle close price reaches or exceeds the upper Bollinger Band when there is no short position
- **Buy signal**: candle close price reaches or falls below the lower Bollinger Band when there is no long position
- Position volume increases by the current position amount with each new trade

## Features

- The strategy automatically determines instruments to work with via the `GetWorkingSecurities()` method
- The strategy only works with completed candles
- The indicator and trades are visualized on a chart when a graphic area is available
- Parameter optimization is supported to find optimal strategy settings
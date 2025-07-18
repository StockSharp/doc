# Bollinger Strategy with Focus on Upper Band

## Overview

`BollingerStrategyUpBandStrategy` is a strategy based on the [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) indicator. It opens a long position when the price reaches the upper boundary of the Bollinger Bands and closes it when the price reaches the middle line.

## Main Components

The strategy inherits from [Strategy](xref:StockSharp.Algo.Strategies.Strategy) and uses parameters for configuration:

```cs
public class BollingerStrategyUpBandStrategy : Strategy
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

In the [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) method, the Bollinger Bands indicator is created, candle subscription is set up, and visualization is prepared:

```cs
protected override void OnStarted(DateTimeOffset time)
{
	base.OnStarted(time);

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
	// Buy when price touches the upper band (only when no position exists)
	if (candle.ClosePrice >= typed.UpBand && Position == 0)
	{
		BuyMarket(Volume);
	}
	// Sell to close the position when price reaches the middle line (only with a long position)
	else if (candle.ClosePrice <= typed.MiddleBand && Position > 0)
	{
		SellMarket(Math.Abs(Position));
	}
}
```

## Trading Logic

- **Buy signal**: candle close price reaches or exceeds the upper Bollinger Band when there is no open position
- **Sell signal** (closing long position): candle close price reaches or falls below the middle Bollinger Band line when there is a long position
- Position volume is fixed when opening and equals the entire current position when closing

## Features

- The strategy automatically determines instruments to work with via the `GetWorkingSecurities()` method
- The strategy only works with completed candles
- The strategy uses only the upper band and middle line of the Bollinger Bands indicator
- Only long positions are opened
- The indicator and trades are visualized on a chart when a graphic area is available
- Parameter optimization is supported to find optimal strategy settings
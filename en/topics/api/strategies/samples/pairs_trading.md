# Pairs Trading Strategy

## Overview

`PairsTradingStrategy` is a pairs trading strategy based on statistical arbitrage between two related instruments. It tracks the spread between the prices of two assets and opens positions when the spread deviates significantly from the mean, expecting a reversion to the mean.

## Main Components

The strategy inherits from [Strategy](xref:StockSharp.Algo.Strategies.Strategy) and uses parameters for configuration:

```cs
public class PairsTradingStrategy : Strategy
{
	private readonly StrategyParam<int> _spreadLength;
	private readonly StrategyParam<decimal> _entryThreshold;
	private readonly StrategyParam<decimal> _exitThreshold;
	private readonly StrategyParam<DataType> _candleType;

	// Latest prices for each instrument
	private decimal? _lastPrice1;
	private decimal? _lastPrice2;
}
```

## Strategy Parameters

The strategy allows customizing the following parameters:

- **SpreadLength** - period for calculating the spread mean and standard deviation (default 20)
- **EntryThreshold** - Z-Score threshold for entering a position (default 2.0)
- **ExitThreshold** - Z-Score threshold for exiting a position (default 0.5)
- **CandleType** - candle type to work with (default 5-minute)

All parameters are available for optimization with specified value ranges.

## Strategy Initialization

In the [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) method, indicators are created and candle subscriptions are set up for two instruments:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Get two instruments for pairs trading
	var securities = GetWorkingSecurities().ToArray();
	if (securities.Length < 2)
		throw new InvalidOperationException("Необходимо указать 2 инструмента.");

	var sec1 = securities[0].sec;
	var sec2 = securities[1].sec;

	// Indicators for calculating the spread mean and standard deviation
	var sma = new SimpleMovingAverage { Length = SpreadLength };
	var stdDev = new StandardDeviation { Length = SpreadLength };

	_lastPrice1 = null;
	_lastPrice2 = null;

	// Subscribe to candles of the first instrument
	SubscribeCandles(CandleType, security: sec1)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice1 = c.ClosePrice;
		})
		.Start();

	// Subscribe to candles of the second instrument with spread processing
	SubscribeCandles(CandleType, security: sec2)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice2 = c.ClosePrice;

			if (_lastPrice1 == null)
				return;

			ProcessSpread(_lastPrice1.Value, _lastPrice2.Value, sma, stdDev);
		})
		.Start();
}
```

## Spread Processing

The `ProcessSpread` method calculates the Z-Score of the spread and generates trading signals:

```cs
private void ProcessSpread(decimal price1, decimal price2,
	SimpleMovingAverage sma, StandardDeviation stdDev)
{
	// Calculate spread as the price difference
	var spread = price1 - price2;

	// Process indicators
	var smaValue = sma.Process(new DecimalIndicatorValue(sma, spread));
	var devValue = stdDev.Process(new DecimalIndicatorValue(stdDev, spread));

	if (!sma.IsFormed || !stdDev.IsFormed)
		return;

	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var mean = smaValue.ToDecimal();
	var dev = devValue.ToDecimal();

	if (dev == 0)
		return;

	// Calculate Z-Score: spread deviation from the mean in standard deviation units
	var zScore = (spread - mean) / dev;

	// Spread too high: sell the first instrument, buy the second
	if (zScore > EntryThreshold && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Spread too low: buy the first instrument, sell the second
	else if (zScore < -EntryThreshold && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Reversion to the mean: close position
	else if (Math.Abs(zScore) < ExitThreshold && Position != 0)
	{
		ClosePosition();
	}
}
```

## Trading Logic

- **Sell signal**: the spread Z-Score exceeds the entry threshold (default 2.0) when there is no short position
- **Buy signal**: the spread Z-Score falls below the negative entry threshold (default -2.0) when there is no long position
- **Close position**: the absolute Z-Score value drops below the exit threshold (default 0.5), signaling that the spread is reverting to the mean

## Features

- The strategy works with two instruments obtained via the `GetWorkingSecurities()` method
- The spread is calculated as the difference of closing prices of candles from two instruments
- Z-Score is used to normalize the spread deviation from the mean
- The strategy implements the classic mean reversion concept
- The strategy only works with completed candles
- Parameter optimization is supported to find optimal strategy settings

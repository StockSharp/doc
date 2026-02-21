# Multi-Timeframe Strategy

## Overview

`MultiTimeframeStrategy` is a strategy that uses two timeframes for making trading decisions. Hourly candles determine the trend direction via moving average crossovers, while 5-minute candles with the [RelativeStrengthIndex](xref:StockSharp.Algo.Indicators.RelativeStrengthIndex) indicator are used for precise entry in the trend direction.

## Main Components

The strategy inherits from [Strategy](xref:StockSharp.Algo.Strategies.Strategy) and uses parameters for configuration:

```cs
public class MultiTimeframeStrategy : Strategy
{
	private readonly StrategyParam<int> _fastSmaLength;
	private readonly StrategyParam<int> _slowSmaLength;
	private readonly StrategyParam<int> _rsiLength;
	private readonly StrategyParam<decimal> _takeProfit;
	private readonly StrategyParam<decimal> _stopLoss;

	// Trend direction on the higher timeframe
	private Sides? _hourlyTrend;
}
```

## Strategy Parameters

The strategy allows customizing the following parameters:

- **FastSmaLength** - fast moving average period for the hourly chart (default 10)
- **SlowSmaLength** - slow moving average period for the hourly chart (default 30)
- **RsiLength** - RSI period for the 5-minute chart (default 14)
- **TakeProfit** - take-profit size in percent (default 2)
- **StopLoss** - stop-loss size in percent (default 1)

All parameters are available for optimization with specified value ranges.

## Strategy Initialization

In the [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) method, indicators are created and candle subscriptions are set up for two timeframes:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	var fastSma = new SimpleMovingAverage { Length = FastSmaLength };
	var slowSma = new SimpleMovingAverage { Length = SlowSmaLength };
	var rsi = new RelativeStrengthIndex { Length = RsiLength };

	_hourlyTrend = null;

	// Hourly candles for trend detection (SMA crossover)
	SubscribeCandles(TimeSpan.FromHours(1))
		.Bind(fastSma, slowSma, ProcessHourlyCandle)
		.Start();

	// 5-minute candles for precise entry (RSI)
	SubscribeCandles(TimeSpan.FromMinutes(5))
		.Bind(rsi, ProcessEntryCandle)
		.Start();

	// Set up position protection (take-profit and stop-loss)
	StartProtection(
		new Unit(TakeProfit, UnitTypes.Percent),
		new Unit(StopLoss, UnitTypes.Percent)
	);

	// Set up visualization on the chart
	var area = CreateChartArea();
	if (area != null)
	{
		DrawIndicator(area, fastSma, System.Drawing.Color.Blue);
		DrawIndicator(area, slowSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Processing Hourly Candles

The `ProcessHourlyCandle` method determines the trend direction on the higher timeframe:

```cs
private void ProcessHourlyCandle(ICandleMessage candle, decimal fastValue, decimal slowValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Determine trend by moving average crossover
	_hourlyTrend = fastValue > slowValue ? Sides.Buy : Sides.Sell;
}
```

## Processing 5-Minute Candles

The `ProcessEntryCandle` method implements position entry based on the RSI signal in the trend direction:

```cs
private void ProcessEntryCandle(ICandleMessage candle, decimal rsiValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	if (_hourlyTrend == null || !IsFormedAndOnlineAndAllowTrading())
		return;

	// Buy: uptrend and RSI in oversold zone
	if (_hourlyTrend == Sides.Buy && rsiValue < 30 && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// Sell: downtrend and RSI in overbought zone
	else if (_hourlyTrend == Sides.Sell && rsiValue > 70 && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## Trading Logic

- **Trend detection**: fast SMA above slow SMA on the hourly chart indicates an uptrend, below indicates a downtrend
- **Buy signal**: uptrend on the hourly chart and RSI < 30 on the 5-minute chart when there is no long position
- **Sell signal**: downtrend on the hourly chart and RSI > 70 on the 5-minute chart when there is no short position
- **Position protection**: automatic take-profit and stop-loss via `StartProtection`

## Features

- The strategy uses two timeframes: hourly for trend and 5-minute for entry
- Position entry is only performed in the direction of the higher timeframe trend
- RSI is used as a filter to find optimal entry points (oversold/overbought)
- Positions are automatically protected with stop-loss and take-profit
- The strategy only works with completed candles
- Indicators and trades are visualized on the chart when a graphic area is available
- Parameter optimization is supported to find optimal strategy settings

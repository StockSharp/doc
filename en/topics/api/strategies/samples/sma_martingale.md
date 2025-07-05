# Moving Averages with Martingale

## Overview

`SmaStrategyMartingaleStrategy` is a trading strategy based on the crossover of two simple moving averages ([SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage)) with martingale elements. The strategy uses long and short SMAs to determine entry and exit signals, increasing position size with each new trade.

## Main Components

```cs
public class SmaStrategyMartingaleStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;
	private readonly StrategyParam<DataType> _candleType;

	// Variables to store previous indicator values
	private decimal _prevLongValue;
	private decimal _prevShortValue;
	private bool _isFirstValue = true;
}
```

## Strategy Parameters

The strategy allows customizing the following parameters:

- **LongSmaLength** - long moving average period (default 80)
- **ShortSmaLength** - short moving average period (default 30)
- **CandleType** - candle type to work with (default 5-minute)

All parameters are available for optimization with specified value ranges.

## Strategy Initialization

In the [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) method, SMA indicators are created, candle subscription is set up, and visualization is prepared:

```cs
protected override void OnStarted(DateTimeOffset time)
{
	base.OnStarted(time);

	// Create indicators
	var longSma = new SimpleMovingAverage { Length = LongSmaLength };
	var shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

	// Add indicators to the strategy collection for automatic IsFormed tracking
	Indicators.Add(longSma);
	Indicators.Add(shortSma);

	// Create subscription and bind indicators
	var subscription = SubscribeCandles(CandleType);
	subscription
		.Bind(longSma, shortSma, ProcessCandle)
		.Start();

	// Set up visualization on the chart
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, longSma, System.Drawing.Color.Blue);
		DrawIndicator(area, shortSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## Processing Candles

The `ProcessCandle` method is called for each completed candle and implements the trading logic:

```cs
private void ProcessCandle(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Skip incomplete candles
	if (candle.State != CandleStates.Finished)
		return;

	// Check if the strategy is ready for trading
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// For the first value, only save data without generating signals
	if (_isFirstValue)
	{
		_prevLongValue = longValue;
		_prevShortValue = shortValue;
		_isFirstValue = false;
		return;
	}

	// Get current and previous comparison of indicator values
	var isShortLessThenLongCurrent = shortValue < longValue;
	var isShortLessThenLongPrevious = _prevShortValue < _prevLongValue;

	// Save current values as previous for the next candle
	_prevLongValue = longValue;
	_prevShortValue = shortValue;

	// Check for crossover (signal)
	if (isShortLessThenLongPrevious == isShortLessThenLongCurrent)
		return;

	// Cancel active orders before placing new ones
	CancelActiveOrders();

	// Determine trade direction
	var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

	// Calculate position size (increase position with each trade - martingale approach)
	var volume = Volume + Math.Abs(Position);

	// Create and register an order with the appropriate price
	var price = Security.ShrinkPrice(shortValue);
	RegisterOrder(CreateOrder(direction, price, volume));
}
```

## Trading Logic

- **Buy signal**: short SMA crosses the long SMA from below
- **Sell signal**: short SMA crosses the long SMA from above
- Position size increases by the current position amount with each new trade (martingale element)
- Order price is set to the current short SMA value, rounded to the instrument's tick size

## Features

- The strategy automatically determines instruments to work with via the `GetWorkingSecurities()` method
- The strategy only works with completed candles
- The strategy tracks indicator crossovers by comparing the current and previous relationship between SMAs
- All active orders are canceled before placing new ones
- Martingale principle is implemented - increasing position size with each new trade
- Indicators and trades are visualized on the chart when a graphic area is available
- Parameter optimization is supported to find optimal strategy settings
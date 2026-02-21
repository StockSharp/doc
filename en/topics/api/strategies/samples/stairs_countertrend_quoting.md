# Countertrend Strategy with Quoting

## Overview

`StairsCountertrendStrategy` is a countertrend trading strategy that opens positions against an established trend of a specific length, using a quoting mechanism for more precise market entry.

## Main Components

```cs
public class StairsCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleDataType;
	private readonly StrategyParam<int> _length;
	private QuotingProcessor _quotingProcessor;

	private int _bullLength;
	private int _bearLength;
}
```

## Strategy Parameters

The strategy allows customizing the following parameters:

- **CandleDataType** - candle type to work with (default 1-minute)
- **Length** - number of consecutive candles in one direction to identify a trend (default 5)

The Length parameter is available for optimization in the range from 2 to 10 with a step of 1.

## Strategy Initialization

In the [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) method, counters are reset, candle subscription is created, and visualization is prepared:

```cs
protected override void OnStarted2(DateTime time)
{
	// Reset counters at start
	_bullLength = 0;
	_bearLength = 0;

	// Create candle subscription
	var subscription = SubscribeCandles(CandleDataType);

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

	base.OnStarted2(time);
}
```

## Processing Candles

The `ProcessCandle` method is called for each completed candle and implements the logic for trend detection and quoting processor management:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Identify bullish or bearish candle
	if (candle.OpenPrice < candle.ClosePrice)
	{
		_bullLength++;
		_bearLength = 0;

		this.AddInfoLog($"Bullish candle detected. Streak: {_bullLength}");
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		_bullLength = 0;
		_bearLength++;

		this.AddInfoLog($"Bearish candle detected. Streak: {_bearLength}");
	}

	// Stop existing processor when direction change is needed
	if (_quotingProcessor != null)
	{
		// Check if processor needs to be cleared (trend change or position)
		var shouldClearProcessor = false;

		// Need to sell if bullish trend and no short position
		if (_bullLength >= Length && Position >= 0)
			shouldClearProcessor = true;
		// Need to buy if bearish trend and no long position
		else if (_bearLength >= Length && Position <= 0)
			shouldClearProcessor = true;

		if (shouldClearProcessor)
		{
			_quotingProcessor?.Dispose();
			_quotingProcessor = null;
		}
	}

	// Create new quoting processor when needed
	if (_quotingProcessor == null && IsFormedAndOnlineAndAllowTrading())
	{
		if (_bullLength >= Length && Position >= 0)
		{
			// Bullish trend - open short position
			CreateQuotingProcessor(Sides.Sell);
			this.AddInfoLog($"Starting SELL quoting after {_bullLength} bullish candles");
		}
		else if (_bearLength >= Length && Position <= 0)
		{
			// Bearish trend - open long position
			CreateQuotingProcessor(Sides.Buy);
			this.AddInfoLog($"Starting BUY quoting after {_bearLength} bearish candles");
		}
	}
}
```

## Creating Quoting Processor

The `CreateQuotingProcessor` method creates a quoting processor with the specified direction:

```cs
private void CreateQuotingProcessor(Sides side)
{
	// Create behavior for market quoting
	var behavior = new MarketQuotingBehavior(
		0, // No price offset
		new Unit(0.1m, UnitTypes.Percent), // Use 0.1% as minimum deviation
		MarketPriceTypes.Following // Follow market price
	);

	// Create quoting processor
	_quotingProcessor = new(
		behavior,
		Security,
		Portfolio,
		side,
		Volume, // Quoting volume
		Volume, // Maximum order volume
		TimeSpan.Zero, // No timeout
		this, // Strategy implements ISubscriptionProvider
		this, // Strategy implements IMarketRuleContainer
		this, // Strategy implements ITransactionProvider
		this, // Strategy implements ITimeProvider
		this, // Strategy implements IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Check trading permission
		true, // Use order book prices
		true // Use last trade price if order book is empty
	)
	{
		Parent = this
	};

	// Subscribe to processor events
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Order {order.TransactionId} registered at price {order.Price}");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Order failed: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk =>
	{
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// Initialize processor
	_quotingProcessor.Start();
}
```

## Trading Logic

- **Sell signal**: `Length` consecutive bullish candles (close price above open price) when there is no short position
- **Buy signal**: `Length` consecutive bearish candles (close price below open price) when there is no long position
- Quoting processor is used for market entry, following the market price

## Features

- The strategy automatically determines instruments to work with via the `GetWorkingSecurities()` method
- The strategy only works with completed candles
- Quoting is used instead of market orders for more efficient market entry
- The strategy applies a countertrend approach, opening positions against the established trend
- Detailed logging of main events for debugging is implemented
- The quoting processor is automatically cleared when the trend direction changes or when targets are reached
- Candles and trades visualization on the chart is supported
- The sequence length parameter optimization is implemented for strategy configuration
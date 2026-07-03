# 逆势策略与报价

## 概览

`StairsCountertrendStrategy` 是一种逆势交易策略，它在特定长度的既定趋势下开仓，使用报价机制以实现更精确的市场入场。

## 主要组件

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

## 策略参数

该策略允许自定义以下参数：

- **CandleDataType** - 要使用的K线类型（默认1分钟）
- **长度** - 用于识别趋势的连续同向K线数量（默认值 5）

Length 参数可在 2 到 10 的范围内进行优化，步长为 1。

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，计数器被重置，K线订阅被创建，并且可视化被准备好：

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

## 处理 K线

`ProcessCandle` 方法会在每根完成的K线上调用，并实现趋势检测和报价处理器管理的逻辑：

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

## 创建引用处理器

`CreateQuotingProcessor` 方法创建一个具有指定方向的引用处理器：

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

## 交易逻辑

- **卖出信号**：`Length` 连续看涨K线（收盘价高于开盘价），且没有空头持仓时
- **买入信号**：`Length` 连续的看跌K线（收盘价低于开盘价），且当前没有多头持仓
- 报价处理器用于市场进入，跟随市场价格

## 特征

- 该策略通过 `GetWorkingSecurities()` 方法自动确定要使用的工具
- 该策略仅适用于已完成的K线
- 为了更高效地进入市场，使用报价而不是市价单
- 该策略采用逆势方法，开仓与已建立的趋势相反的方向
- 已实现主要事件的详细日志记录以进行调试
- 当趋势方向发生变化或达到目标时，引用处理器会自动清除
- 图表支持K线和交易可视化
- 序列长度参数优化已用于策略配置
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
	// 启动时重置计数器
	_bullLength = 0;
	_bearLength = 0;

	// 创建K线订阅
	var subscription = SubscribeCandles(CandleDataType);

	subscription
		.Bind(ProcessCandle)
		.Start();

	// 在图表上设置可视化
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

	// 识别看涨或看跌K线
	if (candle.OpenPrice < candle.ClosePrice)
	{
		_bullLength++;
		_bearLength = 0;

		this.AddInfoLog($"检测到看涨K线，连续数量: {_bullLength}");
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		_bullLength = 0;
		_bearLength++;

		this.AddInfoLog($"检测到看跌K线，连续数量: {_bearLength}");
	}

	// 需要改变方向时停止现有处理器
	if (_quotingProcessor != null)
	{
		// 检查是否需要清理处理器（趋势变化或持仓变化）
		var shouldClearProcessor = false;

		// 如果是看涨趋势且没有空头持仓，需要卖出
		if (_bullLength >= Length && Position >= 0)
			shouldClearProcessor = true;
		// 如果是看跌趋势且没有多头持仓，需要买入
		else if (_bearLength >= Length && Position <= 0)
			shouldClearProcessor = true;

		if (shouldClearProcessor)
		{
			_quotingProcessor?.Dispose();
			_quotingProcessor = null;
		}
	}

	// 需要时创建新的报价处理器
	if (_quotingProcessor == null && IsFormedAndOnlineAndAllowTrading())
	{
		if (_bullLength >= Length && Position >= 0)
		{
			// 看涨趋势 - 开空头持仓
			CreateQuotingProcessor(Sides.Sell);
			this.AddInfoLog($"在 {_bullLength} 根看涨K线后开始卖出报价");
		}
		else if (_bearLength >= Length && Position <= 0)
		{
			// 看跌趋势 - 开多头持仓
			CreateQuotingProcessor(Sides.Buy);
			this.AddInfoLog($"在 {_bearLength} 根看跌K线后开始买入报价");
		}
	}
}
```

## 创建报价处理器

`CreateQuotingProcessor` 方法创建一个具有指定方向的报价处理器：

```cs
private void CreateQuotingProcessor(Sides side)
{
	// 创建市价报价行为
	var behavior = new MarketQuotingBehavior(
		0, // 无价格偏移
		new Unit(0.1m, UnitTypes.Percent), // 使用 0.1% 作为最小偏差
		MarketPriceTypes.Following // 跟随市场价格
	);

	// 创建报价处理器
	_quotingProcessor = new(
		behavior,
		Security,
		Portfolio,
		side,
		Volume, // 报价数量
		Volume, // 最大订单数量
		TimeSpan.Zero, // 无超时
		this, // 策略实现 ISubscriptionProvider
		this, // 策略实现 IMarketRuleContainer
		this, // 策略实现 ITransactionProvider
		this, // 策略实现 ITimeProvider
		this, // 策略实现 IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // 检查交易权限
		true, // 使用订单簿价格
		true // 订单簿为空时使用最后成交价
	)
	{
		Parent = this
	};

	// 订阅处理器事件
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"订单 {order.TransactionId} 已以价格 {order.Price} 注册");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"订单失败: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"成交已执行: {trade.Trade.Volume}，价格 {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk =>
	{
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// 初始化处理器
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
- 当趋势方向发生变化或达到目标时，报价处理器会自动清除
- 图表支持K线和交易可视化
- 序列长度参数优化已用于策略配置

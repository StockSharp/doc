# 报价策略

## 概览

`MqStrategy` 是一种使用报价机制来管理市场持仓的策略。它基于当前持仓创建一个报价处理器，使其能够自适应地响应市场条件的变化。

## 主要组件

```cs
public class MqStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _quotingProcessor;
}
```

## 策略参数

该策略允许自定义以下参数：

- **PriceType** - 报价的市场价格类型（默认 跟随）
- **价格偏移** - 与市场价格的偏移
- **BestPriceOffset** - 报价更新的最小偏差（默认 0.1%）

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，该策略订阅市场时间的变化：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// 订阅市场时间变化以更新报价
	Connector.CurrentTimeChanged += Connector_CurrentTimeChanged;
	Connector_CurrentTimeChanged(default);
}
```

## 管理报价处理器

当市场时间变化时，会调用 `Connector_CurrentTimeChanged` 方法，它管理报价处理器的创建和更新：

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// 仅在当前处理器已停止或不存在时创建新处理器
	if (_quotingProcessor != null && _quotingProcessor.LeftVolume > 0)
		return;

	// 如果旧处理器存在，则释放其资源
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	// 根据当前持仓确定报价方向
	var side = Position <= 0 ? Sides.Buy : Sides.Sell;

	// 创建新的报价行为
	var behavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// 计算报价数量
	var quotingVolume = Volume + Math.Abs(Position);

	// 创建并初始化处理器
	_quotingProcessor = new QuotingProcessor(
		behavior,
		Security,
		Portfolio,
		side,
		quotingVolume,
		Volume, // Maximum order volume
		TimeSpan.Zero, // No timeout
		this, // Strategy implements ISubscriptionProvider
		this, // Strategy implements IMarketRuleContainer
		this, // Strategy implements ITransactionProvider
		this, // Strategy implements ITimeProvider
		this, // Strategy implements IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Check trading permission
		true, // Use order book prices
		true  // Use last trade price if the order book is empty
	)
	{
		Parent = this
	};

	// 订阅处理器事件以记录日志
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Order {order.TransactionId} registered at price {order.Price}");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Order failed: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk => {
		this.AddInfoLog($"Quoting finished with success: {isOk}");
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// 启动处理器
	_quotingProcessor.Start();
}
```

## 资源释放

在 [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped) 方法中，该策略会释放资源：

```cs
protected override void OnStopped()
{
	// 取消订阅以防止内存泄漏
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// 如果当前处理器存在，则释放其资源
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	base.OnStopped();
}
```

## 交易逻辑

- 该策略应对市场时间变化
- 引用方向是根据当前位置确定的：
  - 如果位置 <= 0，则创建一个买入报价
  - 如果持仓 > 0，将创建一个卖出报价
- 报价量的计算方式是基础量加上当前持仓的绝对值
- [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) 与 [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) 一起用于报价

## 特征

- 使用现代引用处理器而不是传统引用策略
- 通过改变报价方向来自适应地响应位置变化
- 支持配置各种报价参数（价格类型、偏移量、最小偏差）
- 包括报价处理器事件的详细日志记录
- 在停止策略和创建新处理器时，正确管理资源
- 支持处理不同类型的市场价格（跟随价、最佳价、相反价等）
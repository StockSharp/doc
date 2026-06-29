# 策略中的交易操作

在 StockSharp 中，[Strategy](xref:StockSharp.Algo.Strategies.Strategy) 类提供了多种处理订单的方法，使得实现交易策略变得方便。

## 下单方式

在 StockSharp 策略中，有几种下单方式：

### 1. 使用高级方法

最简单的方法是使用内置方法，在一次调用中创建并注册订单：

```cs
// Buy at market price
BuyMarket(volume);

// Sell at market price
SellMarket(volume);

// Buy at limit price
BuyLimit(price, volume);

// Sell at limit price
SellLimit(price, volume);

// Close the current position at market price
ClosePosition();
```

这些方法提供了最大的简便性和代码可读性。它们会自动：
- 使用指定的参数创建一个订单对象
- 填写必要的字段（工具、投资组合等）
- 在交易系统中登记订单

### 2. 使用 CreateOrder + RegisterOrder

一种更灵活的方法是将订单的创建和注册分开：

```cs
// Create an order object
var order = CreateOrder(Sides.Buy, price, volume);

// Additional order settings
order.Comment = "My special order";
order.TimeInForce = TimeInForce.MatchOrCancel;

// Register the order
RegisterOrder(order);
```

[CreateOrder](xref:StockSharp.Algo.Strategies.Strategy.CreateOrder(StockSharp.Messages.Sides,System.Decimal,System.Nullable{System.Decimal})) 方法创建一个已初始化的订单对象，该对象可以在注册之前进行进一步自定义。

### 3. 直接创建和注册订单

为了获得最大的控制力，您可以直接创建一个订单对象并注册它：

```cs
// Create an order object directly
var order = new Order
{
	Security = Security,
	Portfolio = Portfolio,
	Side = Sides.Buy,
	Type = OrderTypes.Limit,
	Price = price,
	Volume = volume,
	Comment = "Custom order"
};

// Register the order
RegisterOrder(order);
```

有关处理订单的更多详细信息，请参阅[订单](../orders_management.md)部分。

## 处理订单事件

在注册订单后，跟踪其状态非常重要。在一个策略中，你可以：

### 1. 使用事件处理程序

```cs
// Subscribe to the order received event
OrderReceived += OnOrderReceived;

// Subscribe to the order registration failure event
OrderRegisterFailed += OnOrderRegisterFailed;

private void OnOrderReceived(Order order)
{
	if (order.State == OrderStates.Done)
	{
		// Order executed - perform corresponding logic
	}
}

private void OnOrderRegisterFailed(OrderFail fail)
{
	// Handle order registration error
	LogError($"Order registration error: {fail.Error}");
}
```

### 2. 使用订单规则

一种更强大的方法是使用[规则](event_model.md)来处理订单：

```cs
// Create an order
var order = BuyLimit(price, volume);

// Create a rule that will trigger when the order is executed
order
	.WhenMatched(this)
	.Do(() => {
		// Actions after order execution
		LogInfo($"Order {order.TransactionId} executed");
		
		// For example, place a stop order
		var stopOrder = SellLimit(price * 0.95, volume);
	})
	.Apply(this);

// Rule for handling registration error
order
	.WhenRegisterFailed(this)
	.Do(fail => {
		LogError($"Order registration error: {fail.Error}");
		// Possibly retry with different parameters
	})
	.Apply(this);
```

关于在订单中使用规则的详细示例可以在 [订单规则示例](event_model/samples/rule_order.md) 部分找到。

## 头寸管理

该策略还提供了头寸管理的方法：

```cs
// Get current position
decimal currentPosition = Position;

// Close current position
ClosePosition();

// Protect position with stop-loss and take-profit
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute),   // take-profit
	stopLoss: new Unit(20, UnitTypes.Absolute),     // stop-loss
	isStopTrailing: true,                        // trailing stop
	useMarketOrders: true                        // use market orders
);
```

## 交易前的策略状态

在执行交易操作之前，确保策略处于正确状态非常重要。StockSharp 提供了几个属性和方法来检查策略的准备情况：

### IsFormed 属性

[IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) 属性指示策略中使用的所有指标是否已形成（已准备好）。默认情况下，它会检查添加到 [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) 集合中的所有指标是否处于状态 [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) = `true`。

关于在策略中使用指标的更多信息，请参见 [Indicators in Strategy](indicators.md) 部分。

### 是否在线属性

[IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) 属性显示策略是否处于实时模式。仅当策略已启动且其所有市场数据订阅都已转换为 [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) 状态时，它才会变为 `true`。

有关策略中市场数据订阅的更多详细信息，请参见 [策略中的市场数据订阅](subscriptions.md) 部分。

### 交易模式属性

[TradingMode](xref:StockSharp.Algo.Strategies.Strategy.TradingMode) 属性定义了策略的交易模式。可能的值有：

- [StrategyTradingModes.Full](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Full) - 允许所有交易操作（默认模式）
- [StrategyTradingModes.Disabled](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Disabled) - 交易已完全禁用
- [StrategyTradingModes.CancelOrdersOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.CancelOrdersOnly) - 只允许取消订单
- [StrategyTradingModes.ReducePositionOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.ReducePositionOnly) - 只允许位置减操作

此属性可以通过策略参数进行配置：

```cs
public SmaStrategy()
{
	_tradingMode = Param(nameof(TradingMode), StrategyTradingModes.Full)
					.SetDisplay("Trading Mode", "Allowed trading operations", "Basic settings");
}
```

### 状态检查的辅助方法

为了方便检查策略的交易准备情况，StockSharp 提供了辅助方法：

- [IsFormedAndOnline()](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnline) - 检查策略是否处于状态 `IsFormed = true` 和 `IsOnline = true`

- [IsFormedAndOnlineAndAllowTrading(StrategyTradingModes)](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) - 检查策略是否已形成，是否处于在线模式，以及是否具有必要的交易权限

`IsFormedAndOnlineAndAllowTrading` 方法接受一个可选参数 `required`，类型为 [StrategyTradingModes](xref:StockSharp.Algo.Strategies.StrategyTradingModes)：

```cs
public bool IsFormedAndOnlineAndAllowTrading(StrategyTradingModes required = StrategyTradingModes.Full)
```

此参数允许您指定执行特定操作所需的最低交易权限级别：

1. **StrategyTradingModes.Full**（默认值）——仅当策略处于全交易模式（`TradingMode = StrategyTradingModes.Full`）时才返回 `true`。用于可以增加仓位的操作。

2. **StrategyTradingModes.ReducePositionOnly** - 如果策略处于全交易模式或仅处于减仓模式，则返回 `true`。用于平仓或部分平仓操作。

3. **StrategyTradingModes.CancelOrdersOnly** - 在任何活跃的交易模式下（除了 `Disabled`）返回 `true`。用于订单取消操作。

这允许你根据当前的交易模式有选择地允许或禁止各种交易操作：

```cs
// For placing a new order that increases a position, full trading mode is required
if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.Full))
{
	// We can place any orders
	RegisterOrder(CreateOrder(Sides.Buy, price, volume));
}
// For closing a position, the position reduction mode is sufficient
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly) && Position != 0)
{
	// We can only close the position
	ClosePosition();
}
// For cancelling active orders, the order cancellation mode is sufficient
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
{
	// We can only cancel orders
	CancelActiveOrders();
}
```

因此，这种方法允许您为交易功能实现安全的访问控制机制，其中更关键的操作（例如开立新仓位）需要更高等级的权限，而较不关键的操作（如撤销订单）即使在有限的交易模式下也可以执行。

在执行交易操作之前使用这些方法是良好的实践：

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Check if the strategy is formed and in online mode,
	// and if trading is allowed
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
	
	// Trading logic
	// ...
}
```

## 交易操作示例

下面是一个示例，演示在策略中下单的不同方式以及处理它们的执行方法：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Subscribe to candles
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);
	
	// Create a rule for processing candles
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);
	
	Connector.Subscribe(subscription);
}

private void ProcessCandle(ICandleMessage candle)
{
	// Check if the strategy is ready to trade
	if (!this.IsFormedAndOnlineAndAllowTrading())
		return;
	
	// Example trading logic based on closing price
	if (candle.ClosePrice > _previousClose * 1.01)
	{
		// Option 1: Using a high-level method
		var order = BuyLimit(candle.ClosePrice, Volume);
		
		// Create a rule for handling order execution
		order
			.WhenMatched(this)
			.Do(() => {
				// When the order is executed, set stop-loss and take-profit
				StartProtection(
					takeProfit: new Unit(50, UnitTypes.Absolute),
					stopLoss: new Unit(20, UnitTypes.Absolute)
				);
			})
			.Apply(this);
	}
	else if (candle.ClosePrice < _previousClose * 0.99)
	{
		// Option 2: Separate creation and registration
		var order = CreateOrder(Sides.Sell, candle.ClosePrice, Volume);
		RegisterOrder(order);
		
		// Alternative way of handling through the event
		OrderReceived += (o) => {
			if (o == order && o.State == OrderStates.Done)
			{
				// Actions after execution
			}
		};
	}
	
	_previousClose = candle.ClosePrice;
}
```

## 另请参阅

- [订单](../orders_management.md)
- [订单规则](event_model/samples/rule_order.md)
- [事件模型](event_model.md)
- [仓位保护](take_profit_and_stop_loss.md)

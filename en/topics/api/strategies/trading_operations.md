# Trading Operations in Strategies

In StockSharp, the [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class provides various methods for working with orders, making it convenient to implement trading strategies.

## Order Placement Methods

There are several ways to place orders in StockSharp strategies:

### 1. Using High-Level Methods

The simplest way is to use built-in methods that create and register an order in a single call:

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

These methods provide maximum simplicity and code readability. They automatically:
- Create an order object with the specified parameters
- Fill in the necessary fields (instrument, portfolio, etc.)
- Register the order in the trading system

### 2. Using CreateOrder + RegisterOrder

A more flexible approach is to separate the creation and registration of orders:

```cs
// Create an order object
var order = CreateOrder(Sides.Buy, price, volume);

// Additional order settings
order.Comment = "My special order";
order.TimeInForce = TimeInForce.MatchOrCancel;

// Register the order
RegisterOrder(order);
```

The [CreateOrder](xref:StockSharp.Algo.Strategies.Strategy.CreateOrder(StockSharp.Messages.Sides,System.Decimal,System.Nullable{System.Decimal})) method creates an initialized order object that can be further customized before registration.

### 3. Direct Creation and Registration of an Order

For maximum control, you can create an order object directly and register it:

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

For more details on working with orders, see the [Orders](../orders_management.md) section.

## Handling Order Events

After registering an order, it's important to track its status. In a strategy, you can:

### 1. Use Event Handlers

```cs
// Subscribe to the order change event
OrderChanged += OnOrderChanged;

// Subscribe to the order registration failure event
OrderRegisterFailed += OnOrderRegisterFailed;

private void OnOrderChanged(Order order)
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

### 2. Use Rules for Orders

A more powerful approach is to use [rules](event_model.md) for orders:

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

Detailed examples of using rules with orders can be found in the [Order Rule Examples](event_model/samples/rule_order.md) section.

## Position Management

The strategy also provides methods for position management:

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

## Strategy State Before Trading

Before executing trading operations, it's important to ensure that the strategy is in the correct state. StockSharp provides several properties and methods to check the readiness of the strategy:

### IsFormed Property

The [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) property indicates whether all indicators used in the strategy are formed (warmed up). By default, it checks that all indicators added to the [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) collection are in the state [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) = `true`.

More about working with indicators in a strategy can be found in the [Indicators in Strategy](indicators.md) section.

### IsOnline Property

The [IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) property shows whether the strategy is in real-time mode. It becomes `true` only when the strategy is started and all its market data subscriptions have transitioned to the [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) state.

More details about market data subscriptions in strategies can be found in the [Market Data Subscriptions in Strategies](subscriptions.md) section.

### TradingMode Property

The [TradingMode](xref:StockSharp.Algo.Strategies.Strategy.TradingMode) property defines the trading mode for the strategy. Possible values:

- [StrategyTradingModes.Full](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Full) - all trading operations are allowed (default mode)
- [StrategyTradingModes.Disabled](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Disabled) - trading is completely disabled
- [StrategyTradingModes.CancelOrdersOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.CancelOrdersOnly) - only order cancellation is allowed
- [StrategyTradingModes.ReducePositionOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.ReducePositionOnly) - only position reduction operations are allowed

This property can be configured through strategy parameters:

```cs
public SmaStrategy()
{
	_tradingMode = Param(nameof(TradingMode), StrategyTradingModes.Full)
					.SetDisplay("Trading Mode", "Allowed trading operations", "Basic settings");
}
```

### Helper Methods for State Checking

For convenient checking of the strategy's readiness to trade, StockSharp provides helper methods:

- [IsFormedAndOnline()](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnline) - checks that the strategy is in the state `IsFormed = true` and `IsOnline = true`

- [IsFormedAndOnlineAndAllowTrading(StrategyTradingModes)](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) - checks that the strategy is formed, is in online mode, and has the necessary trading permissions

The `IsFormedAndOnlineAndAllowTrading` method accepts an optional parameter `required` of type [StrategyTradingModes](xref:StockSharp.Algo.Strategies.StrategyTradingModes):

```cs
public bool IsFormedAndOnlineAndAllowTrading(StrategyTradingModes required = StrategyTradingModes.Full)
```

This parameter allows you to specify the minimum level of trading permissions required for a specific operation:

1. **StrategyTradingModes.Full** (default value) - returns `true` only if the strategy is in full trading mode (`TradingMode = StrategyTradingModes.Full`). Used for operations that can increase a position.

2. **StrategyTradingModes.ReducePositionOnly** - returns `true` if the strategy is in full trading mode or in position reduction mode only. Used for position closing or partial closing operations.

3. **StrategyTradingModes.CancelOrdersOnly** - returns `true` with any active trading mode (except `Disabled`). Used for order cancellation operations.

This allows you to selectively permit or prohibit various trading operations depending on the current trading mode:

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

Thus, this method allows you to implement a secure access control mechanism for trading functions, where more critical operations (such as opening new positions) require a higher level of permissions, and less critical ones (cancelling orders) are performed even in a limited trading mode.

It's good practice to use these methods before performing trading operations:

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

## Trading Operations Example

Below is an example demonstrating different ways of placing orders in a strategy and handling their execution:

```cs
protected override void OnStarted(DateTimeOffset time)
{
	base.OnStarted(time);
	
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
		OrderChanged += (o) => {
			if (o == order && o.State == OrderStates.Done)
			{
				// Actions after execution
			}
		};
	}
	
	_previousClose = candle.ClosePrice;
}
```

## See Also

- [Orders](../orders_management.md)
- [Order Rules](event_model/samples/rule_order.md)
- [Event Model](event_model.md)
- [Position Protection](take_profit_and_stop_loss.md)

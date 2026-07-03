# 策略中的市场数据订阅

在 StockSharp 中，策略使用订阅机制来接收市场数据。这种方法是交易策略中获取数据的主要且首选方法。

## 订阅基础

策略中的订阅是基于通用的 [StockSharp 订阅机制](../market_data/subscriptions.md)。它们提供了一种集中统一的方式来获取所有类型的市场数据。

## 在策略中创建订阅

在策略的 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，您可以为所需的数据创建并启动订阅：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Creating a subscription for 5-minute candles directly through DataType
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);
	
	// If additional parameters are required, you can configure the subscription
	subscription.From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(7));
	
	// Creating a rule to process incoming candles
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);
	
	// Starting the subscription
	Connector.Subscribe(subscription);
}
```

在此示例中，使用一个方便的构造函数为5分钟K线创建订阅，该构造函数接受`DataType`和`Security`。如果需要，您还可以额外配置订阅参数，例如历史周期。

## 策略中订阅的优势

在策略中使用订阅相比直接订阅 [Strategy.Connector](xref:StockSharp.Algo.Strategies.Strategy.Connector) 事件有几个优势：

1. **隔离**——每个订阅独立工作，允许为不同的工具接收不同类型的数据而互不干扰。这也保护策略不会接收到为其他并行运行的策略准备的数据。通过直接订阅连接器事件，你还需要额外过滤数据以排除来自其他策略的信息。

2. **状态管理** - 订阅具有清晰的状态（[SubscriptionStates](xref:StockSharp.Messages.SubscriptionStates)），这使得可以准确判断历史数据是否正在接收，或者订阅是否已经转为在线模式。

3. **自动策略状态控制** - 该策略会自动跟踪其所有订阅的状态，只有当所有订阅都处于在线状态时，才会切换到在线模式（[IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline)）。

4. **代码统一性** - 订阅使用统一的方法，与所请求数据的类型无关。

5. **与规则的整合** - 订阅可以通过规则轻松与策略 [事件模型](event_model.md) 集成。

6. **自动订阅管理** - 当策略停止时，所有订阅将自动取消，从而释放资源。

7. **历史数据支持** - 能够在切换到实时数据之前加载历史数据。

## 监控订阅状态

该策略会自动跟踪所有订阅的状态以控制其操作模式：

```cs
private void CheckRefreshOnlineState()
{
	bool nowOnline = ProcessState == ProcessStates.Started;

	if (nowOnline)
		nowOnline = _subscriptions.CachedKeys
			.Where(s => !s.SubscriptionMessage.IsHistoryOnly())
			.All(s => s.State == SubscriptionStates.Online);
	
	// Update strategy's IsOnline state
	IsOnline = nowOnline;
}
```

[Strategy.IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) 属性只有在所有策略订阅都已转为 [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) 状态时才会是 `true`。这使策略能够判断自己何时正在使用当前市场数据。

## 订阅类型

在策略中，您可以订阅各种类型的市场数据：

```cs
// Subscription to candles
var candleSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	Security);

// Subscription to market depth
var depthSubscription = new Subscription(
	DataType.MarketDepth,
	Security);

// Subscription to tick trades
var tickSubscription = new Subscription(
	DataType.Ticks,
	Security);

// Subscription to Level1 (best bid/ask and other basic information)
var level1Subscription = new Subscription(
	DataType.Level1,
	Security);
```

## 通过规则处理订阅数据

要处理通过订阅传入的数据，建议使用 [规则](event_model.md)：

```cs
// Subscription to candles
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), Security);

// Creating a rule for processing incoming candles
Connector
	.WhenCandlesFinished(subscription)  // Rule activation when a completed candle is received
	.Do(ProcessCandle)                   // Call processing method
	.Apply(this);                        // Apply rule to strategy

// Start subscription
Connector.Subscribe(subscription);
```

在上面的例子中，创建了一个规则，在接收到每个完成的K线时将调用 `ProcessCandle` 方法。

## 请求历史数据

该策略通过 [Strategy.HistorySize](xref:StockSharp.Algo.Strategies.Strategy.HistorySize) 属性自动设置历史加载周期：

```cs
// Set history load period to 30 days
strategy.HistorySize = TimeSpan.FromDays(30);
```

在创建订阅时，如果没有明确指定，策略会自动将 `From` 参数设置为加载历史记录。

## 取消订阅

可以通过调用 [UnSubscribe](xref:StockSharp.BusinessEntities.ISubscriptionProvider.UnSubscribe(StockSharp.BusinessEntities.Subscription)) 方法手动取消订阅：

```cs
// Cancel subscription
Connector.UnSubscribe(subscription);
```

在停止策略时，如果 [UnsubscribeOnStop](xref:StockSharp.Algo.Strategies.Strategy.UnsubscribeOnStop) 参数设置为 `true`（默认），所有订阅将被自动取消。

## 另请参阅

- [市场数据订阅](../market_data/subscriptions.md)
- [事件模型](event_model.md)
- [策略平台兼容性](compatibility.md)

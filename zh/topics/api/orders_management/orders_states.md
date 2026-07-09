# 订单状态

StockSharp API 提供通过内置订阅机制接收订单信息的能力。与市场数据一样，交易信息使用基于 [Subscription](xref:StockSharp.BusinessEntities.Subscription) 的统一方法。

## 订单相关事件

[Connector](xref:StockSharp.Algo.Connector) 提供以下用于处理订单信息的事件：

| 事件 | 描述 |
|---------|----------|
| [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) | 接收订单信息的事件 |
| [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) | 订单注册失败事件 |
| [OrderCancelFailReceived](xref:StockSharp.Algo.Connector.OrderCancelFailReceived) | 订单取消失败事件 |
| [OrderEditFailReceived](xref:StockSharp.Algo.Connector.OrderEditFailReceived) | 订单修改失败的事件 |
| [OwnTradeReceived](xref:StockSharp.Algo.Connector.OwnTradeReceived) | 接收有关自身交易信息的事件 |

## 订单状态枚举

在其生命周期内，订单会经历以下状态：

![订单状态](../../../images/orderstates.png)

- [OrderStates.None](xref:StockSharp.Messages.OrderStates.None) - 订单已在交易算法中创建，但尚未发送注册。
- [OrderStates.Pending](xref:StockSharp.Messages.OrderStates.Pending) - 订单已发送以进行注册 ([RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order))。系统正在等待交易所确认其接受。如果接受成功，将触发 [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived) 事件，订单将被转移到 [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) 状态。[Order.Id](xref:StockSharp.BusinessEntities.Order.Id) 和 [Order.ServerTime](xref:StockSharp.BusinessEntities.Order.ServerTime) 属性也将被初始化。如果订单被拒绝，将触发带有错误描述的 [OrderRegisterFailReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderRegisterFailReceived) 事件，订单将转移到 [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) 状态。
- [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) - 该订单在交易所上是活跃的。此类订单将保持活跃状态，直到其全部交易量 [Order.Volume](xref:StockSharp.BusinessEntities.Order.Volume) 执行完毕，或者通过 [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order)) 被强制取消。如果订单被部分执行，关于所下订单的新交易的 [OwnTradeReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OwnTradeReceived) 事件将被触发，同时 [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived) 事件也会触发，该事件会传递关于订单余额变化的通知 [Order.Balance](xref:StockSharp.BusinessEntities.Order.Balance)。在订单取消的情况下，后一个事件也会被触发。
- [OrderStates.Done](xref:StockSharp.Messages.OrderStates.Done) - 该订单在交易所上不再有效（已完全执行或已取消）。
- [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) - 由于某种原因，该订单未被交易所（或中间系统，例如交易平台的服务器部分）接受。

## 自动订阅

默认情况下，[Connector](xref:StockSharp.Algo.Connector) 在连接时自动创建交易信息的订阅 ([SubscriptionsOnConnect](xref:StockSharp.Algo.Connector.SubscriptionsOnConnect))。这包括以下订阅：

- 订单信息
- 贸易信息
- 位置信息
- 基本交易品种查询

处理订单接收事件的示例：

```cs
private void InitConnector()
{
	// 订阅订单接收事件
	Connector.OrderReceived += OnOrderReceived;
	
	// 订阅 own trade 接收事件
	Connector.OwnTradeReceived += OnOwnTradeReceived;
	
	// 订阅订单注册失败事件
	Connector.OrderRegisterFailReceived += OnOrderRegisterFailed;
}

private void OnOrderReceived(Subscription subscription, Order order)
{
	// 处理收到的订单
	_ordersWindow.OrderGrid.Orders.TryAdd(order);
	
	// 重要！检查订单是否属于当前订阅
	// 以避免重复处理
	if (subscription == _myOrdersSubscription)
	{
		// 针对特定订阅的附加处理
		Console.WriteLine($"订单: {order.TransactionId}, 状态: {order.State}");
	}
}
```

## 手动创建订单订阅

在某些情况下，您可能需要明确地请求有关订单的信息。为此，您可以创建单独的订阅：

```cs
// 为特定投资组合的订单创建订阅
var ordersSubscription = new Subscription(DataType.Transactions, portfolio)
{
	TransactionId = Connector.TransactionIdGenerator.GetNextId(),
};

// 订单接收处理器
Connector.OrderReceived += (subscription, order) =>
{
	if (subscription == ordersSubscription)
	{
		Console.WriteLine($"订单: {order.TransactionId}, 状态: {order.State}, 投资组合: {order.Portfolio.Name}");
	}
};

// 启动订阅
Connector.Subscribe(ordersSubscription);
```

## 检查订单状态

扩展方法用于确定订单的当前状态：

```cs
// 检查订单状态
Order order = ...; // 收到的订单

// 订单是否已撤销
bool isCanceled = order.IsCanceled();

// 订单是否已完全成交
bool isMatched = order.IsMatched();

// 订单是否部分成交
bool isPartiallyMatched = order.IsMatchedPartially();

// 订单是否至少部分成交
bool isNotEmpty = order.IsMatchedEmpty();

// 获取已成交数量
decimal matchedVolume = order.GetMatchedVolume();
```

## 高级方法：处理多个订阅

在复杂的场景中，您可能需要同时处理多个订单订阅。在这种情况下，正确处理事件以避免重复是很重要的：

```cs
private Subscription _portfolio1OrdersSubscription;
private Subscription _portfolio2OrdersSubscription;

private void RequestOrdersForDifferentPortfolios()
{
	// 第一个投资组合订单的订阅
	_portfolio1OrdersSubscription = new Subscription(DataType.Transactions, _portfolio1);
	
	// 第二个投资组合订单的订阅
	_portfolio2OrdersSubscription = new Subscription(DataType.Transactions, _portfolio2);
	
	// 订单接收通用处理器
	Connector.OrderReceived += OnMultipleSubscriptionOrderReceived;
	
	// 启动订阅
	Connector.Subscribe(_portfolio1OrdersSubscription);
	Connector.Subscribe(_portfolio2OrdersSubscription);
}

private void OnMultipleSubscriptionOrderReceived(Subscription subscription, Order order)
{
	// 确定订单属于哪个订阅
	if (subscription == _portfolio1OrdersSubscription)
	{
		// 处理第一个投资组合的订单
	}
	else if (subscription == _portfolio2OrdersSubscription)
	{
		// 处理第二个投资组合的订单
	}
}
```

> [!NOTE]
> 只有在标准订阅机制不足的特殊情况下，才应使用这种具有多重订单订阅的先进方法。

## 交易的异步性质

交易发送（订单的注册、替换或取消）是异步执行的。这允许交易程序不必等待来自交易所的确认，而是继续工作，从而加快对市场状况变化的反应。

要跟踪订单的状态，您需要订阅相应的事件：
- [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) 用于接收订单状态更新
- [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) 用于处理注册错误

## 另请参阅

- [订阅](../market_data/subscriptions.md)
- [订单状态](orders_states.md)
- [创建新订单](create_new_order.md)
- [取消订单](order_cancel.md)

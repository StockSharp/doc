# 交易操作

在为交易所创建自己的适配器时，有必要实现执行交易操作的方法，例如注册、替换和取消订单。当从 StockSharp 核心接收到相应消息时，将调用这些方法。

## 订单注册

要注册新订单，实现了 **RegisterOrderAsync** 方法。接收到 [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) 消息时会调用此方法。

注册订单时的主要步骤如下：

1. 检查订单类型和附加条件。
2. 将订单参数转换为交易所能理解的格式。
3. 通过交易所 API 发送请求以注册订单。
4. 处理来自交易所的响应并发送相应的[ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)消息。

```cs
public override async ValueTask RegisterOrderAsync(OrderRegisterMessage regMsg, CancellationToken cancellationToken)
{
	var condition = (CoinbaseOrderCondition)regMsg.Condition;

	switch (regMsg.OrderType)
	{
		case null:
		case OrderTypes.Limit:
		case OrderTypes.Market:
			break;
		case OrderTypes.Conditional:
		{
			// Handling conditional orders, for example, withdrawal of funds
			if (!condition.IsWithdraw)
				break;

			var withdrawId = await _restClient.Withdraw(regMsg.SecurityId.SecurityCode, regMsg.Volume, condition.WithdrawInfo, cancellationToken);

			await SendOutMessageAsync(new ExecutionMessage
			{
				DataTypeEx = DataType.Transactions,
				OrderStringId = withdrawId,
				ServerTime = CurrentTime.ConvertToUtc(),
				OriginalTransactionId = regMsg.TransactionId,
				OrderState = OrderStates.Done,
				HasOrderInfo = true,
			}, cancellationToken);

			await PortfolioLookupAsync(null, cancellationToken);
			return;
		}
		default:
			throw new NotSupportedException(LocalizedStrings.OrderUnsupportedType.Put(regMsg.OrderType, regMsg.TransactionId));
	}

	// Determining the order type (market or limit)
	var isMarket = regMsg.OrderType == OrderTypes.Market;
	var price = isMarket ? (decimal?)null : regMsg.Price;
	
	// Sending the order to the exchange
	var result = await _restClient.RegisterOrder(
		regMsg.TransactionId.To<string>(), regMsg.SecurityId.ToSymbol(),
		regMsg.OrderType.ToNative(), regMsg.Side.ToNative(), price,
		condition?.StopPrice, regMsg.Volume, regMsg.TimeInForce,
		regMsg.TillDate.EnsureToday(), regMsg.Leverage, cancellationToken);

	var orderState = result.Status.ToOrderState();

	// Processing the order registration result
	if (orderState == OrderStates.Failed)
	{
		await SendOutMessageAsync(new ExecutionMessage
		{
			DataTypeEx = DataType.Transactions,
			ServerTime = result.CreationTime,
			OriginalTransactionId = regMsg.TransactionId,
			OrderState = OrderStates.Failed,
			Error = new InvalidOperationException(),
			HasOrderInfo = true,
		}, cancellationToken);
	}
}
```

## 订单更换

要替换现有订单，已实现 **ReplaceOrderAsync** 方法。收到 [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) 消息时会调用此方法。

替换订单的主要步骤如下：

1. 检查在交易所替换订单的可能性。
2. 通过交易所 API 发送请求以更换订单。
3. 处理来自交易所的响应并发送相应的[ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)消息。

```cs
public override async ValueTask ReplaceOrderAsync(OrderReplaceMessage replaceMsg, CancellationToken cancellationToken)
{
	// Sending a request to replace the order
	await _restClient.EditOrder(
		replaceMsg.OldOrderId.To<string>(), 
		replaceMsg.Price, 
		replaceMsg.Volume, 
		cancellationToken);
	
	// Note: Processing the order replacement result usually occurs
	// in a separate method that is called when receiving an update from the exchange
}
```

### 订单更换详情

在实现订单替换方法时，重要的是要考虑交易所协议的具体细节。为此，StockSharp 提供了 [MessageAdapter.IsReplaceCommandEditCurrent](xref:StockSharp.Messages.MessageAdapter.IsReplaceCommandEditCurrent) 属性。

如果交易所协议假定在修改订单时保留其旧标识符，则需要重写此属性并返回 `true`。这向 StockSharp 表示，在替换订单时，不需要从交易所期望新的标识符。

public override bool IsReplaceCommandEditCurrent => true;

如果在修改订单时，旧订单被取消并且用新的交易所标识符注册新订单，则不需要重写此属性。默认情况下，它返回 `false`，这对应于大多数交易所的行为。

## 订单取消

要取消现有订单，已实现 **CancelOrderAsync** 方法。接收到 [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) 消息时会调用此方法。

取消订单的主要步骤如下：

1. 检查订单标识符的存在。
2. 通过交易所 API 发送取消订单的请求。
3. 处理来自交易所的响应并发送相应的[ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)消息。

```cs
public override async ValueTask CancelOrderAsync(OrderCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	// Checking the presence of the order identifier
	if (cancelMsg.OrderStringId.IsEmpty())
		throw new InvalidOperationException(LocalizedStrings.OrderNoExchangeId.Put(cancelMsg.OriginalTransactionId));

	// Sending a request to cancel the order
	await _restClient.CancelOrder(cancelMsg.OrderStringId, cancellationToken);

	// Note: Processing the order cancellation result usually occurs
	// in a separate method that is called when receiving an update from the exchange
}
```

## 批量订单取消

一些交易所支持批量撤单功能，该功能允许通过一次请求取消多个或所有活动订单。这在某些市场条件下，对于快速平仓或清理订单簿非常有用。

要在适配器中实现批量撤单，通常使用 **CancelOrderGroupAsync** 方法。当接收到 [OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage) 消息时会调用此方法。

值得注意的是，并非所有交易所都支持此功能。例如，Coinbase 并未提供批量撤单的 API。在这种情况下，可能需要实现对单个订单的顺序撤单。

下面是一个批量订单取消方法实现的示例，取自支持此功能的[BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp) 连接器：

```cs
public override async ValueTask CancelOrderGroupAsync(OrderGroupCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	await _httpClient.CancelAllOrders(cancellationToken);
}
```

重要的是不要忘记从适配器构造函数中删除对该命令类型支持的删除：

```cs
//this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);
```

## 跟踪订单状态

在 Coinbase 以及其他一些现代交易所的情况下，订单状态更新是通过 WebSocket 连接进行广播的。这意味着在执行交易操作（注册、替换或取消订单）之后，不需要立即通过 REST API 请求新的订单状态。相反，适配器会通过已建立的 WebSocket 连接自动接收更新。

处理这些更新的方法类似于 `SessionOnOrderReceived`，这在[请求组合和订单的当前状态](portfolio_and_orders_state.md)一节中已讨论过。每当交易所发送关于订单状态的更新时，无论该更新是由用户操作触发还是交易所自身变化触发，该方法都会被调用。

这种方法可以更高效地跟踪订单状态，减少交易所 API 的负载，并确保实时接收更新。然而，在实现自己的适配器时，有必要仔细研究所使用交易所的 API 文档，以正确配置和处理这些 WebSocket 更新。

## 错误处理

在执行交易操作时，正确处理可能的错误和异常非常重要。如果发生错误，则需要发送带有设置了 [Error](xref:StockSharp.Messages.ExecutionMessage.Error) 属性的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息。

## 实现细节

在实现用于处理交易操作的方法时，有必要考虑特定交易所的具体情况：

- 支持的订单类型（市价单、限价单、止损单等）。
- 订单标识符的格式。
- 用于处理订单的交易所 API 的具体细节。
- 发送请求频率可能的限制。
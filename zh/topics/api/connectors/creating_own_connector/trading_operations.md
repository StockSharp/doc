# 交易操作

在为交易所创建自己的适配器时，需要实现执行交易操作的方法，例如注册、替换和撤销订单。当 StockSharp 核心收到相应消息时，会调用这些方法。

## 订单注册

要注册新订单，需要实现 **RegisterOrderAsync** 方法。收到 [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) 消息时会调用此方法。

注册订单时的主要步骤如下：

1. 检查订单类型和附加条件。
2. 将订单参数转换为交易所能理解的格式。
3. 通过交易所 API 发送订单注册请求。
4. 处理交易所响应，并发送相应的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息。

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

## 订单替换

要替换现有订单，需要实现 **ReplaceOrderAsync** 方法。收到 [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) 消息时会调用此方法。

替换订单的主要步骤如下：

1. 检查交易所是否支持订单替换。
2. 通过交易所 API 发送订单替换请求。
3. 处理交易所响应，并发送相应的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息。

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

### 订单替换的细节

实现订单替换时，需要考虑交易所协议的具体行为。StockSharp 为此提供了 [MessageAdapter.IsReplaceCommandEditCurrent](xref:StockSharp.Messages.MessageAdapter.IsReplaceCommandEditCurrent) 属性。

如果交易所协议在修改订单时保留旧标识符，则需要重写此属性并返回 `true`。这样 StockSharp 就知道，替换订单时不应等待交易所返回新的标识符。

```cs
public override bool IsReplaceCommandEditCurrent => true;
```

如果修改订单时旧订单会被撤销，并以新的交易所标识符注册新订单，则不需要重写此属性。默认情况下它返回 `false`，这符合大多数交易所的行为。

## 订单撤销

要撤销现有订单，需要实现 **CancelOrderAsync** 方法。收到 [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) 消息时会调用此方法。

撤销订单的主要步骤如下：

1. 检查订单标识符的存在。
2. 通过交易所 API 发送撤单请求。
3. 处理交易所响应，并发送相应的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息。

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

## 批量撤单

一些交易所支持批量撤单功能，允许通过一次请求撤销多个或全部活动订单。在某些市场条件下，这对于快速平仓或清理订单簿非常有用。

要在适配器中实现批量撤单，通常使用 **CancelOrderGroupAsync** 方法。当接收到 [OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage) 消息时会调用此方法。

需要注意的是，并非所有交易所都支持此功能。例如，Coinbase 并未提供批量撤单的 API。在这种情况下，可能需要实现对单个订单的顺序撤单。

下面是批量撤单方法的实现示例，取自支持此功能的 [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp) 连接器：

```cs
public override async ValueTask CancelOrderGroupAsync(OrderGroupCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	await _httpClient.CancelAllOrders(cancellationToken);
}
```

注意不要在适配器构造函数中移除此命令类型的支持：

```cs
//this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);
```

## 跟踪订单状态

在 Coinbase 以及其他一些现代交易所中，订单状态更新会通过 WebSocket 连接广播。这意味着执行交易操作（注册、替换或撤销订单）后，不需要立即通过 REST API 请求新的订单状态。适配器会通过已建立的 WebSocket 连接自动接收更新。

处理这些更新的方法类似于 `SessionOnOrderReceived`，这在[请求投资组合和订单的当前状态](portfolio_and_orders_state.md)一节中已经讨论过。每当交易所发送订单状态更新时，无论该更新是由用户操作触发，还是由交易所自身状态变化触发，都会调用该方法。

这种方式可以更高效地跟踪订单状态，降低交易所 API 负载，并确保实时接收更新。不过，在实现自己的适配器时，需要仔细研究所用交易所的 API 文档，才能正确配置和处理这些 WebSocket 更新。

## 错误处理

执行交易操作时，必须正确处理可能出现的错误和异常。如果发生错误，需要发送设置了 [Error](xref:StockSharp.Messages.ExecutionMessage.Error) 属性的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息。

## 实现细节

实现处理交易操作的方法时，需要考虑具体交易所的特点：

- 支持的订单类型（市价单、限价单、止损单等）。
- 订单标识符的格式。
- 交易所处理订单的 API 细节。
- 可能存在的请求频率限制。

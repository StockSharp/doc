# Información sobre carteras y órdenes

Al crear su propio adaptador para trabajar con una bolsa, debe implementar métodos para solicitar el estado actual de la cartera y las órdenes. Estos métodos se llaman al recibir los mensajes [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) y [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) respectivamente.

## Solicitud del estado de la cartera

Para solicitar el estado de la cartera, se implementa el método **PortfolioLookupAsync**. Este método normalmente realiza las siguientes acciones:

1. Envía una confirmación de recepción de la solicitud usando [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)).
2. Comprueba si la solicitud es de suscripción o cancelación de suscripción usando la propiedad [IsSubscribe](xref:StockSharp.Messages.PortfolioLookupMessage.IsSubscribe).
3. En caso de suscripción:
  - Envía un mensaje [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage) con información sobre la cartera.
  - Solicita los saldos actuales de la cuenta desde la bolsa.
  - Para cada cuenta, crea y envía un mensaje [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) con información sobre la posición.
4. Envía un mensaje sobre el resultado de la suscripción usando [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask PortfolioLookupAsync(PortfolioLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	var transId = lookupMsg.TransactionId;

	// Enviar confirmación de recepción de la solicitud
	await SendSubscriptionReplyAsync(transId, cancellationToken);

	if (!lookupMsg.IsSubscribe)
		return;

	// Enviar mensaje con información de la cartera
	await SendOutMessageAsync(new PortfolioMessage
	{
		PortfolioName = PortfolioName,
		BoardCode = BoardCodes.Coinbase,
		OriginalTransactionId = transId,
	}, cancellationToken);

	// Solicitar saldos actuales de la cuenta
	var accounts = await _restClient.GetAccounts(cancellationToken);

	foreach (var account in accounts)
	{
		// Para cada cuenta, crear y enviar un mensaje con información sobre la posición
		await SendOutMessageAsync(new PositionChangeMessage
		{
			PortfolioName = PortfolioName,
			SecurityId = new SecurityId
			{
				SecurityCode = account.Currency,
				BoardCode = BoardCodes.Coinbase,
			},
			ServerTime = CurrentTime.ConvertToUtc(),
		}
		.TryAdd(PositionChangeTypes.CurrentValue, (decimal)account.Available, true)
		.TryAdd(PositionChangeTypes.BlockedValue, (decimal)account.Hold, true), cancellationToken);
	}

	// Enviar un mensaje sobre la finalización correcta de la suscripción
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

## Solicitud del estado de las órdenes

Para solicitar el estado de las órdenes, se implementa el método **OrderStatusAsync**. Este método normalmente realiza las siguientes acciones:

1. Envía una confirmación de recepción de la solicitud usando [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)).
2. Comprueba si la solicitud es de suscripción o cancelación de suscripción usando la propiedad [OrderStatusMessage.IsSubscribe](xref:StockSharp.Messages.OrderStatusMessage.IsSubscribe).
3. En caso de suscripción:
  - Solicita la lista de órdenes actuales a la bolsa.
  - Para cada orden, crea y envía un mensaje [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) con información sobre la orden.
  - Si es necesario, configura una suscripción para recibir actualizaciones de órdenes en tiempo real.
4. Envía un mensaje sobre el resultado de la suscripción usando [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask OrderStatusAsync(OrderStatusMessage statusMsg, CancellationToken cancellationToken)
{
	// Enviar confirmación de recepción de la solicitud
	await SendSubscriptionReplyAsync(statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsSubscribe)
		return;

	// Solicitar lista de órdenes actuales
	var orders = await _restClient.GetOrders(cancellationToken);

	foreach (var order in orders)
		await ProcessOrder(order, statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsHistoryOnly())
	{
		// Configurar suscripción para recibir actualizaciones de órdenes en tiempo real
		await _socketClient.SubscribeOrders(cancellationToken);
	}

	// Enviar un mensaje sobre la finalización correcta de la suscripción
	await SendSubscriptionResultAsync(statusMsg, cancellationToken);
}

private async ValueTask ProcessOrder(Order order, long originTransId, CancellationToken cancellationToken)
{
	if (!long.TryParse(order.ClientOrderId, out var transId))
		return;

	var state = order.Status.ToOrderState();

	// Crear y enviar mensaje con información de la orden
	await SendOutMessageAsync(new ExecutionMessage
	{
		ServerTime = originTransId == 0 ? CurrentTime.ConvertToUtc() : order.CreationTime,
		DataTypeEx = DataType.Transactions,
		SecurityId = order.Product.ToStockSharp(),
		TransactionId = originTransId == 0 ? 0 : transId,
		OriginalTransactionId = originTransId,
		OrderState = state,
		Error = state == OrderStates.Failed ? new InvalidOperationException() : null,
		OrderType = order.Type.ToOrderType(),
		Side = order.Side.ToSide(),
		OrderStringId = order.Id,
		OrderPrice = order.Price?.ToDecimal() ?? 0,
		OrderVolume = order.Size?.ToDecimal(),
		TimeInForce = order.TimeInForce.ToTimeInForce(),
		Balance = (decimal?)order.LeavesQuantity,
		HasOrderInfo = true,
	}, cancellationToken);
}
```

## Procesamiento de actualizaciones en tiempo real

Para procesar las actualizaciones del estado de las órdenes en tiempo real, normalmente se implementa un método separado, que se llama al recibir los eventos correspondientes del cliente WebSocket:

```cs
private async ValueTask SessionOnOrderReceived(Order order, CancellationToken cancellationToken)
{
	// Procesar actualización de orden recibida
	// OriginTransId = 0, porque es una actualización en tiempo real, no una respuesta a una solicitud concreta
	await ProcessOrder(order, 0, cancellationToken);
}
```

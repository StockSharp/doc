# Operaciones de trading

Al crear su propio adaptador para un exchange, implemente los métodos que realizan las operaciones de trading: registro, reemplazo y cancelación de órdenes. Estos métodos se llaman cuando el adaptador recibe los mensajes correspondientes del núcleo de StockSharp.

## Registro de órdenes

Para registrar una nueva orden, se implementa el método **RegisterOrderAsync**. Este método se llama al recibir el mensaje [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage).

Los pasos principales al registrar una orden:

1. Comprobación del tipo de orden y las condiciones adicionales.
2. Conversión de los parámetros de la orden a un formato comprensible para el exchange.
3. Envío de una solicitud para registrar la orden a través de la API del exchange.
4. Procesamiento de la respuesta del exchange y envío del mensaje [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) correspondiente.

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

## Reemplazo de órdenes

Para reemplazar una orden existente, se implementa el método **ReplaceOrderAsync**. Este método se llama al recibir el mensaje [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage).

Los pasos principales al reemplazar una orden:

1. Comprobación de la posibilidad de reemplazar la orden en el exchange.
2. Envío de una solicitud para reemplazar la orden a través de la API del exchange.
3. Procesamiento de la respuesta del exchange y envío del mensaje [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) correspondiente.

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

### Particularidades del reemplazo de órdenes

Al implementar el reemplazo de órdenes, tenga en cuenta el protocolo del exchange. StockSharp proporciona la propiedad [MessageAdapter.IsReplaceCommandEditCurrent](xref:StockSharp.Messages.MessageAdapter.IsReplaceCommandEditCurrent) para ello.

Si el protocolo del exchange modifica una orden manteniendo el identificador antiguo, sobrescriba esta propiedad y devuelva `true`. Esto le indica a StockSharp que el reemplazo de una orden no debe esperar un nuevo identificador del exchange.

```cs
public override bool IsReplaceCommandEditCurrent => true;
```

Si, al cambiar una orden, la antigua se cancela y se registra una nueva con un nuevo identificador de exchange, entonces no es necesario sobrescribir esta propiedad. Por defecto, devuelve `false`, lo que corresponde al comportamiento de la mayoría de los exchanges.

## Cancelación de órdenes

Para cancelar una orden existente, se implementa el método **CancelOrderAsync**. Este método se llama al recibir el mensaje [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage).

Los pasos principales al cancelar una orden:

1. Comprobación de la presencia del identificador de la orden.
2. Envío de una solicitud para cancelar la orden a través de la API del exchange.
3. Procesamiento de la respuesta del exchange y envío del mensaje [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) correspondiente.

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

## Cancelación masiva de órdenes

Algunos exchanges admiten la función de cancelación masiva de órdenes, que permite cancelar varias o todas las órdenes activas con una sola solicitud. Esto puede ser útil para cerrar posiciones rápidamente o limpiar el libro de órdenes bajo ciertas condiciones de mercado.

Para implementar la cancelación masiva de órdenes en el adaptador, normalmente se utiliza el método **CancelOrderGroupAsync**. Este método se llama al recibir el mensaje [OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage).

No todos los exchanges admiten esta función. Por ejemplo, Coinbase no proporciona una API para la cancelación masiva de órdenes. En tales casos, implemente la cancelación secuencial de órdenes individuales si es necesario.

A continuación se muestra un ejemplo de implementación del método de cancelación masiva de órdenes, tomado del conector [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp), que admite esta función:

```cs
public override async ValueTask CancelOrderGroupAsync(OrderGroupCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	await _httpClient.CancelAllOrders(cancellationToken);
}
```

No elimine el soporte de este tipo de comando en el constructor del adaptador:

```cs
//this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);
```

## Seguimiento del estado de las órdenes

En el caso de Coinbase, así como de algunos otros exchanges modernos, las actualizaciones del estado de las órdenes se transmiten a través de una conexión WebSocket. Esto significa que, después de realizar operaciones de trading (registro, reemplazo o cancelación de una orden), no es necesario solicitar inmediatamente el nuevo estado de la orden a través de la API REST. En su lugar, el adaptador recibirá las actualizaciones automáticamente a través de la conexión WebSocket establecida.

El procesamiento de estas actualizaciones ocurre en un método similar a `SessionOnOrderReceived`, que se trató en la sección sobre [solicitud del estado actual de la cartera y las órdenes](portfolio_and_orders_state.md). Este método se llama cada vez que el exchange envía una actualización sobre el estado de la orden, independientemente de si esta actualización fue provocada por acciones del usuario o por cambios en el propio exchange.

Este enfoque permite realizar un seguimiento más eficiente de los estados de las órdenes, reduce la carga sobre la API del exchange y proporciona actualizaciones en tiempo real. Al implementar su propio adaptador, estudie cuidadosamente la documentación de la API del exchange para que estas actualizaciones de WebSocket se configuren y manejen correctamente.

## Manejo de errores

Al realizar operaciones de trading, maneje correctamente los posibles errores y excepciones. Si se produce un error, envíe un mensaje [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) con la propiedad [Error](xref:StockSharp.Messages.ExecutionMessage.Error) establecida.

## Particularidades de la implementación

Al implementar los métodos de operaciones de trading, tenga en cuenta las particularidades de cada exchange en concreto:

- Tipos de órdenes admitidos (mercado, límite, órdenes stop, etc.).
- Formato de los identificadores de órdenes.
- Particularidades de la API del exchange para trabajar con órdenes.
- Posibles restricciones sobre la frecuencia de envío de solicitudes.

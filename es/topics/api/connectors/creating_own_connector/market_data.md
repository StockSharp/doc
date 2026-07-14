# Datos de Mercado

Al crear su propio adaptador para trabajar con una bolsa, necesita implementar métodos para suscribirse a varios tipos de datos de mercado. Estos métodos se llaman cuando se recibe un mensaje [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) y proporcionan **para** recibir y procesar datos de la bolsa.

Esquemáticamente, el algoritmo para procesar una solicitud de suscripción o cancelación de suscripción se ve así:

1. Envía una confirmación de recepción de la solicitud de suscripción utilizando el método [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)).
2. Comprueba si la solicitud es una suscripción o una cancelación de suscripción utilizando la propiedad [MarketDataMessage.IsSubscribe](xref:StockSharp.Messages.MarketDataMessage.IsSubscribe).
3. En caso de suscripción, configura una suscripción para recibir datos en tiempo real a través de WebSocket u otro mecanismo (específico de cada bolsa).
4. En caso de cancelación de suscripción, cancela la suscripción correspondiente (específico de cada bolsa).
5. Envía un mensaje sobre el resultado de la suscripción utilizando los métodos [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) o [SendSubscriptionFinishedAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionFinishedAsync(System.Int64,System.Nullable{System.DateTimeOffset})), según el tipo de suscripción y el resultado de la operación.

## Datos de Velas

Al implementar una suscripción a datos de velas en su propio adaptador, tenga en cuenta cómo trabaja la bolsa con este tipo de datos. En Coinbase, se sobrescribieron los siguientes métodos y propiedades:

### Marcos Temporales Admitidos

La propiedad `TimeFrames` define la lista de marcos temporales admitidos por el adaptador para las velas. Esto permite a StockSharp saber qué marcos temporales se pueden solicitar a través de este adaptador.

```cs
protected override IEnumerable<TimeSpan> TimeFrames { get; } = Extensions.TimeFrames.Keys.ToArray();
```

### Soporte para Actualizaciones de Velas

El método `IsSupportCandlesUpdates` determina si el adaptador admite actualizaciones de velas en tiempo real para una solicitud de suscripción específica. En el caso de Coinbase, solo se admiten actualizaciones para velas de 5 minutos.

```cs
private static readonly DataType _tf5min = DataType.TimeFrame(TimeSpan.FromMinutes(5));

public override bool IsSupportCandlesUpdates(MarketDataMessage subscription)
{
	// Coinbase solo admite velas de 5 minutos para actualización vía WebSocket
	// Por lo tanto, otros marcos temporales se construirán a partir de ticks (automáticamente por el núcleo de StockSharp)
	return subscription.DataType2 == _tf5min;
}
```

Sobrescribir estos métodos y propiedades permite que el adaptador maneje correctamente las solicitudes de suscripción a datos de velas, teniendo en cuenta las particularidades de la API de Coinbase. Por ejemplo, si se solicita un marco temporal distinto de 5 minutos, StockSharp sabrá que necesita usar datos de ticks para construir velas de otros marcos temporales.

### Suscripción a Datos de Velas

Para suscribirse a datos de velas, se implementa el método **OnTFCandlesSubscriptionAsync**. Este método, al igual que el método de suscripción a datos de ticks, puede solicitar datos históricos, así como configurar una suscripción para recibir nuevas velas en tiempo real.

```cs
protected override async ValueTask OnTFCandlesSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmación de recepción de la solicitud de suscripción
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		var tf = mdMsg.GetTimeFrame();

		// Si se solicitan datos históricos
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix();
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix();
			var left = mdMsg.Count ?? long.MaxValue;
			var step = (long)tf.Multiply(200).TotalSeconds;
			var granularity = mdMsg.GetTimeFrame().ToNative();

			while (from < to)
			{
				// Solicitar velas históricas
				var candles = await _restClient.GetCandles(symbol, from, from + step, granularity, cancellationToken);
				var needBreak = true;
				var last = from;

				foreach (var candle in candles.OrderBy(t => t.Time))
				{
					cancellationToken.ThrowIfCancellationRequested();

					var time = (long)candle.Time.ToUnix();

					if (time < from)
						continue;

					if (time > to)
					{
						needBreak = true;
						break;
					}

					// Enviar información de cada vela histórica
					await SendOutMessageAsync(new TimeFrameCandleMessage
					{
						OpenPrice = (decimal)candle.Open,
						ClosePrice = (decimal)candle.Close,
						HighPrice = (decimal)candle.High,
						LowPrice = (decimal)candle.Low,
						TotalVolume = (decimal)candle.Volume,
						OpenTime = candle.Time,
						State = CandleStates.Finished,

						// Al identificar datos por suscripción no es necesario rellenar la información del instrumento
						OriginalTransactionId = mdMsg.TransactionId,
					}, cancellationToken);

					if (--left <= 0)
					{
						needBreak = true;
						break;
					}

					last = time;
					needBreak = false;
				}

				if (needBreak)
					break;

				from = last;
			}
		}

		if (!mdMsg.IsHistoryOnly() && mdMsg.DataType2 == _tf5min)
		{
			// Suscribirse para recibir nuevas velas en tiempo real
			_candlesTransIds[symbol] = mdMsg.TransactionId;
			await _socketClient.SubscribeCandles(symbol, cancellationToken);

			// Notificar que la suscripción pasó al estado online
			await SendSubscriptionResultAsync(mdMsg, cancellationToken);
		}
		else
		{
			// Enviar una respuesta indicando que la suscripción ha finalizado (no online)
			await SendSubscriptionFinishedAsync(mdMsg.TransactionId, cancellationToken);
		}
	}
	else
	{
		// Cancelar recepción de velas
		_candlesTransIds.Remove(symbol);
		await _socketClient.UnSubscribeCandles(symbol, cancellationToken);
	}
}
```

### Procesamiento de Datos de Velas

Para procesar los datos de velas recibidos de la bolsa en tiempo real, generalmente se implementa un método con código como en el método **SessionOnCandleReceived**. Este método convierte los datos recibidos en un mensaje [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) y lo envía utilizando el método SendOutMessageAsync.

```cs
private async ValueTask SessionOnCandleReceived(Ohlc candle, CancellationToken cancellationToken)
{
	// Comprobar si hay una suscripción activa a velas para este instrumento
	if (!_candlesTransIds.TryGetValue(candle.Symbol, out var transId))
		return;

	// Crear y enviar mensaje sobre una nueva vela
	await SendOutMessageAsync(new TimeFrameCandleMessage
	{
		OpenPrice = (decimal)candle.Open,
		ClosePrice = (decimal)candle.Close,
		HighPrice = (decimal)candle.High,
		LowPrice = (decimal)candle.Low,
		TotalVolume = (decimal)candle.Volume,
		OpenTime = candle.Time,
		State = CandleStates.Active,  // La vela se considera activa porque aún puede cambiar

		// Al identificar datos por suscripción no es necesario rellenar la información del instrumento
		OriginalTransactionId = transId,
	}, cancellationToken);
}
```

## Nivel 1 (Mejores Precios de Compra y Venta, Último Precio)

### Suscripción a Datos de Nivel 1

Para suscribirse a los cambios de Nivel 1, se implementa el método **OnLevel1SubscriptionAsync**. Este método generalmente realiza las siguientes acciones:

```cs
protected override async ValueTask OnLevel1SubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmación de recepción de la solicitud de suscripción
	// Esto informa al sistema de que la solicitud fue recibida y se está procesando
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Convertir el identificador del instrumento en un símbolo entendido por la bolsa
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Si es una solicitud de suscripción
		// Suscribirse para recibir datos Level1 vía WebSocket
		await _socketClient.SubscribeTicker(symbol, cancellationToken);

		// Enviar un mensaje sobre la suscripción correcta
		// Esto informa al sistema de que la suscripción está configurada y se recibirán datos
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Si es una solicitud de cancelación de suscripción
		// Cancelar suscripción para recibir datos Level1
		await _socketClient.UnSubscribeTicker(symbol, cancellationToken);
	}
}
```

### Procesamiento de Datos de Nivel 1

Para procesar los datos de Nivel 1 recibidos de la bolsa en tiempo real, generalmente se implementa un método con código como en el ejemplo **SessionOnTickerChanged**. Este método convierte los datos recibidos en un mensaje [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) y lo envía utilizando el método SendOutMessageAsync.

```cs
private async ValueTask SessionOnTickerChanged(Ticker ticker, CancellationToken cancellationToken)
{
	// Crear mensaje con cambios de datos Level1
	await SendOutMessageAsync(new Level1ChangeMessage
	{
		// Especificar identificador del instrumento
		SecurityId = ticker.Product.ToStockSharp(),
		// Establecer hora de recepción de datos
		ServerTime = CurrentTime.ConvertToUtc(),
	}
	// Añadir varios campos Level1 si están presentes en los datos de la bolsa
	.TryAdd(Level1Fields.LastTradeId, ticker.LastTradeId)
	.TryAdd(Level1Fields.LastTradePrice, ticker.LastTradePrice?.ToDecimal())
	.TryAdd(Level1Fields.LastTradeVolume, ticker.LastTradePrice?.ToDecimal())
	.TryAdd(Level1Fields.LastTradeOrigin, ticker.LastTradeSide?.ToSide())
	.TryAdd(Level1Fields.HighPrice, ticker.High?.ToDecimal())
	.TryAdd(Level1Fields.LowPrice, ticker.Low?.ToDecimal())
	.TryAdd(Level1Fields.Volume, ticker.Volume?.ToDecimal())
	.TryAdd(Level1Fields.Change, ticker.Change?.ToDecimal())
	.TryAdd(Level1Fields.BestBidPrice, ticker.Bid?.ToDecimal())
	.TryAdd(Level1Fields.BestAskPrice, ticker.Ask?.ToDecimal())
	.TryAdd(Level1Fields.BestBidVolume, ticker.BidSize?.ToDecimal())
	.TryAdd(Level1Fields.BestAskVolume, ticker.AskSize?.ToDecimal())
	, cancellationToken);
}
```

## Libro de Órdenes

### Soporte para Actualizaciones Incrementales del Libro de Órdenes

Al implementar la funcionalidad del libro de órdenes en su propio adaptador, compruebe si la bolsa admite actualizaciones incrementales del libro de órdenes. El adaptador de Coinbase sobrescribe la propiedad `IsSupportOrderBookIncrements` para esto:

```cs
public override bool IsSupportOrderBookIncrements => true;
```

La propiedad `IsSupportOrderBookIncrements` indica si el adaptador admite actualizaciones incrementales del libro de órdenes. Establecer esta propiedad en `true` significa que la bolsa puede enviar actualizaciones parciales del libro de órdenes en lugar de una instantánea completa con cada cambio.

Sobrescribir esta propiedad permite que StockSharp optimice el procesamiento de los datos del libro de órdenes. Si la propiedad se establece en `true`, el sistema esperará y manejará correctamente las actualizaciones incrementales.

### Suscripción a Datos del Libro de Órdenes

Para suscribirse a los cambios del libro de órdenes, se implementa el método **OnMarketDepthSubscriptionAsync**. Este método realiza acciones similares al método OnLevel1SubscriptionAsync, pero para los datos del libro de órdenes.

```cs
protected override async ValueTask OnMarketDepthSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmación de recepción de la solicitud de suscripción
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Convertir el identificador del instrumento en un símbolo entendido por la bolsa
	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Si es una solicitud de suscripción
		// Suscribirse para recibir datos del libro de órdenes vía WebSocket
		await _socketClient.SubscribeOrderBook(symbol, cancellationToken);

		// Enviar un mensaje sobre la suscripción correcta
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Si es una solicitud de cancelación de suscripción
		// Cancelar suscripción para recibir datos del libro de órdenes
		await _socketClient.UnSubscribeOrderBook(symbol, cancellationToken);
	}
}
```

### Procesamiento de Datos del Libro de Órdenes

Para procesar los datos del libro de órdenes recibidos de la bolsa en tiempo real, generalmente se implementa un método con código como en el método **SessionOnOrderBookReceived**. Este método convierte los datos recibidos en un mensaje [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) y lo envía utilizando el método SendOutMessageAsync.

```cs
private async ValueTask SessionOnOrderBookReceived(string type, string symbol, IEnumerable<OrderBookChange> changes, CancellationToken cancellationToken)
{
	var bids = new List<QuoteChange>();
	var asks = new List<QuoteChange>();

	// Distribuir cambios por bids y asks
	foreach (var change in changes)
	{
		var side = change.Side.ToSide();
		var quotes = side == Sides.Buy ? bids : asks;
		quotes.Add(new((decimal)change.Price, (decimal)change.Size));
	}

	// Crear y enviar mensaje con cambios en el libro de órdenes
	await SendOutMessageAsync(new QuoteChangeMessage
	{
		SecurityId = symbol.ToStockSharp(),
		Bids = bids.ToArray(),
		Asks = asks.ToArray(),
		ServerTime = CurrentTime.ConvertToUtc(),

		// Determinar si es una instantánea completa del libro de órdenes o una actualización incremental.
		// Si la bolsa siempre envía solo libros de órdenes completos y no admite incrementalidad,
		// entonces no es necesario establecer esta propiedad
		State = type == "snapshot" ? QuoteChangeStates.SnapshotComplete : QuoteChangeStates.Increment,
	}, cancellationToken);
}
```

## Datos de Ticks (Operaciones)

### Suscripción a Datos de Ticks

Para suscribirse a datos de ticks, se implementa el método **OnTicksSubscriptionAsync**. Este método, además de realizar acciones similares a los métodos de suscripción anteriores, también puede solicitar datos históricos si se especifica en la solicitud.

```cs
protected override async ValueTask OnTicksSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmación de recepción de la solicitud de suscripción
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	var symbol = mdMsg.SecurityId.ToSymbol();

	if (mdMsg.IsSubscribe)
	{
		// Si se solicitan datos históricos
		if (mdMsg.From is not null)
		{
			var from = (long)mdMsg.From.Value.ToUnix(false);
			var to = (long)(mdMsg.To ?? DateTimeOffset.UtcNow).ToUnix(false);
			var left = mdMsg.Count ?? long.MaxValue;

			while (from < to)
			{
				// Solicitar operaciones históricas
				var trades = await _restClient.GetTrades(symbol, from, to, cancellationToken);
				var needBreak = true;
				var last = from;

				foreach (var trade in trades.OrderBy(t => t.Time))
				{
					cancellationToken.ThrowIfCancellationRequested();

					var time = (long)trade.Time.ToUnix();

					if (time < from)
						continue;

					if (time > to)
					{
						needBreak = true;
						break;
					}

					// Enviar información de cada operación histórica
					await SendOutMessageAsync(new ExecutionMessage
					{
						// Indicar que el mensaje contiene información sobre una operación tick
						// (no una transacción como una orden o una operación propia)
						DataTypeEx = DataType.Ticks,

						TradeId = trade.TradeId,
						TradePrice = trade.Price?.ToDecimal(),
						TradeVolume = trade.Size?.ToDecimal(),
						ServerTime = trade.Time,
						OriginSide = trade.Side.ToSide(),

						// Para el historial, establezca siempre el identificador de suscripción,
						// para que el código externo entienda para qué suscripción se recibieron los datos.
						// Al identificar datos por suscripción no es necesario rellenar la información del instrumento
						OriginalTransactionId = mdMsg.TransactionId,
					}, cancellationToken);

					if (--left <= 0)
					{
						needBreak = true;
						break;
					}

					last = time;
					needBreak = false;
				}

				if (needBreak)
					break;

				from = last;
			}
		}

		if (!mdMsg.IsHistoryOnly())
		{
			// Suscribirse para recibir nuevas operaciones en tiempo real
			await _socketClient.SubscribeTrades(symbol, cancellationToken);
		}

		// Enviar un mensaje sobre la suscripción correcta
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
	{
		// Cancelar recepción de operaciones en tiempo real
		await _socketClient.UnSubscribeTrades(symbol, cancellationToken);
	}
}
```

### Procesamiento de Datos de Ticks

Para procesar los datos de ticks recibidos de la bolsa en tiempo real, generalmente se implementa un método con código como en el método **SessionOnTradeReceived**. Este método convierte los datos recibidos en un mensaje [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) con el tipo [DataType.Ticks](xref:StockSharp.Messages.DataType.Ticks) y lo envía utilizando el método SendOutMessageAsync.

```cs
private async ValueTask SessionOnTradeReceived(Trade trade, CancellationToken cancellationToken)
{
	// Crear y enviar mensaje sobre una nueva operación
	await SendOutMessageAsync(new ExecutionMessage
	{
		// Indicar que el mensaje contiene información sobre una operación tick
		// (no una transacción como una orden o una operación propia)
		DataTypeEx = DataType.Ticks,

		SecurityId = trade.ProductId.ToStockSharp(),
		TradeId = trade.TradeId,
		TradePrice = (decimal)trade.Price,
		TradeVolume = (decimal)trade.Size,
		ServerTime = trade.Time,
		OriginSide = trade.Side.ToSide(),
	}, cancellationToken);
}
```

## Suscripción al Registro de Órdenes

El registro de órdenes es información detallada sobre todos los cambios en el libro de órdenes, incluyendo la adición, modificación y eliminación de órdenes. Estos datos son específicos y no están disponibles en todas las fuentes de datos. Por ejemplo, Coinbase no admite el suministro de un registro de órdenes.

Para implementar una suscripción a un registro de órdenes en un adaptador, se utiliza el método **OnOrderLogSubscriptionAsync**. Este método se llama cuando se recibe un mensaje [MarketDataMessage](xref:StockSharp.Messages.MarketDataMessage) con el tipo de datos [DataType.OrderLog](xref:StockSharp.Messages.DataType.OrderLog).

A continuación se muestra un ejemplo de implementación de este método tomado del conector [BitStamp](https://github.com/StockSharp/Connectors/tree/main/BitStamp), que admite el registro de órdenes:

```cs
protected override async ValueTask OnOrderLogSubscriptionAsync(MarketDataMessage mdMsg, CancellationToken cancellationToken)
{
	// Enviar confirmación de recepción de la solicitud de suscripción
	await SendSubscriptionReplyAsync(mdMsg.TransactionId, cancellationToken);

	// Convertir identificador del instrumento en par de divisas
	var symbol = mdMsg.SecurityId.ToCurrency();

	if (mdMsg.IsSubscribe)
	{
		if (!mdMsg.IsHistoryOnly())
		{
			// Suscribirse para recibir registro de órdenes en tiempo real
			await _pusherClient.SubscribeOrderLog(symbol, cancellationToken);
		}

		// Enviar un mensaje sobre la suscripción correcta
		await SendSubscriptionResultAsync(mdMsg, cancellationToken);
	}
	else
		// Cancelar recepción del registro de órdenes
		await _pusherClient.UnSubscribeOrderLog(symbol, cancellationToken);
}
```

Al procesar los datos del registro de órdenes recibidos de la bolsa, generalmente se utiliza un método separado, que convierte los datos recibidos en mensajes [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) con el tipo [ExecutionTypes.OrderLog](xref:StockSharp.Messages.ExecutionTypes.OrderLog):

```cs
private async ValueTask SessionOnNewOrderLog(string symbol, OrderStates state, Order order, CancellationToken cancellationToken)
{
	// Crear y enviar mensaje con información sobre una nueva entrada en el registro de órdenes
	await SendOutMessageAsync(new ExecutionMessage
	{
		DataTypeEx = DataType.OrderLog,
		SecurityId = symbol.ToStockSharp(),
		ServerTime = order.Time,
		OrderVolume = (decimal)order.Amount,
		OrderPrice = (decimal)order.Price,
		OrderId = order.Id,
		Side = order.Type.ToSide(),
		OrderState = state,
	}, cancellationToken);
}
```

Añada soporte para este tipo de datos en el constructor del adaptador:

```cs
this.AddSupportedMarketDataType(DataType.OrderLog);
```

## Particularidades del Procesamiento de Datos Históricos y en Vivo

Al implementar solicitudes de datos históricos y el procesamiento de datos en vivo en su propio adaptador, tenga en cuenta los siguientes puntos:

### Datos Históricos

Al enviar datos históricos en respuesta a una solicitud:

1. Establecer [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) es obligatorio. Esto permite que el sistema asocie los datos recibidos con la solicitud original.

2. Establecer [SecurityId](xref:StockSharp.Messages.SecurityId) o [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) (en el caso de las velas) no es obligatorio, pero tampoco está prohibido. El núcleo de StockSharp rellenará automáticamente estos campos con los valores necesarios de la solicitud original.

### Datos en Vivo

Al procesar datos en vivo, por ejemplo, recibidos a través de WebSocket:

1. Establecer [OriginalTransactionId](xref:StockSharp.Messages.IOriginalTransactionIdMessage.OriginalTransactionId) es opcional. Si no se establece el ID de transacción, el sistema distribuirá los datos a todas las suscripciones activas para el instrumento y el tipo de datos correspondientes.

2. Establecer [SecurityId](xref:StockSharp.Messages.SecurityId) y otros campos específicos (por ejemplo, [TimeFrameCandleMessage.TimeFrame](xref:StockSharp.Messages.TimeFrameCandleMessage.TimeFrame) para las velas) es obligatorio, ya que esta información es necesaria para el correcto enrutamiento de los datos en el sistema.
</content>

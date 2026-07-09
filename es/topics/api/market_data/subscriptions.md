# Suscripciones

**StockSharp API** ofrece un modelo de adquisición de datos basado en suscripciones. Es un mecanismo universal para recibir tanto datos de mercado como información transaccional. Este enfoque tiene ventajas significativas:

- **Aislamiento de suscripciones** — cada suscripción funciona de forma independiente, lo que permite ejecutar en paralelo cualquier número de suscripciones con distintos parámetros (con o sin solicitud de histórico).
- **Seguimiento de estado** — las suscripciones tienen estados específicos que permiten controlar si actualmente están llegando datos históricos o si la suscripción cambió al modo en tiempo real.
- **Universalidad** — el código para trabajar con suscripciones es el mismo independientemente de los tipos de datos solicitados, lo que hace más eficiente el desarrollo.

Para trabajar con suscripciones, debe usar la clase [Subscription](xref:StockSharp.BusinessEntities.Subscription). Veamos ejemplos de uso de suscripciones para obtener varios tipos de datos.

## Ejemplo de suscripción a velas

```cs
// Crear una suscripción para velas de 5 minutos
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	// Configurar parámetros de suscripción mediante la propiedad MarketData
	MarketData =
	{
		// Solicitar datos de los últimos 30 días
		From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
		// null significa que la suscripción cambiará al modo en tiempo real después de recibir el histórico
		To = null
	}
};

// Procesamiento de velas recibidas
_connector.CandleReceived += (sub, candle) =>
{
	if (sub != subscription)
		return;

	// Procesar la vela
	Console.WriteLine($"Candle: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
};

// Manejo de la transición de la suscripción al modo online
_connector.SubscriptionOnline += (sub) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine("Subscription switched to real-time mode");
};

// Manejo de errores de suscripción
_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine($"Error de suscripción: {error}");
};

// Iniciar la suscripción
_connector.Subscribe(subscription);
```

## Ejemplo de suscripción al libro de órdenes

```cs
// Crear una suscripción al libro de órdenes del instrumento seleccionado
var depthSubscription = new Subscription(DataType.MarketDepth, security);

// Procesamiento de libros de órdenes recibidos
_connector.OrderBookReceived += (sub, depth) =>
{
	if (sub != depthSubscription)
		return;

	// Procesar el libro de órdenes
	Console.WriteLine($"Libro de órdenes: {depth.SecurityId}, hora: {depth.ServerTime}");
	Console.WriteLine($"Bids: {depth.Bids.Count}, Asks: {depth.Asks.Count}");
};

// Iniciar la suscripción
_connector.Subscribe(depthSubscription);
```

## Ejemplo de suscripción a tick trades

```cs
// Crear una suscripción a tick trades del instrumento seleccionado
var tickSubscription = new Subscription(DataType.Ticks, security);

// Procesamiento de ticks recibidos
_connector.TickTradeReceived += (sub, tick) =>
{
	if (sub != tickSubscription)
		return;

	// Procesar el tick
	Console.WriteLine($"Tick: {tick.SecurityId}, Time: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
};

// Iniciar la suscripción
_connector.Subscribe(tickSubscription);
```

## Ejemplo de suscripción con configuración del modo de construcción de velas

```cs
// Suscripción a velas de 5 minutos que se construirán a partir de ticks
var candleSubscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	MarketData =
	{
		// Especificar el modo de construcción y la fuente de datos
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		// También puede habilitar la construcción de perfil de volumen
		IsCalcVolumeProfile = true,
	}
};

_connector.Subscribe(candleSubscription);
```

## Ejemplo de suscripción Level1 (información básica del instrumento)

```cs
// Crear una suscripción para información básica del instrumento
var level1Subscription = new Subscription(DataType.Level1, security);

// Procesamiento de datos Level1 recibidos
_connector.Level1Received += (sub, level1) =>
{
	if (sub != level1Subscription)
		return;

	Console.WriteLine($"Level1: {level1.SecurityId}, Time: {level1.ServerTime}");

	// Mostrar valores de campos Level1
	foreach (var pair in level1.Changes)
	{
		Console.WriteLine($"Field: {pair.Key}, Value: {pair.Value}");
	}
};

// Iniciar la suscripción
_connector.Subscribe(level1Subscription);
```

## Cancelación de suscripción a datos

Para dejar de recibir datos, use el método `UnSubscribe`:

```cs
// Cancelar una suscripción específica
_connector.UnSubscribe(subscription);

// O puede cancelar todas las suscripciones
foreach (var sub in _connector.Subscriptions)
{
	_connector.UnSubscribe(sub);
}
```

## Estados de suscripción

Las suscripciones pueden estar en los siguientes estados:

- [SubscriptionStates.Stopped](xref:StockSharp.Messages.SubscriptionStates.Stopped) — la suscripción está inactiva (detenida o no iniciada).
- [SubscriptionStates.Active](xref:StockSharp.Messages.SubscriptionStates.Active) — la suscripción está activa y puede transmitir datos históricos hasta cambiar al modo en tiempo real o finalizar.
- [SubscriptionStates.Error](xref:StockSharp.Messages.SubscriptionStates.Error) — la suscripción está inactiva y en estado de error.
- [SubscriptionStates.Finished](xref:StockSharp.Messages.SubscriptionStates.Finished) — la suscripción ha completado su trabajo (todos los datos recibidos).
- [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) — la suscripción cambió al modo en tiempo real y transmite solo datos actuales.

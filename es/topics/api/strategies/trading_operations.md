# Operaciones de negociación en estrategias

En StockSharp, la clase [Strategy](xref:StockSharp.Algo.Strategies.Strategy) proporciona varios métodos para trabajar con órdenes, lo que facilita la implementación de estrategias de negociación.

## Métodos de colocación de órdenes

Hay varias formas de colocar órdenes en estrategias StockSharp:

### 1. Uso de métodos de alto nivel

La forma más simple es usar métodos integrados que crean y registran una orden en una sola llamada:

```cs
// Comprar a precio de mercado
BuyMarket(volume);

// Vender a precio de mercado
SellMarket(volume);

// Comprar a precio límite
BuyLimit(price, volume);

// Vender a precio límite
SellLimit(price, volume);

// Cerrar la posición actual a precio de mercado
ClosePosition();
```

Estos métodos proporcionan máxima simplicidad y legibilidad del código. Automáticamente:
- Crean un objeto de orden con los parámetros especificados
- Rellenan los campos necesarios (instrumento, cartera, etc.)
- Registran la orden en el sistema de negociación

### 2. Uso de CreateOrder + RegisterOrder

Un enfoque más flexible consiste en separar la creación y el registro de órdenes:

```cs
// Crear un objeto de orden
var order = CreateOrder(Sides.Buy, price, volume);

// Ajustes adicionales de la orden
order.Comment = "Mi orden especial";
order.TimeInForce = TimeInForce.MatchOrCancel;

// Registrar la orden
RegisterOrder(order);
```

El método [CreateOrder](xref:StockSharp.Algo.Strategies.Strategy.CreateOrder(StockSharp.Messages.Sides,System.Decimal,System.Nullable{System.Decimal})) crea un objeto de orden inicializado que puede personalizarse antes del registro.

### 3. Creación y registro directos de una orden

Para el máximo control, puede crear directamente un objeto de orden y registrarlo:

```cs
// Crear directamente un objeto de orden
var order = new Order
{
	Security = Security,
	Portfolio = Portfolio,
	Side = Sides.Buy,
	Type = OrderTypes.Limit,
	Price = price,
	Volume = volume,
	Comment = "Orden personalizada"
};

// Registrar la orden
RegisterOrder(order);
```

Para obtener más detalles sobre el trabajo con órdenes, consulte la sección [Órdenes](../orders_management.md).

## Manejo de eventos de órdenes

Después de registrar una orden, es importante seguir su estado. En una estrategia, puede:

### 1. Usar controladores de eventos

```cs
// Suscribirse al evento de orden recibida
OrderReceived += OnOrderReceived;

// Suscribirse al evento de fallo de registro de orden
OrderRegisterFailed += OnOrderRegisterFailed;

private void OnOrderReceived(Order order)
{
	if (order.State == OrderStates.Done)
	{
		// Orden ejecutada: realizar la lógica correspondiente
	}
}

private void OnOrderRegisterFailed(OrderFail fail)
{
	// Manejar error de registro de orden
	LogError($"Error al registrar la orden: {fail.Error}");
}
```

### 2. Usar reglas para órdenes

Un enfoque más potente es usar [reglas](event_model.md) para órdenes:

```cs
// Crear una orden
var order = BuyLimit(price, volume);

// Crear una regla que se activará cuando la orden se ejecute
order
	.WhenMatched(this)
	.Do(() => {
		// Acciones después de la ejecución de la orden
		LogInfo($"Orden {order.TransactionId} ejecutada");

		// Por ejemplo, colocar una orden stop
		var stopOrder = SellLimit(price * 0.95, volume);
	})
	.Apply(this);

// Regla para manejar error de registro
order
	.WhenRegisterFailed(this)
	.Do(fail => {
		LogError($"Error al registrar la orden: {fail.Error}");
		// Posiblemente reintentar con parámetros distintos
	})
	.Apply(this);
```

Puede encontrar ejemplos detallados de uso de reglas con órdenes en la sección [Ejemplos de reglas de órdenes](event_model/samples/rule_order.md).

## Gestión de posiciones

La estrategia también proporciona métodos para la gestión de posiciones:

```cs
// Obtener posición actual
decimal currentPosition = Position;

// Cerrar posición actual
ClosePosition();

// Proteger posición con stop-loss y take-profit
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute),   // take-profit
	stopLoss: new Unit(20, UnitTypes.Absolute),     // stop-loss
	isStopTrailing: true,                        // stop dinámico
	useMarketOrders: true                        // usar órdenes de mercado
);
```

## Estado de la estrategia antes de operar

Antes de ejecutar operaciones de negociación, es importante asegurarse de que la estrategia esté en el estado correcto. StockSharp proporciona varias propiedades y métodos para comprobar la preparación de la estrategia:

### Propiedad IsFormed

La propiedad [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) indica si todos los indicadores usados en la estrategia están formados (calentados). De forma predeterminada, comprueba que todos los indicadores agregados a la colección [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) estén en el estado [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) = `true`.

Puede encontrar más información sobre el trabajo con indicadores en una estrategia en la sección [Indicadores en estrategias](indicators.md).

### Propiedad IsOnline

La propiedad [IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) muestra si la estrategia está en modo de tiempo real. Se vuelve `true` solo cuando la estrategia se ha iniciado y todas sus suscripciones a datos de mercado han pasado al estado [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online).

Puede encontrar más detalles sobre suscripciones a datos de mercado en estrategias en la sección [Suscripciones a datos de mercado en estrategias](subscriptions.md).

### Propiedad TradingMode

La propiedad [TradingMode](xref:StockSharp.Algo.Strategies.Strategy.TradingMode) define el modo de negociación para la estrategia. Valores posibles:

- [StrategyTradingModes.Full](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Full) - todas las operaciones de negociación están permitidas (modo predeterminado)
- [StrategyTradingModes.Disabled](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Disabled) - la negociación está completamente deshabilitada
- [StrategyTradingModes.CancelOrdersOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.CancelOrdersOnly) - solo se permite cancelar órdenes
- [StrategyTradingModes.ReducePositionOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.ReducePositionOnly) - solo se permiten operaciones de reducción de posición

Esta propiedad se puede configurar mediante parámetros de estrategia:

```cs
public SmaStrategy()
{
	_tradingMode = Param(nameof(TradingMode), StrategyTradingModes.Full)
					.SetDisplay("Modo de negociación", "Operaciones de negociación permitidas", "Ajustes básicos");
}
```

### Métodos auxiliares para comprobar el estado

Para comprobar cómodamente si la estrategia está lista para operar, StockSharp proporciona métodos auxiliares:

- [IsFormedAndOnline()](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnline) - comprueba que la estrategia esté en el estado `IsFormed = true` e `IsOnline = true`

- [IsFormedAndOnlineAndAllowTrading(StrategyTradingModes)](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) - comprueba que la estrategia esté formada, esté en modo online y tenga los permisos de negociación necesarios

El método `IsFormedAndOnlineAndAllowTrading` acepta un parámetro opcional `required` de tipo [StrategyTradingModes](xref:StockSharp.Algo.Strategies.StrategyTradingModes):

```cs
public bool IsFormedAndOnlineAndAllowTrading(StrategyTradingModes required = StrategyTradingModes.Full)
```

Este parámetro permite especificar el nivel mínimo de permisos de negociación requerido para una operación concreta:

1. **StrategyTradingModes.Full** (valor predeterminado) - devuelve `true` solo si la estrategia está en modo de negociación completo (`TradingMode = StrategyTradingModes.Full`). Se usa para operaciones que pueden aumentar una posición.

2. **StrategyTradingModes.ReducePositionOnly** - devuelve `true` si la estrategia está en modo de negociación completo o solo en modo de reducción de posición. Se usa para operaciones de cierre total o parcial de posición.

3. **StrategyTradingModes.CancelOrdersOnly** - devuelve `true` con cualquier modo de negociación activo (excepto `Disabled`). Se usa para operaciones de cancelación de órdenes.

Esto permite permitir o prohibir selectivamente varias operaciones de negociación según el modo de negociación actual:

```cs
// Para colocar una nueva orden que aumente una posición, se requiere modo de negociación completo
if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.Full))
{
	// Podemos colocar cualquier orden
	RegisterOrder(CreateOrder(Sides.Buy, price, volume));
}
// Para cerrar una posición, el modo de reducción de posición es suficiente
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly) && Position != 0)
{
	// Solo podemos cerrar la posición
	ClosePosition();
}
// Para cancelar órdenes activas, el modo de cancelación de órdenes es suficiente
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
{
	// Solo podemos cancelar órdenes
	CancelActiveOrders();
}
```

Así, este método permite implementar un mecanismo seguro de control de acceso para funciones de negociación, donde las operaciones más críticas (como abrir nuevas posiciones) requieren un nivel de permisos más alto, y las menos críticas (cancelar órdenes) se realizan incluso en un modo de negociación limitado.

Es una buena práctica usar estos métodos antes de realizar operaciones de negociación:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Comprobar si la estrategia está formada y en modo online,
	// y si la negociación está permitida
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Lógica de negociación
	// ...
}
```

## Ejemplo de operaciones de negociación

A continuación se muestra un ejemplo que demuestra distintas formas de colocar órdenes en una estrategia y manejar su ejecución:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Suscribirse a velas
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);

	// Crear una regla para procesar velas
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);

	Connector.Subscribe(subscription);
}

private void ProcessCandle(ICandleMessage candle)
{
	// Comprobar si la estrategia está lista para operar
	if (!this.IsFormedAndOnlineAndAllowTrading())
		return;

	// Ejemplo de lógica de negociación basada en el precio de cierre
	if (candle.ClosePrice > _previousClose * 1.01)
	{
		// Opción 1: usar un método de alto nivel
		var order = BuyLimit(candle.ClosePrice, Volume);

		// Crear una regla para manejar la ejecución de la orden
		order
			.WhenMatched(this)
			.Do(() => {
				// Cuando la orden se ejecuta, establecer stop-loss y take-profit
				StartProtection(
					takeProfit: new Unit(50, UnitTypes.Absolute),
					stopLoss: new Unit(20, UnitTypes.Absolute)
				);
			})
			.Apply(this);
	}
	else if (candle.ClosePrice < _previousClose * 0.99)
	{
		// Opción 2: creación y registro separados
		var order = CreateOrder(Sides.Sell, candle.ClosePrice, Volume);
		RegisterOrder(order);

		// Forma alternativa de manejo mediante el evento
		OrderReceived += (o) => {
			if (o == order && o.State == OrderStates.Done)
			{
				// Acciones después de la ejecución
			}
		};
	}

	_previousClose = candle.ClosePrice;
}
```

## Ver también

- [Órdenes](../orders_management.md)
- [Reglas de órdenes](event_model/samples/rule_order.md)
- [Modelo de eventos](event_model.md)
- [Protección de posiciones](take_profit_and_stop_loss.md)

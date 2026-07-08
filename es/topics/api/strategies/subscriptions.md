# Suscripciones a datos de mercado en estrategias

En StockSharp, las estrategias usan un mecanismo de suscripción para recibir datos de mercado. Este enfoque es el método principal y recomendado para obtener datos en estrategias de trading.

## Fundamentos de las suscripciones

Las suscripciones en estrategias se basan en el [mecanismo general de suscripciones de StockSharp](../market_data/subscriptions.md). Proporcionan una forma centralizada y unificada de obtener todos los tipos de datos de mercado.

## Crear una suscripción en una estrategia

En el método [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) de la estrategia, puede crear e iniciar una suscripción para los datos requeridos:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Crear una suscripción para velas de 5 minutos directamente mediante DataType
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);
	
	// Si se requieren parámetros adicionales, puede configurar la suscripción
	subscription.From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(7));
	
	// Crear una regla para procesar velas entrantes
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);
	
	// Iniciar la suscripción
	Connector.Subscribe(subscription);
}
```

En este ejemplo, se crea una suscripción para velas de 5 minutos mediante un constructor práctico que acepta `DataType` y `Security`. Si es necesario, puede configurar adicionalmente parámetros de la suscripción, como el periodo de historial.

## Ventajas de las suscripciones en estrategias

El uso de suscripciones en estrategias tiene varias ventajas frente a la suscripción directa a eventos de [Strategy.Connector](xref:StockSharp.Algo.Strategies.Strategy.Connector):

1. **Aislamiento** - cada suscripción funciona de forma independiente, lo que permite recibir distintos tipos de datos para distintos instrumentos sin interferencia mutua. Esto también protege a la estrategia de recibir datos destinados a otras estrategias que se ejecutan en paralelo. Con la suscripción directa a eventos del conector, tendría que filtrar adicionalmente los datos para excluir información de otras estrategias.

2. **Gestión de estado** - las suscripciones tienen estados claros ([SubscriptionStates](xref:StockSharp.Messages.SubscriptionStates)), que permiten determinar con precisión si se están recibiendo datos históricos o si la suscripción ya pasó al modo online.

3. **Control automático del estado de la estrategia** - la estrategia realiza seguimiento automáticamente del estado de todas sus suscripciones y pasa al modo online ([IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline)) solo cuando todas las suscripciones están online.

4. **Uniformidad de código** - las suscripciones usan un enfoque unificado, independiente del tipo de datos solicitado.

5. **Integración con reglas** - las suscripciones se integran fácilmente con el [Modelo de eventos](event_model.md) de la estrategia mediante reglas.

6. **Gestión automática de suscripciones** - cuando la estrategia se detiene, todas sus suscripciones se cancelan automáticamente y liberan recursos.

7. **Soporte de datos históricos** - posibilidad de cargar datos históricos antes de pasar a datos en tiempo real.

## Supervisión de estados de suscripción

La estrategia realiza seguimiento automáticamente del estado de todas las suscripciones para controlar su modo de funcionamiento:

```cs
private void CheckRefreshOnlineState()
{
	bool nowOnline = ProcessState == ProcessStates.Started;

	if (nowOnline)
		nowOnline = _subscriptions.CachedKeys
			.Where(s => !s.SubscriptionMessage.IsHistoryOnly())
			.All(s => s.State == SubscriptionStates.Online);
	
	// Actualizar el estado IsOnline de la estrategia
	IsOnline = nowOnline;
}
```

La propiedad [Strategy.IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) será `true` solo cuando todas las suscripciones de la estrategia hayan pasado al estado [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online). Esto permite a la estrategia comprender el momento en que trabaja con datos de mercado actuales.

## Tipos de suscripciones

En las estrategias, puede usar suscripciones a distintos tipos de datos de mercado:

```cs
// Suscripción a velas
var candleSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	Security);

// Suscripción a market depth
var depthSubscription = new Subscription(
	DataType.MarketDepth,
	Security);

// Suscripción a operaciones tick
var tickSubscription = new Subscription(
	DataType.Ticks,
	Security);

// Suscripción a Level1 (mejor bid/ask y otra información básica)
var level1Subscription = new Subscription(
	DataType.Level1,
	Security);
```

## Procesamiento de datos de suscripción mediante reglas

Para procesar los datos que llegan mediante una suscripción, se recomienda usar [reglas](event_model.md):

```cs
// Suscripción a velas
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), Security);

// Crear una regla para procesar velas entrantes
Connector
	.WhenCandlesFinished(subscription)  // Activación de la regla cuando se recibe una vela completada
	.Do(ProcessCandle)                   // Llamar al método de procesamiento
	.Apply(this);                        // Aplicar la regla a la estrategia

// Iniciar la suscripción
Connector.Subscribe(subscription);
```

En el ejemplo anterior, se crea una regla que llamará al método `ProcessCandle` cuando se reciba cada vela completada.

## Solicitud de datos históricos

La estrategia establece automáticamente el periodo de carga de historial mediante la propiedad [Strategy.HistorySize](xref:StockSharp.Algo.Strategies.Strategy.HistorySize):

```cs
// Establecer el periodo de carga de historial en 30 días
strategy.HistorySize = TimeSpan.FromDays(30);
```

Al crear una suscripción, la estrategia establece automáticamente el parámetro `From` para cargar historial si no se especificó explícitamente.

## Cancelación de suscripciones

Las suscripciones se pueden cancelar manualmente llamando al método [UnSubscribe](xref:StockSharp.BusinessEntities.ISubscriptionProvider.UnSubscribe(StockSharp.BusinessEntities.Subscription)):

```cs
// Cancelar suscripción
Connector.UnSubscribe(subscription);
```

Al detener la estrategia, si el parámetro [UnsubscribeOnStop](xref:StockSharp.Algo.Strategies.Strategy.UnsubscribeOnStop) está establecido en `true` (valor predeterminado), todas las suscripciones se cancelarán automáticamente.

## Ver también

- [Suscripciones a datos de mercado](../market_data/subscriptions.md)
- [Modelo de eventos](event_model.md)
- [Compatibilidad de estrategias con plataformas](compatibility.md)

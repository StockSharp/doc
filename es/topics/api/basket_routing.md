# Enrutamiento de adaptadores

StockSharp admite conexiones simultáneas a múltiples bolsas y brokers. El sistema de enrutamiento (basket routing) gestiona qué mensajes se dirigen a qué adaptadores, garantizando un funcionamiento transparente con múltiples conexiones.

## Arquitectura general

Al usar varios adaptadores, el conector crea automáticamente una cesta (basket) que combina todas las conexiones. El enrutador determina a qué adaptador debe dirigirse cada mensaje específico: suscripciones a datos de mercado, transacciones, solicitudes de cartera, etc.

## AdapterRouter

La interfaz [IAdapterRouter](xref:StockSharp.Algo.IAdapterRouter) define la lógica para enrutar mensajes entre adaptadores.

### Métodos principales

| Método | Descripción |
|--------|-------------|
| `GetAdapters` | Devuelve una lista de adaptadores adecuados para procesar el mensaje dado |
| `GetSubscriptionAdaptersAsync` | Determina de forma asíncrona los adaptadores para las suscripciones de datos de mercado |
| `GetPortfolioAdapter` | Devuelve el adaptador vinculado a una cartera específica |
| `TryGetOrderAdapter` | Encuentra el adaptador a través del cual se registró una orden |
| `SetSecurityAdapter` | Vincula un instrumento a un adaptador específico |
| `SetPortfolioAdapter` | Vincula una cartera a un adaptador específico |

### Prioridades de enrutamiento

El sistema determina el adaptador de destino en el siguiente orden de prioridad:

1. **Especificación explícita** — si se especifica un adaptador en el mensaje a través de la propiedad `message.Adapter`, se usa ese adaptador.
2. **Vinculación de instrumento** — mapeo establecido mediante `SetSecurityAdapter`.
3. **Vinculación por tipo de datos** — adaptadores registrados para un tipo de mensaje específico.
4. **Filtrado por tipo admitido** — se seleccionan los adaptadores que admiten el tipo de mensaje dado.

### Configuración del enrutamiento

```cs
var router = connector.Adapter.InnerAdapters;

// Vincular el instrumento al adaptador para recibir datos tick
router.SetSecurityAdapter(
    secId,
    DataType.Ticks,
    binanceAdapter
);

// Vincular la cartera al adaptador para transacciones
router.SetPortfolioAdapter(
    "MyPortfolio",
    interactiveBrokersAdapter
);
```

## Gestión de conexiones

### Estados de conexión

Cada adaptador de la cesta pasa por los estados de conexión estándar:

- **Disconnected** — desconectado
- **Connecting** — conexión en curso
- **Connected** — conectado
- **Disconnecting** — desconexión en curso

La cesta agrega los estados de todos los adaptadores anidados.

### Parámetros de agregación

La propiedad `ConnectDisconnectEventOnFirstAdapter` determina cuándo se considera conectada la cesta:

- `true` — el evento de conexión se dispara cuando se conecta el **primer** adaptador (valor predeterminado). Permite comenzar a trabajar sin esperar a todas las conexiones.
- `false` — el evento se dispara solo después de que **todos** los adaptadores se hayan conectado.

```cs
// Esperar a que todos los adaptadores se conecten
connector.Adapter.InnerAdapters.ConnectDisconnectEventOnFirstAdapter = false;

connector.Connected += () =>
{
    Console.WriteLine("All adapters connected");
};

connector.Connect();
```

## Suscripciones padre e hijas

Al trabajar con varios adaptadores, una única suscripción puede dividirse en varias suscripciones hijas, cada una dirigida a su propio adaptador. El sistema automáticamente:

- Crea suscripciones hijas para cada adaptador adecuado
- Agrega las respuestas antes de notificar a la suscripción padre
- Maneja errores parciales (si un adaptador falla al suscribirse, los demás continúan funcionando)

### Ejemplo multi-bolsa

```cs
// Añadir adaptadores
connector.Adapter.InnerAdapters.Add(binanceAdapter);
connector.Adapter.InnerAdapters.Add(bybitAdapter);

connector.Connect();

// Suscripción a ticks -- se enrutará automáticamente
// a todos los adaptadores que admiten el instrumento indicado
var subscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(subscription);
```

## Cola de mensajes pendientes

Si no hay ningún adaptador conectado cuando se envía un mensaje, el mensaje se coloca en una cola pendiente (`IPendingMessageState`). Cuando un adaptador se conecta, todos los mensajes acumulados se envían automáticamente.

```cs
// Registrar una orden antes de conectar -- la orden se enviará
// automáticamente después de establecer la conexión
connector.RegisterOrder(order);
connector.Connect();
```

## Configuración de múltiples conexiones

### Configuración programática

```cs
// Crear adaptadores
var binance = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
    Key = "<API_KEY>",
    Secret = "<API_SECRET>".Secure(),
};

var ib = new InteractiveBrokersMessageAdapter(connector.TransactionIdGenerator)
{
    Address = InteractiveBrokersMessageAdapter.DefaultAddress,
};

// Añadir a la cesta
connector.Adapter.InnerAdapters.Add(binance);
connector.Adapter.InnerAdapters.Add(ib);

// Configurar el enrutamiento
connector.Adapter.InnerAdapters.SetPortfolioAdapter("BinancePortfolio", binance);
connector.Adapter.InnerAdapters.SetPortfolioAdapter("IBPortfolio", ib);

connector.Connect();
```

### Configuración gráfica

Para la configuración visual de conexiones, use el componente de configuración gráfica. Consulte la sección [Configuración gráfica](connectors/graphical_configuration.md) para más detalles.

## Seguimiento de órdenes

El enrutador rastrea automáticamente qué adaptador se usó para registrar cada orden. Al recibir actualizaciones de órdenes (cambios de estado, operaciones), el sistema las enruta a través del mismo adaptador:

```cs
// La orden se registrará a través del adaptador vinculado a la cartera
var order = new Order
{
    Security = security,
    Portfolio = portfolio,
    Side = Sides.Buy,
    Price = price,
    Volume = volume,
};

connector.RegisterOrder(order);

// La cancelación pasará automáticamente por el mismo adaptador
connector.CancelOrder(order);
```

## Véase también

- [Conectores](connectors.md)
- [Configuración gráfica](connectors/graphical_configuration.md)
- [Gestión de posiciones](positions.md)

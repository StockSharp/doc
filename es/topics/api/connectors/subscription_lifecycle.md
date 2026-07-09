# Ciclo de vida de la suscripción

Las suscripciones en StockSharp pasan por etapas específicas del ciclo de vida. La interfaz [ISubscriptionProvider](xref:StockSharp.BusinessEntities.ISubscriptionProvider) proporciona eventos para rastrear cada etapa.

## Eventos del ciclo de vida

### SubscriptionStarted

```cs
event Action<Subscription> SubscriptionStarted;
```

Se llama cuando una suscripción se inició correctamente: el adaptador aceptó la solicitud y comenzó a transmitir datos. Para suscripciones históricas, esto significa que comenzó la carga de datos. Para suscripciones en vivo, que el servidor aceptó la solicitud.

### SubscriptionOnline

```cs
event Action<Subscription> SubscriptionOnline;
```

Se llama cuando la suscripción pasó al modo en tiempo real. Para suscripciones en vivo, esto significa que la sincronización histórica inicial (si existe) finalizó y los datos ahora llegan en tiempo real. Es una señal importante para que las estrategias entiendan que los indicadores están "calentados" y el trading puede comenzar.

### SubscriptionStopped

```cs
event Action<Subscription, Exception> SubscriptionStopped;
```

Se llama cuando la suscripción finalizó. El parámetro `Exception` contiene la razón de la detención:
- `null` -- finalización normal (el usuario canceló la suscripción o finalizaron los datos históricos).
- Un objeto de excepción -- un error (pérdida de conexión, error del lado del servidor, etc.).

### SubscriptionFailed

```cs
event Action<Subscription, Exception, bool> SubscriptionFailed;
```

Se llama ante un error de suscripción. El tercer parámetro `bool` indica si fue una operación de suscripción (`true`) o cancelación de suscripción (`false`).

## Orden de eventos

Secuencia típica para una suscripción en vivo:

1. Llamada a `Subscribe(subscription)`
2. `SubscriptionStarted` -- suscripción aceptada
3. Llegada de datos (velas, libros de órdenes, operaciones, etc.)
4. `SubscriptionOnline` -- transición al modo en tiempo real
5. Llegada continua de datos en tiempo real
6. Llamada a `UnSubscribe(subscription)` o pérdida de conexión
7. `SubscriptionStopped` -- suscripción finalizada

Para una suscripción histórica (con un rango de fechas especificado):

1. Llamada a `Subscribe(subscription)`
2. `SubscriptionStarted` -- suscripción aceptada
3. Llegada de datos históricos
4. `SubscriptionStopped` con `null` -- todos los datos recibidos

## SubscriptionsOnConnect

La propiedad [Connector.SubscriptionsOnConnect](xref:StockSharp.Algo.Connector) define el conjunto de suscripciones que se envían automáticamente al conectarse:

```cs
ISet<Subscription> SubscriptionsOnConnect { get; }
```

Por defecto, se incluyen suscripciones para búsquedas de instrumentos, portfolios y órdenes:

```cs
SubscriptionsOnConnect.Add(SecurityLookup);
SubscriptionsOnConnect.Add(PortfolioLookup);
SubscriptionsOnConnect.Add(OrderLookup);
```

Puede agregar sus propias suscripciones que se iniciarán automáticamente en cada conexión:

```cs
// Añadir suscripción automática a datos Level1
var l1Sub = new Subscription(DataType.Level1, security);
connector.SubscriptionsOnConnect.Add(l1Sub);

// Eliminar búsqueda automática de órdenes al conectar
connector.SubscriptionsOnConnect.Remove(connector.OrderLookup);
```

## Eventos de conexión por adaptador

Cuando se trabaja con varias conexiones (varios adaptadores), resultan útiles los eventos que indican qué adaptador específico se conectó o desconectó:

### ConnectedEx

```cs
event Action<IMessageAdapter> ConnectedEx;
```

Se llama cuando un adaptador específico se conectó correctamente. El parámetro es el adaptador que inició el evento.

### DisconnectedEx

```cs
event Action<IMessageAdapter> DisconnectedEx;
```

Se llama al desconectarse un adaptador específico.

### ConnectionErrorEx

```cs
event Action<IMessageAdapter, Exception> ConnectionErrorEx;
```

Se llama ante un error de conexión de un adaptador específico.

También están disponibles los eventos agregados `Connected`, `Disconnected` y `ConnectionError`, que se disparan sin especificar un adaptador concreto.

## Ejemplo

```cs
private readonly Connector _connector = new();

public void SetupSubscriptionTracking()
{
    // Seguir ciclo de vida de la suscripción
    _connector.SubscriptionStarted += subscription =>
    {
        Console.WriteLine($"Subscription started: {subscription.DataType}, " +
            $"Security: {subscription.SecurityId}");
    };

    _connector.SubscriptionOnline += subscription =>
    {
        Console.WriteLine($"Subscription online: {subscription.DataType}");
    };

    _connector.SubscriptionStopped += (subscription, error) =>
    {
        if (error == null)
            Console.WriteLine($"Subscription completed: {subscription.DataType}");
        else
            Console.WriteLine($"Subscription interrupted: {subscription.DataType}, " +
                $"Error: {error.Message}");
    };

    // Seguir conexiones de adaptadores individuales
    _connector.ConnectedEx += adapter =>
    {
        Console.WriteLine($"Adapter connected: {adapter.Name}");
    };

    _connector.DisconnectedEx += adapter =>
    {
        Console.WriteLine($"Adapter disconnected: {adapter.Name}");
    };

    _connector.ConnectionErrorEx += (adapter, error) =>
    {
        Console.WriteLine($"Adapter connection error {adapter.Name}: {error.Message}");
    };

    // Conectar
    _connector.Connect();

    // Después de conectar -- crear suscripción
    _connector.Connected += () =>
    {
        var subscription = new Subscription(DataType.Ticks, security);
        _connector.Subscribe(subscription);
    };
}
```

## Véase también

[Conexión](../connectors.md)

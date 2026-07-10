# Conexión mediante el protocolo FIX

[Hydra](../../hydra.md) puede usarse en modo servidor, lo que permite conectarse remotamente a [Hydra](../../hydra.md) para acceder a los datos del almacenamiento. La activación del modo servidor de [Hydra](../../hydra.md) se describe en la sección [Configuración](settings.md).

Para conectarse mediante el [Protocolo FIX](../../api/connectors/common/fix_protocol.md), debe crear y configurar una conexión Fix ([Inicialización del adaptador FIX](../../api/connectors/common/fix_protocol/adapter_initialization_fix.md)).

```cs
// Crear una instancia del conector
private readonly Connector _connector = new Connector();

// Configurar el adaptador para datos de mercado mediante el protocolo FIX
var marketDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(marketDataAdapter);

// Configurar el adaptador para datos de transacciones
var transactionDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(transactionDataAdapter);
```

Suscríbase a eventos y configure los manejadores de datos:

```cs
// Evento de conexión correcta
_connector.Connected += () =>
{
	Console.WriteLine("Connection established");
	
	// Crear una suscripción para buscar instrumentos
	var lookupSubscription = new Subscription(DataType.Securities);
	_connector.Subscribe(lookupSubscription);
};

// Evento de pérdida de conexión
_connector.Disconnected += () =>
{
	Console.WriteLine("Conexión perdida");
};

// Evento de instrumento recibido
_connector.SecurityReceived += (subscription, security) =>
{
	Console.WriteLine($"Instrumento recibido: {security.Code}, {security.Id}");
	BufferSecurity.Add(security);
	
	// Si este es el instrumento objetivo, suscribirse a sus datos
	if (security.Id == targetSecurityId)
	{
		// Suscripción al libro de órdenes
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// Suscripción a operaciones tick
		var tradesSubscription = new Subscription(DataType.Ticks, security);
		_connector.Subscribe(tradesSubscription);
		
		// Suscripción a velas
		var candleSubscription = new Subscription(
			DataType.TimeFrame(TimeSpan.FromMinutes(5)),
			security)
		{
			MarketData =
			{
				From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
				To = DateTime.Now
			}
		};
		_connector.Subscribe(candleSubscription);
	}
};

// Evento de operación tick recibida
_connector.TickTradeReceived += (subscription, trade) =>
{
	Console.WriteLine($"Operación recibida: {trade.Security.Code}, {trade.Time}, {trade.Price}, {trade.Volume}");
};

// Evento de cambio del libro de órdenes
_connector.OrderBookReceived += (subscription, depth) =>
{
	Console.WriteLine($"Libro de órdenes recibido: {depth.SecurityId}, mejor bid: {depth.BestBid()?.Price}, mejor ask: {depth.BestAsk()?.Price}");
};

// Evento de vela recibida
_connector.CandleReceived += (subscription, candle) =>
{
	Console.WriteLine($"Vela recibida: {candle.SecurityId}, {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
};

// Evento de error de conexión
_connector.ConnectionError += error =>
{
	Console.WriteLine($"Error de conexión: {error.Message}");
};

// Evento de error general
_connector.Error += error =>
{
	Console.WriteLine($"Error: {error.Message}");
};

// Evento de error de suscripción a datos de mercado
_connector.SubscriptionFailed += (subscription, error) =>
{
	Console.WriteLine($"Error de suscripción {subscription.DataType} para {subscription.SecurityId}: {error}");
};

// Conectarse al servidor
_connector.Connect();
```

## Uso de servicios de Hydra

Hydra en modo servidor proporciona acceso a distintos tipos de datos. Veamos ejemplos de obtención de datos históricos:

```cs
// Obtener velas históricas
private void RequestHistoricalCandles(Security security, DateTime from, DateTime to)
{
	// Crear una suscripción a velas históricas
	var candleSubscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		security)
	{
		MarketData =
		{
			From = from,
			To = to
		}
	};
	
	// Suscribirse para procesar las velas recibidas
	_connector.CandleReceived += OnCandleReceived;
	
	// Iniciar la suscripción
	_connector.Subscribe(candleSubscription);
}

private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Comprobar que la vela pertenece a nuestra suscripción
	if (subscription.DataType != DataType.TimeFrame(TimeSpan.FromMinutes(5)))
		return;
		
	Console.WriteLine($"Vela histórica: {candle.OpenTime}, O: {candle.OpenPrice}, H: {candle.HighPrice}, L: {candle.LowPrice}, C: {candle.ClosePrice}, V: {candle.TotalVolume}");
	
	// Procesar las velas recibidas, por ejemplo, guardarlas en almacenamiento local
	// o usarlas para análisis/visualización
}
```

## Desconexión del servidor Hydra

```cs
// Cierre correcto de la conexión
private void DisconnectFromServer()
{
	// Cancelar la suscripción a todas las suscripciones
	foreach (var subscription in _connector.Subscriptions.ToArray())
	{
		_connector.UnSubscribe(subscription);
	}
	
	// Desconectarse del servidor
	_connector.Disconnect();
}
```

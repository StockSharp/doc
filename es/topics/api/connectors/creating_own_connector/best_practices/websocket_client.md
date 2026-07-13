# Cliente WebSocket

Al desarrollar un conector para una serie de bolsas, un componente importante es el cliente WebSocket, que proporciona la recuperación de datos en tiempo real. En StockSharp, para este propósito, a menudo se crea una clase `SocketClient` al desarrollar un conector, construida sobre la base de `WebSocketClient`.

## Características de WebSocketClient

`WebSocketClient` es una clase del sistema desarrollada para la reconexión automática en caso de pérdida de la conexión WebSocket. Su código fuente está disponible en el [repositorio Ecng](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/WebSocketClient.cs).

## Estructura de SocketClient

`SocketClient` generalmente incluye los siguientes elementos clave:

1. **Constructor**
  - Inicializa el `WebSocketClient` base
  - Configura los manejadores de eventos

2. **Métodos de conexión y desconexión**
  - `Connect` / `ConnectAsync`
  - `Disconnect`

3. **Métodos de suscripción para diferentes tipos de datos**
  - Por ejemplo, `SubscribeTrades`, `SubscribeOrderBook`

4. **Métodos de cancelación de suscripción para datos**
  - Métodos de cancelación de suscripción correspondientes para cada tipo de suscripción

5. **Manejadores de eventos**
  - Para manejar diferentes tipos de mensajes entrantes

6. **Métodos auxiliares**
  - Para formar mensajes de suscripción/cancelación de suscripción
  - Para procesar los datos recibidos

```cs
class SocketClient : BaseLogReceiver
{
	private readonly WebSocketClient _client;
	private readonly Authenticator _authenticator;

	// Eventos para distintos tipos de datos
	public event Action<Heartbeat> HeartbeatReceived;
	public event Action<Ticker> TickerReceived;
	public event Action<Trade> TradeReceived;
	public event Action<string, string, IEnumerable<OrderBookChange>> OrderBookReceived;
	public event Action<Order> OrderReceived;
	public event Action<Exception> Error;
	public event Action Connected;
	public event Action<bool> Disconnected;

	public SocketClient(Authenticator authenticator, int reconnectAttempts)
	{
		_authenticator = authenticator;
		_client = new WebSocketClient(/* parámetros */);
		_client.ReconnectAttempts = reconnectAttempts;
	}

	public ValueTask Connect(CancellationToken cancellationToken)
	{
		// Lógica de conexión
	}

	public void Disconnect()
	{
		// Lógica de desconexión
	}

	public ValueTask SubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// Lógica de suscripción a ticker
	}

	public ValueTask UnSubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// Lógica de cancelación de suscripción a ticker
	}

	// Métodos similares para otros tipos de suscripciones (operaciones, libro de órdenes, etc.)

	private void OnProcess(dynamic obj)
	{
		// Procesar mensajes entrantes
	}

	// Métodos auxiliares
}
```

## Recomendaciones de Implementación

- Adapte la estructura de `SocketClient` a la bolsa específica, teniendo en cuenta las particularidades de su API.
- Utilice métodos asíncronos para un trabajo eficiente con WebSocket.
- Implemente el manejo de diferentes tipos de mensajes de la bolsa.
- Asegure un manejo adecuado de errores y la reconexión tras la pérdida de la conexión.

Recuerde que la implementación específica puede diferir según los requisitos y las particularidades de la API de una bolsa en particular.
</content>

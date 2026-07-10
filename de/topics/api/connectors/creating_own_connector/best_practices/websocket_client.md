# WebSocket-Client

Bei der Entwicklung eines Connectors für eine Reihe von Börsen ist eine wichtige Komponente der WebSocket-Client, der den Empfang von Echtzeitdaten ermöglicht. In StockSharp wird zu diesem Zweck bei der Entwicklung eines Connectors häufig eine Klasse `SocketClient` erstellt, die auf der Grundlage von `WebSocketClient` aufgebaut ist.

## WebSocketClient-Funktionen

`WebSocketClient` ist eine Systemklasse, die für die automatische Wiederverbindung im Falle eines Verlusts der WebSocket-Verbindung entwickelt wurde. Ihr Quellcode ist im [Ecng-Repository](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/WebSocketClient.cs) verfügbar.

## Struktur von SocketClient

`SocketClient` umfasst in der Regel die folgenden Schlüsselelemente:

1. **Konstruktor**
  - Initialisiert den Basis-`WebSocketClient`
  - Richtet Ereignishandler ein

2. **Methoden für Verbindung und Trennung**
  - `Connect` / `ConnectAsync`
  - `Disconnect`

3. **Abonnementmethoden für verschiedene Datentypen**
  - Zum Beispiel `SubscribeTrades`, `SubscribeOrderBook`

4. **Methoden zum Kündigen von Abonnements**
  - Entsprechende Methoden zum Kündigen für jeden Abonnementtyp

5. **Ereignishandler**
  - Zur Verarbeitung verschiedener Arten eingehender Nachrichten

6. **Hilfsmethoden**
  - Zum Bilden von Abonnement-/Kündigungsnachrichten
  - Zum Verarbeiten empfangener Daten

```cs
class SocketClient : BaseLogReceiver
{
	private readonly WebSocketClient _client;
	private readonly Authenticator _authenticator;

	// Ereignisse für verschiedene Datentypen
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
		_client = new WebSocketClient(/* Parameter */);
		_client.ReconnectAttempts = reconnectAttempts;
	}

	public ValueTask Connect(CancellationToken cancellationToken)
	{
		// Verbindungslogik
	}

	public void Disconnect()
	{
		// Trennungslogik
	}

	public ValueTask SubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// Ticker-Abonnementlogik
	}

	public ValueTask UnSubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// Ticker-Abbestelllogik
	}

	// Ähnliche Methoden für andere Abonnementtypen (Trades, Orderbuch usw.)

	private void OnProcess(dynamic obj)
	{
		// Eingehende Nachrichten verarbeiten
	}

	// Hilfsmethoden
}
```

## Implementierungsempfehlungen

- Passen Sie die Struktur von `SocketClient` an die jeweilige Börse an und berücksichtigen Sie dabei die Besonderheiten ihrer API.
- Verwenden Sie asynchrone Methoden für eine effiziente Arbeit mit WebSocket.
- Implementieren Sie die Behandlung verschiedener Arten von Nachrichten der Börse.
- Stellen Sie eine ordnungsgemäße Fehlerbehandlung und Wiederverbindung bei Verbindungsverlust sicher.

Denken Sie daran, dass sich die konkrete Implementierung je nach Anforderungen und Besonderheiten der API einer bestimmten Börse unterscheiden kann.

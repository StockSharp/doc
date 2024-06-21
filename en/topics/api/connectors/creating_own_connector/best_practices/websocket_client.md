# WebSocket Client

When developing a connector for a number of exchanges, an important component is the WebSocket client, which provides real-time data retrieval. In StockSharp, for this purpose, a `SocketClient` class is often created when developing a connector, built on the basis of `WebSocketClient`.

## WebSocketClient Features

`WebSocketClient` is a system class developed for automatic reconnection in case of a WebSocket connection loss. Its source code is available in the [Ecng repository](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/WebSocketClient.cs).

## SocketClient Structure

`SocketClient` usually includes the following key elements:

1. **Constructor**
  - Initializes the base `WebSocketClient`
  - Sets up event handlers

2. **Connection and disconnection methods**
  - `Connect` / `ConnectAsync`
  - `Disconnect`

3. **Subscription methods for different types of data**
  - For example, `SubscribeTrades`, `SubscribeOrderBook`

4. **Unsubscription methods for data**
  - Corresponding unsubscription methods for each type of subscription

5. **Event handlers**
  - For handling different types of incoming messages

6. **Helper methods**
  - For forming subscription/unsubscription messages
  - For processing received data

```cs
class SocketClient : BaseLogReceiver
{
    private readonly WebSocketClient _client;
    private readonly Authenticator _authenticator;

    // Events for different types of data
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
        _client = new WebSocketClient(/* parameters */);
        _client.ReconnectAttempts = reconnectAttempts;
    }

    public ValueTask Connect(CancellationToken cancellationToken)
    {
        // Connection logic
    }

    public void Disconnect()
    {
        // Disconnection logic
    }

    public ValueTask SubscribeTicker(string symbol, CancellationToken cancellationToken)
    {
        // Ticker subscription logic
    }

    public ValueTask UnSubscribeTicker(string symbol, CancellationToken cancellationToken)
    {
        // Ticker unsubscription logic
    }

    // Similar methods for other types of subscriptions (trades, order book, etc.)

    private void OnProcess(dynamic obj)
    {
        // Processing incoming messages
    }

    // Helper methods
}
```

## Implementation Recommendations

- Adapt the `SocketClient` structure to the specific exchange, taking into account the specifics of its API.
- Use asynchronous methods for efficient work with WebSocket.
- Implement handling of different types of messages from the exchange.
- Ensure proper error handling and reconnection upon connection loss.

Remember that the specific implementation may differ depending on the requirements and specifics of the API of a particular exchange.
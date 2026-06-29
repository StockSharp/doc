# WebSocket 客户端

在为多个交易所开发连接器时，一个重要的组成部分是 WebSocket 客户端，它提供实时数据检索。在 StockSharp 中，为了这个目的，在开发连接器时通常会创建一个 `SocketClient` 类，该类是基于 `WebSocketClient` 构建的。

## WebSocket客户端功能

`WebSocketClient` 是一个系统类，用于在 WebSocket 连接丢失时自动重新连接。其源代码可在 [Ecng 仓库](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/WebSocketClient.cs) 中获取。

## SocketClient 结构

`SocketClient` 通常包括以下关键要素：

1. **构造函数**
  - 初始化基础 `WebSocketClient`
  - 设置事件处理程序

2. **连接和断开方法**
  - `Connect` / `ConnectAsync`
  - `Disconnect`

3. **不同类型数据的订阅方式**
  - 例如，`SubscribeTrades`，`SubscribeOrderBook`

4. **数据退订方法**
  - 每种订阅类型对应的退订方式

5. **事件处理程序**
  - 用于处理不同类型的传入消息

6. **辅助方法**
  - 用于形成订阅/退订消息
  - 用于处理接收的数据

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

## 实施建议

- 根据特定交易所的情况调整`SocketClient`结构，同时考虑其API的具体特点。
- 使用异步方法高效处理 WebSocket。
- 实现对来自交易所的不同类型消息的处理。
- 确保在连接丢失时进行适当的错误处理和重新连接。

请记住，具体实现可能会根据特定交易所 API 的要求和细节有所不同。
# WebSocket 客户端

在为多个交易所开发连接器时，另一个重要组成部分是 WebSocket 客户端，它负责实时数据的获取。在 StockSharp 中，为此目的，开发连接器时通常会创建一个基于 `WebSocketClient` 构建的 `SocketClient` 类。

## WebSocketClient 功能

`WebSocketClient` 是一个系统类，用于在 WebSocket 连接丢失时自动重新连接。其源代码可在 [Ecng 仓库](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/WebSocketClient.cs) 中获取。

## SocketClient 结构

`SocketClient` 通常包含以下关键要素：

1. **构造函数**
  - 初始化基础的 `WebSocketClient`
  - 设置事件处理程序

2. **连接和断开方法**
  - `Connect` / `ConnectAsync`
  - `Disconnect`

3. **不同类型数据的订阅方法**
  - 例如 `SubscribeTrades`、`SubscribeOrderBook`

4. **数据取消订阅方法**
  - 与每种订阅类型对应的取消订阅方法

5. **事件处理程序**
  - 用于处理不同类型的传入消息

6. **辅助方法**
  - 用于构造订阅/取消订阅消息
  - 用于处理接收到的数据

```cs
class SocketClient : BaseLogReceiver
{
	private readonly WebSocketClient _client;
	private readonly Authenticator _authenticator;

	// 不同数据类型的事件
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
		_client = new WebSocketClient(/* 参数 */);
		_client.ReconnectAttempts = reconnectAttempts;
	}

	public ValueTask Connect(CancellationToken cancellationToken)
	{
		// 连接逻辑
	}

	public void Disconnect()
	{
		// 断开连接逻辑
	}

	public ValueTask SubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// ticker 订阅逻辑
	}

	public ValueTask UnSubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// ticker 取消订阅逻辑
	}

	// 其他订阅类型（成交、订单簿等）的类似方法

	private void OnProcess(dynamic obj)
	{
		// 处理传入消息
	}

	// 辅助方法
}
```

## 实现建议

- 根据特定交易所的情况调整 `SocketClient` 的结构，同时考虑其 API 的具体特点。
- 使用异步方法以高效处理 WebSocket。
- 实现对来自交易所的不同类型消息的处理。
- 确保在连接丢失时进行妥善的错误处理和重新连接。

请注意，具体实现可能会因特定交易所 API 的要求和细节而有所不同。

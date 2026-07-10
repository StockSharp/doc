# WebSocket クライアント

多くの取引所向けコネクターを開発する場合、重要なコンポーネントの 1 つが WebSocket クライアントです。これはリアルタイムデータの取得を提供します。StockSharp では、この目的のために、コネクター開発時に `WebSocketClient` を基盤とした `SocketClient` クラスが作成されることがよくあります。

## WebSocketClient の機能

`WebSocketClient` は、WebSocket 接続が失われた場合に自動再接続を行うために開発されたシステムクラスです。そのソースコードは [Ecng リポジトリ](https://github.com/StockSharp/Ecng/blob/master/Net.SocketIO/WebSocketClient.cs) で利用できます。

## SocketClient の構造

`SocketClient` には通常、次の主要要素が含まれます。

1. **コンストラクター**
  - 基底 `WebSocketClient` を初期化します
  - イベントハンドラーを設定します

2. **接続および切断メソッド**
  - `Connect` / `ConnectAsync`
  - `Disconnect`

3. **さまざまな種類のデータ用の購読メソッド**
  - 例: `SubscribeTrades`、`SubscribeOrderBook`

4. **データの購読解除メソッド**
  - 各購読種別に対応する購読解除メソッド

5. **イベントハンドラー**
  - さまざまな種類の受信メッセージを処理するため

6. **ヘルパーメソッド**
  - 購読/購読解除メッセージの形成用
  - 受信データの処理用

```cs
class SocketClient : BaseLogReceiver
{
	private readonly WebSocketClient _client;
	private readonly Authenticator _authenticator;

	// さまざまな種類のデータ用イベント
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
		_client = new WebSocketClient(/* パラメーター */);
		_client.ReconnectAttempts = reconnectAttempts;
	}

	public ValueTask Connect(CancellationToken cancellationToken)
	{
		// 接続ロジック
	}

	public void Disconnect()
	{
		// 切断ロジック
	}

	public ValueTask SubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// ティッカー購読ロジック
	}

	public ValueTask UnSubscribeTicker(string symbol, CancellationToken cancellationToken)
	{
		// ティッカー購読解除ロジック
	}

	// 他の種類の購読（取引、板など）用の同様のメソッド

	private void OnProcess(dynamic obj)
	{
		// 受信メッセージの処理
	}

	// ヘルパーメソッド
}
```

## 実装に関する推奨事項

- 対象取引所の API 仕様を考慮して、`SocketClient` の構造をその取引所に合わせて調整してください。
- WebSocket を効率的に扱うために非同期メソッドを使用してください。
- 取引所からのさまざまな種類のメッセージを処理する実装を行ってください。
- 接続喪失時の適切なエラー処理と再接続を確保してください。

具体的な実装は、特定の取引所 API の要件や仕様によって異なる場合があることに留意してください。

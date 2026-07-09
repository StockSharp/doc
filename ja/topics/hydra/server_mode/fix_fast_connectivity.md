# FIX プロトコル経由の接続

[Hydra](../../hydra.md) はサーバーモードで使用できます。これにより、[Hydra](../../hydra.md) にリモート接続してストレージ内のデータにアクセスできます。[Hydra](../../hydra.md) サーバーモードの有効化については、[設定](settings.md) セクションで説明しています。

[FIX プロトコル](../../api/connectors/common/fix_protocol.md) 経由で接続するには、Fix 接続を作成して設定する必要があります ([FIX アダプターの初期化](../../api/connectors/common/fix_protocol/adapter_initialization_fix.md))。

```cs
// コネクターインスタンスを作成
private readonly Connector _connector = new Connector();

// FIX プロトコル経由のマーケットデータ用アダプターを設定
var marketDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(marketDataAdapter);

// トランザクションデータ用アダプターを設定
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

イベントを購読し、データハンドラーを設定します:

```cs
// 接続成功イベント
_connector.Connected += () =>
{
	Console.WriteLine("Connection established");
	
	// 銘柄検索用のサブスクリプションを作成
	var lookupSubscription = new Subscription(DataType.Securities);
	_connector.Subscribe(lookupSubscription);
};

// 接続喪失イベント
_connector.Disconnected += () =>
{
	Console.WriteLine("Connection lost");
};

// 銘柄受信イベント
_connector.SecurityReceived += (subscription, security) =>
{
	Console.WriteLine($"銘柄を受信: {security.Code}, {security.Id}");
	BufferSecurity.Add(security);
	
	// これが対象銘柄である場合、そのデータを購読する
	if (security.Id == targetSecurityId)
	{
		// 板情報サブスクリプション
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// ティック取引サブスクリプション
		var tradesSubscription = new Subscription(DataType.Ticks, security);
		_connector.Subscribe(tradesSubscription);
		
		// ローソク足サブスクリプション
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

// ティック取引受信イベント
_connector.TickTradeReceived += (subscription, trade) =>
{
	Console.WriteLine($"約定を受信: {trade.Security.Code}, {trade.Time}, {trade.Price}, {trade.Volume}");
};

// 板情報変更イベント
_connector.OrderBookReceived += (subscription, depth) =>
{
	Console.WriteLine($"板情報を受信: {depth.SecurityId}, 最良買い気配: {depth.BestBid()?.Price}, 最良売り気配: {depth.BestAsk()?.Price}");
};

// ローソク足受信イベント
_connector.CandleReceived += (subscription, candle) =>
{
	Console.WriteLine($"キャンドルを受信: {candle.SecurityId}, {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
};

// 接続エラーイベント
_connector.ConnectionError += error =>
{
	Console.WriteLine($"Connection error: {error.Message}");
};

// 一般エラーイベント
_connector.Error += error =>
{
	Console.WriteLine($"Error: {error.Message}");
};

// マーケットデータサブスクリプションエラーイベント
_connector.SubscriptionFailed += (subscription, error) =>
{
	Console.WriteLine($"サブスクリプションエラー {subscription.DataType} ({subscription.SecurityId}): {error}");
};

// サーバーへ接続
_connector.Connect();
```

## Hydra サービスの使用

サーバーモードの Hydra は、さまざまな種類のデータへのアクセスを提供します。履歴データを取得する例を見てみましょう:

```cs
// 履歴ローソク足の取得
private void RequestHistoricalCandles(Security security, DateTime from, DateTime to)
{
	// 履歴ローソク足へのサブスクリプションを作成
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
	
	// 受信したローソク足を処理するために購読
	_connector.CandleReceived += OnCandleReceived;
	
	// サブスクリプションを開始
	_connector.Subscribe(candleSubscription);
}

private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// ローソク足が当該サブスクリプションに属していることを確認
	if (subscription.DataType != DataType.TimeFrame(TimeSpan.FromMinutes(5)))
		return;
		
	Console.WriteLine($"過去キャンドル: {candle.OpenTime}, O: {candle.OpenPrice}, H: {candle.HighPrice}, L: {candle.LowPrice}, C: {candle.ClosePrice}, V: {candle.TotalVolume}");
	
	// 受信したローソク足を処理します。たとえば、ローカルストレージへ保存
	// または分析/可視化に使用します
}
```

## Hydra サーバーからの切断

```cs
// 適切な接続終了
private void DisconnectFromServer()
{
	// すべてのサブスクリプションを解除
	foreach (var subscription in _connector.Subscriptions.ToArray())
	{
		_connector.UnSubscribe(subscription);
	}
	
	// サーバーから切断
	_connector.Disconnect();
}
```

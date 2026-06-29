# 通过 FIX 协议连接

[Hydra](../../hydra.md) 可以在服务器模式下运行，允许远程客户端连接到 [Hydra](../../hydra.md) 并访问存储中的数据。启用 [Hydra](../../hydra.md) 服务器模式的方法请参阅[设置](settings.md)。

要通过 [FIX 协议](../../api/connectors/common/fix_protocol.md)连接，需要创建并配置 FIX 连接（参阅 [FIX 适配器初始化](../../api/connectors/common/fix_protocol/adapter_initialization_fix.md)）。

```cs
// Create a connector instance
private readonly Connector _connector = new Connector();

// Configure the adapter for market data via FIX protocol
var marketDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(marketDataAdapter);

// Configure the adapter for transaction data
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

订阅相关事件并配置数据处理程序：

```cs
// Successful connection event
_connector.Connected += () =>
{
	Console.WriteLine("Connection established");
	
	// Create a subscription to search for instruments
	var lookupSubscription = new Subscription(DataType.Securities);
	_connector.Subscribe(lookupSubscription);
};

// Connection lost event
_connector.Disconnected += () =>
{
	Console.WriteLine("Connection lost");
};

// Instrument received event
_connector.SecurityReceived += (subscription, security) =>
{
	Console.WriteLine($"Instrument received: {security.Code}, {security.Id}");
	BufferSecurity.Add(security);
	
	// If this is the target instrument, subscribe to its data
	if (security.Id == targetSecurityId)
	{
		// Order book subscription
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// Tick trades subscription
		var tradesSubscription = new Subscription(DataType.Ticks, security);
		_connector.Subscribe(tradesSubscription);
		
		// Candles subscription
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

// Tick trade received event
_connector.TickTradeReceived += (subscription, trade) =>
{
	Console.WriteLine($"Trade received: {trade.Security.Code}, {trade.Time}, {trade.Price}, {trade.Volume}");
};

// Order book changed event
_connector.OrderBookReceived += (subscription, depth) =>
{
	Console.WriteLine($"Order book received: {depth.SecurityId}, Best bid: {depth.BestBid()?.Price}, Best ask: {depth.BestAsk()?.Price}");
};

// Candle received event
_connector.CandleReceived += (subscription, candle) =>
{
	Console.WriteLine($"Candle received: {candle.SecurityId}, {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
};

// Connection error event
_connector.ConnectionError += error =>
{
	Console.WriteLine($"Connection error: {error.Message}");
};

// General error event
_connector.Error += error =>
{
	Console.WriteLine($"Error: {error.Message}");
};

// Market data subscription error event
_connector.SubscriptionFailed += (subscription, error) =>
{
	Console.WriteLine($"Subscription error {subscription.DataType} for {subscription.SecurityId}: {error}");
};

// Connect to the server
_connector.Connect();
```

## 使用 Hydra 服务

服务器模式下的 Hydra 支持访问多种数据。下面以获取历史数据为例：

```cs
// Getting historical candles
private void RequestHistoricalCandles(Security security, DateTime from, DateTime to)
{
	// Create a subscription to historical candles
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
	
	// Subscribe to process received candles
	_connector.CandleReceived += OnCandleReceived;
	
	// Start the subscription
	_connector.Subscribe(candleSubscription);
}

private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Check that the candle belongs to our subscription
	if (subscription.DataType != DataType.TimeFrame(TimeSpan.FromMinutes(5)))
		return;
		
	Console.WriteLine($"Historical candle: {candle.OpenTime}, O: {candle.OpenPrice}, H: {candle.HighPrice}, L: {candle.LowPrice}, C: {candle.ClosePrice}, V: {candle.TotalVolume}");
	
	// Process the received candles, for example, save to local storage
	// or use for analysis/visualization
}
```

## 断开与 Hydra 服务器的连接

```cs
// Proper connection closing
private void DisconnectFromServer()
{
	// Unsubscribe from all subscriptions
	foreach (var subscription in _connector.Subscriptions.ToArray())
	{
		_connector.UnSubscribe(subscription);
	}
	
	// Disconnect from the server
	_connector.Disconnect();
}
```

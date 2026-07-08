# 通过 FIX 协议连接

[Hydra](../../hydra.md) 可以在服务器模式下运行，允许远程客户端连接到 [Hydra](../../hydra.md) 并访问存储中的数据。启用 [Hydra](../../hydra.md) 服务器模式的方法请参阅[设置](settings.md)。

要通过 [FIX 协议](../../api/connectors/common/fix_protocol.md)连接，需要创建并配置 FIX 连接（参阅 [FIX 适配器初始化](../../api/connectors/common/fix_protocol/adapter_initialization_fix.md)）。

```cs
// 创建连接器实例
private readonly Connector _connector = new Connector();

// 配置通过 FIX 协议接收市场数据的适配器
var marketDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(marketDataAdapter);

// 配置交易数据适配器
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
// 连接成功事件
_connector.Connected += () =>
{
	Console.WriteLine("Connection established");
	
	// 创建搜索工具的订阅
	var lookupSubscription = new Subscription(DataType.Securities);
	_connector.Subscribe(lookupSubscription);
};

// 连接丢失事件
_connector.Disconnected += () =>
{
	Console.WriteLine("Connection lost");
};

// 收到工具事件
_connector.SecurityReceived += (subscription, security) =>
{
	Console.WriteLine($"Instrument received: {security.Code}, {security.Id}");
	BufferSecurity.Add(security);
	
	// If this is the target instrument, subscribe to its data
	if (security.Id == targetSecurityId)
	{
		// 订单簿订阅
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// 逐笔成交订阅
		var tradesSubscription = new Subscription(DataType.Ticks, security);
		_connector.Subscribe(tradesSubscription);
		
		// 蜡烛订阅
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

// 收到逐笔成交事件
_connector.TickTradeReceived += (subscription, trade) =>
{
	Console.WriteLine($"Trade received: {trade.Security.Code}, {trade.Time}, {trade.Price}, {trade.Volume}");
};

// 订单簿变更事件
_connector.OrderBookReceived += (subscription, depth) =>
{
	Console.WriteLine($"Order book received: {depth.SecurityId}, Best bid: {depth.BestBid()?.Price}, Best ask: {depth.BestAsk()?.Price}");
};

// 收到蜡烛事件
_connector.CandleReceived += (subscription, candle) =>
{
	Console.WriteLine($"Candle received: {candle.SecurityId}, {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
};

// 连接错误事件
_connector.ConnectionError += error =>
{
	Console.WriteLine($"Connection error: {error.Message}");
};

// 通用错误事件
_connector.Error += error =>
{
	Console.WriteLine($"Error: {error.Message}");
};

// 市场数据订阅错误事件
_connector.SubscriptionFailed += (subscription, error) =>
{
	Console.WriteLine($"Subscription error {subscription.DataType} for {subscription.SecurityId}: {error}");
};

// 连接到服务器
_connector.Connect();
```

## 使用 Hydra 服务

服务器模式下的 Hydra 支持访问多种数据。下面以获取历史数据为例：

```cs
// 获取历史蜡烛
private void RequestHistoricalCandles(Security security, DateTime from, DateTime to)
{
	// 创建历史蜡烛订阅
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
	
	// 订阅以处理收到的蜡烛
	_connector.CandleReceived += OnCandleReceived;
	
	// 启动订阅
	_connector.Subscribe(candleSubscription);
}

private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 检查蜡烛是否属于当前订阅
	if (subscription.DataType != DataType.TimeFrame(TimeSpan.FromMinutes(5)))
		return;
		
	Console.WriteLine($"Historical candle: {candle.OpenTime}, O: {candle.OpenPrice}, H: {candle.HighPrice}, L: {candle.LowPrice}, C: {candle.ClosePrice}, V: {candle.TotalVolume}");
	
	// 处理收到的蜡烛，例如保存到本地存储
	// 或用于分析/可视化
}
```

## 断开与 Hydra 服务器的连接

```cs
// 正确关闭连接
private void DisconnectFromServer()
{
	// 取消所有订阅
	foreach (var subscription in _connector.Subscriptions.ToArray())
	{
		_connector.UnSubscribe(subscription);
	}
	
	// 断开服务器连接
	_connector.Disconnect();
}
```

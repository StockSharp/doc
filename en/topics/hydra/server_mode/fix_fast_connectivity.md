# Connecting via FIX Protocol

[Hydra](../../hydra.md) can be used in server mode, which allows remote connection to [Hydra](../../hydra.md) to access data in the storage. Enabling [Hydra](../../hydra.md) server mode is described in the [Settings](settings.md) section.

To connect via the [FIX protocol](../../api/connectors/common/fix_protocol.md), you need to create and configure a Fix connection ([FIX Adapter Initialization](../../api/connectors/common/fix_protocol/adapter_initialization_fix.md)).

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

Subscribe to events and set up data handlers:

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

## Using Hydra Services

Hydra in server mode provides access to various types of data. Let's look at examples of getting historical data:

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

## Disconnecting from Hydra Server

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
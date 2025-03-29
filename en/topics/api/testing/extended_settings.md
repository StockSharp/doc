# Testing Settings

This section describes the main settings of [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) for testing trading strategies.

## Basic Emulator Settings

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - interval for time change event arrival. If trade generators are used, trades will be generated with this frequency. The default is 1 minute.
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - minimum delay value for submitted orders. The default is TimeSpan.Zero, which means instant acceptance of submitted orders by the exchange.
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - satisfy orders if the price "touches" the level (this assumption is sometimes too "optimistic" and should be turned off for realistic testing). If disabled, limit orders will be fulfilled only if the price "passes through them" by at least 1 step. This option works in all modes except order log mode. It's disabled by default.

## Market Data Subscriptions

For correct strategy testing, it's necessary to set up subscriptions to the required market data types. Even if the strategy is tested on candles, for correct trade emulation, a subscription to tick trades is required:

```cs
// Create a subscription to tick trades
var tickSubscription = new Subscription(DataType.Ticks, security);
_connector.Subscribe(tickSubscription);
```

If the strategy requires order book data:

```cs
// Create a subscription to order books
var depthSubscription = new Subscription(DataType.MarketDepth, security);
_connector.Subscribe(depthSubscription);
```

## Order Book Generation for Testing

If there are no historical order books, but they are needed for strategy testing, you can enable order book generation:

```cs
// Create an order book generator with trend behavior
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// Send a subscription message to the generator
_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
    IsSubscribe = true,
    Generator = mdGenerator
});
```

### Order Book Generator Settings

- Order book update interval ([MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval)) - updates cannot be more frequent than the arrival of tick trades, as order books are generated before each trade:

```cs
mdGenerator.Interval = TimeSpan.FromSeconds(1);
```

- Order book depth ([MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) and [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth)) - the deeper, the slower the testing:

```cs
mdGenerator.MaxAsksDepth = 1; 
mdGenerator.MaxBidsDepth = 1;
```

- To get a realistic level volume in the order book, you can use the [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume) option, which takes the volumes for the best quotes from the trade volume being generated:

```cs
mdGenerator.UseTradeVolume = true;
```

- Level volume range ([MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) and [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume)):

```cs
mdGenerator.MinVolume = 1;
mdGenerator.MaxVolume = 1;
```

- Spread settings - the minimum generated spread equals [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep). It is recommended not to generate a spread between the best quotes greater than 5 price steps, so that when generating from candles, the spread doesn't become too wide:

```cs
mdGenerator.MinSpreadStepCount = 1;
mdGenerator.MaxSpreadStepCount = 5;
```

## Comprehensive Testing Configuration Example

```cs
// Create a historical connection
var connector = new HistoryEmulationConnector();

// Configure basic parameters
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.EmulationAdapter.Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.EmulationAdapter.Emulator.Settings.MatchOnTouch = false;

// Load historical data
var storage = new StorageRegistry();
var security = new Security { Id = "AAPL", PriceStep = 0.01m };

// Create a subscription to candles
var candleSubscription = new Subscription(
    DataType.TimeFrame(TimeSpan.FromMinutes(5)),
    security)
{
    MarketData =
    {
        From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
        To = DateTime.Today,
        BuildMode = MarketDataBuildModes.Load
    }
};
connector.Subscribe(candleSubscription);

// Create a subscription to ticks for correct emulation
var tickSubscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(tickSubscription);

// Configure order book generation
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId())
{
    Interval = TimeSpan.FromSeconds(1),
    MaxAsksDepth = 5,
    MaxBidsDepth = 5,
    UseTradeVolume = true,
    MinVolume = 1,
    MaxVolume = 100,
    MinSpreadStepCount = 1,
    MaxSpreadStepCount = 5
};

connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
    IsSubscribe = true,
    Generator = mdGenerator
});

// Subscribe to data reception
connector.CandleReceived += OnCandleReceived;
connector.TickTradeReceived += OnTickReceived;
connector.OrderBookReceived += OnOrderBookReceived;

// Start testing
connector.Connect();
```

## Event Handling

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
    // Processing received candles
    Console.WriteLine($"Candle: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
{
    // Processing received ticks
    Console.WriteLine($"Tick: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
}

private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
    // Using extension methods for IOrderBookMessage
    var bestBid = orderBook.GetBestBid();
    var bestAsk = orderBook.GetBestAsk();
    var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);
    
    // Processing received order books
    Console.WriteLine($"Order Book: {orderBook.ServerTime}, Best Bid: {bestBid?.Price}, Best Ask: {bestAsk?.Price}, Middle of Spread: {spreadMiddle}");
    
    // Getting price by order side
    var bidPrice = orderBook.GetPrice(Sides.Buy);
    var askPrice = orderBook.GetPrice(Sides.Sell);
    
    Console.WriteLine($"Bid Price: {bidPrice}, Ask Price: {askPrice}");
}
```

## Working with Order Book Data

When working with order books as IOrderBookMessage, you can use the following extension methods:

```cs
// Get the best bid
var bestBid = orderBook.GetBestBid();

// Get the best ask
var bestAsk = orderBook.GetBestAsk();

// Get the middle of the spread
var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);

// Get the price by order side
var price = orderBook.GetPrice(Sides.Buy); // or Sides.Sell, or null for the middle of the spread
```

When working with Level1 data, you can also get the middle of the spread:

```cs
// Get the middle of the spread from a Level1 message
var spreadMiddle = level1.GetSpreadMiddle(Security.PriceStep);
```

These extension methods simplify access to order book data and allow you to write cleaner and more understandable code when working with exchange quotes.
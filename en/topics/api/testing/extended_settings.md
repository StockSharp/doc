# Testing Settings

This section describes the main settings of [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) for testing trading strategies.

## Basic Emulator Settings

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - time change event interval. If trade generators are used, trades will be generated with this frequency. Default is 1 minute.
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - minimum latency value for submitted orders. Default is TimeSpan.Zero, which means instant acceptance of submitted orders by the exchange.
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - satisfy orders if the price "touches" the level (this assumption is sometimes too "optimistic" and should be turned off for realistic testing). If disabled, limit orders will be executed when the price "passes through them" by at least 1 step. This option works in all modes except order log. Disabled by default.

## Market Data Subscriptions

For correct strategy testing, you need to configure subscriptions to the necessary market data types. Even if a strategy is tested on candles, you need to subscribe to tick trades for correct trade emulation:

```cs
// Create a subscription to tick trades
var tickSubscription = new [Subscription](xref:StockSharp.BusinessEntities.Subscription)([DataType](xref:StockSharp.Messages.DataType).[Ticks](xref:StockSharp.Messages.DataType.Ticks), security);
_connector.Subscribe(tickSubscription);
```

If your strategy requires order book data:

```cs
// Create a subscription to order books
var depthSubscription = new [Subscription](xref:StockSharp.BusinessEntities.Subscription)([DataType](xref:StockSharp.Messages.DataType).[MarketDepth](xref:StockSharp.Messages.DataType.MarketDepth), security);
_connector.Subscribe(depthSubscription);
```

## Order Book Generation for Testing

If you don't have historical order books but need them for strategy testing, you can enable order book generation:

```cs
// Create an order book generator with trend behavior
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// Send a subscription message to the generator
_connector.[MarketDataAdapter](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketDataAdapter).SendInMessage(new [GeneratorMessage](xref:StockSharp.Messages.GeneratorMessage)
{
    IsSubscribe = true,
    Generator = mdGenerator
});
```

### Order Book Generator Settings

- Order book update interval ([MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval)) - updates cannot be more frequent than tick trades arrive, as order books are generated before each trade:

```cs
mdGenerator.Interval = TimeSpan.FromSeconds(1);
```

- Order book depth ([MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) and [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth)) - the greater the depth, the slower the testing:

```cs
mdGenerator.MaxAsksDepth = 1; 
mdGenerator.MaxBidsDepth = 1;
```

- To get a realistic level volume in the order book, you can use the [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume) option, which sets the volumes at [MarketDepth.BestBid](xref:StockSharp.BusinessEntities.MarketDepth.BestBid) and [MarketDepth.BestAsk](xref:StockSharp.BusinessEntities.MarketDepth.BestAsk) from the trade volume being generated:

```cs
mdGenerator.UseTradeVolume = true;
```

- Level volume range ([MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) and [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume)):

```cs
mdGenerator.MinVolume = 1;
mdGenerator.MaxVolume = 1;
```

- Spread configuration - the minimum generated spread equals [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep). It's recommended not to generate a spread between [MarketDepth.BestBid](xref:StockSharp.BusinessEntities.MarketDepth.BestBid) and [MarketDepth.BestAsk](xref:StockSharp.BusinessEntities.MarketDepth.BestAsk) greater than 5 price steps to avoid too wide spreads when generating from candles:

```cs
mdGenerator.MinSpreadStepCount = 1;
mdGenerator.MaxSpreadStepCount = 5;
```

## Comprehensive Testing Setup Example

```cs
// Create a historical connection
var connector = new [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector)();

// Configure basic parameters
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.[EmulationAdapter](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.EmulationAdapter).Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.[EmulationAdapter](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.EmulationAdapter).Emulator.Settings.MatchOnTouch = false;

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
var mdGenerator = new [TrendMarketDepthGenerator](xref:StockSharp.Algo.Testing.TrendMarketDepthGenerator)(security.ToSecurityId())
{
    [Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval) = TimeSpan.FromSeconds(1),
    [MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth) = 5,
    [MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) = 5,
    [UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume) = true,
    [MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) = 1,
    [MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume) = 100,
    [MinSpreadStepCount](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MinSpreadStepCount) = 1,
    [MaxSpreadStepCount](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxSpreadStepCount) = 5
};

connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
    IsSubscribe = true,
    Generator = mdGenerator
});

// Subscribe to data reception
connector.[CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) += OnCandleReceived;
connector.[TickTradeReceived](xref:StockSharp.Algo.Connector.TickTradeReceived) += OnTickReceived;
connector.[OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) += OnDepthReceived;

// Start testing
connector.Connect();
```

// Event handling

```cs
private void OnCandleReceived([Subscription](xref:StockSharp.BusinessEntities.Subscription) subscription, [ICandleMessage](xref:StockSharp.Messages.ICandleMessage) candle)
{
    // Process received candles
    Console.WriteLine($"Candle: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived([Subscription](xref:StockSharp.BusinessEntities.Subscription) subscription, [ITickTradeMessage](xref:StockSharp.Messages.ITickTradeMessage) tick)
{
    // Process received ticks
    Console.WriteLine($"Tick: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
}

private void OnDepthReceived([Subscription](xref:StockSharp.BusinessEntities.Subscription) subscription, [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) depth)
{
    // Process received order books
    Console.WriteLine($"Order book: {depth.ServerTime}, Best bid: {depth.GetBestBid()?.Price}, Best ask: {depth.GetBestAsk()?.Price}");
}
```
# Getting Historical Data

StockSharp API provides convenient mechanisms for obtaining historical data, which can be used both for testing trading strategies and for building [indicators](../indicators.md).

## Getting Historical Data via Connector

### Setting Up the Connection

To get historical data, you first need to configure a connection to the trading system:

```cs
// Create a Connector instance
var connector = new Connector();

// Add an adapter for connecting to Binance
var messageAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API Key>",
	Secret = "<Your Secret Key>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);

// Connect
connector.Connect();
```

The connection can also be configured using the graphical interface, as described in the [Connection Settings Window](../graphical_user_interface/connection_settings_window.md) section.

### Subscribing to Historical Candles

To receive historical candles, you need to create a subscription and specify the parameters of the requested data:

```cs
// Create a subscription for 5-minute candles for the selected instrument
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		// Specify the period for which to get historical data
		From = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now,
		// Set the flag to receive only completed candles
		IsFinishedOnly = true
	}
};

// Subscribe to the candle received event
connector.CandleReceived += OnCandleReceived;

// Start the subscription
connector.Subscribe(subscription);

// Event handler for receiving candles
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Check that the candle belongs to our subscription
	if (subscription != _subscription)
		return;

	// Process the received candle
	Console.WriteLine($"Candle received: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}, V:{candle.TotalVolume}");

	// For display on the chart, you can use:
	// Chart.Draw(_candleElement, candle);
}
```

### Using Candles for Charts

The received candles can be displayed on a chart using StockSharp's built-in graphical components:

```cs
// Create and configure chart elements
var chart = new Chart();
var area = new ChartArea();
var candleElement = new ChartCandleElement();

// Add area and element to the chart
chart.AddArea(area);
chart.AddElement(area, candleElement, subscription);

// In the CandleReceived event handler, draw candles
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Check that the candle belongs to our subscription
	if (subscription != _subscription)
		return;

	// If you need to display only completed candles
	if (candle.State == CandleStates.Finished)
	{
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(candleElement, candle);
		chart.Draw(chartData);
	}
}
```

## Getting Other Types of Historical Data

Similarly, you can get other types of historical data:

### Getting Historical Ticks

```cs
var tickSubscription = new Subscription(DataType.Ticks, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
		To = DateTime.Now
	}
};

connector.TickTradeReceived += (subscription, tick) =>
{
	if (subscription == tickSubscription)
		Console.WriteLine($"Tick: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
};

connector.Subscribe(tickSubscription);
```

### Getting Historical Order Books

```cs
var depthSubscription = new Subscription(DataType.MarketDepth, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
		To = DateTime.Now
	}
};

connector.OrderBookReceived += (subscription, depth) =>
{
	if (subscription == depthSubscription)
		Console.WriteLine($"Order book: {depth.ServerTime}, Best bid: {depth.GetBestBid()?.Price}, Best ask: {depth.GetBestAsk()?.Price}");
};

connector.Subscribe(depthSubscription);
```

## See Also

- [Candles](../candles.md)
- [Subscriptions](subscriptions.md)
- [Indicators](../indicators.md)

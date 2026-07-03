# 获取历史数据

StockSharp API 提供了获取历史数据的便捷机制，这些数据既可用于测试交易策略，也可用于构建 [指标](../indicators.md)。

## 通过连接器获取历史数据

### 设置连接

要获取历史数据，您首先需要配置与交易系统的连接：

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

连接也可以使用图形界面进行配置，如[连接设置窗口](../graphical_user_interface/connection_settings_window.md)部分所述。

### 订阅历史K线

要获取历史K线，您需要创建一个订阅并指定请求数据的参数：

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

### 使用K线进行绘图

接收到的K线可以使用StockSharp内置的图形组件显示在图表上：

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

## 获取其他类型的历史数据

同样，你可以获取其他类型的历史数据：

### 获取历史逐笔成交

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

### 获取历史订单簿

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

## 另请参阅

- [K线](../candles.md)
- [订阅](subscriptions.md)
- [指标](../indicators.md)

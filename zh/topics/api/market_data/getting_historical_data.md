# 获取历史数据

StockSharp API 提供了获取历史数据的便捷机制，这些数据既可用于测试交易策略，也可用于构建 [指标](../indicators.md)。

## 通过连接器获取历史数据

### 设置连接

要获取历史数据，您首先需要配置与交易系统的连接：

```cs
// 创建 Connector 实例
var connector = new Connector();

// 添加用于连接 Binance 的适配器
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
// 为所选交易品种创建 5 分钟 K线订阅
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		// 指定获取历史数据的期间
		From = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now,
		// 设置仅接收已完成 K线的标志
		IsFinishedOnly = true
	}
};

// 订阅 K线接收事件
connector.CandleReceived += OnCandleReceived;

// 启动订阅
connector.Subscribe(subscription);

// K线接收事件处理器
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 检查 K线是否属于我们的订阅
	if (subscription != _subscription)
		return;

	// 处理收到的 K线
	Console.WriteLine($"收到K线: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}, V:{candle.TotalVolume}");

	// For display on the chart, you can use:
	// Chart.Draw(_candleElement, candle);
}
```

### 使用K线进行绘图

接收到的K线可以使用StockSharp内置的图形组件显示在图表上：

```cs
// 创建并配置图表元素
var chart = new Chart();
var area = new ChartArea();
var candleElement = new ChartCandleElement();

// 向图表添加区域和元素
chart.AddArea(area);
chart.AddElement(area, candleElement, subscription);

// 在 CandleReceived 事件处理器中绘制 K线
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 检查 K线是否属于我们的订阅
	if (subscription != _subscription)
		return;

	// 如果只需要显示已完成的 K线
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
		Console.WriteLine($"订单簿: {depth.ServerTime}, 最优买价: {depth.GetBestBid()?.Price}, 最优卖价: {depth.GetBestAsk()?.Price}");
};

connector.Subscribe(depthSubscription);
```

## 另请参阅

- [K线](../candles.md)
- [订阅](subscriptions.md)
- [指标](../indicators.md)

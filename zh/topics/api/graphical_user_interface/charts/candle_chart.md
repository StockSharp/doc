# K线

[Chart](xref:StockSharp.Xaml.Charting.Chart) 是一个图形组件，允许构建股票图表：K线、指标，并在图表上显示订单和交易标记。

下面是使用 [Chart](xref:StockSharp.Xaml.Charting.Chart) 组件构建图表的示例。该示例基于 Samples/02_Candles/01_Realtime，并进行了一些修改。

![Gui ChartSample](../../../../images/gui_chartsample.png)

## 使用 Chart 构建图表的示例

1. 在 XAML 中，我们创建一个窗口并将 [Chart](xref:StockSharp.Xaml.Charting.Chart) 图形组件添加到其中。我们将组件命名为 **Chart**。注意，在创建窗口时，需要添加命名空间 *http://schemas.stocksharp.com/xaml*。

   ```xaml
   <Window x:Class="SampleCandles.ChartWindow"
           xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
           xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
           xmlns:charting="http://schemas.stocksharp.com/xaml"
           Title="ChartWindow" Height="300" Width="300">
      <charting:Chart x:Name="Chart" x:FieldModifier="public" />
   </Window>
   ```

2. 在主窗口代码中，我们声明图表区域、图表元素、指标和订阅的变量。

   ```cs
   private readonly Dictionary<Subscription, ChartWindow> _chartWindows = new Dictionary<Subscription, ChartWindow>();
   private readonly Connector _connector = new Connector();
   private readonly LogManager _logManager;
   private ChartArea _candlesArea;
   private ChartArea _indicatorsArea;
   private ChartIndicatorElement _smaChartElement;
   private ChartIndicatorElement _macdChartElement;
   private ChartCandleElement _candlesElem;
   private SimpleMovingAverage _sma;
   private MovingAverageConvergenceDivergence _macd;
   ```

3. 在 **Connect** 按钮的 **Click** 事件处理程序中，除了订阅连接器事件并调用 [IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect) 方法之外，我们还订阅了 [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) 事件。在该事件处理程序中，当收到新的K线时，将绘制图表。

   ```cs
   private void ConnectClick(object sender, RoutedEventArgs e)
   {
       _connector.CandleReceived += OnCandleReceived;
       
       // Subscribe to other necessary events
       _connector.Connected += () => this.GuiAsync(() => { /* Handle connection */ });
       _connector.Disconnected += () => this.GuiAsync(() => { /* Handle disconnection */ });
       
       // Connect to the trading system
       _connector.Connect();
   }
   ```

4. 在 **ShowChart** 按钮处理程序中，我们创建指标对象、区域和图表元素。我们将元素添加到区域，将区域添加到图表。我们打开图表窗口并开始订阅K线。

   ```cs
   private void ShowChartClick(object sender, RoutedEventArgs e)
   {
       var security = SelectedSecurity;
       
       // Create a subscription to candles
       var subscription = new Subscription(
           DataType.TimeFrame(TimeSpan.FromMinutes(5)),
           security)
       {
           MarketData = 
           {
               // Request historical data for 30 days
               From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
               To = DateTime.Now,
               // Get only finished candles
               IsFinishedOnly = true
           }
       };
       
       // Create a chart window
       _chartWindows.SafeAdd(subscription, key =>
       {
           var wnd = new ChartWindow
           {
               Title = $"{security.Code} {TimeSpan.FromMinutes(5)}"
           };
           wnd.MakeHideable();
           
           // Initialize indicators
           _sma = new SimpleMovingAverage() { Length = 11 };
           _macd = new MovingAverageConvergenceDivergence();
           
           // Initialize chart elements
           _smaChartElement = new ChartIndicatorElement();
           _macdChartElement = new ChartIndicatorElement();
           _candlesElem = new ChartCandleElement();
           
           // Set MACD display style as histogram
           _macdChartElement.DrawStyle = DrawStyles.Histogram;
           
           // Initialize chart areas
           _candlesArea = new ChartArea();
           _indicatorsArea = new ChartArea();
           
           // Add areas to the chart
           wnd.Chart.Areas.Add(_candlesArea);
           wnd.Chart.Areas.Add(_indicatorsArea);
           
           // Add elements to areas
           _candlesArea.Elements.Add(_candlesElem);
           _candlesArea.Elements.Add(_smaChartElement);
           _indicatorsArea.Elements.Add(_macdChartElement);
           
           // Bind chart elements to subscription for automatic drawing
           wnd.Chart.AddElement(_candlesArea, _candlesElem, subscription);
           wnd.Chart.AddElement(_candlesArea, _smaChartElement, subscription);
           wnd.Chart.AddElement(_indicatorsArea, _macdChartElement, subscription);
           
           return wnd;
       }).Show();
       
       // Start subscription to candles
       _connector.Subscribe(subscription);
   }
   ```

5. 在 [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) 事件处理程序中，我们绘制每根完成K线的K线和指标值。

   ```cs
   private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
   {
       var wnd = _chartWindows.TryGetValue(subscription);
       if (wnd == null)
           return;
       
       // Process only finished candles
       if (candle.State != CandleStates.Finished)
           return;
       
       // Calculate indicator values
       var smaValue = _sma.Process(candle);
       var macdValue = _macd.Process(candle);
       
       // Create data for drawing
       var data = new ChartDrawData();
       data
           .Group(candle.OpenTime)
               .Add(_candlesElem, candle)
               .Add(_smaChartElement, smaValue)
               .Add(_macdChartElement, macdValue);
       
       // Draw data on the chart in the user interface thread
       this.GuiAsync(() => wnd.Chart.Draw(data));
   }
   ```

## 带自动绘图的示例

从最新版本的 StockSharp 开始，可以设置自动绘制图表，而无需显式调用 Draw 方法。为此，在配置 Chart 时，使用 AddElement 方法，它将图表元素与订阅关联起来：

```cs
private void SetupAutoDrawingChart()
{
	var security = SelectedSecurity;
	
	// Create chart elements
	var candleElement = new ChartCandleElement();
	var smaElement = new ChartIndicatorElement { Title = "SMA" };
	
	// Create chart areas
	var area = new ChartArea();
	
	// Add area to the chart
	Chart.Areas.Add(area);
	
	// Create a subscription to candles
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		security)
	{
		MarketData = 
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now
		}
	};
	
	// Bind elements to the chart area and subscription
	Chart.AddElement(area, candleElement, subscription);
	Chart.AddElement(area, smaElement, subscription);
	
	// Create an indicator
	var sma = new SimpleMovingAverage { Length = 14 };
	
	// Subscribe to the candle receiving event for indicator processing
	_connector.CandleReceived += (sub, candle) => 
	{
		if (sub == subscription && candle.State == CandleStates.Finished)
		{
			// Process the candle with the indicator and get the value
			var smaValue = sma.Process(candle);
			
			// Draw the indicator value
			var data = new ChartDrawData();
			data
				.Group(candle.OpenTime)
					.Add(smaElement, smaValue);
			
			this.GuiAsync(() => Chart.Draw(data));
		}
	};
	
	// Start subscription
	_connector.Subscribe(subscription);
}
```

## 在图表上显示订单和交易

您可以直接在图表上显示订单和交易标记：

```cs
// Create elements for displaying orders and trades
var orderElement = new ChartOrderElement();
var tradeElement = new ChartTradeElement();

// Add elements to the chart area
_candlesArea.Elements.Add(orderElement);
_candlesArea.Elements.Add(tradeElement);

// Subscribe to order and trade receiving events
_connector.OrderReceived += (sub, order) => 
{
	if (order.Security != _security)
		return;
	
	// Draw the order on the chart
	var data = new ChartDrawData();
	data.Group(order.Time).Add(orderElement, order);
	
	this.GuiAsync(() => Chart.Draw(data));
};

_connector.OwnTradeReceived += (sub, trade) => 
{
	if (trade.Order.Security != _security)
		return;
	
	// Draw the trade on the chart
	var data = new ChartDrawData();
	data.Group(trade.Time).Add(tradeElement, trade);
	
	this.GuiAsync(() => Chart.Draw(data));
};
```

## 清除图表

要清除图表，您可以使用 Reset 方法：

```cs
// Clear the entire chart
Chart.Reset();

// Clear a specific area
_candlesArea.Reset();

// Clear a specific element
_candlesElem.Reset();
```
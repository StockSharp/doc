# 图表

要以图形方式显示蜡烛，可以使用专用的 [Chart](xref:StockSharp.Xaml.Charting.Chart) 组件（请参阅[图表构建组件](../graphical_user_interface/charts.md)）。蜡烛显示效果如下：

![sample candleschart](../../../images/sample_candleschart.png)

## 显示蜡烛的基本方式

在图表上显示蜡烛有两种方式。第一种是在接收数据时手动绘制：

```cs
// CandlesChart - StockSharp.Xaml.Chart
private ChartArea _areaComb;
private ChartCandleElement _candleElement;

// Chart initialization
private void InitializeChart()
{
	// Create chart area
	_areaComb = new ChartArea();
	_chart.Areas.Add(_areaComb);
	
	// Create chart element representing candles
	_candleElement = new ChartCandleElement() { FullTitle = "Candles" };
	_areaComb.Elements.Add(_candleElement);
	
	// Subscribe to candle reception event
	_connector.CandleReceived += OnCandleReceived;
}

// Create subscription to 5-minute candles
private void SubscribeToCandles()
{
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData = 
		{
			// Request historical data for 5 days
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};
	
	// Start subscription
	_connector.Subscribe(subscription);
}

// Handler for candle reception event
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Check if the candle is completed
	if (candle.State == CandleStates.Finished) 
	{
		// Create data for drawing
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);
		
		// Draw on chart in UI thread
		this.GuiAsync(() => _chart.Draw(chartData));
	}
}
```

## 将订阅自动绑定到图表元素

第二种方式是将订阅自动绑定到图表元素，从而自动显示接收到的数据：

```cs
// Chart initialization with automatic binding
private void InitializeChartWithAutoBinding()
{
	// Create chart area
	var area = new ChartArea();
	_chart.Areas.Add(area);
	
	// Create element for displaying candles
	var candleElement = new ChartCandleElement();
	
	// Create subscription to candles
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData = 
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};
	
	// Bind element to subscription
	_chart.AddElement(area, candleElement, subscription);
	
	// Start subscription
	_connector.Subscribe(subscription);
}
```

## 使用指标

要在图表中同时显示蜡烛和指标，请使用 [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) 类型的元素：

```cs
// Adding indicator to chart
private void AddIndicatorToChart()
{
	// Create element for indicator
	var smaElement = new ChartIndicatorElement
	{
		Title = "SMA (14)",
		Color = Colors.Red
	};
	
	// Add element to the same area as candles
	_areaComb.Elements.Add(smaElement);
	
	// Create indicator
	var sma = new SimpleMovingAverage { Length = 14 };
	
	// Subscribe to candle reception event for indicator calculation
	_connector.CandleReceived += (subscription, candle) =>
	{
		// Calculate indicator value
		var indicatorValue = sma.Process(candle);
		
		// Draw value on chart
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(smaElement, indicatorValue);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## 在不同区域中显示多个指标

可以将指标放置到不同的独立图表区域中：

```cs
// Adding indicators to different areas
private void AddIndicatorsToSeparateAreas()
{
	// Main area for candles
	var candleArea = new ChartArea();
	_chart.Areas.Add(candleArea);
	
	// Element for candles
	var candleElement = new ChartCandleElement();
	candleArea.Elements.Add(candleElement);
	
	// Element for SMA on the same area
	var smaElement = new ChartIndicatorElement { Title = "SMA (14)" };
	candleArea.Elements.Add(smaElement);
	
	// Separate area for RSI
	var rsiArea = new ChartArea();
	_chart.Areas.Add(rsiArea);
	
	// Element for RSI
	var rsiElement = new ChartIndicatorElement { Title = "RSI (14)" };
	rsiArea.Elements.Add(rsiElement);
	
	// Create indicators
	var sma = new SimpleMovingAverage { Length = 14 };
	var rsi = new RelativeStrengthIndex { Length = 14 };
	
	// Subscription to candles
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security);
	
	// Bind candle element to subscription
	_chart.AddElement(candleArea, candleElement, subscription);
	
	// Start subscription and process indicators
	_connector.Subscribe(subscription);
	
	_connector.CandleReceived += (sub, candle) =>
	{
		if (sub != subscription || candle.State != CandleStates.Finished)
			return;
		
		// Calculate indicator values
		var smaValue = sma.Process(candle);
		var rsiValue = rsi.Process(candle);
		
		// Draw values on chart
		var chartData = new ChartDrawData();
		chartData
			.Group(candle.OpenTime)
				.Add(smaElement, smaValue)
				.Add(rsiElement, rsiValue);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## 在图表上显示订单和成交

使用专用元素在图表上显示订单和成交：

```cs
// Adding elements for displaying orders and trades
private void AddOrdersAndTradesToChart()
{
	// Create elements for displaying orders and trades
	var orderElement = new ChartOrderElement();
	var tradeElement = new ChartTradeElement();
	
	// Add elements to chart area
	_areaComb.Elements.Add(orderElement);
	_areaComb.Elements.Add(tradeElement);
	
	// Subscribe to order and trade reception events
	_connector.OrderReceived += (subscription, order) =>
	{
		if (order.Security != _security)
			return;
		
		// Draw order on chart
		var chartData = new ChartDrawData();
		chartData.Group(order.Time).Add(orderElement, order);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
	
	_connector.OwnTradeReceived += (subscription, trade) =>
	{
		if (trade.Order.Security != _security)
			return;
		
		// Draw trade on chart
		var chartData = new ChartDrawData();
		chartData.Group(trade.Time).Add(tradeElement, trade);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## 配置图表外观

可以配置图表外观的多个方面：

```cs
// Configuring chart appearance
private void ConfigureChartAppearance()
{
	// Configuring chart area
	_areaComb.Height = 300;
	_areaComb.BackgroundMajorGridColor = Colors.Gray;
	_areaComb.BackgroundMinorGridColor = Colors.LightGray;
	
	// Configuring candle element
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
	_candleElement.UpBrush = Brushes.Green;
	_candleElement.DownBrush = Brushes.Red;
	_candleElement.StrokeThickness = 1;
	
	// Configuring entire chart
	_chart.IsAutoRange = true;            // Automatic scaling
	_chart.IsManualVerticalValues = false; // Automatic calculation of vertical values
	_chart.BidEnabled = false;            // Disable display of best bid price
	_chart.AskEnabled = false;            // Disable display of best ask price
}
```

## 缩放和滚动图表

以下代码用于控制图表缩放和滚动：

```cs
// Configuring zooming and scrolling
private void ConfigureChartZoomAndScroll()
{
	// Setting initial and final dates for display
	_chart.SetXRange(DateTime.Today.AddDays(-10), DateTime.Today);
	
	// Setting Y-axis range
	_chart.SetYRange(100, 150);
	
	// Buttons for zoom control
	zoomInButton.Click += (s, e) => _chart.ZoomIn();
	zoomOutButton.Click += (s, e) => _chart.ZoomOut();
	
	// Buttons for scrolling
	scrollLeftButton.Click += (s, e) => _chart.ScrollLeft();
	scrollRightButton.Click += (s, e) => _chart.ScrollRight();
	
	// Reset zoom to automatic
	resetZoomButton.Click += (s, e) => _chart.IsAutoRange = true;
}
```

## 将图表导出为图像

可以将图表保存为图像文件：

```cs
// Exporting chart to image
private void ExportChartToImage()
{
	// Create object for saving image
	var saveFileDialog = new SaveFileDialog
	{
		Filter = "PNG Image|*.png|JPEG Image|*.jpg|BMP Image|*.bmp",
		Title = "Save Chart Image"
	};
	
	if (saveFileDialog.ShowDialog() == true)
	{
		// Create image from chart
		var rtb = new RenderTargetBitmap(
			(int)_chart.ActualWidth, 
			(int)_chart.ActualHeight, 
			96, 96, 
			PixelFormats.Pbgra32);
		
		rtb.Render(_chart);
		
		// Save image in selected format
		BitmapEncoder encoder;
		
		switch (Path.GetExtension(saveFileDialog.FileName).ToLower())
		{
			case ".jpg":
				encoder = new JpegBitmapEncoder();
				break;
			case ".bmp":
				encoder = new BmpBitmapEncoder();
				break;
			default:
				encoder = new PngBitmapEncoder();
				break;
		}
		
		encoder.Frames.Add(BitmapFrame.Create(rtb));
		
		using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
		{
			encoder.Save(fileStream);
		}
	}
}
```

## 清除图表

使用以下代码清除图表数据：

```cs
// Clearing chart or its elements
private void ClearChart()
{
	// Clear entire chart
	_chart.Reset();
	
	// Clear specific area
	_areaComb.Reset();
	
	// Clear specific element
	_candleElement.Reset();
}
```

有关在图表上显示蜡烛的示例，请参阅 [Candles](../candles.md) 章节。

## 另请参阅

[图表构建组件](../graphical_user_interface/charts.md)

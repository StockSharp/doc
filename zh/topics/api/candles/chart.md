# 图表

要以图形方式显示K线，可以使用专用的 [Chart](xref:StockSharp.Xaml.Charting.Chart) 组件（请参阅[图表构建组件](../graphical_user_interface/charts.md)）。K线显示效果如下：

![sample candleschart](../../../images/sample_candleschart.png)

## 显示K线的基本方式

在图表上显示K线有两种方式。第一种是在接收数据时手动绘制：

```cs
// CandlesChart - StockSharp.Xaml.Chart
private ChartArea _areaComb;
private ChartCandleElement _candleElement;

// 图表初始化
private void InitializeChart()
{
	// 创建图表区域
	_areaComb = new ChartArea();
	_chart.Areas.Add(_areaComb);
	
	// 创建表示 K线的图表元素
	_candleElement = new ChartCandleElement() { FullTitle = "K线" };
	_areaComb.Elements.Add(_candleElement);
	
	// 订阅 K线接收事件
	_connector.CandleReceived += OnCandleReceived;
}

// 创建 5 分钟 K线订阅
private void SubscribeToCandles()
{
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData = 
		{
			// 请求 5 天的历史数据
			From = DateTime.Today.Subtract(TimeSpan.FromDays(5)),
			To = DateTime.Now
		}
	};
	
	// 启动订阅
	_connector.Subscribe(subscription);
}

// K线接收事件处理器
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 检查 K线是否已完成
	if (candle.State == CandleStates.Finished) 
	{
		// 创建绘制数据
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);
		
		// 在 UI 线程中绘制到图表
		this.GuiAsync(() => _chart.Draw(chartData));
	}
}
```

## 将订阅自动绑定到图表元素

第二种方式是将订阅自动绑定到图表元素，从而自动显示接收到的数据：

```cs
// 带自动绑定的图表初始化
private void InitializeChartWithAutoBinding()
{
	// 创建图表区域
	var area = new ChartArea();
	_chart.Areas.Add(area);
	
	// 创建用于显示 K线的元素
	var candleElement = new ChartCandleElement();
	
	// 创建 K线订阅
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
	
	// 将元素绑定到订阅
	_chart.AddElement(area, candleElement, subscription);
	
	// 启动订阅
	_connector.Subscribe(subscription);
}
```

## 使用指标

要在图表中同时显示K线和指标，请使用 [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) 类型的元素：

```cs
// 向图表添加指标
private void AddIndicatorToChart()
{
	// 为指标创建元素
	var smaElement = new ChartIndicatorElement
	{
		Title = "SMA (14)",
		Color = Colors.Red
	};
	
	// 将元素添加到与 K线相同的区域
	_areaComb.Elements.Add(smaElement);
	
	// 创建指标
	var sma = new SimpleMovingAverage { Length = 14 };
	
	// 订阅 K线接收事件以计算指标
	_connector.CandleReceived += (subscription, candle) =>
	{
		// 计算指标值
		var indicatorValue = sma.Process(candle);
		
		// 在图表上绘制值
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(smaElement, indicatorValue);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## 在不同区域中显示多个指标

可以将指标放置到不同的独立图表区域中：

```cs
// 向不同区域添加指标
private void AddIndicatorsToSeparateAreas()
{
	// K线的主区域
	var candleArea = new ChartArea();
	_chart.Areas.Add(candleArea);
	
	// K线元素
	var candleElement = new ChartCandleElement();
	candleArea.Elements.Add(candleElement);
	
	// 同一区域中的 SMA 元素
	var smaElement = new ChartIndicatorElement { Title = "SMA (14)" };
	candleArea.Elements.Add(smaElement);
	
	// RSI 的单独区域
	var rsiArea = new ChartArea();
	_chart.Areas.Add(rsiArea);
	
	// RSI 元素
	var rsiElement = new ChartIndicatorElement { Title = "RSI (14)" };
	rsiArea.Elements.Add(rsiElement);
	
	// 创建指标
	var sma = new SimpleMovingAverage { Length = 14 };
	var rsi = new RelativeStrengthIndex { Length = 14 };
	
	// K线订阅
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security);
	
	// 将 K线元素绑定到订阅
	_chart.AddElement(candleArea, candleElement, subscription);
	
	// 启动订阅并处理指标
	_connector.Subscribe(subscription);
	
	_connector.CandleReceived += (sub, candle) =>
	{
		if (sub != subscription || candle.State != CandleStates.Finished)
			return;
		
		// 计算指标值
		var smaValue = sma.Process(candle);
		var rsiValue = rsi.Process(candle);
		
		// 在图表上绘制值
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
// 添加用于显示订单和成交的元素
private void AddOrdersAndTradesToChart()
{
	// 创建用于显示订单和成交的元素
	var orderElement = new ChartOrderElement();
	var tradeElement = new ChartTradeElement();
	
	// 向图表区域添加元素
	_areaComb.Elements.Add(orderElement);
	_areaComb.Elements.Add(tradeElement);
	
	// 订阅订单和成交接收事件
	_connector.OrderReceived += (subscription, order) =>
	{
		if (order.Security != _security)
			return;
		
		// 在图表上绘制订单
		var chartData = new ChartDrawData();
		chartData.Group(order.Time).Add(orderElement, order);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
	
	_connector.OwnTradeReceived += (subscription, trade) =>
	{
		if (trade.Order.Security != _security)
			return;
		
		// 在图表上绘制成交
		var chartData = new ChartDrawData();
		chartData.Group(trade.Time).Add(tradeElement, trade);
		
		this.GuiAsync(() => _chart.Draw(chartData));
	};
}
```

## 配置图表外观

可以配置图表外观的多个方面：

```cs
// 配置图表外观
private void ConfigureChartAppearance()
{
	// 配置图表区域
	_areaComb.Height = 300;
	_areaComb.BackgroundMajorGridColor = Colors.Gray;
	_areaComb.BackgroundMinorGridColor = Colors.LightGray;
	
	// 配置 K线元素
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
	_candleElement.UpBrush = Brushes.Green;
	_candleElement.DownBrush = Brushes.Red;
	_candleElement.StrokeThickness = 1;
	
	// 配置整个图表
	_chart.IsAutoRange = true;            // 自动缩放
	_chart.IsManualVerticalValues = false; // 自动计算垂直值
	_chart.BidEnabled = false;            // 禁用最佳买价显示
	_chart.AskEnabled = false;            // 禁用最佳卖价显示
}
```

## 缩放和滚动图表

以下代码用于控制图表缩放和滚动：

```cs
// 配置缩放和滚动
private void ConfigureChartZoomAndScroll()
{
	// 设置显示的开始和结束日期
	_chart.SetXRange(DateTime.Today.AddDays(-10), DateTime.Today);
	
	// 设置 Y 轴范围
	_chart.SetYRange(100, 150);
	
	// 缩放控制按钮
	zoomInButton.Click += (s, e) => _chart.ZoomIn();
	zoomOutButton.Click += (s, e) => _chart.ZoomOut();
	
	// 滚动按钮
	scrollLeftButton.Click += (s, e) => _chart.ScrollLeft();
	scrollRightButton.Click += (s, e) => _chart.ScrollRight();
	
	// 将缩放重置为自动
	resetZoomButton.Click += (s, e) => _chart.IsAutoRange = true;
}
```

## 将图表导出为图像

可以将图表保存为图像文件：

```cs
// 将图表导出为图像
private void ExportChartToImage()
{
	// 创建用于保存图像的对象
	var saveFileDialog = new SaveFileDialog
	{
		Filter = "PNG 图像|*.png|JPEG 图像|*.jpg|BMP 图像|*.bmp",
		Title = "保存图表图像"
	};
	
	if (saveFileDialog.ShowDialog() == true)
	{
		// 从图表创建图像
		var rtb = new RenderTargetBitmap(
			(int)_chart.ActualWidth, 
			(int)_chart.ActualHeight, 
			96, 96, 
			PixelFormats.Pbgra32);
		
		rtb.Render(_chart);
		
		// 以所选格式保存图像
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
// 清除图表或其元素
private void ClearChart()
{
	// 清除整个图表
	_chart.Reset();
	
	// 清除指定区域
	_areaComb.Reset();
	
	// 清除指定元素
	_candleElement.Reset();
}
```

有关在图表上显示K线的示例，请参阅 [K线](../candles.md) 章节。

## 另请参阅

[图表构建组件](../graphical_user_interface/charts.md)

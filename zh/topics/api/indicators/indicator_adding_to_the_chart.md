# 将指标添加到图表

以下示例演示了如何在图表上添加绘图指标：

```cs
private readonly Connector _connector = new Connector();
private Security _security;
private Subscription _candleSubscription;
private SimpleMovingAverage _sma;
readonly TimeSpan _timeFrame = TimeSpan.FromMinutes(1);
private ChartArea _area;
private ChartCandleElement _candlesElem;
private ChartIndicatorElement _longMaElem;

// 初始化图表和指标
private void InitializeChart()
{
	// _chart - StockSharp.Xaml.Charting.Chart
	// 创建图表区域
	_area = new ChartArea();
	_chart.Areas.Add(_area);
	
	// 创建表示 K线的图表元素
	_candlesElem = new ChartCandleElement();
	_area.Elements.Add(_candlesElem);
	
	// 创建表示指标的图表元素
	_longMaElem = new ChartIndicatorElement
	{
		Title = "Long"
	};
	_area.Elements.Add(_longMaElem);
	
	// 创建指标
	_sma = new SimpleMovingAverage() { Length = 80 };
	
	// 订阅 K线接收事件
	_connector.CandleReceived += OnCandleReceived;
}

// 订阅 K线的方法
private void SubscribeToCandles()
{
	// 创建指定时间框架的 K线订阅
	_candleSubscription = new Subscription(
		DataType.TimeFrame(_timeFrame),
		_security)
	{
		MarketData = 
		{
			// 请求 30 天的历史数据
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now,
			// 仅接收已完成的 K线
			IsFinishedOnly = true
		}
	};
	
	// 启动订阅
	_connector.Subscribe(_candleSubscription);
}

// K线接收事件处理器
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 检查 K线是否属于我们的订阅
	if (subscription != _candleSubscription)
		return;
	
	// 检查 K线状态
	if (candle.State != CandleStates.Finished)
		return;
	
	// 用指标处理 K线
	var longValue = _sma.Process(candle);
	
	// 创建绘制数据
	var data = new ChartDrawData();
	data
		.Group(candle.OpenTime)
			.Add(_candlesElem, candle)
			.Add(_longMaElem, longValue);
	
	// 在 UI 线程中绘制到图表
	this.GuiAsync(() => _chart.Draw(data));
}

// 窗口关闭时取消订阅的方法
private void UnsubscribeFromCandles()
{
	if (_candleSubscription != null)
	{
		_connector.CandleReceived -= OnCandleReceived;
		_connector.UnSubscribe(_candleSubscription);
		_candleSubscription = null;
	}
}
```

![指标图表](../../../images/indicators_chart.png)

## 使用多个指标的示例

```cs
private readonly Connector _connector = new Connector();
private Security _security;
private Subscription _candleSubscription;
private SimpleMovingAverage _shortSma;
private SimpleMovingAverage _longSma;
private ChartArea _mainArea;
private ChartArea _indicatorArea;
private ChartCandleElement _candlesElem;
private ChartIndicatorElement _shortSmaElem;
private ChartIndicatorElement _longSmaElem;
private RelativeStrengthIndex _rsi;
private ChartIndicatorElement _rsiElem;

// 初始化图表和指标
private void InitializeChartWithMultipleIndicators()
{
	// 创建 K线和移动平均的主区域
	_mainArea = new ChartArea();
	_chart.Areas.Add(_mainArea);
	
	// 创建 RSI 区域
	_indicatorArea = new ChartArea();
	_chart.Areas.Add(_indicatorArea);
	
	// 创建图表元素
	_candlesElem = new ChartCandleElement();
	_shortSmaElem = new ChartIndicatorElement { Title = "SMA (short)" };
	_longSmaElem = new ChartIndicatorElement { Title = "SMA (long)" };
	_rsiElem = new ChartIndicatorElement { Title = "RSI" };
	
	// 设置元素颜色
	_shortSmaElem.Color = Colors.Red;
	_longSmaElem.Color = Colors.Blue;
	_rsiElem.Color = Colors.Green;
	
	// 将元素添加到对应区域
	_mainArea.Elements.Add(_candlesElem);
	_mainArea.Elements.Add(_shortSmaElem);
	_mainArea.Elements.Add(_longSmaElem);
	_indicatorArea.Elements.Add(_rsiElem);
	
	// 创建指标
	_shortSma = new SimpleMovingAverage { Length = 9 };
	_longSma = new SimpleMovingAverage { Length = 20 };
	_rsi = new RelativeStrengthIndex { Length = 14 };
	
	// 订阅 K线接收事件
	_connector.CandleReceived += OnCandleReceivedMultipleIndicators;
	
	// 创建 K线订阅
	_candleSubscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		_security)
	{
		MarketData = 
		{
			From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
			To = DateTime.Now,
			IsFinishedOnly = true
		}
	};
	
	// 启动订阅
	_connector.Subscribe(_candleSubscription);
}

// 多个指标的 K线接收事件处理器
private void OnCandleReceivedMultipleIndicators(Subscription subscription, ICandleMessage candle)
{
	// 检查 K线是否属于我们的订阅
	if (subscription != _candleSubscription)
		return;
	
	if (candle.State != CandleStates.Finished)
		return;
	
	// 用指标处理 K线
	var shortSmaValue = _shortSma.Process(candle);
	var longSmaValue = _longSma.Process(candle);
	var rsiValue = _rsi.Process(candle);
	
	// 创建绘制数据
	var data = new ChartDrawData();
	data
		.Group(candle.OpenTime)
			.Add(_candlesElem, candle)
			.Add(_shortSmaElem, shortSmaValue)
			.Add(_longSmaElem, longSmaValue)
			.Add(_rsiElem, rsiValue);
	
	// 在 UI 线程中绘制到图表
	this.GuiAsync(() => _chart.Draw(data));
}
```

## 另请参阅

[构建图表的组件](../graphical_user_interface/charts.md)
# 使用马丁格尔的移动平均

## 概览

`SmaStrategyMartingaleStrategy` 是一种基于两个简单移动平均线 ([SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage)) 交叉的交易策略，并包含马丁格尔元素。该策略使用长短期 SMA 来确定进出场信号，并在每次新交易时增加持仓大小。

## 主要组件

```cs
public class SmaStrategyMartingaleStrategy : Strategy
{
	private readonly StrategyParam<int> _longSmaLength;
	private readonly StrategyParam<int> _shortSmaLength;
	private readonly StrategyParam<DataType> _candleType;

	// Variables to store previous indicator values
	private decimal _prevLongValue;
	private decimal _prevShortValue;
	private bool _isFirstValue = true;
}
```

## 策略参数

该策略允许自定义以下参数：

- **LongSmaLength** - 长期移动平均周期（默认 80）
- **ShortSmaLength** - 短期移动平均周期（默认值 30）
- **K线类型** - 要使用的K线类型（默认5分钟）

所有参数都可以在指定的取值范围内进行优化。

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，创建了 SMA 指标，设置了K线订阅，并准备了可视化：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Create indicators
	var longSma = new SimpleMovingAverage { Length = LongSmaLength };
	var shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

	// Add indicators to the strategy collection for automatic IsFormed tracking
	Indicators.Add(longSma);
	Indicators.Add(shortSma);

	// Create subscription and bind indicators
	var subscription = SubscribeCandles(CandleType);
	subscription
		.Bind(longSma, shortSma, ProcessCandle)
		.Start();

	// Set up visualization on the chart
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, longSma, System.Drawing.Color.Blue);
		DrawIndicator(area, shortSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## 处理 K线

`ProcessCandle` 方法在每个完成的K线上被调用，并实现交易逻辑：

```cs
private void ProcessCandle(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Skip incomplete candles
	if (candle.State != CandleStates.Finished)
		return;

	// Check if the strategy is ready for trading
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// For the first value, only save data without generating signals
	if (_isFirstValue)
	{
		_prevLongValue = longValue;
		_prevShortValue = shortValue;
		_isFirstValue = false;
		return;
	}

	// Get current and previous comparison of indicator values
	var isShortLessThenLongCurrent = shortValue < longValue;
	var isShortLessThenLongPrevious = _prevShortValue < _prevLongValue;

	// Save current values as previous for the next candle
	_prevLongValue = longValue;
	_prevShortValue = shortValue;

	// Check for crossover (signal)
	if (isShortLessThenLongPrevious == isShortLessThenLongCurrent)
		return;

	// Cancel active orders before placing new ones
	CancelActiveOrders();

	// Determine trade direction
	var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

	// Calculate position size (increase position with each trade - martingale approach)
	var volume = Volume + Math.Abs(Position);

	// Create and register an order with the appropriate price
	var price = Security.ShrinkPrice(shortValue);
	RegisterOrder(CreateOrder(direction, price, volume));
}
```

## 交易逻辑

- **买入信号**：短期SMA从下方穿过长期SMA
- **卖出信号**：短期SMA从上方穿过长期SMA
- 每次新交易时，持仓大小都会按当前持仓数量增加（马丁格尔元素）
- 订单价格设置为当前短期SMA值，并四舍五入到该工具的最小价格变动单位

## 特征

- 该策略通过 `GetWorkingSecurities()` 方法自动确定要使用的工具
- 该策略仅适用于已完成的K线
- 该策略通过比较当前与之前的SMA关系来跟踪指标交叉情况
- 在下新订单之前，所有活跃的订单都已被取消
- 马丁格尔原理已实施——每进行一次新交易就增加持仓规模
- 当图表区域可用时，指标和交易会在图表上可视化
- 支持参数优化以寻找最佳策略设置

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

	// 用于保存上一周期指标值的变量
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

	// 创建指标
	var longSma = new SimpleMovingAverage { Length = LongSmaLength };
	var shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

	// 将指标添加到策略集合，以自动跟踪 IsFormed
	Indicators.Add(longSma);
	Indicators.Add(shortSma);

	// 创建订阅并绑定指标
	var subscription = SubscribeCandles(CandleType);
	subscription
		.Bind(longSma, shortSma, ProcessCandle)
		.Start();

	// 在图表上设置可视化
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
	// 跳过未完成的K线
	if (candle.State != CandleStates.Finished)
		return;

	// 检查策略是否已准备好交易
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

	// 获取当前和上一周期指标值的比较结果
	var isShortLessThenLongCurrent = shortValue < longValue;
	var isShortLessThenLongPrevious = _prevShortValue < _prevLongValue;

	// 将当前值保存为下一根K线的上一周期值
	_prevLongValue = longValue;
	_prevShortValue = shortValue;

	// 检查交叉（信号）
	if (isShortLessThenLongPrevious == isShortLessThenLongCurrent)
		return;

	// 下新单前取消活动订单
	CancelActiveOrders();

	// 确定交易方向
	var direction = isShortLessThenLongCurrent ? Sides.Sell : Sides.Buy;

	// 计算持仓规模（每次交易增加持仓——马丁格尔方法）
	var volume = Volume + Math.Abs(Position);

	// 按合适价格创建并注册订单
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

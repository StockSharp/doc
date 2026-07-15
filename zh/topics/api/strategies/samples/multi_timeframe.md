# 多时间框架策略

## 概览

`MultiTimeframeStrategy` 是一种使用两个时间框架做出交易决策的策略。小时线K线通过移动平均线交叉来确定趋势方向，而 5 分钟K线结合 [相对强弱指数](xref:StockSharp.Algo.Indicators.RelativeStrengthIndex) 指标用于在趋势方向上进行精确入场。

## 主要组件

该策略继承自 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 并使用参数进行配置：

```cs
public class MultiTimeframeStrategy : Strategy
{
	private readonly StrategyParam<int> _fastSmaLength;
	private readonly StrategyParam<int> _slowSmaLength;
	private readonly StrategyParam<int> _rsiLength;
	private readonly StrategyParam<decimal> _takeProfit;
	private readonly StrategyParam<decimal> _stopLoss;

	// 较高时间周期上的趋势方向
	private Sides? _hourlyTrend;
}
```

## 策略参数

该策略允许自定义以下参数：

- **快速移动平均周期** - 小时图的快速移动平均周期（默认值 10）
- **慢速移动平均周期** - 小时图的慢速移动平均周期（默认值 30）
- **RSI周期** - 5 分钟图表的 RSI 周期（默认 14）
- **止盈** - 止盈比例（百分比）（默认值 2）
- **止损** - 止损比例（百分比）（默认1）

所有参数都可以在指定的取值范围内进行优化。

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，创建了指标并为两个时间范围设置了K线订阅：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	var fastSma = new SimpleMovingAverage { Length = FastSmaLength };
	var slowSma = new SimpleMovingAverage { Length = SlowSmaLength };
	var rsi = new RelativeStrengthIndex { Length = RsiLength };

	_hourlyTrend = null;

	// 用于趋势检测的小时K线（SMA 交叉）
	SubscribeCandles(TimeSpan.FromHours(1))
		.Bind(fastSma, slowSma, ProcessHourlyCandle)
		.Start();

		// 用于精确入场的 5 分钟K线 (RSI)
	SubscribeCandles(TimeSpan.FromMinutes(5))
		.Bind(rsi, ProcessEntryCandle)
		.Start();

	// 设置持仓保护（止盈和止损）
	StartProtection(
		new Unit(TakeProfit, UnitTypes.Percent),
		new Unit(StopLoss, UnitTypes.Percent)
	);

	// 在图表上设置可视化
	var area = CreateChartArea();
	if (area != null)
	{
		DrawIndicator(area, fastSma, System.Drawing.Color.Blue);
		DrawIndicator(area, slowSma, System.Drawing.Color.Red);
		DrawOwnTrades(area);
	}
}
```

## 处理每小时K线

`ProcessHourlyCandle` 方法在更高时间框架上确定趋势方向：

```cs
private void ProcessHourlyCandle(ICandleMessage candle, decimal fastValue, decimal slowValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	// 通过移动平均线交叉确定趋势
	_hourlyTrend = fastValue > slowValue ? Sides.Buy : Sides.Sell;
}
```

## 处理5分钟K线

`ProcessEntryCandle` 方法基于趋势方向上的 RSI 信号实现建仓：

```cs
private void ProcessEntryCandle(ICandleMessage candle, decimal rsiValue)
{
	if (candle.State != CandleStates.Finished)
		return;

	if (_hourlyTrend == null || !IsFormedAndOnlineAndAllowTrading())
		return;

	// 买入：上升趋势且 RSI 位于超卖区
	if (_hourlyTrend == Sides.Buy && rsiValue < 30 && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// 卖出：下降趋势且 RSI 位于超买区
	else if (_hourlyTrend == Sides.Sell && rsiValue > 70 && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
}
```

## 交易逻辑

- **趋势检测**：小时图上快速SMA位于慢速SMA之上表示上升趋势，位于之下表示下降趋势
- **买入信号**：当没有多头持仓时，小时图呈上升趋势且5分钟图RSI < 30
- **卖出信号**：当没有空头持仓时，小时图呈下降趋势且5分钟图RSI > 70
- **持仓保护**：通过 `StartProtection` 自动止盈和止损

## 特征

- 该策略使用两个时间框架：小时图用于趋势，5分钟图用于入场
- 持仓入场仅在高时间框架趋势的方向上进行
- RSI被用作过滤器来寻找最佳入场点（超卖/超买）
- 持仓会自动设置止损和止盈
- 该策略仅适用于已完成的K线
- 当图表区域可用时，指标和交易会在图表上可视化
- 支持参数优化以寻找最佳策略设置

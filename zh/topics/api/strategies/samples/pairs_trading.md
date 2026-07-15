# 配对交易策略

## 概览

`PairsTradingStrategy` 是一种基于两种相关交易品种之间统计套利的配对交易策略。它跟踪两种资产价格之间的价差，并在价差显著偏离均值时开仓，期望价差回归均值。

## 主要组件

该策略继承自 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 并使用参数进行配置：

```cs
public class PairsTradingStrategy : Strategy
{
	private readonly StrategyParam<int> _spreadLength;
	private readonly StrategyParam<decimal> _entryThreshold;
	private readonly StrategyParam<decimal> _exitThreshold;
	private readonly StrategyParam<DataType> _candleType;

	// 每个交易品种的最新价格
	private decimal? _lastPrice1;
	private decimal? _lastPrice2;
}
```

## 策略参数

该策略允许自定义以下参数：

- **价差周期** - 用于计算点差均值和标准差的周期（默认值20）
- **入场阈值** - 建仓的 Z 分数阈值（默认值 2.0）
- **退出阈值** - 用于退出持仓的 Z 分数阈值（默认 0.5）
- **K线类型** - 要使用的K线类型（默认5分钟）

所有参数都可以在指定的取值范围内进行优化。

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，为两个交易品种创建指标并设置K线订阅：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// 获取用于配对交易的两个交易品种
	var securities = GetWorkingSecurities().ToArray();
	if (securities.Length < 2)
		throw new InvalidOperationException("必须指定两个交易品种。");

	var sec1 = securities[0].sec;
	var sec2 = securities[1].sec;

	// 用于计算价差均值和标准差的指标
	var sma = new SimpleMovingAverage { Length = SpreadLength };
	var stdDev = new StandardDeviation { Length = SpreadLength };

	_lastPrice1 = null;
	_lastPrice2 = null;

	// 订阅第一个交易品种的K线
	SubscribeCandles(CandleType, security: sec1)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice1 = c.ClosePrice;
		})
		.Start();

	// 订阅第二个交易品种的K线并处理价差
	SubscribeCandles(CandleType, security: sec2)
		.Bind(c =>
		{
			if (c.State != CandleStates.Finished)
				return;

			_lastPrice2 = c.ClosePrice;

			if (_lastPrice1 == null)
				return;

			ProcessSpread(_lastPrice1.Value, _lastPrice2.Value, sma, stdDev);
		})
		.Start();
}
```

## 传播处理

`ProcessSpread` 方法计算利差的 Z 分数并生成交易信号：

```cs
private void ProcessSpread(decimal price1, decimal price2,
	SimpleMovingAverage sma, StandardDeviation stdDev)
{
	// 将价差计算为价格差
	var spread = price1 - price2;

	// 处理指标
	var smaValue = sma.Process(new DecimalIndicatorValue(sma, spread));
	var devValue = stdDev.Process(new DecimalIndicatorValue(stdDev, spread));

	if (!sma.IsFormed || !stdDev.IsFormed)
		return;

	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var mean = smaValue.ToDecimal();
	var dev = devValue.ToDecimal();

	if (dev == 0)
		return;

	// 计算 Z-Score：以标准差为单位的价差偏离均值程度
	var zScore = (spread - mean) / dev;

	// 价差过高：卖出第一个交易品种，买入第二个交易品种
	if (zScore > EntryThreshold && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// 价差过低：买入第一个交易品种，卖出第二个交易品种
	else if (zScore < -EntryThreshold && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
	// 回归均值：平仓
	else if (Math.Abs(zScore) < ExitThreshold && Position != 0)
	{
		ClosePosition();
	}
}
```

## 交易逻辑

- **卖出信号**：当没有空头持仓时，利差 Z 分数超过入场阈值（默认 2.0）
- **买入信号**：当没有多头持仓时，价差 Z 分数跌破负入场阈值（默认 -2.0）
- **平仓**：当绝对 Z 分数值降至退出阈值以下（默认 0.5）时，表示价差正在回归均值

## 特征

- 该策略适用于通过 `GetWorkingSecurities()` 方法获得的两种交易品种
- 价差是通过两个交易品种的K线收盘价的差额来计算的
- Z 分数用于标准化与平均值的偏差分布
- 该策略实现了经典的均值回归概念
- 该策略仅适用于已完成的K线
- 支持参数优化以寻找最佳策略设置

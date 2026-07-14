# 布林带策略，重点关注下轨

## 概览

`BollingerStrategyLowBandStrategy` 是一种基于 [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) 指标的策略。当价格达到布林带的下边界时，它开空仓；当价格达到中线时，它平仓。

## 主要组件

该策略继承自 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 并使用参数进行配置：

```cs
public class BollingerStrategyLowBandStrategy : Strategy
{
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	private readonly StrategyParam<DataType> _candleType;

	private BollingerBands _bollingerBands;
}
```

## 策略参数

该策略允许自定义以下参数：

- **BollingerLength** - 布林带指标周期（默认 20）
- **BollingerDeviation** - 标准差乘数（默认值 2.0）
- **CandleType** - 要使用的K线类型（默认5分钟）

所有参数都可以在指定的取值范围内进行优化。

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，会创建布林带指标、设置K线订阅，并准备可视化：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// 创建指标
	_bollingerBands = new BollingerBands
	{
		Length = BollingerLength,
		Width = BollingerDeviation
	};

	// 创建订阅并绑定指标
	var subscription = SubscribeCandles(CandleType);
	subscription
		.BindEx(_bollingerBands, ProcessCandle)
		.Start();

	// 在图表上设置可视化
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, _bollingerBands, System.Drawing.Color.Purple);
		DrawOwnTrades(area);
	}
}
```

## 处理 K线

`ProcessCandle` 方法在每个完成的K线上被调用，并实现交易逻辑：

```cs
private void ProcessCandle(ICandleMessage candle, IIndicatorValue bollingerValue)
{
	// 跳过未完成的K线
	if (candle.State != CandleStates.Finished)
		return;

	// 检查策略是否已准备好交易
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var typed = (BollingerBandsValue)bollingerValue;

	// 交易逻辑：
	// 价格触及下轨时卖出（仅在无持仓时）
	if (candle.ClosePrice <= typed.LowBand && Position == 0)
	{
		SellMarket(Volume);
	}
	// 价格到达中轨时买入平仓（仅在持有空头时）
	else if (candle.ClosePrice >= typed.MiddleBand && Position < 0)
	{
		BuyMarket(Math.Abs(Position));
	}
}
```

## 交易逻辑

- **卖出信号**：当没有未平仓持仓时，K线收盘价达到或跌破下轨布林带
- **买入信号**（平空仓）：当有空头持仓时，K线收盘价达到或超过布林带中轨线
- 开仓时持仓固定，平仓时等于当前持仓的全部

## 特征

- 该策略通过 `GetWorkingSecurities()` 方法自动确定要使用的工具
- 该策略仅适用于已完成的K线
- 该策略仅使用布林带指标的下轨和中轨
- 只开空头持仓
- 当图表区域可用时，指标和交易会在图表上可视化
- 支持参数优化以寻找最佳策略设置

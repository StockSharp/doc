# 布林带策略，重点关注上轨

## 概览

`BollingerStrategyUpBandStrategy` 是一种基于 [布林带](xref:StockSharp.Algo.Indicators.BollingerBands) 指标的策略。当价格达到布林带上轨时开多仓，当价格达到中轨时平仓。

## 主要组件

该策略继承自 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 并使用参数进行配置：

```cs
public class BollingerStrategyUpBandStrategy : Strategy
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
- **布林格偏差** - 标准差乘数（默认值 2.0）
- **蜡烛类型** - 要使用的蜡烛类型（默认5分钟）

所有参数都可以在指定的取值范围内进行优化。

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，会创建布林带指标、设置蜡烛图订阅，并准备可视化：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Create indicator
	_bollingerBands = new BollingerBands
	{
		Length = BollingerLength,
		Width = BollingerDeviation
	};

	// Create subscription and bind indicator
	var subscription = SubscribeCandles(CandleType);
	subscription
		.BindEx(_bollingerBands, ProcessCandle)
		.Start();

	// Set up visualization on the chart
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawIndicator(area, _bollingerBands, System.Drawing.Color.Purple);
		DrawOwnTrades(area);
	}
}
```

## 加工蜡烛

`ProcessCandle` 方法在每个完成的蜡烛图上被调用，并实现交易逻辑：

```cs
private void ProcessCandle(ICandleMessage candle, IIndicatorValue bollingerValue)
{
	// Skip incomplete candles
	if (candle.State != CandleStates.Finished)
		return;

	// Check if the strategy is ready for trading
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	var typed = (BollingerBandsValue)bollingerValue;

	// Trading logic:
	// Buy when price touches the upper band (only when no position exists)
	if (candle.ClosePrice >= typed.UpBand && Position == 0)
	{
		BuyMarket(Volume);
	}
	// Sell to close the position when price reaches the middle line (only with a long position)
	else if (candle.ClosePrice <= typed.MiddleBand && Position > 0)
	{
		SellMarket(Math.Abs(Position));
	}
}
```

## 交易逻辑

- **买入信号**：当没有持仓时，蜡烛收盘价达到或超过上布林带
- **卖出信号**（平多仓）：当存在多仓时，K线收盘价达到或跌破布林带中轨
- 开仓时仓位固定，平仓时等于当前持仓的全部

## 特征

- 该策略通过 `GetWorkingSecurities()` 方法自动确定要使用的工具
- 该策略仅适用于已完成的蜡烛
- 该策略仅使用布林带指标的上轨和中轨
- 只开多头仓位
- 当图形区域可用时，指标和交易会在图表上可视化
- 支持参数优化以寻找最佳策略设置
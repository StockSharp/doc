# 经典布林格策略

## 概览

`BollingerStrategyClassicStrategy` 是一种基于 [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands) 指标的策略。当价格达到布林带的上边界或下边界时，它会开仓。

## 主要组件

该策略继承自 [Strategy](xref:StockSharp.Algo.Strategies.Strategy) 并使用参数进行配置：

```cs
public class BollingerStrategyClassicStrategy : Strategy
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
- **K线类型** - 要使用的K线类型（默认5分钟）

所有参数都可以在指定的取值范围内进行优化。

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，会创建布林带指标、设置K线订阅，并准备可视化：

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

## 处理 K线

`ProcessCandle` 方法在每个完成的K线上被调用，并实现交易逻辑：

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
	// Sell when price reaches or exceeds the upper band
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// Buy when price reaches or falls below the lower band
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## 交易逻辑

- **卖出信号**：当没有空头持仓时，K线收盘价达到或超过上布林带
- **买入信号**：当没有多头持仓时，K线收盘价达到或低于下布林带
- 每次新交易时，持仓量按当前持仓数量增加

## 特征

- 该策略通过 `GetWorkingSecurities()` 方法自动确定要使用的工具
- 该策略仅适用于已完成的K线
- 当图表区域可用时，指标和交易会在图表上可视化
- 支持参数优化以寻找最佳策略设置
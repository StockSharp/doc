# 一根K线逆势策略

## 概览

`OneCandleCountertrendStrategy` 是一个简单的反趋势策略，它基于对单根K线的分析来做出决策。

## 主要组件

```cs
public class OneCandleCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleType;
}
```

## 策略参数

该策略允许自定义以下参数：

- **K线类型** - 要使用的K线类型（默认5分钟）

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，创建了K线订阅并准备了可视化：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// 创建订阅
	var subscription = SubscribeCandles(CandleType);
	
	subscription
		.Bind(ProcessCandle)
		.Start();

	// 在图表上设置可视化
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawOwnTrades(area);
	}
}
```

## 处理 K线

`ProcessCandle` 方法在每个完成的K线上被调用，并实现交易逻辑：

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// 检查K线是否已完成
	if (candle.State != CandleStates.Finished)
		return;

	// 检查策略是否已准备好交易
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// 逆势策略：看跌K线买入，看涨K线卖出
	if (candle.OpenPrice < candle.ClosePrice && Position >= 0)
	{
		// 看涨K线 - 卖出
		SellMarket(Volume + Math.Abs(Position));
	}
	else if (candle.OpenPrice > candle.ClosePrice && Position <= 0)
	{
		// 看跌K线 - 买入
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## 交易逻辑

- **卖出信号**：当没有做空持仓时出现看涨K线（收盘价高于开盘价）
- **买入信号**：当没有多头持仓时出现看跌K线（收盘价低于开盘价）
- 每次新交易时，持仓量按当前持仓数量增加

## 特征

- 该策略通过 `GetWorkingSecurities()` 方法自动确定要使用的工具
- 该策略仅适用于已完成的K线
- 该策略使用市价单进行建仓
- 该策略基于单根K线应用了一个简单的反趋势检测逻辑
- 当图表区域可用时，K线和交易将在图表上显示
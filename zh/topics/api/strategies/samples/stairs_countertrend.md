# 阶梯反趋势策略

## 概览

`StairsCountertrendStrategy` 是一种逆势交易策略，用于在特定长度的已建立趋势上开仓。

## 主要组件

```cs
public class StairsCountertrendStrategy : Strategy
{
	private readonly StrategyParam<int> _length;
	private readonly StrategyParam<DataType> _candleType;
	
	private int _bullLength;
	private int _bearLength;
}
```

## 策略参数

该策略允许自定义以下参数：

- **长度** - 用于识别趋势的连续同向K线数量（默认值 3）
- **K线类型** - 要使用的K线类型（默认5分钟）

Length 参数可在 2 到 10 的范围内进行优化，步长为 1。

## 策略初始化

在 [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) 方法中，计数器被重置，K线订阅被创建，并且可视化被准备好：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// 重置计数器
	_bullLength = 0;
	_bearLength = 0;

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
	// 检查蜡烛是否已完成
	if (candle.State != CandleStates.Finished)
		return;

	// 检查策略是否已准备好交易
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// 根据蜡烛方向更新计数器
	if (candle.OpenPrice < candle.ClosePrice)
	{
		// 看涨蜡烛
		_bullLength++;
		_bearLength = 0;
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		// 看跌蜡烛
		_bullLength = 0;
		_bearLength++;
	}

	// 逆势策略： 
	// 连续 Length 根看涨蜡烛后卖出
	if (_bullLength >= Length && Position >= 0)
	{
		SellMarket(Volume + Math.Abs(Position));
	}
	// 连续 Length 根看跌蜡烛后买入
	else if (_bearLength >= Length && Position <= 0)
	{
		BuyMarket(Volume + Math.Abs(Position));
	}
}
```

## 交易逻辑

- **卖出信号**：`Length` 连续看涨K线（收盘价高于开盘价），且没有空头持仓时
- **买入信号**：`Length` 连续的看跌K线（收盘价低于开盘价），且当前没有多头持仓
- 每次新交易时，持仓量按当前持仓数量增加

## 特征

- 该策略通过 `GetWorkingSecurities()` 方法自动确定要使用的工具
- 该策略仅适用于已完成的K线
- 该策略使用市价单进行建仓
- 该策略采用逆势方法，开仓与已建立的趋势相反的方向
- 当出现反方向的K线时，K线计数器会被重置
- 当图表区域可用时，K线和交易将在图表上显示
- 支持序列长度优化以找到最佳策略设置
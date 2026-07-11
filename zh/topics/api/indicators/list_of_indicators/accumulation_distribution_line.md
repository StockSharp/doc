# ADL

**累积/分布线（ADL）** 是由 Mark Chaikin 开发的一个成交量指标。该指标通过分析价格与成交量之间的相关性来评估市场的供需关系。

使用该指标时，需要使用 [AccumulationDistributionLine](xref:StockSharp.Algo.Indicators.AccumulationDistributionLine) 类。

## 描述

累积/分配线是一个累计指标，它使用成交量和价格来判断一只交易品种处于累积（买入）还是分配（卖出）阶段。

ADL指标有助于确认趋势或警示其潜在反转：
- 如果价格在上涨而ADL在下跌，这可能表示上升趋势中的疲弱。
- 如果价格在下跌而ADL在上升，这可能表明下跌趋势可能会反转。

## 计算

累积/分配线的计算分为两个步骤：

**1. 成交量乘数（CLV - 收盘位置值）计算：**
```
CLV = ((Close - Low) - (High - Close)) / (High - Low)
```

**2. ADL 计算：**
```
ADL = 前一 ADL 值 + CLV * Volume
```

其中：
- 收盘价 - 该时期的收盘价格
- 低 - 本期最低价格
- 高 - 该时期的最高价格
- 成交量 - 该期间的交易量

如果（高 - 低）等于零，CLV 设置为零。

![ADL 指标图表](../../../../images/indicator_accumulation_distribution_line.png)

## 另请参阅

[OBV](on_balance_volume.md)

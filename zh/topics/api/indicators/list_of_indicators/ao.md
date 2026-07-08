# AO

**强力振荡器 (AO)** 是一个经典的技术指标，通过减去不同周期的移动平均线 (SMA) 构建。

要使用该指标，应使用 [AwesomeOscillator](xref:StockSharp.Algo.Indicators.AwesomeOscillator) 类。
##### 计算

精彩振荡器柱状图是一个34周期的简单移动平均线，它构建于K线的中间价(高+低)/2上，并从5周期的中间价简单移动平均线上减去。因此，用慢速移动平均线减去快速移动平均线，以了解价格运动的强度及其未来走势。

中间价 = (最高价 + 最低价) / 2
AO = SMA(中间价, 5) — SMA(中间价, 34)，其中

中间价 — 中间价
最高价 — K线的最高价
最低价 — K线的最低价
SMA — 简单移动平均线

这些数值是经典指标使用的，在设置中，可以随时指定自己的参数。

![IndicatorAwesomeOscillator](../../../../images/indicatorawesomeoscillator.png)

## 另请参阅

[布林带](bollinger_bands.md)

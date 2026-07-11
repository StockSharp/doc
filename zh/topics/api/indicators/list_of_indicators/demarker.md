# 德马克指标

**DeMarker (DeM)** 指标通过比较当前K线与前一K线的极值来评估买入和卖出压力
一。它突出了超买和超卖区域，并有助于发现潜在的转折点。

使用 [DeMarker](xref:StockSharp.Algo.Indicators.DeMarker) 类用于处理此指标。

## 计算

1. 对于每个条形计算中间值：
   `DeMax = max(High − PreviousHigh, 0)`
`DeMin = max(PreviousLow − Low, 0)`
2. 对 `DeMax` 和 `DeMin` 使用长度为 **Length** 的移动平均进行平滑处理。
3. 计算最终值：
`DeMarker = SMA(DeMax, Length) / (SMA(DeMax, Length) + SMA(DeMin, Length))`。

输出归一化到0到1之间。

## 参数

- **周期** — 平滑周期，用于控制指标的响应速度。

## 解释

- **高于0.7** — 超买状态，可能出现下跌修正。
- **低于0.3** — 超卖状态，可能向上反转。
- **价格与指标的背离** 警示趋势可能发生变化。

DeMarker 可用于反趋势入场，也可用于确认来自动量振荡器的信号。

![德马克指标](../../../../images/indicator_demarker.png)

## 另请参阅

[相对强弱指数](rsi.md)
[随机振荡器](stochastic_oscillator.md)
[动量](momentum.md)

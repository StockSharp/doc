# 百分比价格振荡器信号

**百分比价格振荡器信号 (PPOS)** 指标通过增加通常用于过滤交易的伴随信号线来增强标准百分比价格振荡器。

要使用该指标，请使用 [PercentagePriceOscillatorSignal](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorSignal) 类。

## 描述

百分比价格振荡器（PPO）衡量两个指数移动平均线（EMA）之间的百分比差异。信号版本侧重于通过额外的EMA平滑PPO线，帮助交易者仅对更持久的动量变化做出反应。

该指标由以下组成部分构成：

1. **PPO线** – 快速EMA和慢速EMA之间的百分比差异。
2. **信号线** – 从PPO线计算得出的EMA（默认9周期）。

当PPO线向上穿越信号线时，表明看涨动能增强；向下穿越则表明看跌动能增强。保持在信号线之上或之下可以确认当前趋势的强度。

## 计算

1. 计算所选价格序列的快速和慢速EMA。
2. 将PPO线计算为快速EMA和慢速EMA之间的百分比距离。
3. 使用EMA平滑PPO线以获得信号线。

```
FastEMA = EMA(Price, ShortPeriod)
SlowEMA = EMA(Price, LongPeriod)
PPO = ((FastEMA - SlowEMA) / SlowEMA) * 100
Signal = EMA(PPO, SignalPeriod)
```

## 解释

- **信号交叉。** 当PPO线从下方穿过信号线时，会出现看涨信号；相反的交叉则表示看跌动能。
- **趋势确认。** 持续在信号线之上确认了上升趋势，而持续在其下方则支持下降趋势。
- **背离。** 当价格走势与PPO线在与信号线互动时出现背离，可能预示反转。

![指标_百分比价格振荡器信号](../../../../images/indicator_percentage_price_oscillator_signal.png)

## 另请参阅

- [百分比价格振荡器](percentage_price_oscillator.md)
- [百分比价格振荡器柱状图](percentage_price_oscillator_histogram.md)

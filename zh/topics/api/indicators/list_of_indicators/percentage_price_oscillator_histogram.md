# 百分比价格振荡器柱状图

**百分比价格振荡柱状图（PPOH）** 显示 PPO 线与其信号线之间的距离，以柱状图的形式呈现，帮助交易者立即评估动量平衡。

要使用该指标，请使用 [PercentagePriceOscillatorHistogram](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorHistogram) 类。

## 描述

PPO柱状图来源于标准的百分比价格振荡器（PPO）。它不是绘制PPO线和信号线，而是将它们的差值以围绕零位的柱状形式可视化。正柱表示PPO线高于信号线（看涨动能），而负柱表示PPO线低于信号线（看跌动能）。

柱状图对PPO线与信号线之间差距的变化反应迅速，因此适合用于识别趋势强度的早期变化或发现动能背离。

## 计算

1. 使用所需的周期计算PPO线及其信号线。
2. 从PPO线中减去信号线以获得柱状图数值。

```
Histogram = PPO - 信号
```

高于零的数值显示看涨压力，而低于零的数值反映看跌压力。直方图柱体扩张或收缩的速度提供了关于动能加速或减速的线索。

## 解释

- **零线交叉。** 上穿零线确认PPO线已穿过信号线，暗示看涨趋势。下穿零线则表示看跌交叉。
- **动能激增。** 阳柱快速增长表明看涨动能增强；柱体缩小则暗示力量减弱，可能出现反转。
- **背离。** 价格走势与直方图之间的背离可以提醒交易者潜在的趋势衰竭，在价格图表上尚未显现之前。

![百分比价格振荡器柱状图 指标图表](../../../../images/indicator_percentage_price_oscillator_histogram.png)

## 另请参阅

- [百分比价格振荡器](percentage_price_oscillator.md)
- [百分比价格振荡器信号](percentage_price_oscillator_signal.md)

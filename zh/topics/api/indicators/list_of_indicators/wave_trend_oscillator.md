# WTO

**波浪趋势振荡器 (WTO)** 是一种技术指标，用于识别市场的超买和超卖水平，以及检测周期性的价格波动。WTO 结合了通道和振荡器的元素，使其成为识别市场动量和潜在反转点的有效工具。

要使用该指标，您需要使用 [WaveTrendOscillator](xref:StockSharp.Algo.Indicators.WaveTrendOscillator) 类。

## 描述

波浪趋势振荡器旨在过滤市场噪音并突出主要价格运动。该指标围绕零线振荡，产生与周期性价格运动相关的波形模式。

WTO的主要特征：
- 围绕零线的振荡，其中正值表示上升趋势，负值表示下降趋势
- 超买水平（通常高于 +60）和超卖水平（通常低于 -60）
- 能够过滤价格噪音并突出主要走势

关键指标信号：
- 零线穿越（趋势方向变化）
- 退出超买/超卖区
- 世贸组织与价格的分歧（潜在回调）
- 特定波形

## 参数

- **ESA周期** - 用于计算ESA值的EMA周期（通常为10）
- **偏差周期** - 用于计算偏差的周期（通常为21）
- **平均周期** - 用于计算最终振荡器平均值的周期（通常为4）

## 计算

波动趋势振荡器的计算涉及几个步骤：

1. 计算典型价格：
   ```
   AP = (High + Low + Close) / 3
   ```

2. 创建平滑和绝对的第一个测量值：
   ```
   ESA = EMA(AP, EsaPeriod)
   D = EMA(Abs(AP - ESA), DPeriod)
   ```

3. 计算第一个振荡器线：
   ```
   CI = (AP - ESA) / (0.015 * D)
   ```

4. 平滑振荡器以获得最终的WTO值：
   ```
   WTO = EMA(CI, AveragePeriod)
   ```

指标参数的典型值为：EsaPeriod = 10，DPeriod = 21，AveragePeriod = 4，但它们可以根据不同的时间框架和交易品种进行调整。

![WTO 指标图表](../../../../images/indicator_wave_trend_oscillator.png)

## 另请参阅

[MACD](macd.md)
[随机振荡器](stochastic_oscillator.md)

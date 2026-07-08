# EFT

**Ehlers Fisher变换（EFT）** 是由John Ehlers开发的一种技术指标，它使用Fisher统计变换将价格数据转换为正态分布形式。

要使用该指标，你需要使用 [EhlersFisherTransform](xref:StockSharp.Algo.Indicators.EhlersFisherTransform) 类。

## 描述

Ehlers费舍尔变换基于这样一个概念：市场价格并不呈现正态（高斯）分布。相反，它们往往表现为不对称分布。该指标应用数学上的费舍尔变换公式，将这些不对称分布转换为正态分布值。

这种变换使极端价格波动更加显著，并有助于更清楚地识别市场反转点。当应用费舍尔变换时，峰值会急剧上升，使市场行为的极端情况更加明显。

EFT特别适用于：
- 确定潜在的市场反转点
- 识别超买和超卖条件
- 检测价格与指标之间的隐藏背离
- 生成更准确的进场和出场信号

## 参数

该指标具有以下参数：
- **长度** - 计算周期（默认值：10）

## 计算

Ehlers Fisher变换的计算涉及几个步骤：

1. 将价格数据转换为介于 -1 和 +1 之间的数值（通常使用归一化的价格排名或其他振荡器）：
   ```
   Value = (2 * ((Price - Min) / (Max - Min))) - 1
   ```
其中 Min 和 Max 是在这个周期长度内的最低和最高价格。

2. 应用费舍尔变换：
   ```
   If Value >= 0.999, then Value = 0.999
   If Value <= -0.999, then Value = -0.999
   
   Fisher = 0.5 * ln((1 + Value) / (1 - Value))
   ```
其中 ln 是自然对数。

3. 平滑以减少噪音：
   ```
   EFT = EMA(Fisher, Period)
   ```
其中 EMA 是指数移动平均。

## 解释

Ehlers费舍尔变换可以解释如下：

1. **极端值**：
   - 高于 +2 的数值通常表示市场超买状态
   - 低于-2的数值通常表示市场超卖状态

2. **零线交叉**：
   - 从下向上穿越零线可以被视为看涨信号
   - 从上向下穿越零线可以被视为看跌信号

3. **指标反转**：
   - 从极端值的指标反转通常先于价格反转

4. **分歧**：
   - 看涨背离：价格创出新低，而EFT形成更高的低点
   - 看跌背离：价格创出新高，而EFT形成较低的高点

5. **指标线斜率**：
   - 陡峭的上升坡度表示强劲的上行动能
   - 陡峭的下坡表明强烈的下行动能

Ehlers Fisher变换与许多其他振荡器不同，它可以达到极值并在一段时间内保持，而不一定立即反转。这使其在识别强趋势运动时非常有用。

![indicator_ehlers_fisher_transform](../../../../images/indicator_ehlers_fisher_transform.png)

## 另请参阅

[重心振荡器](center_of_gravity_oscillator.md)
[正弦波](sine_wave.md)
[谐振子](harmonic_oscillator.md)
[相对强弱指数](rsi.md)

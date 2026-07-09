# HO

**谐波振荡器（HO）** 是一种基于谐波振荡理论的技术指标，有助于识别价格运动中的周期性成分。

要使用该指标，您需要使用 [HarmonicOscillator](xref:StockSharp.Algo.Indicators.HarmonicOscillator) 类。

## 描述

谐波振荡器（HO）是一种用于识别市场价格波动的周期性和循环性质的指标。它基于这样一个原理：许多价格波动包含可以被分离并用于预测未来价格波动的谐波（周期性）成分。

该指标运用频谱分析方法将价格序列分解为谐波成分，突出主要周期。然后，它将这些循环成分显示为振荡器，帮助交易者确定价格在识别的周期内可能达到的局部最高点或最低点。

HO尤其适用于：
- 确定市场的周期性特征
- 识别潜在的反转点
- 过滤市场噪音
- 预测价格可能改变方向的时刻

## 参数

该指标具有以下参数：
- **长度** - 分析周期（默认值：30）

## 计算

谐振子计算涉及以下步骤：

1. 价格序列预处理（去趋势）:
   ```
   Detrended Price = Price - SMA(Price, Length)
   ```

2. 应用频谱分析来识别主要周期：
   ```
   Spectral Components = FFT(Detrended Price)
   ```
   
3. 提取最显著的谐波分量：
   ```
   Dominant Cycles = Extract Top N Spectral Components based on amplitude
   ```

4. 基于主导周期的谐振子合成：
   ```
   HO = Reconstruction of Dominant Cycles through Inverse FFT
   ```

其中：
- 价格 - 价格（通常指收盘价）
- SMA - 简单移动平均
- FFT - 快速傅里叶变换
- 长度 - 分析周期

## 解释

谐振子可以被解释如下：

1. **零线交叉**：
   - 当HO从下向上穿过零线时，可以视为看涨信号
   - 当HO从上向下穿过零线时，可以视为看跌信号

2. **振荡器极值**:
   - 当 HO 达到局部最大值时，可能表明价格可能达到峰值
   - 当 HO 达到局部最低点时，可能表明价格底部的潜在迹象

3. **分歧**：
   - 看涨背离：价格创出新低，而HO形成更高的低点
   - 看跌背离：价格创出新高，而HO形成了较低的高点

4. **循环预测**：
   - 规则的 HO 高峰和低谷可用于预测未来的反转点
   - 分析峰值/谷值之间的持续时间可以帮助确定主导周期长度

5. **幅度变化**：
   - HO振荡幅度增加可能表明周期成分增强
   - HO振荡幅度减小可能表明周期性成分的衰减

6. **与其他指标的结合**：
   - HO 与趋势指标结合使用效果最佳
   - 在趋势市场中，HO 信号可用于确定顺势的入场点

![谐振子指标](../../../../images/indicator_harmonic_oscillator.png)

## 另请参阅

[正弦波](sine_wave.md)
[重心振荡器](center_of_gravity_oscillator.md)
[费舍尔变换](ehlers_fisher_transform.md)
[去趋势合成价格](detrended_synthetic_price.md)

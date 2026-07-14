# 超级趋势

**超级趋势指标**是一种基于平均真实波幅（ATR）的趋势跟随指标。它有助于识别当前的趋势方向和可能的反转点。

要使用该指标，应使用[SuperTrend](xref:StockSharp.Algo.Indicators.SuperTrend)类。

## 描述

SuperTrend 是使用平均价格和 ATR 值构建的。当趋势发生变化时，指标线会从价格之上切换到价格之下（反之亦然）。通过这种方式，SuperTrend 可以直观地突出显示当前趋势，直到价格穿越指标线。

## 参数

- **ATR周期** – 用于ATR计算的周期。
- **Multiplier** – 定义线条偏离平均价格程度的因子。

## 计算

1. 计算所选周期的ATR。
2. 计算两个边界：
   ```
   UpperBand = (High + Low) / 2 + Multiplier * ATR
   LowerBand = (High + Low) / 2 - Multiplier * ATR
   ```
3. SuperTrend 最初等于其中一条带，具体取决于当前趋势。
4. 如果收盘价突破 SuperTrend 线，趋势方向会改变，且该线会移到相反的一侧。

![超级趋势 指标图表](../../../../images/indicator_supertrend.png)

## 另请参阅

[平均真实波幅](atr.md)
[抛物线SAR](parabolic_sar.md)

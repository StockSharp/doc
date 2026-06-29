# CKS

**Chande Kroll 停损（CKS）** 是一个用于确定止损水平的指标，由 Tushar Chande 和 Stanley Kroll 开发，该指标能够适应市场波动，并帮助交易者设定仓位退出点。

要使用该指标，您需要使用 [ChandeKrollStop](xref:StockSharp.Algo.Indicators.ChandeKrollStop) 类。

## 描述

Chande Kroll 停损指标被开发为一种动态工具，用于设置随着市场波动性和趋势变化而调整的止损水平。它由两条线组成：上止损线（用于空头仓位）和下止损线（用于多头仓位）。

CKS 的主要优点在于其能够适应当前的市场状况。在高波动时期，止损线会远离价格，从而帮助避免因市场噪音导致的仓位过早平仓。在低波动时期，止损线会靠近价格，从而提供更紧密的趋势跟随。

CKS 尤其适用于：
- 确定多头和空头头寸的止损水平
- 跟随趋势进行自适应风险控制
- 识别潜在的趋势反转点
- 创建具有明确退出规则的机械交易系统

## 参数

该指标具有以下参数：
- **周期** - 用于计算极值的主要周期（默认值：10）
- **乘数** - ATR 的乘数，用于确定距离极值的距离（默认值：1.5）
- **StopPeriod** - 用于计算止损水平的周期（默认值：20）

## 计算

Chande Kroll 停止计算涉及以下步骤：

1. 确定周期内的高低极值：
   ```
   HighestHigh = Highest High value over Period
   LowestLow = Lowest Low value over Period
   ```

2. 计算一段时间的平均真实波幅（ATR）:
   ```
   ATR = Average TR value over Period
   ```

3. 计算上轨和下轨：
   ```
   Upper Band = HighestHigh - (Multiplier * ATR)
   Lower Band = LowestLow + (Multiplier * ATR)
   ```

4. 根据停车周期确定最终停止线：
   ```
   Upper Stop = Highest value of upper band over StopPeriod
   Lower Stop = Lowest value of lower band over StopPeriod
   ```

## 解释

- **上止点** 用于空头仓位。如果收盘价超过上止点，这可以被视为平空头仓位或开多头仓位的信号。

- **下止损**用于多头头寸。如果收盘价跌破下止损，这可以被视为平多头头寸或开空头头寸的信号。

- **价格穿越止损线** 可能表明潜在的趋势反转或新动能的开始。

- **止损线的突然变化**可能会随着市场波动的显著变化而发生。

- **与其他指标一起使用**：CKS 在与其他趋势和动量指标结合使用时效果最佳，这些指标有助于确定市场进入方向。

![指示器_chande_kroll_stop](../../../../images/indicator_chande_kroll_stop.png)

## 另请参阅

[ATR](atr.md)
[抛物线SAR](parabolic_sar.md)
[唐奇安通道](donchian_channels.md)
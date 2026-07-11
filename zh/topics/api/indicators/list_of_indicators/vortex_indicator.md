# VI

**涡旋指标 (VI)** 是由 Etienne 和 Julia Boisse 于 2009 年开发的技术指标。该指标由两条线组成，VI+ 和 VI-，显示价格的上升和下降趋势，帮助识别新趋势的开始并确认现有趋势。

要使用该指标，需要使用 [VortexIndicator](xref:StockSharp.Algo.Indicators.VortexIndicator) 类。

## 描述

涡轮指标的灵感来源于自然界中的涡旋运动原理，旨在反映市场运动的周期性特征。它由两条线组成：

- **VI+**（正涡旋指标）- 测量价格的上升运动
- **VI-**（负涡旋指标）- 测量价格下跌幅度

主要指标信号：
- 当 VI+ 从下向上穿过 VI- 时买入
- 当 VI- 从下向上穿过 VI+ 时卖出
- 线条之间的分离程度表示趋势强度

涡旋指标特别适用于：
- 确定新趋势的开始
- 评估现有趋势的强度
- 识别潜在的反转点

## 参数

- **长度** - 计算周期，通常使用14的数值。

## 计算

漩涡指标的计算分为几个步骤进行：

1. 计算上涨和下跌：
   ```
   VM+ = |当前最高价 - 前一最低价|
   VM- = |当前最低价 - 前一最高价|
   ```

2. 计算真实波幅：
   ```
   TR = Max(High - Low, |High - 前一收盘价|, |Low - 前一收盘价|)
   ```

3. 在长度周期内对 VM+ 和 VM- 值求和：
   ```
   Sum_VM+ = Sum(VM+, Length)
   Sum_VM- = Sum(VM-, Length)
   ```

4. 在指定周期内求真实波动范围的总和：
   ```
   Sum_TR = Sum(TR, Length)
   ```

5. 计算归一化的 VI+ 和 VI- 值：
   ```
   VI+ = Sum_VM+ / Sum_TR
   VI- = Sum_VM- / Sum_TR
   ```

这两条线的交叉会生成交易信号：当 VI+ 上升至 VI- 之上时，表示看涨趋势；相反，当 VI- 上升至 VI+ 之上时，表示看跌趋势。

![IndicatorVortexIndicator](../../../../images/indicator_vortex_indicator.png)

## 另请参阅

[ADX](adx.md)
[DMI](dmi.md)

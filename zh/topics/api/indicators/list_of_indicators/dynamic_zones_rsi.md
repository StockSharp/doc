# DZRSI

**动态区域 RSI (DZRSI)** 是经典相对强弱指数 (RSI) 的一种改进版本，它使用动态变化的超买和超卖水平，而不是静态水平。

要使用该指标，您需要使用 [DynamicZonesRSI](xref:StockSharp.Algo.Indicators.DynamicZonesRSI) 类。

## 描述

动态区域RSI（DZRSI）基于传统的RSI，但有一个重要改进：它不再使用固定的超买和超卖水平（通常为70和30），而是根据当前市场状况调整这些水平。

DZRSI的主要理念是，不同的市场状况需要不同的阈值来判断超买和超卖状态。在强劲的上涨趋势中，RSI可能长时间高于传统的超买水平70，而无法提供准确的买入或卖出信号。类似地，在强劲的下跌趋势中，RSI可能长时间低于超卖水平30。

DZRSI通过根据RSI自身的历史行为动态调整这些水平来解决这一问题，使该指标更能适应各种市场环境。

## 参数

该指标具有以下参数：
- **长度** - 用于计算基础 RSI 的周期（默认值：14）
- **超买水平** - 初始超买水平（默认值：70）
- **OversoldLevel** - 初始超卖水平（默认值：30）

## 计算

DZRSI 计算涉及几个步骤：

1. 计算指定周期长度的标准RSI：
   ```
   RSI = 100 - (100 / (1 + RS))
   RS = Average Positive Change / Average Negative Change
   ```

2. 确定特定历史期间的RSI振荡范围。

3. 根据此范围调整超买和超卖水平：
   ```
   Dynamic Overbought Level = Base Overbought Level + Adjustment Based on Historical Data
   Dynamic Oversold Level = Base Oversold Level - Adjustment Based on Historical Data
   ```

4. 根据当前趋势的强度调整动态区域。

## 解释

DZRSI 的解释类似于传统 RSI，但考虑了动态区域：

1. **超买和超卖信号**：
   - 当 DZRSI 升至当前动态超买水平以上时，可能表明市场处于超买状态
   - 当 DZRSI 跌破当前的动态超卖水平时，可能表明市场处于超卖状态

2. **反转信号**:
   - 从动态超买水平向下的反转可以被视为卖出信号
   - 从动态超卖水平向上的反转可以被视为买入信号

3. **分歧**：
   - 看涨背离：价格创出新低，而DZRSI形成更高的低点
   - 看跌背离：价格创出新高，而DZRSI形成较低高点

4. **趋势分析**:
   - 在上升趋势中，动态超卖水平可能高于传统的30
   - 在下行趋势中，动态超买水平可能低于传统的70

5. **中心线 (50) 交叉点**：
   - 从下向上穿越可以被视为看涨信号
   - 从上到下的穿越可以被视为看跌信号

![动态区域RSI指标](../../../../images/indicator_dynamic_zones_rsi.png)

## 另请参阅

[相对强弱指数](rsi.md)
[ConnorsRSI](connors_rsi.md)
[LRSI](laguerre_rsi.md)
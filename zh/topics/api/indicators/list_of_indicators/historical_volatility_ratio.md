# HVR

**历史波动率比率（HVR）** 是一种技术指标，用于比较短期历史波动率与长期历史波动率，以评估市场活动的变化。

要使用该指标，您需要使用 [HistoricalVolatilityRatio](xref:StockSharp.Algo.Indicators.HistoricalVolatilityRatio) 类。

## 描述

历史波动率比率（HVR）是一种相对波动率指标，用于比较短期波动率与长期市场波动率。该指标有助于确定当前波动率相对于其历史水平是增加还是减少。

HVR 的计算方法是短期历史波动率与长期历史波动率的比值。数值高于 1.0 表明当前（短期）波动率高于长期波动率，这可能表明市场活动增加或潜在趋势变化。

该指标特别适用于：
- 识别高波动性和低波动性时期
- 确定潜在的趋势反转点
- 根据当前市场情况调整交易策略
- 评估市场风险并设定适当的持仓规模

## 参数

该指标具有以下参数：
- **ShortPeriod** - 用于计算短期波动率的周期（默认值：5）
- **LongPeriod** - 用于计算长期波动率的周期（默认值：20）

## 计算

历史波动率比率的计算涉及以下步骤：

1. 计算短期历史波动率：
   ```
   Short-term Volatility = Standard Deviation of Log Returns over ShortPeriod * Sqrt(Trading Days Per Year)
   ```

2. 计算长期历史波动率：
   ```
   Long-term Volatility = Standard Deviation of Log Returns over LongPeriod * Sqrt(Trading Days Per Year)
   ```

3. 将 HVR 计算为短期波动率与长期波动率的比率：
   ```
   HVR = Short-term Volatility / Long-term Volatility
   ```

其中：
- 对数收益 - 对数回报 (ln(价格[i] / 价格[i-1]))
- 标准差 - 标准差
- 每年交易日 - 每年的交易日数量（股票市场通常为252天）
- ShortPeriod - 波动率计算的短周期
- LongPeriod - 波动率计算的长期周期

## 解释

历史波动率比率可以解释如下：

1. **等级 1.0**：
   - HVR = 1.0 意味着短期波动率等于长期波动率
   - HVR > 1.0 表示短期波动性高于长期波动性
   - HVR < 1.0 表示短期波动低于长期波动

2. **极端值**：
   - 非常高的HVR值（e.g., > 2.0）可能表明波动率急剧增加，这种情况通常发生在市场恐慌或大幅波动期间
   - 非常低的HVR值（e.g., < 0.5）可能表明波动压缩期，通常先于强烈的波动

3. **HVR 趋势**:
   - HVR 上升表示当前波动性增加
   - HVR下降表示当前波动性降低

4. **交易策略**：
   - 当 HVR 较高时，使用基于突破的策略可能是合适的
   - 当HVR较低时，均值回归或区间交易策略可能更适合

5. **风险管理**：
   - 高HVR值可能表示由于波动性增加，需要减少持仓规模
   - 低HVR值可能由于波动性降低而允许增加持仓规模

6. **潜在反转**：
   - 极端的HVR值通常预示着显著的价格波动
   - 在低波动期之后，HVR的急剧上升可能预示着新趋势的开始

![历史波动率比率指标](../../../../images/indicator_historical_volatility_ratio.png)

## 另请参阅

[平均真实波幅](atr.md)
[标准差](standard_deviation.md)
[波动指数](choppiness_index.md)
# CBCI

**康斯坦斯·布朗综合指数（CBCI）** 是由康斯坦斯·布朗开发的一个指标，它结合了各种技术指标的元素，以创建一个全面的市场分析工具。

要使用该指标，你需要使用 [ConstanceBrownCompositeIndex](xref:StockSharp.Algo.Indicators.ConstanceBrownCompositeIndex) 类。

## 描述

康斯坦斯·布朗综合指数（CBCI）是为了将多种指标的优势融合到一个综合工具中而创建的。它结合了随机振荡器、相对强弱指数（RSI）及其他振荡器的元素，以提供关于潜在市场反转和趋势变动的更准确信号。

CBCI 旨在用于：
- 识别潜在的趋势反转点
- 确定超买和超卖水平
- 检测隐藏背离
- 确认当前趋势的强度

该指标在各种时间框架和市场类型中都表现良好，包括股票、外汇和商品市场。

## 参数

该指标具有以下参数：
- **周期** - 指数的主要计算周期（默认值：14）
- **StochasticKPeriod** - 用于计算随机振荡器 %K 的周期（默认值：5）
- **StochasticDPeriod** - 用于计算随机振荡器 %D 的周期（默认值：3）

## 计算

CBCI 计算包括以下步骤：

1. 在指定周期内计算RSI：
   ```
   RSI = 100 - (100 / (1 + RS))
   RS = Average Gain / Average Loss
   ```

2. 计算随机振荡器：
   ```
   %K = ((Close - Lowest Low) / (Highest High - Lowest Low)) * 100
   %D = SMA(%K, StochasticDPeriod)
   ```

3. 结合 RSI 和随机振荡器：
   ```
   CBCI = (RSI + %K + %D) / 3
   ```

然后可以对这个组合指数进行平滑处理以减少噪声。

## 解释

- **超买和超卖水平**：
  - 高于80的数值可能表明市场处于超买状态
  - 低于20的数值可能表示市场超卖状态

- **中心线交叉**：
  - 从下方穿越到50线以上可以被视为看涨信号
  - 从上方穿过50线到下方可以被视为一个看跌信号

- **分歧**：
  - 经典背离：当价格与CBCI走势相反时
  - 隐藏的背离：当价格和CBCI创造不同类型的高点或低点时

- **趋势移动**:
  - 如果 CBCI 持续保持在 50 以上，可能表明上涨趋势的强度
  - 如果 CBCI 持续低于 50，可能表明下跌趋势的强度

![指标_constance_brown_复合指数](../../../../images/indicator_constance_brown_composite_index.png)

## 另请参阅

[相对强弱指数](rsi.md)
[随机震荡指标](stochastic_oscillator.md)
[随机K](stochastic_oscillator_k.md)
[CCI](cci.md)
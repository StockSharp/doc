# WVAD

**威廉姆斯可变累积分布(WVAD)** 是由 Larry Williams 开发的累积成交量指标。它通过分析开盘价、收盘价、最高价、最低价与交易量之间的关系来评估买卖压力。

使用该指标时，请使用 [WilliamsVariableAccumulationDistribution](xref:StockSharp.Algo.Indicators.WilliamsVariableAccumulationDistribution) 类。

## 描述

WVAD 指标衡量每根柱中买方或卖方对价格变动的控制程度，并按成交量加权此值。如果收盘价高于开盘价，则表示买方占主导，反之亦然。高低价区间用作标准化因子。

指标的主要应用：
- 确认当前趋势
- 识别指标与价格之间的背离
- 确定买入或卖出压力
- 在考虑成交量的情况下评估价格走势的强度

## 计算

WVAD 指标的计算公式如下：

```
WVAD = WVAD(previous) + ((Close - Open) / (High - Low)) * Volume
```

在哪里：
- 收盘价 - 当前周期的收盘价格
- 开盘价 - 当前周期的开盘价格
- 高点 - 当前周期的最高价格
- 最低 - 当前周期的最低价格
- 成交量 - 当前期间的交易量
- WVAD(前) - 前一个指标值

如果高点 = 低点（范围为零），则该周期的数值不被累加。

该指标是累积的——数值会随着每个新周期累积。

## 另请参阅

[WAD](williams_accumulation_distribution.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)

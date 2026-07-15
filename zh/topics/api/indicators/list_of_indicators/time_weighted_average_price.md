# 时间加权平均价格

**时间加权平均价格 (TWAP)** 是一种通过时间加权计算特定时期内交易品种平均价格的指标。TWAP 被机构投资者广泛用于以最小的市场影响执行大额订单。

要使用该指标，你需要使用 [TimeWeightedAveragePrice](xref:StockSharp.Algo.Indicators.TimeWeightedAveragePrice) 类。

## 描述

TWAP 是最常见的订单执行算法之一，它将大额订单分解为一系列在时间上均匀分布的小额订单。TWAP 的目标是在特定时间间隔内获得平均价格，同时将市场影响降到最低。

TWAP 的主要应用:
- 用于评估订单执行质量的基准价格
- 执行算法以最小化市场影响
- 用于市场分析和交易决策的工具

与VWAP（成交量加权平均价格）不同，TWAP不考虑交易量，仅专注于时间因素。

## 计算

TWAP 计算是通过将等时间间隔的价格相加，然后将该和除以时间间隔的数量来进行的：

```
TWAP = (P₁ + P₂ + P₃ + ... + Pₙ) / n
```

在哪里：
- P₁、P₂、...、Pₙ - 连续时间点的价格
- n - 时间间隔的数量

在实际操作中，各个周期（K线）的典型价格最常被使用：

```
典型价格 = (High + Low + Close) / 3
TWAP = Sum(典型价格) / 周期数量
```

递归公式也可以用于实时确定当前的TWAP值：

```
TWAP(current) = (TWAP(previous) * (n-1) + P(current)) / n
```

其中 n 是 TWAP 窗口中的观测数。

![时间加权平均价格 指标图表](../../../../images/indicator_time_weighted_average_price.png)
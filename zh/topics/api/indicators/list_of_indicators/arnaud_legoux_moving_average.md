# 阿尔诺·勒古移动平均线 (ALMA)

**阿尔诺·勒古移动平均线（ALMA）** 是由阿尔诺·勒古开发的一种指标，优化用于消除市场噪音并减少信号滞后。

要使用该指标，您需要使用 [ArnaudLegouxMovingAverage](xref:StockSharp.Algo.Indicators.ArnaudLegouxMovingAverage) 类。

## 描述

ALMA结合了两种数据平滑方法的优点：
1. 消除市场噪音（像大多数移动平均线一样）
2. 最小化滞后（许多平滑指标的典型特征）

ALMA 指标使用正态（高斯）分布作为权重函数，可以通过偏移量和西格玛参数进行微调。这使其成为技术分析中非常灵活且有效的工具。

ALMA 用于：
- 确定当前趋势
- 识别反转点
- 基于交叉点创建交易系统

## 参数

该指标具有以下参数：
- **长度** - 计算周期（要分析的K线数量）
- **Sigma** - σ，一个控制高斯曲线形状的参数（推荐值：6）
- **偏移** - 偏移，一个控制平滑度和响应速度的参数（推荐值：0.85）

## 计算

ALMA 计算分几个阶段进行：

1. 根据正态（高斯）分布确定窗口中每个数据点的权重：
   ```
   m = floor(Offset * (Length - 1))
   s = Length / Sigma
   
   对于从 0 到 Length-1 的每个 i：
   w(i) = exp(-((i - m)^2) / (2 * s^2))
   ```

2. 归一化权重：
   ```
   Sum_of_weights = 所有 w(i) 的总和
   
   对于从 0 到 Length-1 的每个 i：
   w_norm(i) = w(i) / Sum_of_weights
   ```

3. 将 ALMA 计算为加权和：
   ```
   ALMA = sum(Price(t-i) * w_norm(i))（对从 0 到 Length-1 的所有 i）
   ```

其中：
- 长度 - ALMA周期
- 偏移 - 偏移参数（从0到1）
- Sigma - sigma 参数（通常从 2 到 8）

![indicator_arnaud_legoux_moving_average](../../../../images/indicator_arnaud_legoux_moving_average.png)

## 另请参阅

[SMA](sma.md)
[EMA](ema.md)
[T3MA](t3_moving_average.md)
[零滞后指数移动平均线](zero_lag_exponential_moving_average.md)

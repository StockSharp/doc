# 零滞后指数移动平均线

**零滞后指数移动平均线（ZLEMA）** 是指数移动平均线（EMA）的一个改进版本，由 John Ehlers 开发。ZLEMA 旨在消除或显著减少传统移动平均线固有的滞后。

要使用该指标，你需要使用 [ZeroLagExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ZeroLagExponentialMovingAverage) 类。

## 描述

零滞后指数移动平均线（ZLEMA）是为解决大多数移动平均线的主要问题——信号滞后而创建的。由于计算所用的时间窗口，传统移动平均线会滞后于价格走势。ZLEMA 通过使用基于当前价格与过去价格差异的修正机制来最小化这种滞后。

ZLEMA 的主要优点：
- 对价格变化的更快反应
- 与传统移动平均相比延迟更少
- 保留EMA特有的平滑效果

ZLEMA 可用于：
- 确定趋势方向
- 寻找进出点
- 识别支撑位和阻力位
- 基于交叉点创建交易系统

## 参数

- **长度** - 决定平滑程度的计算周期（类似于 EMA 中的周期）。

## 计算

ZLEMA 的计算基于通过预测消除滞后，包括以下步骤：

1. 将滞后计算为周期的一半：
   ```
   lag = (Length - 1) / 2
   ```

2. 计算“去趋势”的价格：
   ```
   去趋势价格 = 2 * Price - Price[lag]
   ```
这是一个关键步骤，它可以实现“前瞻”并消除滞后。

3. 对去趋势后的价格应用指数平滑：
   ```
   k = 2 / (Length + 1)
   ZLEMA = k * 去趋势价格 + (1 - k) * ZLEMA[previous]
   ```

结果是一个移动平均线，它比具有相同期的普通EMA更紧密地跟随价格，同时保持平滑效果。

![零滞后指数移动平均线 指标图表](../../../../images/indicator_zero_lag_exponential_moving_average.png)

## 另请参阅

[EMA](ema.md)
[双指数移动平均](dema.md)
[TEMA](tema.md)
[T3MA](t3_moving_average.md)

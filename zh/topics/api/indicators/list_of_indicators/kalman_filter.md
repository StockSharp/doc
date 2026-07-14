# 卡尔曼滤波器

**卡尔曼滤波器**是一种递归算法，用于从有噪声的观测中估计系统的潜在状态。

要使用该指标，你需要使用[KalmanFilter](xref:StockSharp.Algo.Indicators.KalmanFilter)类。

## 描述

卡尔曼滤波器通过预测-校正循环平滑价格数据并减少市场噪音。它会随着新信息的出现而动态调整，这使得它在追踪波动市场中的趋势时非常有用。

## 参数

- **ProcessNoise** – 底层过程中的预期方差。
- **ObservationNoise** – 观测数据中的预期方差。

## 计算

在每一步，滤波器执行：
1. **基于先前估计的下一状态预测。**
2. **更新**此预测，使用最新的价格观察值和噪声估计。

这产生了一个优化的估计，能够快速对价格变化作出反应，同时过滤掉短期波动。

![卡尔曼滤波器 指标图表](../../../../images/indicator_kalman_filter.png)

## 另请参阅

[Kaufman 自适应移动平均](kama.md)
[自适应拉盖尔滤波器](adaptive_laguerre_filter.md)

# 抛物线转向点

**抛物线SAR（SAR）** - 一种趋势指标，用于指示价格的停止和反转点，以及趋势方向。

要使用该指标，应使用[ParabolicSar](xref:StockSharp.Algo.Indicators.ParabolicSar) 类。
##### 指标计算

下一个周期（K线）的指标点（SAR）价格使用以下公式计算：

上升趋势：SAR(n+1) = SAR(n) + a * (high — SAR(n))；
下降趋势：SAR(n+1) = SAR(n) + a * (low — SAR(n))，其中：

SAR(n+1) — 第n+1周期的价格；
SAR(n) — 第n周期的价格；
high和low — 分别为新的最高点和最低点（极值）。它们是在上一次指标信号激活与当前时刻之间的时间间隔中考虑的；

a — 加速因子。

加速因子是一个浮点系数，其特征为最小值、最大值以及变化步长。

该因子在反转点取最小值，等于一步，并且当价格根据趋势达到新的极值（高点或低点）时，该因子增加一个步骤。当因子达到最大值时，其增长将暂停。

![IndicatorParabolicSar](../../../../images/indicatorparabolicsar.png)

## 另请参阅

[峰](peak.md)

# 平均真实波幅

**平均真实波幅 (ATR)** 是一个显示当前波动水平的指标。

要使用该指标，应使用 [AverageTrueRange](xref:StockSharp.Algo.Indicators.AverageTrueRange) 类。
##### 指标的计算

指标的计算从确定真实范围（TR）开始，真实范围计算为以下三个数值中的最大值：
- 当前最大值和最小值之间的差异；
- 当前最高价与前一收盘价之间的差额（绝对值）；
- 当前最低价与前一收盘价之间的差额（绝对值）。

TRt = max(高点(t)-低点(t); 高点(t) - 前收盘(t-1); 前收盘(t-1)-低点(t))

使用绝对值是为了确保数值为正，因为我们关心的是两点之间的距离，而不是价格走势的方向。

基于此指标，计算ATR。它只有一个参数——周期N。默认使用14周期指标，但可以根据自己的策略进行调整。公式如下（这是指数移动平均的一种形式）

ATR(t) = ((ATR(t-1) x (N-1)) + TR(t)) / N

![IndicatorAverageTrueRange](../../../../images/indicatoraveragetruerange.png)

## 另请参阅

[AO](ao.md)
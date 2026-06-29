# MACD柱状图

**移动平均收敛发散指标 (MACD)** 是一种动量指标，用于显示证券价格的两个移动平均线之间的关系，以直方图的形式呈现。

要使用该指标，应使用 [MovingAverageConvergenceDivergenceHistogram](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergenceHistogram) 类。

该指标的计算使用了三个不同周期的指数移动平均线。短周期的快速移动平均线 (EMA_s) 从长周期的慢速移动平均线 (EMA_l) 中相减。由得到的数值构建 MACD 线。

MACD = EMA_s(P) − EMA_l(P)

默认周期为12和26。然后，这条线通过第三条指数移动平均线 (EMA_a) 进行平滑，通常周期为9，从而得到所谓的 MACD 信号线 (Signal)。

Signal = EMA_a(EMA_s(P) − EMA_l(P))

这两条结果曲线表示常规线性MACD。同时，相对于曲线波动的零线通常在指标窗口中标出。

在构建MACD柱状图（MACD Histogram）时，柱状条显示信号线与MACD线之间的差异，进一步简化了对指标的理解。

![IndicatorMovingAverageConvergenceDivergenceHistogram](../../../../images/indicatormovingaverageconvergencedivergencehistogram.png)

## 另请参阅

[带信号线的MACD](macd_with_signal_line.md)
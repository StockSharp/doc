# %R

**Williams %R（%R，Williams—百分比范围）** 是一种动量指标，波动范围在 0 到 -100 之间，并显示超买和超卖水平。

要使用该指标，应使用 [WilliamsR](xref:StockSharp.Algo.Indicators.WilliamsR) 类。
##### 计算

计算威廉指标百分比范围的公式与计算随机振荡器所使用的公式相似：

%R = - (MAX(HIGH(i - n)) - CLOSE(i)) / (MAX(HIGH(i - n)) - MIN(LOW(i - n))) * 100

在哪里：

CLOSE(i) - 今日收盘价；
MAX(HIGH(i - n)) - 过去 n 个周期中的最高值最高点；
MIN(LOW(i - n)) - 过去 n 个周期中的最低值。

n 的值被设置为指示参数。

![IndicatorWilliamsR](../../../../images/indicatorwilliamsr.png)

## 另请参阅

[之字形](zigzag.md)

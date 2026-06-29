# 数据保护官

**去趋势价格振荡器（DPO）**是一种振荡器，它消除了价格趋势，试图估计价格周期从峰到峰或从谷到谷的持续时间。与其他振荡器（如随机收敛或移动平均收敛发散指标（MACD））不同，DPO 不是动量指标。它突出显示价格的峰值和谷值，用于估计进出点。

要使用该指标，应使用 [DetrendedPriceOscillator](xref:StockSharp.Algo.Indicators.DetrendedPriceOscillator) 类。

去趋势价格振荡器的计算方法是用当前价格值减去简单移动平均（SMA）。移动平均的长度由用户确定。

![IndicatorDetrendedPriceOscillator](../../../../images/indicatordetrendedpriceoscillator.png)

## 另请参阅

[DMI](dmi.md)

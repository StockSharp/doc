# Cluster chart

ClusterChart \- is the special type of chart to display the volumes in the form of clusters with the bar charts. To use this type of the chart it is necessary to set the special style [ChartCandleElement.DrawStyle](../api/StockSharp.Xaml.Charting.ChartCandleElement.DrawStyle.html) \= [ChartCandleDrawStyles.ClusterProfile](../api/StockSharp.Xaml.Charting.ChartCandleDrawStyles.ClusterProfile.html). This chart uses the information from the [Candle.PriceLevels](../api/StockSharp.Algo.Candles.Candle.PriceLevels.html) property as source data. 

![Gui ClasterChart](~/images/Gui_ClasterChart.png)

**Main properties**

- [ClusterLineColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.ClusterLineColor.html) \- the basic cluster line color. 
- [ClusterTextColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.ClusterTextColor.html) \- the volumes values color on the chart. 
- [ClusterColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.ClusterColor.html) \- the main bars color in the clusters histograms. 
- [ClusterMaxColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.ClusterMaxColor.html) \- the maximum volume bar color in the clusters histograms. 

An example of using this type of chart is in *Samples\/Common\/SampleChart*. 

> [!TIP]
> To switch between the chart types, use the settings button (gear), located in the upper left corner of the chart.

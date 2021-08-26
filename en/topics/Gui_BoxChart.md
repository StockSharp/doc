# Box chart

BoxChart \- is the special type of chart for displaying the volumes in the form of a numbers grid. To use this chart type, it is necessary to set the special style [ChartCandleElement.DrawStyle](../api/StockSharp.Xaml.Charting.ChartCandleElement.DrawStyle.html) \= [ChartCandleDrawStyles.BoxVolume](../api/StockSharp.Xaml.Charting.ChartCandleDrawStyles.BoxVolume.html) This chart uses the information from the [Candle.PriceLevels](../api/StockSharp.Algo.Candles.Candle.PriceLevels.html) property as source data. 

![Gui BoxChart](../images/Gui_BoxChart.png)

**Main properties**

- [Timeframe2Multiplier](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2Multiplier.html) \- the multiplier factor that is applied to the main timeframe specified in the constructor to get a second timeframe. The displayed candles are joined together in groups of size corresponding to the second timeframe. The groups are drawn on the chart by the grid and frame of the respective colors. 
- [Timeframe3Multiplier](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe3Multiplier.html) \- is similarly to [Timeframe2Multiplier](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2Multiplier.html), but for the third timeframe. The 3rd timeframe is drawn on the chart using the grid of the corresponding color. 
- [FontColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.FontColor.html) \- the volumes values color on the chart. 
- [MaxVolumeColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.MaxVolumeColor.html) \- the volumes values color on the chart for the maximum volume in the specified candle. 
- [Timeframe2Color](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2Color.html) \- the 2nd timeframe grid color. 
- [Timeframe2FrameColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2FrameColor.html) \- the 2nd timeframe frame color. 
- [Timeframe3Color](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe3Color.html) \- the 3rd timeframe grid color. 

An example of using this type of chart is in *Samples\/Common\/SampleChart*. 

> [!TIP]
> To switch between the chart types, use the settings button (gear), located in the upper left corner of the chart.

# Charts

[S\#](StockSharpAbout.md) provides convenient components for charts plotting. These components are assembled in the [StockSharp.Xaml.Charting](../api/StockSharp.Xaml.Charting.html) namespace. 

The key concept in the graphical library is a *chart*. *Chart* \- is a container for other elements, which are used in the chart plotting. There are several types of *charts* in [S\#](StockSharpAbout.md). 

- [Chart](../api/StockSharp.Xaml.Charting.Chart.html) \- the graphical component for displaying the exchange charts.
- [ChartPanel](../api/StockSharp.Xaml.Charting.ChartPanel.html) \- the advanced graphical component for displaying the exchange charts.
- [EquityCurveChart](../api/StockSharp.Xaml.Charting.EquityCurveChart.html) \- the graphical component for displaying the equity curve.
- [BoxChart](Gui_BoxChart.md) \- the chart showing the volumes in a grid of numbers.
- [ClusterChart](Gui_ClasterChart.md) \- the chart showing the volumes in the form of clusters with bar charts.
- [OptionPositionChart](../api/StockSharp.Xaml.Charting.OptionPositionChart.html) \- the graphical component showing the positions and the options Greeks with regard to the underlying instrument. See [OptionPositionChart](OptionPositionChart.md).

Besides that, [S\#](StockSharpAbout.md) includes two types of charts for the volumes analysis: [BoxChart](Gui_BoxChart.md) and [ClusterChart](Gui_ClasterChart.md). 

The following figure shows the basic elements of the graphical component. 

![Gui ChartElements](../images/Gui_ChartElements.png)

### Chart's elements

Chart's elements

### Chart

Chart

[Chart](../api/StockSharp.Xaml.Charting.Chart.html) is the parent container for other elements of the graphical control. It contains methods for adding and removing the "child" elements, properties to customize the appearance of the component and the chart plotting method, as well as the method of charts drawing themselves. *Chart* can contain several areas ([ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html)) to plot charts (see Figure). [Chart](../api/StockSharp.Xaml.Charting.Chart.html) also contains the preview area **OverView** (see Figure). In this area, it is possible to select the chart viewing area using the sliders. In addition, it is possible to scroll and zoom in the chart by means of dragging the [ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html) field, X axis and using the mouse wheel. 

**The [Chart](../api/StockSharp.Xaml.Charting.Chart.html) basic properties and methods:**

- [Areas](../api/StockSharp.Xaml.Charting.Chart.Areas.html) \- the list of the [ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html).
- [ChartTheme](../api/StockSharp.Xaml.Charting.Chart.ChartTheme.html) \- the component theme.
- [IndicatorTypes](../api/StockSharp.Xaml.Charting.Chart.IndicatorTypes.html) \- the list of indicators that can be displayed on the chart.
- [CrossHair](../api/StockSharp.Xaml.Charting.Chart.CrossHair.html) \- to enable\/disable the crosshair.
- [CrossHairAxisLabels](../api/StockSharp.Xaml.Charting.Chart.CrossHairAxisLabels.html) \- to enable\/disable the crosshair marks on the axes.
- [IsAutoRange](../api/StockSharp.Xaml.Charting.Chart.IsAutoRange.html) \- to enable\/disable the X axis automatic scaling.
- [IsAutoScroll](../api/StockSharp.Xaml.Charting.Chart.IsAutoScroll.html) \- to enable\/disable the auto scroll on the X axis.
- [ShowLegend](../api/StockSharp.Xaml.Charting.Chart.ShowLegend.html) \- to enable\/disable the legend display.
- [ShowOverview](../api/StockSharp.Xaml.Charting.Chart.ShowOverview.html) \- to enable\/disable the *OverView* preview area display.
- [AddArea](../api/StockSharp.Xaml.Charting.IChart.AddArea.html) \- to add [ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html).
- [AddElement](../api/Overload:StockSharp.Xaml.Charting.IChart.AddElement.html) \- to add an item of the data series. It has several overloads.
- [Reset](../api/StockSharp.Xaml.Charting.Chart.Reset.html) \- to “reset" the values drawn before.
- [Draw](../api/StockSharp.Xaml.Charting.IChart.Draw.html) \- to draw the value on the chart.
- [OrderCreationMode](../api/StockSharp.Xaml.Charting.Chart.OrderCreationMode.html) \- The order creation mode, when set, allows you to create orders from the chart. Disabled by default.

### ChartArea

ChartArea

[ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html) \- the plotting area, it is a container for the [IChartElement](../api/StockSharp.Xaml.Charting.IChartElement.html) elements (indicators, candles, etc.), which are drawn on the chart and chart axes ([ChartAxis](../api/StockSharp.Xaml.Charting.ChartAxis.html)). 

**The [ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html)** basic properties: 

- [Elements](../api/StockSharp.Xaml.Charting.ChartArea.Elements.html) \- the [IChartElement](../api/StockSharp.Xaml.Charting.IChartElement.html) elements list.
- [XAxises](../api/StockSharp.Xaml.Charting.ChartArea.XAxises.html) \- the list of the horizontal axes.
- [YAxises](../api/StockSharp.Xaml.Charting.ChartArea.YAxises.html) \- the list of the vertical axes.

### IChartElement

IChartElement

All the elements that are displayed in the chart must implement the [IChartElement](../api/StockSharp.Xaml.Charting.IChartElement.html) interface. [S\#](StockSharpAbout.md) has the following classes that implement this interface: 

- [ChartCandleElement](../api/StockSharp.Xaml.Charting.ChartCandleElement.html) \- the element to display the candles.
- [ChartIndicatorElement](../api/StockSharp.Xaml.Charting.ChartIndicatorElement.html) \- the element to display the indicators.
- [ChartOrderElement](../api/StockSharp.Xaml.Charting.ChartOrderElement.html) \- the element to display the orders.
- [ChartTradeElement](../api/StockSharp.Xaml.Charting.ChartTradeElement.html) \- the element to display the trades.

The visual elements classes have several properties to customize the appearance of the chart. It is possible to set the color, the lines thickness and the elements style. For example, using the [ChartCandleElement.DrawStyle](../api/StockSharp.Xaml.Charting.ChartCandleElement.DrawStyle.html) property it is possible to change the appearance of the candle (candle or bar). With the [ChartIndicatorElement.DrawStyle](../api/StockSharp.Xaml.Charting.ChartIndicatorElement.DrawStyle.html) property it is possible to set the indicator line style. So, to display the indicator as a histogram it is necessary to use the [ChartIndicatorDrawStyles.Histogram](../api/StockSharp.Xaml.Charting.ChartIndicatorDrawStyles.Histogram.html) value. Properties [ChartCandleElement.ShowAxisMarker](../api/StockSharp.Xaml.Charting.ChartCandleElement.ShowAxisMarker.html) and [ChartIndicatorElement.ShowAxisMarker](../api/StockSharp.Xaml.Charting.ChartIndicatorElement.ShowAxisMarker.html) allow to enable\/disable the markers display (see Figure) on the chart axes. 

## Recommended content

[Candle chart](Gui_Chart.md)

[Candle chart panel](Gui_ChartPanel.md)

[Equity curve chart](Gui_EquityCurveChart.md)

[Box chart](Gui_BoxChart.md)

[Cluster chart](Gui_ClasterChart.md)

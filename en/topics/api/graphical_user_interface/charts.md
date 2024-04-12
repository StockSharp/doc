# Charts

[S\#](../../api.md) provides convenient components for charts plotting. These components are assembled in the [StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting) namespace. 

The key concept in the graphical library is a *chart*. *Chart* \- is a container for other elements, which are used in the chart plotting. There are several types of *charts* in [S\#](../../api.md). 

- [Chart](xref:StockSharp.Xaml.Charting.Chart) \- the graphical component for displaying the exchange charts.
- [ChartPanel](xref:StockSharp.Xaml.Charting.ChartPanel) \- the advanced graphical component for displaying the exchange charts.
- [EquityCurveChart](xref:StockSharp.Xaml.Charting.EquityCurveChart) \- the graphical component for displaying the equity curve.
- [BoxChart](charts/box_chart.md) \- the chart showing the volumes in a grid of numbers.
- [ClusterChart](charts/cluster_chart.md) \- the chart showing the volumes in the form of clusters with bar charts.
- [OptionPositionChart](xref:StockSharp.Xaml.Charting.OptionPositionChart) \- the graphical component showing the positions and the options Greeks with regard to the underlying instrument. See [OptionPositionChart](options/position_chart.md).

Besides that, [S\#](../../api.md) includes two types of charts for the volumes analysis: [BoxChart](charts/box_chart.md) and [ClusterChart](charts/cluster_chart.md). 

The following figure shows the basic elements of the graphical component. 

![Gui ChartElements](../../../images/gui_chartelements.png)

## Chart's elements

## Chart

[Chart](xref:StockSharp.Xaml.Charting.Chart) is the parent container for other elements of the graphical control. It contains methods for adding and removing the "child" elements, properties to customize the appearance of the component and the chart plotting method, as well as the method of charts drawing themselves. *Chart* can contain several areas ([ChartArea](xref:StockSharp.Xaml.Charting.ChartArea)) to plot charts (see Figure). [Chart](xref:StockSharp.Xaml.Charting.Chart) also contains the preview area **OverView** (see Figure). In this area, it is possible to select the chart viewing area using the sliders. In addition, it is possible to scroll and zoom in the chart by means of dragging the [ChartArea](xref:StockSharp.Xaml.Charting.ChartArea) field, X axis and using the mouse wheel. 

**The [Chart](xref:StockSharp.Xaml.Charting.Chart) basic properties and methods:**

- [Chart.Areas](xref:StockSharp.Xaml.Charting.Chart.Areas) \- the list of the [ChartArea](xref:StockSharp.Xaml.Charting.ChartArea).
- [Chart.ChartTheme](xref:StockSharp.Xaml.Charting.Chart.ChartTheme) \- the component theme.
- [Chart.IndicatorTypes](xref:StockSharp.Xaml.Charting.Chart.IndicatorTypes) \- the list of indicators that can be displayed on the chart.
- [Chart.CrossHair](xref:StockSharp.Xaml.Charting.Chart.CrossHair) \- to enable\/disable the crosshair.
- [Chart.CrossHairAxisLabels](xref:StockSharp.Xaml.Charting.Chart.CrossHairAxisLabels) \- to enable\/disable the crosshair marks on the axes.
- [Chart.IsAutoRange](xref:StockSharp.Xaml.Charting.Chart.IsAutoRange) \- to enable\/disable the X axis automatic scaling.
- [Chart.IsAutoScroll](xref:StockSharp.Xaml.Charting.Chart.IsAutoScroll) \- to enable\/disable the auto scroll on the X axis.
- [Chart.ShowLegend](xref:StockSharp.Xaml.Charting.Chart.ShowLegend) \- to enable\/disable the legend display.
- [Chart.ShowOverview](xref:StockSharp.Xaml.Charting.Chart.ShowOverview) \- to enable\/disable the *OverView* preview area display.
- [IChart.AddArea](xref:StockSharp.Xaml.Charting.IChart.AddArea(StockSharp.Xaml.Charting.ChartArea))**(**[StockSharp.Xaml.Charting.ChartArea](xref:StockSharp.Xaml.Charting.ChartArea) area **)** \- to add [ChartArea](xref:StockSharp.Xaml.Charting.ChartArea).
- [IChart.AddElement](xref:StockSharp.Xaml.Charting.IChart.AddElement(StockSharp.Xaml.Charting.ChartArea,StockSharp.Xaml.Charting.IChartElement))**(**[StockSharp.Xaml.Charting.ChartArea](xref:StockSharp.Xaml.Charting.ChartArea) area, [StockSharp.Xaml.Charting.IChartElement](xref:StockSharp.Xaml.Charting.IChartElement) element **)** \- to add an item of the data series. It has several overloads.
- [Chart.Reset](xref:StockSharp.Xaml.Charting.Chart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Xaml.Charting.IChartElement}))**(**[System.Collections.Generic.IEnumerable\<StockSharp.Xaml.Charting.IChartElement\>](xref:System.Collections.Generic.IEnumerable`1) elements **)** \- to “reset" the values drawn before.
- [IChart.Draw](xref:StockSharp.Xaml.Charting.IChart.Draw(StockSharp.Xaml.Charting.ChartDrawData))**(**[StockSharp.Xaml.Charting.ChartDrawData](xref:StockSharp.Xaml.Charting.ChartDrawData) data **)** \- to draw the value on the chart.
- [Chart.OrderCreationMode](xref:StockSharp.Xaml.Charting.Chart.OrderCreationMode) \- The order creation mode, when set, allows you to create orders from the chart. Disabled by default.

## ChartArea

[ChartArea](xref:StockSharp.Xaml.Charting.ChartArea) \- the plotting area, it is a container for the [IChartElement](xref:StockSharp.Xaml.Charting.IChartElement) elements (indicators, candles, etc.), which are drawn on the chart and chart axes ([ChartAxis](xref:StockSharp.Xaml.Charting.ChartAxis)). 

**The [ChartArea](xref:StockSharp.Xaml.Charting.ChartArea)** basic properties: 

- [ChartArea.Elements](xref:StockSharp.Xaml.Charting.ChartArea.Elements) \- the [IChartElement](xref:StockSharp.Xaml.Charting.IChartElement) elements list.
- [ChartArea.XAxises](xref:StockSharp.Xaml.Charting.ChartArea.XAxises) \- the list of the horizontal axes.
- [ChartArea.YAxises](xref:StockSharp.Xaml.Charting.ChartArea.YAxises) \- the list of the vertical axes.

## IChartElement

All the elements that are displayed in the chart must implement the [IChartElement](xref:StockSharp.Xaml.Charting.IChartElement) interface. [S\#](../../api.md) has the following classes that implement this interface: 

- [ChartCandleElement](xref:StockSharp.Xaml.Charting.ChartCandleElement) \- the element to display the candles.
- [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) \- the element to display the indicators.
- [ChartOrderElement](xref:StockSharp.Xaml.Charting.ChartOrderElement) \- the element to display the orders.
- [ChartTradeElement](xref:StockSharp.Xaml.Charting.ChartTradeElement) \- the element to display the trades.

The visual elements classes have several properties to customize the appearance of the chart. It is possible to set the color, the lines thickness and the elements style. For example, using the [ChartCandleElement.DrawStyle](xref:StockSharp.Xaml.Charting.ChartCandleElement.DrawStyle) property it is possible to change the appearance of the candle (candle or bar). With the [ChartIndicatorElement.DrawStyle](xref:StockSharp.Xaml.Charting.ChartIndicatorElement.DrawStyle) property it is possible to set the indicator line style. So, to display the indicator as a histogram it is necessary to use the [ChartIndicatorDrawStyles.Histogram](xref:StockSharp.Xaml.Charting.ChartIndicatorDrawStyles.Histogram) value. Properties [ChartCandleElement.ShowAxisMarker](xref:StockSharp.Xaml.Charting.ChartCandleElement.ShowAxisMarker) and [ChartIndicatorElement.ShowAxisMarker](xref:StockSharp.Xaml.Charting.ChartIndicatorElement.ShowAxisMarker) allow to enable\/disable the markers display (see Figure) on the chart axes. 

## Recommended content

[Candle chart](charts/candle_chart.md)

[Candle chart panel](charts/candle_chart_panel.md)

[Equity curve chart](charts/equity_curve_chart.md)

[Box chart](charts/box_chart.md)

[Cluster chart](charts/cluster_chart.md)

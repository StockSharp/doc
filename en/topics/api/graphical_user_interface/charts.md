# Charts

[S#](../../api.md) provides convenient components for charting. These components are gathered in the namespace [StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting).

The key concept in the graphics library is the notion of a *chart*. A *chart* is a container for other elements that are used in constructing charts. There are several types of *charts* in [S#](../../api.md):

- [Chart](xref:StockSharp.Xaml.Charting.Chart) - A graphical component for displaying stock charts.
- [ChartPanel](xref:StockSharp.Xaml.Charting.ChartPanel) - An advanced graphical component for displaying stock charts.
- [EquityCurveChart](xref:StockSharp.Xaml.Charting.EquityCurveChart) - A graphical component for displaying equity curves.
- [BoxChart](charts/box_chart.md) - A chart representing volumes as a grid of numbers.
- [ClusterChart](charts/cluster_chart.md) - A chart displaying volumes as clusters with histograms.
- [OptionPositionChart](xref:StockSharp.Xaml.Charting.OptionPositionChart) - A graphical component showing option positions and "Greeks" relative to the underlying asset. See [OptionPositionChart](options/position_chart.md).

Additionally, [S#](../../api.md) includes two types of charts for volume analysis: [BoxChart](charts/box_chart.md) and [ClusterChart](charts/cluster_chart.md).

The following figure shows the main elements of the graphical component.

![Gui ChartElements](../../../images/gui_chartelements.png)

## Graphical Component Elements

## IChart

[IChart](xref:StockSharp.Charting.IChart) is the basic interface for all types of charts. It includes methods for adding and removing "child" elements, properties to customize the appearance and drawing methods of the component, as well as a method to draw the charts themselves. A *chart* can contain several areas ([IChartArea](xref:StockSharp.Charting.IChartArea)) for plotting (see figure). [Chart](xref:StockSharp.Xaml.Charting.Chart) also includes an *OverView* preview area (see figure). In this area, you can select the chart viewing zone using sliders. Additionally, you can scroll and zoom the chart using the dragging of the [IChartArea](xref:StockSharp.Charting.IChartArea), the X-axis, and the mouse wheel.

**Key Properties and Methods of [IChart](xref:StockSharp.Charting.IChart)**

- [IChart.Areas](xref:StockSharp.Charting.IChart.Areas) - List of [IChartArea](xref:StockSharp.Charting.IChartArea) areas.
- [IThemeableChart.ChartTheme](xref:StockSharp.Charting.IThemeableChart.ChartTheme) - Theme of the component.
- [IChart.IndicatorTypes](xref:StockSharp.Charting.IChart.IndicatorTypes) - List of indicators that can be displayed on the chart.
- [IChart.CrossHair](xref:StockSharp.Charting.IChart.CrossHair) - Enable/disable the display of crosshairs.
- [IChart.CrossHairAxisLabels](xref:StockSharp.Charting.IChart.CrossHairAxisLabels) - Enable/disable the display of axis labels at crosshairs.
- [IChart.IsAutoRange](xref:StockSharp.Charting.IChart.IsAutoRange) - Enable/disable automatic X-axis scaling.
- [IChart.IsAutoScroll](xref:StockSharp.Charting.IChart.IsAutoScroll) - Enable/disable auto-scrolling on the X-axis.
- [IChart.ShowLegend](xref:StockSharp.Charting.IChart.ShowLegend) - Enable/disable the display of the legend.
- [IChart.ShowOverview](xref:StockSharp.Charting.IChart.ShowOverview) - Enable/disable the display of the *OverView* preview area.
- [IChart.AddArea](xref:StockSharp.Charting.IChart.AddArea(StockSharp.Charting.IChartArea)) - Add an [IChartArea](xref:StockSharp.Charting.IChartArea).
- [IChart.AddElement](xref:StockSharp.Charting.IChart.AddElement(StockSharp.Charting.IChartArea, StockSharp.Charting.IChartElement)) - Add a data series element. Has several overloads.
- [IChart.Reset](xref:StockSharp.Charting.IChart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Charting.IChartElement})) - "Reset" previously drawn values.
- [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData)) - Draw a value on the chart.
- [IChart.OrderCreationMode](xref:StockSharp.Charting.IChart.OrderCreationMode) - Order creation mode, when set allows creating orders from the chart. Default is off.

## IChartArea

[IChartArea](xref:StockSharp.Charting.IChartArea) - A chart plotting area, acts as a container for [IChartElement](xref:StockSharp.Charting.IChartElement) (indicators, candles, etc.) that are rendered on the chart, and chart axes ([IChartAxis](xref:StockSharp.Charting.IChartAxis)).

**Key Properties of [IChartArea](xref:StockSharp.Charting.IChartArea)**

- [IChartArea.Elements](xref:StockSharp.Charting.IChartArea.Elements) - List of [IChartElement](xref:StockSharp.Charting.IChartElement).
- [IChartArea.XAxises](xref:StockSharp.Charting.IChartArea.XAxises) - List of horizontal axes.
- [IChartArea.YAxises](xref:StockSharp.Charting.IChartArea.YAxises) - List of vertical axes.

## IChartElement

All elements displayed on the chart must implement the [IChartElement](xref:StockSharp.Charting.IChartElement) interface. In [S#](../../api.md), the following classes implement this interface:

- [ChartCandleElement](xref:StockSharp.Xaml.Charting.ChartCandleElement) - An element for displaying candles.
- [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) - An element for displaying indicators.
- [ChartOrderElement](xref:StockSharp.Xaml.Charting.ChartOrderElement) - An element for displaying orders.
- [ChartTradeElement](xref:StockSharp.Xaml.Charting.ChartTradeElement) - An element for displaying trades.

The classes of visual elements have several properties for adjusting the appearance of the chart. You can adjust colors, line thickness, and style of elements. For example, using the property [IChartCandleElement.DrawStyle](xref:StockSharp.Charting.IChartCandleElement.DrawStyle), you can change the appearance of the candle (candle or bar). Using the property [ChartIndicatorElement.DrawStyle](xref:StockSharp.Xaml.Charting.ChartIndicatorElement.DrawStyle), you can set the style of the indicator line. To display the indicator as a histogram, use the value [DrawStyles.Histogram](xref:Ecng.Drawing.DrawStyles.Histogram). The properties [ChartCandleElement.ShowAxisMarker](xref:StockSharp.Xaml.Charting.ChartCandleElement.ShowAxisMarker) and [ChartIndicatorElement.ShowAxisMarker](xref:StockSharp.Xaml.Charting.ChartIndicatorElement.ShowAxisMarker) allow turning on/off the display of markers (see figure) on the axes of the chart.

## See also

- [Candlestick chart](charts/candle_chart.md)
- [Chart panel](charts/candle_chart_panel.md)
- [Equity curve chart](charts/equity_curve_chart.md)
- [Box charts](charts/box_chart.md)
- [Clusters](charts/cluster_chart.md)
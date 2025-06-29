# Графики

[S\#](../../api.md) предоставляет удобные компоненты для построения графиков. Эти компоненты собраны в пространстве имен [StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting). 

Ключевым понятием в графической библиотеке является понятие *chart*. *Chart* \- это контейнер для других элементов, которые используются при построении графиков. В [S\#](../../api.md) имеется несколько типов *чартов*. 

- [Chart](xref:StockSharp.Xaml.Charting.Chart) \- графический компонент для отображения биржевых графиков.
- [ChartPanel](xref:StockSharp.Xaml.Charting.ChartPanel) \- расширенный графический компонент для отображения биржевых графиков.
- [EquityCurveChart](xref:StockSharp.Xaml.Charting.EquityCurveChart) \- графический компонент для отображения кривой доходности.
- [BoxChart](charts/box_chart.md) \- график, представляющий объемы в виде сетки цифр.
- [ClusterChart](charts/cluster_chart.md) \- график, отображающий объемы в виде кластеров с гистограммами.
- [OptionPositionChart](xref:StockSharp.Xaml.Charting.OptionPositionChart) \- графический компонент, показывающий позиции и "греки" опционов относительно базового актива. См. [OptionPositionChart](options/position_chart.md).

Кроме того в состав [S\#](../../api.md) входят два типа графиков для анализа объемов [BoxChart](charts/box_chart.md) и [ClusterChart](charts/cluster_chart.md). 

На следующем рисунке показаны основные элементы графического компонента. 

![Gui ChartElements](../../../images/gui_chartelements.png)

## Элементы графического компонента

## IChart

[IChart](xref:StockSharp.Charting.IChart) является базовым интерфейсом для всех видов графиков. Он содержит методы для добавления и удаления "дочерних" элементов, свойства для настройки внешнего вида компонента и способа отрисовки графиков, а также метод отрисовки самих графиков. *Chart* может содержать несколько областей ([IChartArea](xref:StockSharp.Charting.IChartArea)) для построения графиков (см. рисунок). [Chart](xref:StockSharp.Xaml.Charting.Chart) также содержит область предварительного просмотра **OverView** (см. рисунок). В этой области при помощи ползунков можно выбрать зону просмотра графика. Кроме того, можно скроллировать и зуммировать график при помощи перетаскивания области [IChartArea](xref:StockSharp.Charting.IChartArea), оси X и при помощи колесика мыши. 

**Основные свойства и методы [IChart](xref:StockSharp.Charting.IChart)**

- [IChart.Areas](xref:StockSharp.Charting.IChart.Areas) \- список областей [IChartArea](xref:StockSharp.Charting.IChartArea).
- [IThemeableChart.ChartTheme](xref:StockSharp.Charting.IThemeableChart.ChartTheme) \- тема компонента.
- [IChart.IndicatorTypes](xref:StockSharp.Charting.IChart.IndicatorTypes) \- список индикаторов, которые можно отображать на чарте.
- [IChart.CrossHair](xref:StockSharp.Charting.IChart.CrossHair) \- включить\/выключить отображение перекрестья.
- [IChart.CrossHairAxisLabels](xref:StockSharp.Charting.IChart.CrossHairAxisLabels) \- включить\/выключить отображение меток перекрестья на осях.
- [IChart.IsAutoRange](xref:StockSharp.Charting.IChart.IsAutoRange) \- включить\/выключить автоматическое масштабирование оси X.
- [IChart.IsAutoScroll](xref:StockSharp.Charting.IChart.IsAutoScroll) \- включить\/выключить автопрокрутку по оси X.
- [IChart.ShowLegend](xref:StockSharp.Charting.IChart.ShowLegend) \- включить\/выключить отображение легенды.
- [IChart.ShowOverview](xref:StockSharp.Charting.IChart.ShowOverview) \- включить\/выключить отображение области предпросмотра *OverView*.
- [IChart.AddArea](xref:StockSharp.Charting.IChart.AddArea(StockSharp.Charting.IChartArea))**(**[StockSharp.Charting.IChartArea](xref:StockSharp.Charting.IChartArea) area **)** \- добавить [IChartArea](xref:StockSharp.Charting.IChartArea).
- [IChart.AddElement](xref:StockSharp.Charting.IChart.AddElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement))**(**[StockSharp.Charting.IChartArea](xref:StockSharp.Charting.IChartArea) area, [StockSharp.Charting.IChartElement](xref:StockSharp.Charting.IChartElement) element **)** \- добавить элемент серии данных. Имеет несколько перегрузок.
- [IChart.Reset](xref:StockSharp.Charting.IChart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Charting.IChartElement}))**(**[System.Collections.Generic.IEnumerable\<StockSharp.Charting.IChartElement\>](xref:System.Collections.Generic.IEnumerable`1) elements **)** \- "сбросить" ранее отрисованные значения.
- [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData))**(**[StockSharp.Charting.IChartDrawData](xref:StockSharp.Charting.IChartDrawData) data **)** \- отрисовать значение на графике.
- [IChart.OrderCreationMode](xref:StockSharp.Charting.IChart.OrderCreationMode) \- Режим создания заявок, когда установлен позволяет создавать заявки из графика. По умолчанию выключено.

## IChartArea

[IChartArea](xref:StockSharp.Charting.IChartArea) \- область построения графика, является контейнером для элементов [IChartElement](xref:StockSharp.Charting.IChartElement) (индикаторы, свечи и т.п.), которые отрисовываются на графике, и осей ([IChartAxis](xref:StockSharp.Charting.IChartAxis)) графика. 

**Основные свойства [IChartArea](xref:StockSharp.Charting.IChartArea)**

- [IChartArea.Elements](xref:StockSharp.Charting.IChartArea.Elements) \- список элементов [IChartElement](xref:StockSharp.Charting.IChartElement).
- [IChartArea.XAxises](xref:StockSharp.Charting.IChartArea.XAxises) \- список горизонтальных осей.
- [IChartArea.YAxises](xref:StockSharp.Charting.IChartArea.YAxises) \- список вертикальных осей.

## IChartElement

Все элементы, которые отображаются на графике, должны реализовывать интерфейс [IChartElement](xref:StockSharp.Charting.IChartElement). В [S\#](../../api.md) имеются следующие классы, реализующие этот интерфейс:

- [ChartCandleElement](xref:StockSharp.Xaml.Charting.ChartCandleElement) \- элемент для отображения свечей.
- [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) \- элемент для отображения индикаторов.
- [ChartOrderElement](xref:StockSharp.Xaml.Charting.ChartOrderElement) \- элемент для отображения заявок.
- [ChartTradeElement](xref:StockSharp.Xaml.Charting.ChartTradeElement) \- элемент для отображения сделок.

Классы визуальных элементов имеют ряд свойств для настройки внешнего вида графика. Можно настроить цвета, толщину линий и стиль элементов. Например, при помощи свойства [IChartCandleElement.DrawStyle](xref:StockSharp.Charting.IChartCandleElement.DrawStyle) можно изменять внешний вид свечи (свеча или бар). При помощи свойства [ChartIndicatorElement.DrawStyle](xref:StockSharp.Xaml.Charting.ChartIndicatorElement.DrawStyle) можно установить стиль линии индикатора. Так чтобы отображать индикатор в виде гистограммы нужно использовать значение [DrawStyles.Histogram](xref:Ecng.Drawing.DrawStyles.Histogram). Свойства [ChartCandleElement.ShowAxisMarker](xref:StockSharp.Xaml.Charting.ChartCandleElement.ShowAxisMarker) и [ChartIndicatorElement.ShowAxisMarker](xref:StockSharp.Xaml.Charting.ChartIndicatorElement.ShowAxisMarker) позволяют включать\/выключать отображения маркёров (см. рисунок) на осях графика. 

## См. также

[Свечной график](charts/candle_chart.md)

[График\-панель](charts/candle_chart_panel.md)

[График эквити](charts/equity_curve_chart.md)

[Графики box chart](charts/box_chart.md)

[Кластеры](charts/cluster_chart.md)

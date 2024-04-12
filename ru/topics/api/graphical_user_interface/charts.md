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

## Chart

[Chart](xref:StockSharp.Xaml.Charting.Chart) является родительским контейнером для других элементов графического контрола. Он содержит методы для добавления и удаления "дочерних" элементов, свойства для настройки внешнего вида компонента и способа отрисовки графиков, а также метод отрисовки самих графиков. *Chart* может содержать несколько областей ([ChartArea](xref:StockSharp.Xaml.Charting.ChartArea)) для построения графиков (см. рисунок). [Chart](xref:StockSharp.Xaml.Charting.Chart) также содержит область предварительного просмотра **OverView** (см. рисунок). В этой области при помощи ползунков можно выбрать зону просмотра графика. Кроме того, можно скроллировать и зуммировать график при помощи перетаскивания области [ChartArea](xref:StockSharp.Xaml.Charting.ChartArea), оси X и при помощи колесика мыши. 

**Основные свойства и методы [Chart](xref:StockSharp.Xaml.Charting.Chart)**

- [Chart.Areas](xref:StockSharp.Xaml.Charting.Chart.Areas) \- список областей [ChartArea](xref:StockSharp.Xaml.Charting.ChartArea).
- [Chart.ChartTheme](xref:StockSharp.Xaml.Charting.Chart.ChartTheme) \- тема компонента.
- [Chart.IndicatorTypes](xref:StockSharp.Xaml.Charting.Chart.IndicatorTypes) \- список индикаторов, которые можно отображать на чарте.
- [Chart.CrossHair](xref:StockSharp.Xaml.Charting.Chart.CrossHair) \- включить\/выключить отображение перекрестья.
- [Chart.CrossHairAxisLabels](xref:StockSharp.Xaml.Charting.Chart.CrossHairAxisLabels) \- включить\/выключить отображение меток перекрестья на осях.
- [Chart.IsAutoRange](xref:StockSharp.Xaml.Charting.Chart.IsAutoRange) \- включить\/выключить автоматическое масштабирование оси X.
- [Chart.IsAutoScroll](xref:StockSharp.Xaml.Charting.Chart.IsAutoScroll) \- включить\/выключить автопрокрутку по оси X.
- [Chart.ShowLegend](xref:StockSharp.Xaml.Charting.Chart.ShowLegend) \- включить\/выключить отображение легенды.
- [Chart.ShowOverview](xref:StockSharp.Xaml.Charting.Chart.ShowOverview) \- включить\/выключить отображение области предпросмотра *OverView*.
- [IChart.AddArea](xref:StockSharp.Xaml.Charting.IChart.AddArea(StockSharp.Xaml.Charting.ChartArea))**(**[StockSharp.Xaml.Charting.ChartArea](xref:StockSharp.Xaml.Charting.ChartArea) area **)** \- добавить [ChartArea](xref:StockSharp.Xaml.Charting.ChartArea).
- [IChart.AddElement](xref:StockSharp.Xaml.Charting.IChart.AddElement(StockSharp.Xaml.Charting.ChartArea,StockSharp.Xaml.Charting.IChartElement))**(**[StockSharp.Xaml.Charting.ChartArea](xref:StockSharp.Xaml.Charting.ChartArea) area, [StockSharp.Xaml.Charting.IChartElement](xref:StockSharp.Xaml.Charting.IChartElement) element **)** \- добавить элемент серии данных. Имеет несколько перегрузок.
- [Chart.Reset](xref:StockSharp.Xaml.Charting.Chart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Xaml.Charting.IChartElement}))**(**[System.Collections.Generic.IEnumerable\<StockSharp.Xaml.Charting.IChartElement\>](xref:System.Collections.Generic.IEnumerable`1) elements **)** \- "сбросить" ранее отрисованные значения.
- [IChart.Draw](xref:StockSharp.Xaml.Charting.IChart.Draw(StockSharp.Xaml.Charting.ChartDrawData))**(**[StockSharp.Xaml.Charting.ChartDrawData](xref:StockSharp.Xaml.Charting.ChartDrawData) data **)** \- отрисовать значение на графике.
- [Chart.OrderCreationMode](xref:StockSharp.Xaml.Charting.Chart.OrderCreationMode) \- Режим создания заявок, когда установлен позволяет создавать заявки из графика. По умолчанию выключено.

## ChartArea

[ChartArea](xref:StockSharp.Xaml.Charting.ChartArea) \- область построения графика, является контейнером для элементов [IChartElement](xref:StockSharp.Xaml.Charting.IChartElement) (индикаторы, свечи и т.п.), которые отрисовываются на графике, и осей ([ChartAxis](xref:StockSharp.Xaml.Charting.ChartAxis)) графика. 

**Основные свойства [ChartArea](xref:StockSharp.Xaml.Charting.ChartArea)**

- [ChartArea.Elements](xref:StockSharp.Xaml.Charting.ChartArea.Elements) \- список элементов [IChartElement](xref:StockSharp.Xaml.Charting.IChartElement).
- [ChartArea.XAxises](xref:StockSharp.Xaml.Charting.ChartArea.XAxises) \- список горизонтальных осей.
- [ChartArea.YAxises](xref:StockSharp.Xaml.Charting.ChartArea.YAxises) \- список вертикальных осей.

## IChartElement

Все элементы, которые отображаются на графике должны, реализовывать интерфейс [IChartElement](xref:StockSharp.Xaml.Charting.IChartElement). В [S\#](../../api.md) имеются следующие классы, реализующие этот интерфейс: 

- [ChartCandleElement](xref:StockSharp.Xaml.Charting.ChartCandleElement) \- элемент для отображения свечей.
- [ChartIndicatorElement](xref:StockSharp.Xaml.Charting.ChartIndicatorElement) \- элемент для отображения индикаторов.
- [ChartOrderElement](xref:StockSharp.Xaml.Charting.ChartOrderElement) \- элемент для отображения заявок.
- [ChartTradeElement](xref:StockSharp.Xaml.Charting.ChartTradeElement) \- элемент для оторбажения сделок.

Классы визуальных элементов имеют ряд свойств для настройки внешнего вида графика. Можно настроить цвета, толщину линий и стиль элементов. Например, при помощи свойства [ChartCandleElement.DrawStyle](xref:StockSharp.Xaml.Charting.ChartCandleElement.DrawStyle) можно изменять внешний вид свечи (свеча или бар). При помощи свойства [ChartIndicatorElement.DrawStyle](xref:StockSharp.Xaml.Charting.ChartIndicatorElement.DrawStyle) можно установить стиль линии индикатора. Так чтобы отображать индикатор в виде гистограммы нужно использовать значение [ChartIndicatorDrawStyles.Histogram](xref:StockSharp.Xaml.Charting.ChartIndicatorDrawStyles.Histogram). Свойства [ChartCandleElement.ShowAxisMarker](xref:StockSharp.Xaml.Charting.ChartCandleElement.ShowAxisMarker) и [ChartIndicatorElement.ShowAxisMarker](xref:StockSharp.Xaml.Charting.ChartIndicatorElement.ShowAxisMarker) позволяют включать\/выключать отображения маркёров (см. рисунок) на осях графика. 

## См. также

[Свечной график](charts/candle_chart.md)

[График\-панель](charts/candle_chart_panel.md)

[График эквити](charts/equity_curve_chart.md)

[Графики box chart](charts/box_chart.md)

[Кластеры](charts/cluster_chart.md)

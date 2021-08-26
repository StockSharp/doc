# Графики

[S\#](StockSharpAbout.md) предоставляет удобные компоненты для построения графиков. Эти компоненты собраны в пространстве имен [StockSharp.Xaml.Charting](../api/StockSharp.Xaml.Charting.html). 

Ключевым понятием в графической библиотеке является понятие *chart*. *Chart* \- это контейнер для других элементов, которые используются при построении графиков. В [S\#](StockSharpAbout.md) имеется несколько типов *чартов*. 

- [Chart](../api/StockSharp.Xaml.Charting.Chart.html) \- графический компонент для отображения биржевых графиков.
- [ChartPanel](../api/StockSharp.Xaml.Charting.ChartPanel.html) \- расширенный графический компонент для отображения биржевых графиков.
- [EquityCurveChart](../api/StockSharp.Xaml.Charting.EquityCurveChart.html) \- графический компонент для отображения кривой доходности.
- [BoxChart](Gui_BoxChart.md) \- график, представляющий объемы в виде сетки цифр.
- [ClusterChart](Gui_ClasterChart.md) \- график, отображающий объемы в виде кластеров с гистограммами.
- [OptionPositionChart](../api/StockSharp.Xaml.Charting.OptionPositionChart.html) \- графический компонент, показывающий позиции и "греки" опционов относительно базового актива. См. [OptionPositionChart](OptionPositionChart.md).

Кроме того в состав [S\#](StockSharpAbout.md) входят два типа графиков для анализа объемов [BoxChart](Gui_BoxChart.md) и [ClusterChart](Gui_ClasterChart.md). 

На следующем рисунке показаны основные элементы графического компонента. 

![Gui ChartElements](~/images/Gui_ChartElements.png)

### Элементы графического компонента

Элементы графического компонента

### Chart

Chart

[Chart](../api/StockSharp.Xaml.Charting.Chart.html) является родительским контейнером для других элементов графического контрола. Он содержит методы для добавления и удаления "дочерних" элементов, свойства для настройки внешнего вида компонента и способа отрисовки графиков, а также метод отрисовки самих графиков. *Chart* может содержать несколько областей ([ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html)) для построения графиков (см. рисунок). [Chart](../api/StockSharp.Xaml.Charting.Chart.html) также содержит область предварительного просмотра **OverView** (см. рисунок). В этой области при помощи ползунков можно выбрать зону просмотра графика. Кроме того, можно скроллировать и зуммировать график при помощи перетаскивания области [ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html), оси X и при помощи колесика мыши. 

**Основные свойства и методы [Chart](../api/StockSharp.Xaml.Charting.Chart.html)**

- [Areas](../api/StockSharp.Xaml.Charting.Chart.Areas.html) \- список областей [ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html).
- [ChartTheme](../api/StockSharp.Xaml.Charting.Chart.ChartTheme.html) \- тема компонента.
- [IndicatorTypes](../api/StockSharp.Xaml.Charting.Chart.IndicatorTypes.html) \- список индикаторов, которые можно отображать на чарте.
- [CrossHair](../api/StockSharp.Xaml.Charting.Chart.CrossHair.html) \- включить\/выключить отображение перекрестья.
- [CrossHairAxisLabels](../api/StockSharp.Xaml.Charting.Chart.CrossHairAxisLabels.html) \- включить\/выключить отображение меток перекрестья на осях.
- [IsAutoRange](../api/StockSharp.Xaml.Charting.Chart.IsAutoRange.html) \- включить\/выключить автоматическое масштабирование оси X.
- [IsAutoScroll](../api/StockSharp.Xaml.Charting.Chart.IsAutoScroll.html) \- включить\/выключить автопрокрутку по оси X.
- [ShowLegend](../api/StockSharp.Xaml.Charting.Chart.ShowLegend.html) \- включить\/выключить отображение легенды.
- [ShowOverview](../api/StockSharp.Xaml.Charting.Chart.ShowOverview.html) \- включить\/выключить отображение области предпросмотра *OverView*.
- [AddArea](../api/StockSharp.Xaml.Charting.IChart.AddArea.html) \- добавить [ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html).
- [AddElement](../api/Overload:StockSharp.Xaml.Charting.IChart.AddElement.html) \- добавить элемент серии данных. Имеет несколько перегрузок.
- [Reset](../api/StockSharp.Xaml.Charting.Chart.Reset.html) \- "сбросить" ранее отрисованные значения.
- [Draw](../api/StockSharp.Xaml.Charting.IChart.Draw.html) \- отрисовать значение на графике.
- [OrderCreationMode](../api/StockSharp.Xaml.Charting.Chart.OrderCreationMode.html) \- Режим создания заявок, когда установлен позволяет создавать заявки из графика. По умолчанию выключено.

### ChartArea

ChartArea

[ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html) \- область построения графика, является контейнером для элементов [IChartElement](../api/StockSharp.Xaml.Charting.IChartElement.html) (индикаторы, свечи и т.п.), которые отрисовываются на графике, и осей ([ChartAxis](../api/StockSharp.Xaml.Charting.ChartAxis.html)) графика. 

**Основные свойства [ChartArea](../api/StockSharp.Xaml.Charting.ChartArea.html)**

- [Elements](../api/StockSharp.Xaml.Charting.ChartArea.Elements.html) \- список элементов [IChartElement](../api/StockSharp.Xaml.Charting.IChartElement.html).
- [XAxises](../api/StockSharp.Xaml.Charting.ChartArea.XAxises.html) \- список горизонтальных осей.
- [YAxises](../api/StockSharp.Xaml.Charting.ChartArea.YAxises.html) \- список вертикальных осей.

### IChartElement

IChartElement

Все элементы, которые отображаются на графике должны, реализовывать интерфейс [IChartElement](../api/StockSharp.Xaml.Charting.IChartElement.html). В [S\#](StockSharpAbout.md) имеются следующие классы, реализующие этот интерфейс: 

- [ChartCandleElement](../api/StockSharp.Xaml.Charting.ChartCandleElement.html) \- элемент для отображения свечей.
- [ChartIndicatorElement](../api/StockSharp.Xaml.Charting.ChartIndicatorElement.html) \- элемент для отображения индикаторов.
- [ChartOrderElement](../api/StockSharp.Xaml.Charting.ChartOrderElement.html) \- элемент для отображения заявок.
- [ChartTradeElement](../api/StockSharp.Xaml.Charting.ChartTradeElement.html) \- элемент для оторбажения сделок.

Классы визуальных элементов имеют ряд свойств для настройки внешнего вида графика. Можно настроить цвета, толщину линий и стиль элементов. Например, при помощи свойства [ChartCandleElement.DrawStyle](../api/StockSharp.Xaml.Charting.ChartCandleElement.DrawStyle.html) можно изменять внешний вид свечи (свеча или бар). При помощи свойства [ChartIndicatorElement.DrawStyle](../api/StockSharp.Xaml.Charting.ChartIndicatorElement.DrawStyle.html) можно установить стиль линии индикатора. Так чтобы отображать индикатор в виде гистограммы нужно использовать значение [ChartIndicatorDrawStyles.Histogram](../api/StockSharp.Xaml.Charting.ChartIndicatorDrawStyles.Histogram.html). Свойства [ChartCandleElement.ShowAxisMarker](../api/StockSharp.Xaml.Charting.ChartCandleElement.ShowAxisMarker.html) и [ChartIndicatorElement.ShowAxisMarker](../api/StockSharp.Xaml.Charting.ChartIndicatorElement.ShowAxisMarker.html) позволяют включать\/выключать отображения маркёров (см. рисунок) на осях графика. 

## См. также

[Свечной график](Gui_Chart.md)

[График\-панель](Gui_ChartPanel.md)

[График эквити](Gui_EquityCurveChart.md)

[Графики box chart](Gui_BoxChart.md)

[Кластеры](Gui_ClasterChart.md)

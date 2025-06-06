# Кластеры

ClusterChart \- специальный тип графика для отображения объемов в виде кластеров с гистограммами. Для использования этого типа графика необходимо задать специальный стиль [IChartCandleElement.DrawStyle](xref:StockSharp.Charting.IChartCandleElement.DrawStyle) \= [ChartCandleDrawStyles.ClusterProfile](xref:StockSharp.Charting.ChartCandleDrawStyles.ClusterProfile). В качестве исходных данных этот график использует информацию из свойства [PriceLevels](xref:StockSharp.Messages.CandleMessage.PriceLevels). 

![Gui ClasterChart](../../../../images/gui_clasterchart.png)

**Основные свойства**

- [ChartCandleElement.ClusterLineColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.ClusterLineColor) \- цвет базовой линии кластера. 
- [ChartCandleElement.ClusterTextColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.ClusterTextColor) \- цвет цифр объемов на графике. 
- [ChartCandleElement.ClusterColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.ClusterColor) \- основной цвет баров в гистограммах кластеров. 
- [ChartCandleElement.ClusterMaxColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.ClusterMaxColor) \- цвет бара максимального объема в гистограммах кластеров.

Пример использования этого типа графика расположен в Samples\/Common\/SampleChart. 

> [!TIP]
> Для переключения между типами графиков используйте кнопку настроек (шестерёнка), расположенную в левом верхнем углу графика.

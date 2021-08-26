# Кластеры

ClusterChart \- специальный тип графика для отображения объемов в виде кластеров с гистограммами. Для использования этого типа графика необходимо задать специальный стиль [ChartCandleElement.DrawStyle](../api/StockSharp.Xaml.Charting.ChartCandleElement.DrawStyle.html) \= [ChartCandleDrawStyles.ClusterProfile](../api/StockSharp.Xaml.Charting.ChartCandleDrawStyles.ClusterProfile.html). В качестве исходных данных этот график использует информацию из свойства [Candle.PriceLevels](../api/StockSharp.Algo.Candles.Candle.PriceLevels.html). 

![Gui ClasterChart](~/images/Gui_ClasterChart.png)

**Основные свойства**

- [ClusterLineColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.ClusterLineColor.html) \- цвет базовой линии кластера. 
- [ClusterTextColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.ClusterTextColor.html) \- цвет цифр объемов на графике. 
- [ClusterColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.ClusterColor.html) \- основной цвет баров в гистограммах кластеров. 
- [ClusterMaxColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.ClusterMaxColor.html) \- цвет бара максимального объема в гисторграммах кластеров. 

Пример использования этого типа графика расположен в Samples\/Common\/SampleChart. 

> [!TIP]
> Для переключения между типами графиков используйте кнопку настроек (шестерёнка), расположенную в левом верхнем углу графика.

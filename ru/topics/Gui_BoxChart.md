# Графики box chart

BoxChart \- специальный тип графика, предназначенный для отображения объемов в виде сетки цифр. Для использования этого типа графика необходимо задать специальный стиль [ChartCandleElement.DrawStyle](../api/StockSharp.Xaml.Charting.ChartCandleElement.DrawStyle.html) \= [ChartCandleDrawStyles.BoxVolume](../api/StockSharp.Xaml.Charting.ChartCandleDrawStyles.BoxVolume.html) В качестве исходных данных этот график использует информацию из свойства [Candle.PriceLevels](../api/StockSharp.Algo.Candles.Candle.PriceLevels.html). 

![Gui BoxChart](~/images/Gui_BoxChart.png)

**Основные свойства**

- [Timeframe2Multiplier](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2Multiplier.html) \- множитель, который применяется к основному таймфрейму, заданному в конструкторе, чтобы получить второй таймфрейм. Отображаемые свечи объединяются в группы размером, соответствующим второму таймфрему. Группы рисуются на графике сеткой и рамкой соответствующих цветов. 
- [Timeframe3Multiplier](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe3Multiplier.html) \- аналогично [Timeframe2Multiplier](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2Multiplier.html), но для третьего таймфрейма. На графике 3\-й таймфрейм рисуется сеткой соответствующего цвета. 
- [FontColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.FontColor.html) \- цвет цифр объемов на графике. 
- [MaxVolumeColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.MaxVolumeColor.html) \- цвет цифр объемов на графике для максимального объема в данной свече. 
- [Timeframe2Color](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2Color.html) \- цвет сетки 2\-го таймфрейма. 
- [Timeframe2FrameColor](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2FrameColor.html) \- цвет рамки 2го таймфрейма. 
- [Timeframe3Color](../api/StockSharp.Xaml.Charting.ChartCandleElement.Timeframe3Color.html) \- цвет сетки 3го таймфрейма. 

Пример использования этого типа графика расположен в Samples\/Common\/SampleChart. 

> [!TIP]
> Для переключения между типами графиков используйте кнопку настроек (шестерёнка), расположенную в левом верхнем углу графика.

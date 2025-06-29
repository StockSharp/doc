# Графики box chart

BoxChart \- специальный тип графика, предназначенный для отображения объемов в виде сетки цифр. Для использования этого типа графика необходимо задать специальный стиль [ChartCandleElement.DrawStyle](xref:StockSharp.Xaml.Charting.ChartCandleElement.DrawStyle) \= [ChartCandleDrawStyles.BoxVolume](xref:StockSharp.Charting.ChartCandleDrawStyles.BoxVolume) В качестве исходных данных этот график использует информацию из свойства [PriceLevels](xref:StockSharp.Messages.CandleMessage.PriceLevels). 

![Gui BoxChart](../../../../images/gui_boxchart.png)

**Основные свойства**

- [ChartCandleElement.Timeframe2Multiplier](xref:StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2Multiplier) \- множитель, который применяется к основному таймфрейму, заданному в конструкторе, чтобы получить второй таймфрейм. Отображаемые свечи объединяются в группы размером, соответствующим второму таймфрейму. Группы рисуются на графике сеткой и рамкой соответствующих цветов. 
- [ChartCandleElement.Timeframe3Multiplier](xref:StockSharp.Xaml.Charting.ChartCandleElement.Timeframe3Multiplier) \- аналогично [ChartCandleElement.Timeframe2Multiplier](xref:StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2Multiplier), но для третьего таймфрейма. На графике 3\-й таймфрейм рисуется сеткой соответствующего цвета. 
- [ChartCandleElement.FontColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.FontColor) \- цвет цифр объемов на графике. 
- [ChartCandleElement.MaxVolumeColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.MaxVolumeColor) \- цвет цифр объемов на графике для максимального объема в данной свече. 
- [ChartCandleElement.Timeframe2Color](xref:StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2Color) \- цвет сетки 2\-го таймфрейма. 
- [ChartCandleElement.Timeframe2FrameColor](xref:StockSharp.Xaml.Charting.ChartCandleElement.Timeframe2FrameColor) \- цвет рамки 2-го таймфрейма. 
- [ChartCandleElement.Timeframe3Color](xref:StockSharp.Xaml.Charting.ChartCandleElement.Timeframe3Color) \- цвет сетки 3-го таймфрейма. 

Пример использования этого типа графика расположен в Samples\/Common\/SampleChart. 

> [!TIP]
> Для переключения между типами графиков используйте кнопку настроек (шестерёнка), расположенную в левом верхнем углу графика.

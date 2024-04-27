# График

Для графического отображения свечей можно использовать специальный компонент [Chart](xref:StockSharp.Xaml.Charting.Chart) (см. [Компоненты для построения графиков](../graphical_user_interface/charts.md)), который отрисовывает свечи следующим образом:

![sample candleschart](../../../images/sample_candleschart.png)

Для отображения, поступающие свечи передаются в метод [Chart.Draw](xref:StockSharp.Xaml.Charting.Chart.Draw(StockSharp.Charting.IChartDrawData))**(**[StockSharp.Charting.IChartDrawData](xref:StockSharp.Charting.IChartDrawData) data **)**.

```cs
// CandlesChart - StockSharp.Xaml.Chart
private ChartArea _areaComb;
private ChartCandleElement _candleElement;
...
var series = new CandleSeries(typeof(TimeFrameCandle),_security,TimeSpan.FromMinutes(_timeframe));
_candleElement = new ChartCandleElement() { FullTitle = "Candles" };
Chart.AddElement(_areaComb, _candleElement, series);
...
_connector.SubscribeCandles(series, DateTime.Today.Subtract(TimeSpan.FromDays(5)), DateTime.Now);		
_connector.CandleSeriesProcessing += Connector_CandleSeriesProcessing;
...
private void Connector_CandleSeriesProcessing(CandleSeries candleSeries, Candle candle)
{
    if (candle.State == CandleStates.Finished) 
    {
       var chartData = new ChartDrawData();
       chartData.Group(candle.OpenTime).Add(_candleElement, candle);
       Chart.Draw(chartData);
    }
}
		
```

Пример отображения свечей на графике приведен в пункте [Свечи](../candles.md).

## См. также

[Компоненты для построения графиков](../graphical_user_interface/charts.md)

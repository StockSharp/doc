# Добавление индикатора на график

Добавление индикатора для отрисовки на графике демонстрирует следующий пример:

```cs
private readonly Connector \_connector \= new Connector();
private Security \_security;
private CandleSeries \_candleSeries;
private SimpleMovingAverage \_sma;
readonly TimeSpan \_timeFrame \= TimeSpan.FromMinutes(1);
private ChartArea \_area;
private ChartCandleElement \_candlesElem;
private ChartIndicatorElement \_longMaElem;
...
\/\/ \_chart \- StockSharp.Xaml.Charting.Chart
\/\/ Создание области графика
\_area \= new ChartArea();
\_chart.Areas.Add(\_area);
\/\/ Создание элемента графика представляющего свечи
\_candlesElem \= new ChartCandleElement();
\_area.Elements.Add(\_candlesElem);
\/\/ Создание элемента графика представляющего индикатор
\_longMaElem \= new ChartIndicatorElement
{
	Title \= "Длинная"
};
\_area.Elements.Add(\_longMaElem);
...
\_sma \= new SimpleMovingAverage() { Length \= 80 };
\_connector.CandleSeriesProcessing +\= DrawCandle;
...
\_candleSeries \= new CandleSeries(typeof(TimeFrameCandle), \_security, \_timeFrame);
\_connector.SubscribeCandles(\_candleSeries, DateTime.Today.Subtract(TimeSpan.FromDays(30)), DateTime.Now);
...
private void Draw(CandleSeries series, Candle candle)
{
   if (candle.State \!\= CandleStates.Finished)
       return;
	var longValue \= \_sma.Process(candle);
	var data \= new ChartDrawData();
	data
		.Group(candle.OpenTime)
			.Add(\_candlesElem, candle)
			.Add(\_longElem, longValue);
	\_chart.Draw(data);
}
		
```

![indicators chart](~/images/indicators_chart.png)

## См. также

[Компоненты для построения графиков](GUICharting.md)

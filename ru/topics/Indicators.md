# Индикаторы

[S\#](StockSharpAbout.md) стандартно предоставляет более 70 индикаторов технического анализа. Это позволяет не создавать с нуля нужные индикаторы, а использовать уже готовые. Кроме того можно создавать собственные индикаторы, взяв за основу существующие, как показано в разделе [Собственный индикатор](IndicatorsCustom.md). Все базовые классы для работы с индикаторами, а также сами индикаторы находятся в пространстве имен [StockSharp.Algo.Indicators](../api/StockSharp.Algo.Indicators.html). 

### Подключение индикатора в робот

Подключение индикатора в робот

1. В самом начале нужно создать индикатор. Индикатор создается как и обычный .NET объект:

   ```cs
   var longSma = new SimpleMovingAverage { Length = 80 };
   ```
2. Далее, необходимо заполнять его данными. Например, это может быть цена закрытия свечи:

   ```cs
   foreach (var candle in candles)
   	longSma.Process(candle);
   ```

   Индикатор принимает на вход [IIndicatorValue](../api/StockSharp.Algo.Indicators.IIndicatorValue.html). Некоторые из индикаторов оперируют простым числом, как, например, [SimpleMovingAverage](../api/StockSharp.Algo.Indicators.SimpleMovingAverage.html). Другим требуются полностью свеча, как, например, [MedianPrice](../api/StockSharp.Algo.Indicators.MedianPrice.html). Поэтому входящие значения необходимо приводить или к [DecimalIndicatorValue](../api/StockSharp.Algo.Indicators.DecimalIndicatorValue.html) или к [CandleIndicatorValue](../api/StockSharp.Algo.Indicators.CandleIndicatorValue.html). Результирующее значение индикатора работает по тем же правилам, что и входящее значение. 
3. Результирующее и входящее значение индикатора имеют свойство [IIndicatorValue.IsFinal](../api/StockSharp.Algo.Indicators.IIndicatorValue.IsFinal.html), которое говорит о том, что значение является окончательным и индикатор не будет изменяться в данной точке времени. Например, индикатор [SimpleMovingAverage](../api/StockSharp.Algo.Indicators.SimpleMovingAverage.html) формируется по цене закрытия свечи, но в текущий момент времени окончательная цена закрытия свечи неизвестна и меняется. В таком случае результирующее значение [IIndicatorValue.IsFinal](../api/StockSharp.Algo.Indicators.IIndicatorValue.IsFinal.html) будет false. Eсли в индикатор передать законченную свечу, то входящее и результирующее значения [IIndicatorValue.IsFinal](../api/StockSharp.Algo.Indicators.IIndicatorValue.IsFinal.html) будут true.
4. Для того, чтобы получить текущее значение индикатора, используется метод [GetValue\`\`1](../api/StockSharp.Algo.Indicators.IIndicatorValue.GetValue``1.html):

   ```cs
   // вычисляем новое положение относительно друг друга
   var isShortLessThenLong = ShortSma.GetCurrentValue() < LongSma.GetCurrentValue();
   // если произошло пересечение
   if (_isShortLessThenLong != isShortLessThenLong)
   {
   	// если короткая меньше чем длинная, то продажа, иначе, покупка.
   	var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
   	// регистрируем заявку
   	var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
   	var price = candle.ClosePrice + ((direction == Sides.Buy ? Security.PriceStep : -Security.PriceStep) ?? 1);
       RegisterOrder(this.CreateOrder(direction, price, volume));
   	// запоминаем текущее положение относительно друг друга
   	_isShortLessThenLong = isShortLessThenLong;
   }
   ```
5. У всех индикаторов есть свойство [BaseIndicator.IsFormed](../api/StockSharp.Algo.Indicators.BaseIndicator.IsFormed.html), которое говорит о том готов ли индикатор к использованию. Например, индикатор [SimpleMovingAverage](../api/StockSharp.Algo.Indicators.SimpleMovingAverage.html) имеет период, и пока индикатор не обработает количество свечей, равное периоду индикатора, индикатор будет считаться не готовым к использованию. И свойство [BaseIndicator.IsFormed](../api/StockSharp.Algo.Indicators.BaseIndicator.IsFormed.html) будет false.

## См. также

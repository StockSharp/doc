# Индикаторы

[S\#](StockSharpAbout.md) стандартно предоставляет более 70 индикаторов технического анализа. Это позволяет не создавать с нуля нужные индикаторы, а использовать уже готовые. Кроме того можно создавать собственные индикаторы, взяв за основу существующие, как показано в разделе [Собственный индикатор](IndicatorsCustom.md). Все базовые классы для работы с индикаторами, а также сами индикаторы находятся в пространстве имен [StockSharp.Algo.Indicators](xref:StockSharp.Algo.Indicators). 

## Подключение индикатора в робот

1. В самом начале нужно создать индикатор. Индикатор создается как и обычный .NET объект:

   ```cs
   var longSma = new SimpleMovingAverage { Length = 80 };
   ```
2. Далее, необходимо заполнять его данными. Например, это может быть цена закрытия свечи:

   ```cs
   foreach (var candle in candles)
   	longSma.Process(candle);
   ```

   Индикатор принимает на вход [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue). Некоторые из индикаторов оперируют простым числом, как, например, [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage). Другим требуются полностью свеча, как, например, [MedianPrice](xref:StockSharp.Algo.Indicators.MedianPrice). Поэтому входящие значения необходимо приводить или к [DecimalIndicatorValue](xref:StockSharp.Algo.Indicators.DecimalIndicatorValue) или к [CandleIndicatorValue](xref:StockSharp.Algo.Indicators.CandleIndicatorValue). Результирующее значение индикатора работает по тем же правилам, что и входящее значение. 
3. Результирующее и входящее значение индикатора имеют свойство [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal), которое говорит о том, что значение является окончательным и индикатор не будет изменяться в данной точке времени. Например, индикатор [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) формируется по цене закрытия свечи, но в текущий момент времени окончательная цена закрытия свечи неизвестна и меняется. В таком случае результирующее значение [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) будет false. Eсли в индикатор передать законченную свечу, то входящее и результирующее значения [IIndicatorValue.IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) будут true.
4. Для того, чтобы получить текущее значение индикатора, используется метод [GetValue\`\`1](xref:StockSharp.Algo.Indicators.IIndicatorValue.GetValue``1):

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
5. У всех индикаторов есть свойство [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed), которое говорит о том готов ли индикатор к использованию. Например, индикатор [SimpleMovingAverage](xref:StockSharp.Algo.Indicators.SimpleMovingAverage) имеет период, и пока индикатор не обработает количество свечей, равное периоду индикатора, индикатор будет считаться не готовым к использованию. И свойство [BaseIndicator.IsFormed](xref:StockSharp.Algo.Indicators.BaseIndicator.IsFormed) будет false.

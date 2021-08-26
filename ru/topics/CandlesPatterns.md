# Паттерны

Через класс [CandleHelper](../api/StockSharp.Algo.Candles.CandleHelper.html) можно произвести распознавание паттернов свечей по следующим методам:

- [IsWhiteOrBlack](../api/StockSharp.Algo.Candles.CandleHelper.IsWhiteOrBlack.html)

   \- Является ли свеча белой или черной. 
- [IsMarubozu](../api/StockSharp.Algo.Candles.CandleHelper.IsMarubozu.html)

   \- Является ли свеча бестеневой. 
- [IsSpinningTop](../api/StockSharp.Algo.Candles.CandleHelper.IsSpinningTop.html)

   \- Является ли свеча нейтральной по сделкам. 
- [IsHammer](../api/StockSharp.Algo.Candles.CandleHelper.IsHammer.html)

   \- Является ли свеча молотом. 
- [IsDragonflyOrGravestone](../api/StockSharp.Algo.Candles.CandleHelper.IsDragonflyOrGravestone.html)

   \- Является ли свеча стрекозой или надгробьем. 
- [IsBullishOrBearish](../api/StockSharp.Algo.Candles.CandleHelper.IsBullishOrBearish.html)

   \- Является ли свеча бычьей или медвежьей. 

### Общие методы работы со свечами

Общие методы работы со свечами

[CandleHelper](../api/StockSharp.Algo.Candles.CandleHelper.html) также предоставляет различные алгоритмы для свечей, упрощающие написание кода:

- Получение временных рамок свечи через метод [GetCandleBounds](../api/Overload:StockSharp.Algo.Candles.CandleHelper.GetCandleBounds.html). Например, необходимо точно узнать, когда закончится текущая 10.5\-минутная свеча:

  ```cs
  var candleTimeFrame \= TimeSpan.FromMinutes(10.5);
  Console.WriteLine(candleTimeFrame.GetCandleBounds(DateTime.Now).Max);
  					
  ```
- Получить длину свечи, ее тела, ее теней через методы [GetLength](../api/StockSharp.Algo.Candles.CandleHelper.GetLength.html), [GetBody](../api/StockSharp.Algo.Candles.CandleHelper.GetBody.html), [GetTopShadow](../api/StockSharp.Algo.Candles.CandleHelper.GetTopShadow.html) и [GetBottomShadow](../api/StockSharp.Algo.Candles.CandleHelper.GetBottomShadow.html):

  ```cs
  \/\/ любая свеча
  var candle \= ...
  Console.WriteLine(candle.GetCandleLength());
  Console.WriteLine(candle.GetCandleBody());
  Console.WriteLine(candle.GetCandleTopShadow());
  Console.WriteLine(candle.GetCandleBottomShadow());
  					
  ```

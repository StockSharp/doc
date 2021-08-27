# Паттерны

Через класс [CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) можно произвести распознавание паттернов свечей по следующим методам:

- [IsWhiteOrBlack](xref:StockSharp.Algo.Candles.CandleHelper.IsWhiteOrBlack) \- Является ли свеча белой или черной. 
- [IsMarubozu](xref:StockSharp.Algo.Candles.CandleHelper.IsMarubozu) \- Является ли свеча бестеневой. 
- [IsSpinningTop](xref:StockSharp.Algo.Candles.CandleHelper.IsSpinningTop) \- Является ли свеча нейтральной по сделкам. 
- [IsHammer](xref:StockSharp.Algo.Candles.CandleHelper.IsHammer) \- Является ли свеча молотом. 
- [IsDragonflyOrGravestone](xref:StockSharp.Algo.Candles.CandleHelper.IsDragonflyOrGravestone) \- Является ли свеча стрекозой или надгробьем. 
- [IsBullishOrBearish](xref:StockSharp.Algo.Candles.CandleHelper.IsBullishOrBearish) \- Является ли свеча бычьей или медвежьей. 

### Общие методы работы со свечами

Общие методы работы со свечами

[CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) также предоставляет различные алгоритмы для свечей, упрощающие написание кода:

- Получение временных рамок свечи через метод [GetCandleBounds](xref:Overload:StockSharp.Algo.Candles.CandleHelper.GetCandleBounds). Например, необходимо точно узнать, когда закончится текущая 10.5\-минутная свеча:

  ```cs
  var candleTimeFrame = TimeSpan.FromMinutes(10.5);
  Console.WriteLine(candleTimeFrame.GetCandleBounds(DateTime.Now).Max);
  					
  ```
- Получить длину свечи, ее тела, ее теней через методы [GetLength](xref:StockSharp.Algo.Candles.CandleHelper.GetLength), [GetBody](xref:StockSharp.Algo.Candles.CandleHelper.GetBody), [GetTopShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetTopShadow) и [GetBottomShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetBottomShadow):

  ```cs
  // любая свеча
  var candle = ...
  Console.WriteLine(candle.GetCandleLength());
  Console.WriteLine(candle.GetCandleBody());
  Console.WriteLine(candle.GetCandleTopShadow());
  Console.WriteLine(candle.GetCandleBottomShadow());
  					
  ```

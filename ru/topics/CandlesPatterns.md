# Паттерны

Через класс [CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) можно произвести распознавание паттернов свечей по следующим методам:

- [IsWhiteOrBlack](xref:StockSharp.Algo.Candles.CandleHelper.IsWhiteOrBlack(StockSharp.Algo.Candles.Candle)) \- Является ли свеча белой или черной. 
- [IsMarubozu](xref:StockSharp.Algo.Candles.CandleHelper.IsMarubozu(StockSharp.Algo.Candles.Candle)) \- Является ли свеча бестеневой. 
- [IsSpinningTop](xref:StockSharp.Algo.Candles.CandleHelper.IsSpinningTop(StockSharp.Algo.Candles.Candle)) \- Является ли свеча нейтральной по сделкам. 
- [IsHammer](xref:StockSharp.Algo.Candles.CandleHelper.IsHammer(StockSharp.Algo.Candles.Candle)) \- Является ли свеча молотом. 
- [IsDragonflyOrGravestone](xref:StockSharp.Algo.Candles.CandleHelper.IsDragonflyOrGravestone(StockSharp.Algo.Candles.Candle)) \- Является ли свеча стрекозой или надгробьем. 
- [IsBullishOrBearish](xref:StockSharp.Algo.Candles.CandleHelper.IsBullishOrBearish(StockSharp.Algo.Candles.Candle)) \- Является ли свеча бычьей или медвежьей. 

## Общие методы работы со свечами

[CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) также предоставляет различные алгоритмы для свечей, упрощающие написание кода:

- Получение временных рамок свечи через метод [GetCandleBounds](xref:Overload:StockSharp.Algo.Candles.CandleHelper.GetCandleBounds). Например, необходимо точно узнать, когда закончится текущая 10.5\-минутная свеча:

  ```cs
  var candleTimeFrame = TimeSpan.FromMinutes(10.5);
  Console.WriteLine(candleTimeFrame.GetCandleBounds(DateTime.Now).Max);
  					
  ```
- Получить длину свечи, ее тела, ее теней через методы [GetLength](xref:StockSharp.Algo.Candles.CandleHelper.GetLength(StockSharp.Algo.Candles.Candle)), [GetBody](xref:StockSharp.Algo.Candles.CandleHelper.GetBody(StockSharp.Algo.Candles.Candle)), [GetTopShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetTopShadow(StockSharp.Algo.Candles.Candle)) и [GetBottomShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetBottomShadow(StockSharp.Algo.Candles.Candle)):

  ```cs
  // любая свеча
  var candle = ...
  Console.WriteLine(candle.GetCandleLength());
  Console.WriteLine(candle.GetCandleBody());
  Console.WriteLine(candle.GetCandleTopShadow());
  Console.WriteLine(candle.GetCandleBottomShadow());
  					
  ```

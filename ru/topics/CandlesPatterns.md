# Паттерны

Через класс [CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) можно произвести распознавание паттернов свечей по следующим методам:

- [CandleHelper.IsWhiteOrBlack](xref:StockSharp.Algo.Candles.CandleHelper.IsWhiteOrBlack(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)** \- Является ли свеча белой или черной. 
- [CandleHelper.IsMarubozu](xref:StockSharp.Algo.Candles.CandleHelper.IsMarubozu(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)** \- Является ли свеча бестеневой. 
- [CandleHelper.IsSpinningTop](xref:StockSharp.Algo.Candles.CandleHelper.IsSpinningTop(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)** \- Является ли свеча нейтральной по сделкам. 
- [CandleHelper.IsHammer](xref:StockSharp.Algo.Candles.CandleHelper.IsHammer(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)** \- Является ли свеча молотом. 
- [CandleHelper.IsDragonflyOrGravestone](xref:StockSharp.Algo.Candles.CandleHelper.IsDragonflyOrGravestone(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)** \- Является ли свеча стрекозой или надгробьем. 
- [CandleHelper.IsBullishOrBearish](xref:StockSharp.Algo.Candles.CandleHelper.IsBullishOrBearish(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)** \- Является ли свеча бычьей или медвежьей. 

## Общие методы работы со свечами

[CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) также предоставляет различные алгоритмы для свечей, упрощающие написание кода:

- Получение временных рамок свечи через метод [CandleHelper.GetCandleBounds](xref:StockSharp.Algo.Candles.CandleHelper.GetCandleBounds(System.TimeSpan,System.DateTimeOffset))**(**[System.TimeSpan](xref:System.TimeSpan) timeFrame, [System.DateTimeOffset](xref:System.DateTimeOffset) currentTime**)**. Например, необходимо точно узнать, когда закончится текущая 10.5\-минутная свеча:

  ```cs
  var candleTimeFrame = TimeSpan.FromMinutes(10.5);
  Console.WriteLine(candleTimeFrame.GetCandleBounds(DateTime.Now).Max);
  					
  ```
- Получить длину свечи, ее тела, ее теней через методы [CandleHelper.GetLength](xref:StockSharp.Algo.Candles.CandleHelper.GetLength(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)**, [CandleHelper.GetBody](xref:StockSharp.Algo.Candles.CandleHelper.GetBody(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)**, [CandleHelper.GetTopShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetTopShadow(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)** и [CandleHelper.GetBottomShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetBottomShadow(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle**)**:

  ```cs
  // любая свеча
  var candle = ...
  Console.WriteLine(candle.GetCandleLength());
  Console.WriteLine(candle.GetCandleBody());
  Console.WriteLine(candle.GetCandleTopShadow());
  Console.WriteLine(candle.GetCandleBottomShadow());
  					
  ```

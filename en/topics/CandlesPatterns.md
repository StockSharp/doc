# Patterns

It is possible to make candles pattern recognition through the [CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) class by the following methods:

- [IsWhiteOrBlack](xref:StockSharp.Algo.Candles.CandleHelper.IsWhiteOrBlack(StockSharp.Algo.Candles.Candle)) \- Is it white or black candle. 
- [IsMarubozu](xref:StockSharp.Algo.Candles.CandleHelper.IsMarubozu(StockSharp.Algo.Candles.Candle)) \- Is it a shadowless candle. 
- [IsSpinningTop](xref:StockSharp.Algo.Candles.CandleHelper.IsSpinningTop(StockSharp.Algo.Candles.Candle)) \- Is it a neutral candle by trades. 
- [IsHammer](xref:StockSharp.Algo.Candles.CandleHelper.IsHammer(StockSharp.Algo.Candles.Candle)) \- Is it a hammer candle. 
- [IsDragonflyOrGravestone](xref:StockSharp.Algo.Candles.CandleHelper.IsDragonflyOrGravestone(StockSharp.Algo.Candles.Candle)) \- Is it a dragonfly or gravestone candle. 
- [IsBullishOrBearish](xref:StockSharp.Algo.Candles.CandleHelper.IsBullishOrBearish(StockSharp.Algo.Candles.Candle)) \- Is it a bull or bear candle. 

## Common methods of work with candles

[CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) also provides a variety of algorithms for candles to simplify writing of code:

- The candle time range getting through the [GetCandleBounds](xref:StockSharp.Algo.Candles.CandleHelper.GetCandleBounds(System.TimeSpan,System.DateTimeOffset)) method. For example, you want to find out exactly when the current 10.5\-minute candle ends:

  ```cs
  var candleTimeFrame = TimeSpan.FromMinutes(10.5);
  Console.WriteLine(candleTimeFrame.GetCandleBounds(DateTime.Now).Max);
  					
  ```
- To get the candle length, its body, its shadows through [GetLength](xref:StockSharp.Algo.Candles.CandleHelper.GetLength(StockSharp.Algo.Candles.Candle)), [GetBody](xref:StockSharp.Algo.Candles.CandleHelper.GetBody(StockSharp.Algo.Candles.Candle)), [GetTopShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetTopShadow(StockSharp.Algo.Candles.Candle)) and [GetBottomShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetBottomShadow(StockSharp.Algo.Candles.Candle)):

  ```cs
  // sample candle
  var candle = ...
  Console.WriteLine(candle.GetCandleLength());
  Console.WriteLine(candle.GetCandleBody());
  Console.WriteLine(candle.GetCandleTopShadow());
  Console.WriteLine(candle.GetCandleBottomShadow());
  					
  ```

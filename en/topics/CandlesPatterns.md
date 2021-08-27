# Patterns

It is possible to make candles pattern recognition through the [CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) class by the following methods:

- [IsWhiteOrBlack](xref:StockSharp.Algo.Candles.CandleHelper.IsWhiteOrBlack)

   \- Is it white or black candle. 
- [IsMarubozu](xref:StockSharp.Algo.Candles.CandleHelper.IsMarubozu)

   \- Is it a shadowless candle. 
- [IsSpinningTop](xref:StockSharp.Algo.Candles.CandleHelper.IsSpinningTop)

   \- Is it a neutral candle by trades. 
- [IsHammer](xref:StockSharp.Algo.Candles.CandleHelper.IsHammer)

   \- Is it a hammer candle. 
- [IsDragonflyOrGravestone](xref:StockSharp.Algo.Candles.CandleHelper.IsDragonflyOrGravestone)

   \- Is it a dragonfly or gravestone candle. 
- [IsBullishOrBearish](xref:StockSharp.Algo.Candles.CandleHelper.IsBullishOrBearish)

   \- Is it a bull or bear candle. 

### Common methods of work with candles

Common methods of work with candles

[CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) also provides a variety of algorithms for candles to simplify writing of code:

- The candle time range getting through the [GetCandleBounds](xref:Overload:StockSharp.Algo.Candles.CandleHelper.GetCandleBounds) method. For example, you want to find out exactly when the current 10.5\-minute candle ends:

  ```cs
  var candleTimeFrame = TimeSpan.FromMinutes(10.5);
  Console.WriteLine(candleTimeFrame.GetCandleBounds(DateTime.Now).Max);
  					
  ```
- To get the candle length, its body, its shadows through [GetLength](xref:StockSharp.Algo.Candles.CandleHelper.GetLength), [GetBody](xref:StockSharp.Algo.Candles.CandleHelper.GetBody), [GetTopShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetTopShadow) and [GetBottomShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetBottomShadow):

  ```cs
  // sample candle
  var candle = ...
  Console.WriteLine(candle.GetCandleLength());
  Console.WriteLine(candle.GetCandleBody());
  Console.WriteLine(candle.GetCandleTopShadow());
  Console.WriteLine(candle.GetCandleBottomShadow());
  					
  ```

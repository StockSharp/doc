# Patterns

It is possible to make candles pattern recognition through the [CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) class by the following methods:

- [CandleHelper.IsWhiteOrBlack](xref:StockSharp.Algo.Candles.CandleHelper.IsWhiteOrBlack(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)** \- Is it white or black candle. 
- [CandleHelper.IsMarubozu](xref:StockSharp.Algo.Candles.CandleHelper.IsMarubozu(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)** \- Is it a shadowless candle. 
- [CandleHelper.IsSpinningTop](xref:StockSharp.Algo.Candles.CandleHelper.IsSpinningTop(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)** \- Is it a neutral candle by trades. 
- [CandleHelper.IsHammer](xref:StockSharp.Algo.Candles.CandleHelper.IsHammer(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)** \- Is it a hammer candle. 
- [CandleHelper.IsDragonflyOrGravestone](xref:StockSharp.Algo.Candles.CandleHelper.IsDragonflyOrGravestone(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)** \- Is it a dragonfly or gravestone candle. 
- [CandleHelper.IsBullishOrBearish](xref:StockSharp.Algo.Candles.CandleHelper.IsBullishOrBearish(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)** \- Is it a bull or bear candle. 

## Common methods of work with candles

[CandleHelper](xref:StockSharp.Algo.Candles.CandleHelper) also provides a variety of algorithms for candles to simplify writing of code:

- The candle time range getting through the [CandleHelper.GetCandleBounds](xref:StockSharp.Algo.Candles.CandleHelper.GetCandleBounds(System.TimeSpan,System.DateTimeOffset))**(**[System.TimeSpan](xref:System.TimeSpan) timeFrame, [System.DateTimeOffset](xref:System.DateTimeOffset) currentTime **)** method. For example, you want to find out exactly when the current 10.5\-minute candle ends:

  ```cs
  var candleTimeFrame = TimeSpan.FromMinutes(10.5);
  Console.WriteLine(candleTimeFrame.GetCandleBounds(DateTime.Now).Max);
  					
  ```

- To get the candle length, its body, its shadows through [CandleHelper.GetLength](xref:StockSharp.Algo.Candles.CandleHelper.GetLength(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)**, [CandleHelper.GetBody](xref:StockSharp.Algo.Candles.CandleHelper.GetBody(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)**, [CandleHelper.GetTopShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetTopShadow(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)** and [CandleHelper.GetBottomShadow](xref:StockSharp.Algo.Candles.CandleHelper.GetBottomShadow(StockSharp.Algo.Candles.Candle))**(**[StockSharp.Algo.Candles.Candle](xref:StockSharp.Algo.Candles.Candle) candle **)**:

  ```cs
  // sample candle
  var candle = ...
  Console.WriteLine(candle.GetCandleLength());
  Console.WriteLine(candle.GetCandleBody());
  Console.WriteLine(candle.GetCandleTopShadow());
  Console.WriteLine(candle.GetCandleBottomShadow());
  					
  ```

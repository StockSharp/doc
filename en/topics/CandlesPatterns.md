# Patterns

It is possible to make candles pattern recognition through the [CandleHelper](../api/StockSharp.Algo.Candles.CandleHelper.html) class by the following methods:

- [IsWhiteOrBlack](../api/StockSharp.Algo.Candles.CandleHelper.IsWhiteOrBlack.html)

   \- Is it white or black candle. 
- [IsMarubozu](../api/StockSharp.Algo.Candles.CandleHelper.IsMarubozu.html)

   \- Is it a shadowless candle. 
- [IsSpinningTop](../api/StockSharp.Algo.Candles.CandleHelper.IsSpinningTop.html)

   \- Is it a neutral candle by trades. 
- [IsHammer](../api/StockSharp.Algo.Candles.CandleHelper.IsHammer.html)

   \- Is it a hammer candle. 
- [IsDragonflyOrGravestone](../api/StockSharp.Algo.Candles.CandleHelper.IsDragonflyOrGravestone.html)

   \- Is it a dragonfly or gravestone candle. 
- [IsBullishOrBearish](../api/StockSharp.Algo.Candles.CandleHelper.IsBullishOrBearish.html)

   \- Is it a bull or bear candle. 

### Common methods of work with candles

Common methods of work with candles

[CandleHelper](../api/StockSharp.Algo.Candles.CandleHelper.html) also provides a variety of algorithms for candles to simplify writing of code:

- The candle time range getting through the [GetCandleBounds](../api/Overload:StockSharp.Algo.Candles.CandleHelper.GetCandleBounds.html) method. For example, you want to find out exactly when the current 10.5\-minute candle ends:

  ```cs
  var candleTimeFrame = TimeSpan.FromMinutes(10.5);
  Console.WriteLine(candleTimeFrame.GetCandleBounds(DateTime.Now).Max);
  					
  ```
- To get the candle length, its body, its shadows through [GetLength](../api/StockSharp.Algo.Candles.CandleHelper.GetLength.html), [GetBody](../api/StockSharp.Algo.Candles.CandleHelper.GetBody.html), [GetTopShadow](../api/StockSharp.Algo.Candles.CandleHelper.GetTopShadow.html) and [GetBottomShadow](../api/StockSharp.Algo.Candles.CandleHelper.GetBottomShadow.html):

  ```cs
  // sample candle
  var candle = ...
  Console.WriteLine(candle.GetCandleLength());
  Console.WriteLine(candle.GetCandleBody());
  Console.WriteLine(candle.GetCandleTopShadow());
  Console.WriteLine(candle.GetCandleBottomShadow());
  					
  ```

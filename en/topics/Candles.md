# Candles

[S\#](StockSharpAbout.md) supports the following types:

- [TimeFrameCandle](../api/StockSharp.Algo.Candles.TimeFrameCandle.html)

   \- candle based on timeframe. You can set both popular intervals (minute, hour, day), and customized. For example, 21 seconds, 4.5 minutes, etc. 
- [RangeCandle](../api/StockSharp.Algo.Candles.RangeCandle.html)

   \- price range candle. A new candle is created when a trade appears with a price that is out of range. An allowable range is formed each time based on the price of the first trade. 
- [VolumeCandle](../api/StockSharp.Algo.Candles.VolumeCandle.html)

   \- a candle is created until the total volume of trades is exceeded. If the new trade exceeds the permissible volume, then it falls into the new candle already. 
- [TickCandle](../api/StockSharp.Algo.Candles.TickCandle.html)

   \- is the same as 

  [VolumeCandle](../api/StockSharp.Algo.Candles.VolumeCandle.html)

  , only the number of trades is taken as a restriction instead of volume. 
- [PnFCandle](../api/StockSharp.Algo.Candles.PnFCandle.html)

   \- a candle of the point\-and\-figure chart (tic\-tac\-toe chart). 
- [RenkoCandle](../api/StockSharp.Algo.Candles.RenkoCandle.html)

   \- Renko candle. 

How to work with candles is shown in the SampleConnection example, which is located in the *Samples\/Common\/SampleConnection*.

The following figures show [TimeFrameCandle](../api/StockSharp.Algo.Candles.TimeFrameCandle.html) and [RangeCandle](../api/StockSharp.Algo.Candles.RangeCandle.html) charts:

![sample timeframecandles](../images/sample_timeframecandles.png)

![sample rangecandles](../images/sample_rangecandles.png)

### Start getting data

Start getting data

1. 1. Create a series of [CandleSeries](../api/StockSharp.Algo.Candles.CandleSeries.html) candles: 

   ```cs
   ...
   _candleSeries = new CandleSeries(CandleSettingsEditor.Settings.CandleType, security, CandleSettingsEditor.Settings.Arg);
   ...		
   					
   ```
2. All the necessary methods for getting candles are implemented in the [Connector](../api/StockSharp.Algo.Connector.html) class.

   To get candles, you need to subscribe to the [Connector.CandleSeriesProcessing](../api/StockSharp.Algo.Connector.CandleSeriesProcessing.html), event, which signals the appearance of a new value for processing:

   ```cs
   _connector.CandleSeriesProcessing += Connector_CandleSeriesProcessing;
   ...
   private void Connector_CandleSeriesProcessing(CandleSeries candleSeries, Candle candle)
   {
   	Chart.Draw(_candleElement, candle);
   }
   ...
   					
   ```

   > [!TIP]
   > To display the candles, the [Chart](../api/StockSharp.Xaml.Charting.Chart.html) graphic component is used. 
3. Next, pass the created candle series to the connector and start getting data through [SubscribeCandles](../api/StockSharp.Algo.Connector.SubscribeCandles.html):

   ```cs
   ...
   _connector.SubscribeCandles(_candleSeries, DateTime.Today.Subtract(TimeSpan.FromDays(30)), DateTime.Now);	
   ...
   		
   					
   ```

   After this stage, the [Connector.CandleSeriesProcessing](../api/StockSharp.Algo.Connector.CandleSeriesProcessing.html) event will be raised.
4. The [Connector.CandleSeriesProcessing](../api/StockSharp.Algo.Connector.CandleSeriesProcessing.html) event is raised not only when a new candle appears, but also when the current one changes.

   If you want to display only **"whole"** candles, then you need to check the [State](../api/StockSharp.Algo.Candles.Candle.State.html) property of the arrived candle:

   ```cs
   ...
   private void Connector_CandleSeriesProcessing(CandleSeries candleSeries, Candle candle)
   {
       if (candle.State == CandleStates.Finished) 
       {
          var chartData = new ChartDrawData();
          chartData.Group(candle.OpenTime).Add(_candleElement, candle);
          Chart.Draw(chartData);
       }
   }
   ...
   		
   ```
5. You can set some properties for [CandleSeries](../api/StockSharp.Algo.Candles.CandleSeries.html):
   - [BuildCandlesMode](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesMode.html) sets the mode of building candles. By default, [LoadAndBuild](../api/StockSharp.Messages.MarketDataBuildModes.LoadAndBuild.html) is specified, which means that finished data will be requested, or built from the data type specified in the [BuildCandlesFrom](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesFrom.html) property. You can also set [Load](../api/StockSharp.Messages.MarketDataBuildModes.Load.html) to request only finished data. Or [Build](../api/StockSharp.Messages.MarketDataBuildModes.Build.html), for building from the data type specified in the [BuildCandlesFrom](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesFrom.html) property without requesting the finished data. 
   - When building candles, you need to set the 

     [BuildCandlesFrom](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesFrom.html)

     , property, which indicates which data type is used as a source (

     [Level1](../api/StockSharp.Messages.MarketDataTypes.Level1.html)

     , 

     [MarketDepth](../api/StockSharp.Messages.MarketDataTypes.MarketDepth.html)

     , 

     [Trades](../api/StockSharp.Messages.MarketDataTypes.Trades.html)

      and etc. ). 
   - For some data types, you need to additionally specify the 

     [BuildCandlesField](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesField.html)

     , property from which the data will be built. For example, for 

     [Level1](../api/StockSharp.Messages.MarketDataTypes.Level1.html)

      , you can specify 

     [BestAskPrice](../api/StockSharp.Messages.Level1Fields.BestAskPrice.html)

     , , which means that candles will be built from the 

     [BestAskPrice](../api/StockSharp.Messages.Level1Fields.BestAskPrice.html)

      property of 

     [Level1](../api/StockSharp.Messages.MarketDataTypes.Level1.html)

      data. 
6. Let's consider a few examples of building different candle types:
   - Since most sources provide candles with standard timeframes, it’s enough to set the type and timeframe to get such candles: 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromMinutes(5));
     					
     ```
   - If you just want to load the finished candles, then you need to set the [BuildCandlesMode](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesMode.html) property in [Load](../api/StockSharp.Messages.MarketDataBuildModes.Load.html): 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromMinutes(5))
     {
     	BuildCandlesMode = MarketDataBuildModes.Load,
     };	
     					
     ```
   - If the source does not provide the necessary timeframe candles, then they can be built from other market data. Below is an example of building candles with a timeframe of 21 seconds from trades: 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromSeconds(21))
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.Trades,
     };	
     					
     ```
   - If the data source provides neither candles nor trades, candles can be built from the market depth spread: 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromSeconds(21))
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.MarketDepth,
     	BuildCandlesField = Level1Fields.SpreadMiddle,
     };	
     					
     ```
   - Since there are no sources providing a ready **volume profile**, it also needs to be built from another data type. To draw a **volume profile**, you need to set the [IsCalcVolumeProfile](../api/StockSharp.Algo.Candles.CandleSeries.IsCalcVolumeProfile.html) property to 'true', as well as [BuildCandlesMode](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesMode.html) to [Build](../api/StockSharp.Messages.MarketDataBuildModes.Build.html). And specify the data type from which the **volume profile** will be built. In this case, it's [Trades](../api/StockSharp.Messages.MarketDataTypes.Trades.html): 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromMinutes(5))
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.Trades,
         IsCalcVolumeProfile = true,
     };	
     					
     ```
   - o Since most data sources do not provide finished candles, except for [TimeFrameCandle](../api/StockSharp.Algo.Candles.TimeFrameCandle.html), , other candle types are built similarly to the **volume profile**. You need to set the [BuildCandlesMode](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesMode.html) property to [Build](../api/StockSharp.Messages.MarketDataBuildModes.Build.html) or [LoadAndBuild](../api/StockSharp.Messages.MarketDataBuildModes.LoadAndBuild.html). Also set the [BuildCandlesFrom](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesFrom.html) property and the [BuildCandlesField](../api/StockSharp.Algo.Candles.CandleSeries.BuildCandlesField.html) property, if necessary. 

     The following code shows building a [VolumeCandle](../api/StockSharp.Algo.Candles.VolumeCandle.html) with a volume of 1000 contracts. The middle of the market depth spread is used as the data source for building.

     ```cs
     _candleSeries = new CandleSeries(typeof(VolumeCandle), security, 1000m)
     {
     	BuildCandlesMode = MarketDataBuildModes.LoadAndBuild,
     	BuildCandlesFrom = MarketDataTypes.MarketDepth,
     	BuildCandlesField = Level1Fields.SpreadMiddle,
     };
     					
     ```
   - The following code shows building a [TickCandle](../api/StockSharp.Algo.Candles.TickCandle.html) of 1000 trades. Trades are used as a data source for building.

     ```cs
     	   
     _candleSeries = new CandleSeries(typeof(TickCandle), security, 1000)
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.Trades,
     };
     					
     ```
   - The following code shows building a [RangeCandle](../api/StockSharp.Algo.Candles.RangeCandle.html) with a range of 0.1 c.u. The best buy of a market depth is used as a data source for building:

     ```cs
     _candleSeries = new CandleSeries(typeof(RangeCandle), security, new Unit(0.1m))
     {
     	BuildCandlesMode = MarketDataBuildModes.LoadAndBuild,
         BuildCandlesFrom = MarketDataTypes.MarketDepth,
         BuildCandlesField = Level1Fields.BestBid,
     };
     					
     ```
   - The following code shows the building [RenkoCandle](../api/StockSharp.Algo.Candles.RenkoCandle.html). The price of the last trade from Level1 is used as a data source for building:

     ```cs
     _candleSeries = new CandleSeries(typeof(RenkoCandle), security, new Unit(0.1m))
     {
     	BuildCandlesMode = MarketDataBuildModes.LoadAndBuild,
         BuildCandlesFrom = MarketDataTypes.Level1,
         BuildCandlesField = Level1Fields.LastTradePrice,
     };
     					
     ```
   - The following code shows the building [PnFCandle](../api/StockSharp.Algo.Candles.PnFCandle.html). Trades are used as a data source for building.

     ```cs
     _candleSeries = new CandleSeries(typeof(PnFCandle), security, new PnFArg() { BoxSize = 0.1m, ReversalAmount =1})
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.Trades,
     };	
     					
     ```

## Recommended content

[Chart](CandlesUI.md)

[Patterns](CandlesPatterns.md)

[Custom type of candle](CandlesCandleFactory.md)

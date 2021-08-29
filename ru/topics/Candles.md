# Свечи

[S\#](StockSharpAbout.md) поддерживает следующие виды:

- [TimeFrameCandle](xref:StockSharp.Algo.Candles.TimeFrameCandle) \- свеча на основе временного отрезка, таймфрейма. Можно задавать как популярные отрезки (минутки, часовики, дневные), так и кастомизированные. Например, 21 секунда, 4.5 минуты и т.д. 
- [RangeCandle](xref:StockSharp.Algo.Candles.RangeCandle) \- свеча ценового разброса. Новая свеча создается, когда появляется сделка с ценой, выходящей за допустимые пределы. Допустимый предел формируется каждый раз на основе цены первой сделки. 
- [VolumeCandle](xref:StockSharp.Algo.Candles.VolumeCandle) \- свеча формируется до тех пор, пока суммарно по сделкам не будет превышен объем. Если новая сделка превышает допустимый объем, то она попадает уже в новую свечу. 
- [TickCandle](xref:StockSharp.Algo.Candles.TickCandle) \- то же самое, что и [VolumeCandle](xref:StockSharp.Algo.Candles.VolumeCandle), только в качестве ограничения вместо объема берется количество сделок. 
- [PnFCandle](xref:StockSharp.Algo.Candles.PnFCandle) \- свеча пункто\-цифрового графика (график крестики\-нолики). 
- [RenkoCandle](xref:StockSharp.Algo.Candles.RenkoCandle) \- Рэнко свеча. 

Как работать со свечами, показано в примере SampleConnection, который расположен в папке *Samples\/Common\/SampleConnection*.

На следующих рисунках представлены графики [TimeFrameCandle](xref:StockSharp.Algo.Candles.TimeFrameCandle) и [RangeCandle](xref:StockSharp.Algo.Candles.RangeCandle):

![sample timeframecandles](../images/sample_timeframecandles.png)

![sample rangecandles](../images/sample_rangecandles.png)

## Запуск получения данных

1. Создадим серию свечей [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries): 

   ```cs
   ...
   _candleSeries = new CandleSeries(CandleSettingsEditor.Settings.CandleType, security, CandleSettingsEditor.Settings.Arg);
   ...		
   					
   ```
2. Все необходимые методы для получения свечей реализованы в классе [Connector](xref:StockSharp.Algo.Connector).

   Для получения свечей необходимо подписаться на событие [Connector.CandleSeriesProcessing](xref:StockSharp.Algo.Connector.CandleSeriesProcessing), сигнализирующее о появлении нового значения для обработки:

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
   > Для отображения свечей используется графический компонент [Chart](xref:StockSharp.Xaml.Charting.Chart). 
3. Далее передаём в коннектор созданную серию свечей и запускаем получение данных через [SubscribeCandles](xref:StockSharp.Algo.Connector.SubscribeCandles):

   ```cs
   ...
   _connector.SubscribeCandles(_candleSeries, DateTime.Today.Subtract(TimeSpan.FromDays(30)), DateTime.Now);	
   ...
   		
   					
   ```

   После этого этапа начнёт вызываться событие [Connector.CandleSeriesProcessing](xref:StockSharp.Algo.Connector.CandleSeriesProcessing).
4. Событии [Connector.CandleSeriesProcessing](xref:StockSharp.Algo.Connector.CandleSeriesProcessing) вызывается не только при появлении новой свечи, но и при изменении текущей.

   Если же нужно отображать только **"целые"** свечи, то необходимо проверить свойство [State](xref:StockSharp.Algo.Candles.Candle.State) пришедшей свечи:

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
5. Для [CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) можно задать некоторые свойства:
   - [BuildCandlesMode](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesMode) задает режим построения свечей. По умолчанию задан [LoadAndBuild](xref:StockSharp.Messages.MarketDataBuildModes.LoadAndBuild), что говорит о том, что будут запрошены готовые данные, или построены из заданного в свойстве [BuildCandlesFrom](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesFrom) типа данных. Также можно установить [Load](xref:StockSharp.Messages.MarketDataBuildModes.Load) для запроса только готовых данных. Или [Build](xref:StockSharp.Messages.MarketDataBuildModes.Build), для построения из заданного в свойстве [BuildCandlesFrom](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesFrom) типа данных без запроса готовых данных. 
   - При построении свечей необходимо задать свойство [BuildCandlesFrom](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesFrom), которое говорит о том, какой именно тип данных используется как источник ([Level1](xref:StockSharp.Messages.MarketDataTypes.Level1), [MarketDepth](xref:StockSharp.Messages.MarketDataTypes.MarketDepth), [Trades](xref:StockSharp.Messages.MarketDataTypes.Trades) и тд. ). 
   - Для некоторых типов данных необходимо дополнительно указать свойство [BuildCandlesField](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesField), из которого будут построены данные. Например, для [Level1](xref:StockSharp.Messages.MarketDataTypes.Level1) можно указать [BestAskPrice](xref:StockSharp.Messages.Level1Fields.BestAskPrice), что говорт о том, что свечи будут строиться из свойства [BestAskPrice](xref:StockSharp.Messages.Level1Fields.BestAskPrice) данных [Level1](xref:StockSharp.Messages.MarketDataTypes.Level1). 
6. Рассмотрим несколько примеров построения разных типов свечей:
   - Так как большинство источников предоставляют свечи стандартных таймфреймом, то для получения таких свечей достаточно задать тип и таймфрейм: 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromMinutes(5));
     					
     ```
   - Если необходимо просто загрузить готовые свечи, то необходимо задать свойство [BuildCandlesMode](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesMode) в [Load](xref:StockSharp.Messages.MarketDataBuildModes.Load): 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromMinutes(5))
     {
     	BuildCandlesMode = MarketDataBuildModes.Load,
     };	
     					
     ```
   - Если источник не предоставляет свечей необходимого таймфрейма, то их можно построить из других маркет данных. Ниже приведен приме построения свечей с таймфреймом 21 секунда из сделок: 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromSeconds(21))
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.Trades,
     };	
     					
     ```
   - Если источник данных не предоставляет ни свечей, ни сделок свечи можно построить из спреда стакана: 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromSeconds(21))
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.MarketDepth,
     	BuildCandlesField = Level1Fields.SpreadMiddle,
     };	
     					
     ```
   - Так как не существует источников, предоставляющих готового **профиля объема**, его тоже необходимо строить из другого типа данных. Для прорисовки **профиля объема** необходимо установить свойство [IsCalcVolumeProfile](xref:StockSharp.Algo.Candles.CandleSeries.IsCalcVolumeProfile) в true, а также [BuildCandlesMode](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesMode) в [Build](xref:StockSharp.Messages.MarketDataBuildModes.Build). И указать тип данных из которого будет построен **профиль объема**. В данном случае это [Trades](xref:StockSharp.Messages.MarketDataTypes.Trades): 

     ```cs
     _candleSeries = new CandleSeries(typeof(TimeFrameCandle), security, TimeSpan.FromMinutes(5))
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.Trades,
         IsCalcVolumeProfile = true,
     };	
     					
     ```
   - Так как большинство источников данных не предоставляют готовые свечей, кроме [TimeFrameCandle](xref:StockSharp.Algo.Candles.TimeFrameCandle), то остальные типы свечей строятся аналогично **профилю объема**. Необходимо указать свойство [BuildCandlesMode](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesMode) в [Build](xref:StockSharp.Messages.MarketDataBuildModes.Build) или [LoadAndBuild](xref:StockSharp.Messages.MarketDataBuildModes.LoadAndBuild). А также задать свойство [BuildCandlesFrom](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesFrom) и свойство [BuildCandlesField](xref:StockSharp.Algo.Candles.CandleSeries.BuildCandlesField) если необходимо. 

     Следующий код демонстрирует построение [VolumeCandle](xref:StockSharp.Algo.Candles.VolumeCandle) с объемом в 1000 контрактов. В качестве источника данных для построения используется середина спреда стакана.

     ```cs
     _candleSeries = new CandleSeries(typeof(VolumeCandle), security, 1000m)
     {
     	BuildCandlesMode = MarketDataBuildModes.LoadAndBuild,
     	BuildCandlesFrom = MarketDataTypes.MarketDepth,
     	BuildCandlesField = Level1Fields.SpreadMiddle,
     };
     					
     ```
   - Следующий код демонстрирует построение [TickCandle](xref:StockSharp.Algo.Candles.TickCandle) в 1000 сделок. В качестве источника данных для построения используются сделки.

     ```cs
     	   
     _candleSeries = new CandleSeries(typeof(TickCandle), security, 1000)
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.Trades,
     };
     					
     ```
   - Следующий код демонстрирует построение [RangeCandle](xref:StockSharp.Algo.Candles.RangeCandle) с диапазоном в 0.1 у.е. В качестве источника данных для построения используется лучшая покупка стакана:

     ```cs
     _candleSeries = new CandleSeries(typeof(RangeCandle), security, new Unit(0.1m))
     {
     	BuildCandlesMode = MarketDataBuildModes.LoadAndBuild,
         BuildCandlesFrom = MarketDataTypes.MarketDepth,
         BuildCandlesField = Level1Fields.BestBid,
     };
     					
     ```
   - Следующий код демонстрирует построение [RenkoCandle](xref:StockSharp.Algo.Candles.RenkoCandle). В качестве источника данных для построения используется цена последней сделки из Level1:

     ```cs
     _candleSeries = new CandleSeries(typeof(RenkoCandle), security, new Unit(0.1m))
     {
     	BuildCandlesMode = MarketDataBuildModes.LoadAndBuild,
         BuildCandlesFrom = MarketDataTypes.Level1,
         BuildCandlesField = Level1Fields.LastTradePrice,
     };
     					
     ```
   - Следующий код демонстрирует построение [PnFCandle](xref:StockSharp.Algo.Candles.PnFCandle). В качестве источника данных для построения используются сделки.

     ```cs
     _candleSeries = new CandleSeries(typeof(PnFCandle), security, new PnFArg() { BoxSize = 0.1m, ReversalAmount =1})
     {
     	BuildCandlesMode = MarketDataBuildModes.Build,
     	BuildCandlesFrom = MarketDataTypes.Trades,
     };	
     					
     ```

## Следующие шаги

[График](CandlesUI.md)

[Паттерны](CandlesPatterns.md)

[Собственный тип свечей](CandlesCandleFactory.md)

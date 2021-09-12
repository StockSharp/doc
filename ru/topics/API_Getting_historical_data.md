# Получение исторических данных

[S\#.API](StockSharpAbout.md) предоставляет возможность получать исторические свечи, которые можно использовать как для тестирования, так и для построения [индикаторов](Indicators.md). 

## Работа с историческими свечами через Connector

1. Для получения свечей через [Connector](xref:StockSharp.Algo.Connector) необходимо создать [Connector](xref:StockSharp.Algo.Connector) и добавить в него соответствующий [MessageAdapter](xref:StockSharp.Messages.MessageAdapter). Как это сделать с помощью специального контрола описано в пункте [Окно настройки подключений](API_UI_ConnectorWindow.md).

   Также добавить соответствующий [MessageAdapter](xref:StockSharp.Messages.MessageAdapter) в [Connector](xref:StockSharp.Algo.Connector) можно через код. Например, инициализация адаптера для [Interactive Brokers](IB.md) описана в пункте [Инициализация адаптера Interactive Brokers](IBSample.md) и выглядит следующим образом:

   ```cs
   Connector Connector = new Connector();				
   ...				
   var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
   {
   	Address = "<Your Address>".To<EndPoint>(),
   };
   Connector.Adapter.InnerAdapters.Add(messageAdapter);
   ...	
   Connector.Connect();
   ...
   							
   ```
2. Для того, чтобы получить исторические свечи, необходимо вызвать метод [TraderHelper.SubscribeCandles](xref:StockSharp.Algo.TraderHelper.SubscribeCandles(StockSharp.Algo.ISubscriptionProvider,StockSharp.Algo.Candles.CandleSeries,System.Nullable{System.DateTimeOffset},System.Nullable{System.DateTimeOffset},System.Nullable{System.Int64},System.Nullable{System.Int64},StockSharp.Messages.IMessageAdapter,System.Nullable{System.Int64}))**(**[StockSharp.Algo.ISubscriptionProvider](xref:StockSharp.Algo.ISubscriptionProvider) provider, [StockSharp.Algo.Candles.CandleSeries](xref:StockSharp.Algo.Candles.CandleSeries) series, [System.Nullable\<System.DateTimeOffset\>](xref:System.Nullable`1) from, [System.Nullable\<System.DateTimeOffset\>](xref:System.Nullable`1) to, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) count, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) transactionId, [StockSharp.Messages.IMessageAdapter](xref:StockSharp.Messages.IMessageAdapter) adapter, [System.Nullable\<System.Int64\>](xref:System.Nullable`1) skip**)**: 

   ```cs
   ...
   var tf = (TimeSpan)CandlesPeriods.SelectedItem;
   var series = new CandleSeries(typeof(TimeFrameCandle), SelectedSecurity, tf);
   Connector.SubscribeCandles(SelectedSecurity, DateTime.Now.Subtract(TimeSpan.FromTicks(tf.Ticks * 100)), DateTime.Now);
   ...
   			
   ```
3. Исторические свечи передаются через событие [Connector.CandleSeriesProcessing](xref:StockSharp.Algo.Connector.CandleSeriesProcessing): 

   ```cs
   ...
   Connector.CandleSeriesProcessing += ProcessCandle;
   ...
   			
   ```
4. Появившиеся свечи можно отрисовывать через [графический контрол](CandlesUI.md).

## Работа с историческими свечами через MessageAdapter

1. Для получения свечей через [MessageAdapter](xref:StockSharp.Messages.MessageAdapter) необходимо созлать соответствующий [MessageAdapter](xref:StockSharp.Messages.MessageAdapter).

   Например, инициализация адаптера для [Interactive Brokers](IB.md) описана в пункте [Инициализация адаптера Interactive Brokers](IBSample.md) и выглядит следующим образом:

   ```cs
   		
   ...         
   var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
   {
   	Address = "<Your Address>".To<EndPoint>(),
   };
   ...
   							
   ```
2. Оборачиваем адаптер [Interactive Brokers](IB.md) в адаптер системных идентификатор инструментов [SecurityNativeIdMessageAdapter](xref:StockSharp.Algo.SecurityNativeIdMessageAdapter). Это необходимо в том случае, если торговая система работает с числовыми или любыми другими идентификаторами инструментов, отличных от обычного строкового представления.

   ```cs
   	
   ...
   SecurityNativeIdMessageAdapter _securityAdapter;
   if (adapter.IsNativeIdentifiers)
   	_securityAdapter = new SecurityNativeIdMessageAdapter(adapter, new InMemoryNativeIdStorage());
   var securities = _securityAdapter.GetSecurities(new SecurityLookupMessage
   {
   	SecurityId = new SecurityId
   	{
   		SecurityCode = "EUR"
   	}
   });
   SecurityMessage eurUsd = null;
   foreach (var security in securities)
   {
   	if (security.SecurityId.SecurityCode.CompareIgnoreCase("EURUSD"))
   		eurUsd = security;
   }
   ...
   							
   ```
3. Теперь по полученному идентификатору инструмента получаем свечи из адаптера: 

   ```cs
   ...
   var candles = adapter.GetCandles(eurUsd.SecurityId, TimeSpan.FromDays(1), DateTimeOffset.Now.AddDays(-100), DateTimeOffset.Now);
   foreach (var candle in candles)
   {
   	Console.WriteLine(candle);
   }
   ...
   			
   ```

## См. также

[Свечи](Candles.md)

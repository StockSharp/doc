# Получение исторических данных

[S\#.API](StockSharpAbout.md) предоставляет возможность получать исторические свечи, которые можно использовать как для тестирования, так и для построения [индикаторов](Indicators.md). 

### Работа с историческими свечами через Connector

Работа с историческими свечами через Connector

1. Для получения свечей через [Connector](../api/StockSharp.Algo.Connector.html) необходимо создать [Connector](../api/StockSharp.Algo.Connector.html) и добавить в него соответствующий [MessageAdapter](../api/StockSharp.Messages.MessageAdapter.html). Как это сделать с помощью специального контрола описано в пункте [Окно настройки подключений](API_UI_ConnectorWindow.md).

   Также добавить соответствующий [MessageAdapter](../api/StockSharp.Messages.MessageAdapter.html) в [Connector](../api/StockSharp.Algo.Connector.html) можно через код. Например, инициализация адаптера для [Interactive Brokers](IB.md) описана в пункте [Инициализация адаптера Interactive Brokers](IBSample.md) и выглядит следующим образом:

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
2. Для того, чтобы получить исторические свечи, необходимо вызвать метод [SubscribeCandles](../api/StockSharp.Algo.Connector.SubscribeCandles.html): 

   ```cs
   ...
   var tf = (TimeSpan)CandlesPeriods.SelectedItem;
   var series = new CandleSeries(typeof(TimeFrameCandle), SelectedSecurity, tf);
   Connector.SubscribeCandles(SelectedSecurity, DateTime.Now.Subtract(TimeSpan.FromTicks(tf.Ticks * 100)), DateTime.Now);
   ...
   			
   ```
3. Исторические свечи передаются через событие [CandleSeriesProcessing](../api/StockSharp.Algo.Connector.CandleSeriesProcessing.html): 

   ```cs
   ...
   Connector.CandleSeriesProcessing += ProcessCandle;
   ...
   			
   ```
4. Появившиеся свечи можно отрисовывать через [графический контрол](CandlesUI.md).

### Работа с историческими свечами через MessageAdapter

Работа с историческими свечами через MessageAdapter

1. Для получения свечей через [MessageAdapter](../api/StockSharp.Messages.MessageAdapter.html) необходимо созлать соответствующий [MessageAdapter](../api/StockSharp.Messages.MessageAdapter.html).

   Например, инициализация адаптера для [Interactive Brokers](IB.md) описана в пункте [Инициализация адаптера Interactive Brokers](IBSample.md) и выглядит следующим образом:

   ```cs
   		
   ...         
   var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
   {
   	Address = "<Your Address>".To<EndPoint>(),
   };
   ...
   							
   ```
2. Оборачиваем адаптер [Interactive Brokers](IB.md) в адаптер системных идентификатор инструментов [SecurityNativeIdMessageAdapter](../api/StockSharp.Algo.SecurityNativeIdMessageAdapter.html). Это необходимо в том случае, если торговая система работает с числовыми или любыми другими идентификаторами инструментов, отличных от обычного строкового представления.

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

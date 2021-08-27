# Хранилище сообщений

Наряду с использованием хранилищ торговых объектов, можно использовать хранилища сообщений. Работа с этими хранилищами также осуществляется через интерфейс [IMarketDataStorage\`1](xref:StockSharp.Algo.Storages.IMarketDataStorage`1). Например, для работы со свечами можно использовать хранилище типа IMarketDataStorage\<CandleMessage\>. [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) также содержит набор методов для получения нужных хранилищ сообщений. Так хранилище временных свечей можно получить, как показано в следующем фрагменте кода. 

```cs
   var security = new Security() { Id = "RIM5@FORTS" };
   var candleMessageStorage = _storage.GetCandleMessageStorage(typeof(TimeFrameCandleMessage), security, TimeSpan.FromMinutes(1));
	
```

Далее с хранилищем можно работать при помощи методов [IMarketDataStorage.Load](xref:StockSharp.Algo.Storages.IMarketDataStorage.Load) и\/или [IMarketDataStorage.Save](xref:StockSharp.Algo.Storages.IMarketDataStorage.Save), как показано в примере в [предыдущем разделе](StoragesApi.md). 

Обратите внимание, что [S\#](StockSharpAbout.md) позволяет приводить типы хранилища торговых объектов к соответствующим типам хранилищ сообщений и наоборот. Например, IMarketDataStorage\<MarketDepth\> можно привести к типу IMarketDataStorage\<QuoteChangeMessage\> и наоборот. 

```cs
	
   var security = new Security() { Id = "RIM5@FORTS" };
   
   var depthStorage = _storage.GetMarketDepthStorage(_security);
   var quoteMessageStorage = depthStorage as IMarketDataStorage<QuoteChangeMessage>;
   
   var quoteMessageStorage1 = _storage.GetQuoteMessageStorage(_security);
   var depthStorage1 = quoteMessageStorage1 as IMarketDataStorage<MarketDepth>;
	
```

Следует обратить внимание на актуальность использования хранилища сообщений для хранения собственных заявок и сделок. Дело в том, что для этих торговых объектов нет соответствующих хранилищ. В этом случае придется использовать хранилище сообщений IMarketDataStorage\<ExecutionMessage\>. В следующем примере показано, как сохранить в хранилище собственные сделки. 

### Пример сохранения собственных сделок

Пример сохранения собственных сделок

1. Сначала создается экземпляр коннектора, а также хранилище. Кроме того мы определяем идентификатор инструмента, с которым будем работать и декларируем переменную для хранилища транзакций. Само хранилище транзакций для заданного инструмента будет получено в событии получения инструментов при помощи метода [IMessageStorageRegistry.GetTransactionStorage](xref:StockSharp.Algo.Storages.IMessageStorageRegistry.GetTransactionStorage). 

   ```cs
   // Создаем коннектор
   var connector = new Connector();
   // Путь к корневой папке хранилища
   var storagePath = @"....";
   var securityId = "RIM5@FORTS";
   // Создаем хранилище
   var storage = new StorageRegistry() { DefaultDrive = new LocalMarketDataDrive(storagePath) };
   // Декларируем хранилище транзакций
   IMarketDataStorage<ExecutionMessage> tranStorage = null;
   // В обработчике событии получения инструментов
   // получаем хранилище транзакций для заданного инструмента
   connector.NewSecurity += security =>
   {
   		if (security.Id == securityId)
   			tranStorage = storage.GetTransactionStorage(security.ToSecurityId());
   };
    
   ```
2. Сохранение собственных сделок будет выполняться в обработчике события [Connector.NewMyTrade](xref:StockSharp.Algo.Connector.NewMyTrade) при помощи метода [IMarketDataStorage.Save](xref:StockSharp.Algo.Storages.IMarketDataStorage.Save). Перед сохранением список собственных сделок приводится к типу IEnumerable\<ExecutionMessage\>. Сама процедура выставления заявок в этим примере опущена. 

   ```cs
   // сохраняем сделки в хранилище
   connector.NewMyTrade += trade =>
   {
       tranStorage.Save(new[] {trade.ToMessage()});
   };
   ```

## См. также

[Api](StoragesApi.md)

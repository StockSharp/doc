# API

Вся работа по сохранению и восстановлению данных в [S\#](../../api.md) происходит через специальный API, расположенный в разделе [StockSharp.Algo.Storages](xref:StockSharp.Algo.Storages). В данном разделе находится интерфейс [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry), который создан для описания всех возможных действий с хранилищем, и содержит такие свойства как [IEntityRegistry.Securities](xref:StockSharp.Algo.Storages.IEntityRegistry.Securities), [IEntityRegistry.Positions](xref:StockSharp.Algo.Storages.IEntityRegistry.Positions) и т.д. Через эти свойства можно получить все сохраненные ранее торговые объекты, например, инструменты. Вся работа идет как с обычной коллекцией за счет использования интерфейса [IStorageEntityList\<T\>](xref:StockSharp.Algo.Storages.IStorageEntityList`1). Если требуется сохранить торговый объект в хранилище (например, появилась новая заявка, или обновилась ранее зарегистрированная), то необходимо использовать метод [IStorageEntityList\<T\>.Save](xref:StockSharp.Algo.Storages.IStorageEntityList`1.Save(`0))**(**[T](xref:T) entity **)**.

Реализацией по умолчанию интерфейса [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) является класс [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry). Он взаимодействует с данными через низкоуровневый системный интерфейс [IStorage](xref:Ecng.Serialization.IStorage).

Работа с маркет\-данными, такие как тиковые сделки или стаканы, происходит через отдельный интерфейс [IMarketDataStorage\<TMessage\>](xref:StockSharp.Algo.Storages.IMarketDataStorage`1), который получается на основе информации об инструменте через методы [IStorageRegistry.GetTradeStorage](xref:StockSharp.Algo.Storages.IStorageRegistry.GetTradeStorage(StockSharp.BusinessEntities.Security,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [StockSharp.Algo.Storages.IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) drive, [StockSharp.Algo.Storages.StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) format **)** и [IStorageRegistry.GetMarketDepthStorage](xref:StockSharp.Algo.Storages.IStorageRegistry.GetMarketDepthStorage(StockSharp.BusinessEntities.Security,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [StockSharp.Algo.Storages.IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) drive, [StockSharp.Algo.Storages.StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) format **)** для тиковых сделок и стаканов соответственно.

Если используется [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry), то реализация методов с маркет\-данными не зависит от [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive), так как данные всегда сохраняются в файл. Это внутренний формат [S\#](../../api.md), и он организован таким образом, чтобы сделки или стаканы занимали минимум места на диске. Путь к директории, где будут сохраняться (или считываться) маркет\-данные, указывается через свойство [LocalMarketDataDrive.Path](xref:StockSharp.Algo.Storages.LocalMarketDataDrive.Path) у хранилища [IStorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.IStorageRegistry.DefaultDrive).

По этому пути будут созданы папки с названиями, равные [идентификаторам инструментов](../instruments/instrument_identifier.md) (для каждого инструмента отдельная папка).

Внутри каждой такой папки будут созданы подпапки, обозначающие даты маркет\-данных. Например, если сохранять тиковые сделки за период 3 дня, то для них будут созданы 3 отдельные папки с датами. Формат названия папки всегда фиксирован и равен yyyy\_MM\_dd. 

Внутри каждой папки с датами находятся файлы, с расширением bin. Сделки хранятся в файле *trades.bin*, стаканы в *quotes.bin*. Также могут присутствовать файлы *candles\_XXX.bin*, где хранятся [свечи](../candles.md) разных типов (название файла указывает на тип и параметр свечей) и файлы *orderLog.bin*, в которых хранится ордер лог.

## Пример работы с хранилищем маркет\-данных

1. Пример SampleStorage, находящийся в дистрибутиве [S\#](../../api.md), показывает, как сохранить и загрузить сделки через класс [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry). В начале создается инструмент, у которого инициализируются основные свойства \- [Security.Id](xref:StockSharp.BusinessEntities.Security.Id) (для определения месторасположения), [Security.StepPrice](xref:StockSharp.BusinessEntities.Security.StepPrice) и [Security.Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) (для сжатия цены в файле *trades.bin*):

   ```cs
   var security = new Security
   {
   	Id = "TestId",
   	MinStepSize = 0.1,
   	Decimals = 1,
   };
   					
   ```
2. Далее, в качестве исходных данных, создается список из 1000 произвольных сделок (в реальном приложении это будут те сделки, которые или получены из внешних источников, или торгового приложения):

   ```cs
   var trades = new List<Trade>();
   // генерируем 1000 произвольных сделок
   //
   var tradeGenerator = new RandomWalkTradeGenerator(security, 99)
   {
   	IdGenerator = new IdGenerator
   	{
   		Current = DateTime.Now.Ticks
   	}
   };
   // инициализация генератора
   tradeGenerator.Init();
   for (var i = 0; i < 1000; i++)
   {
   	var t = tradeGenerator.Generate(DateTime.Today + TimeSpan.FromMinutes(i));
   	t.Id = i + 1;
   	trades.Add(t);
   }
   					
   ```
3. Следующим шагом создается сам [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry):

   ```cs
   var storage = new StorageRegistry();
   					
   ```
4. Через хранилище торговых объектов получается хранилище маркет\-данных. В примере используется хранилище тиковых сделок, которое получается через метод [StorageRegistry.GetTradeStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTradeStorage(StockSharp.BusinessEntities.Security,StockSharp.Algo.Storages.IMarketDataDrive,StockSharp.Algo.Storages.StorageFormats))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security, [StockSharp.Algo.Storages.IMarketDataDrive](xref:StockSharp.Algo.Storages.IMarketDataDrive) drive, [StockSharp.Algo.Storages.StorageFormats](xref:StockSharp.Algo.Storages.StorageFormats) format **)**:

   ```cs
   var tradeStorage = storage.GetTradeStorage(security);
   					
   ```
5. Сохранение сделок:

   ```cs
   tradeStorage.Save(trades);
   					
   ```
6. Загрузка сделок, сохраненных на предыдущем шаге:

   ```cs
   var loadedTrades = tradeStorage.Load(DateTime.Today, DateTime.Today + TimeSpan.FromMinutes(1000));
    	  
   foreach (var trade in loadedTrades)
   {
   	Console.WriteLine("Сделка № {0}: {1}", trade.Id, trade);
   }
   					
   ```
7. Так как сделки сохраняются в файл, то при следующем запуске примера они будут там присутствовать, и пример выведет уже не 1000 сделок, а 2000. Чтобы пример работал правильно, сделки в конце работы необходимо удалить:

   ```cs
   tradeStorage.Delete(DateTime.Today, DateTime.Today + TimeSpan.FromMinutes(1000));
   					
   ```

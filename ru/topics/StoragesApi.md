# API

Вся работа по сохранению и восстановлению данных в [S\#](StockSharpAbout.md) происходит через специальный API, расположенный в разделе [Storages](xref:StockSharp.Algo.Storages). В данном разделе находится интерфейс [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry), который создан для описания всех возможных действий с хранилищем, и содержит такие свойства как [Securities](xref:StockSharp.Algo.Storages.IEntityRegistry.Securities), [Positions](xref:StockSharp.Algo.Storages.IEntityRegistry.Positions) и т.д. Через эти свойства можно получить все сохраненные ранее торговые объекты, например, инструменты. Вся работа идет как с обычной коллекцией за счет использования интерфейса [IStorageEntityList\`1](xref:StockSharp.Algo.Storages.IStorageEntityList`1). Если требуется сохранить торговый объект в хранилище (например, появилась новая заявка, или обновилась ранее зарегистрированная), то необходимо использовать метод [IStorageEntityList\`1.Save](xref:StockSharp.Algo.Storages.IStorageEntityList`1.Save).

Реализацией по умолчанию интерфейса [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) является класс [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry). Он взаимодействует с данными через низкоуровневый системный интерфейс [IStorage](xref:Ecng.Serialization.IStorage). Именно данный интерфейс предоставляет прозрачную работу с [Базами данных](StoragesDatabase.md), скрывая особенности.

Работа с маркет\-данными, такие как тиковые сделки или стаканы, происходит через отдельный интерфейс [IMarketDataStorage\`1](xref:StockSharp.Algo.Storages.IMarketDataStorage`1), который получается на основе информации об инструменте через методы [IStorageRegistry.GetTradeStorage](xref:StockSharp.Algo.Storages.IStorageRegistry.GetTradeStorage) и [IStorageRegistry.GetMarketDepthStorage](xref:StockSharp.Algo.Storages.IStorageRegistry.GetMarketDepthStorage) для тиковых сделок и стаканов соответственно.

Если используется [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry), то реализация методов с маркет\-данными не зависит от [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive), так как данные всегда сохраняются в файл. Это внутренний формат [S\#](StockSharpAbout.md), и он организован таким образом, чтобы сделки или стаканы занимали минимум места на диске. Путь к директории, где будут сохраняться (или считываться) маркет\-данные, указывается через свойство [LocalMarketDataDrive.Path](xref:StockSharp.Algo.Storages.LocalMarketDataDrive.Path) у хранилища [IStorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.IStorageRegistry.DefaultDrive).

По этому пути будут созданы папки с названиями, равные [идентификаторам инструментов](SecurityId.md) (для каждого инструмента отдельная папка).

Внутри каждой такой папки будут созданы подпапки, обозначающие даты маркет\-данных. Например, если сохранять тиковые сделки за период 3 дня, то для них будут созданы 3 отдельные папки с датами. Формат названия папки всегда фиксирован и равен yyyy\_MM\_dd. 

Внутри каждой папки с датами находятся файлы, с расширением bin. Сделки хранятся в файле *trades.bin*, стаканы в *quotes.bin*. Также могут присутствовать файлы *candles\_XXX.bin*, где хранятся [свечи](Candles.md) разных типов (название файла указывает на тип и параметр свечей) и файлы *orderLog.bin*, в которых хранится ордер лог.

## Пример работы с хранилищем маркет\-данных

1. Пример SampleStorage, находящийся в дистрибутиве [S\#](StockSharpAbout.md), показывает, как сохранить и загрузить сделки через класс [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry). В начале создается инструмент, у которого инициализируются основные свойства \- [Id](xref:StockSharp.BusinessEntities.Security.Id) (для определения месторасположения), [StepPrice](xref:StockSharp.BusinessEntities.Security.StepPrice) и [Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) (для сжатия цены в файле *trades.bin*):

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
4. Через хранилище торговых объектов получается хранилище маркет\-данных. В примере используется хранилище тиковых сделок, которое получается через метод [StorageRegistry.GetTradeStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTradeStorage):

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

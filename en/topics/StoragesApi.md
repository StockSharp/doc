# API

All processes of the data saving and recovery in the [S\#](StockSharpAbout.md) run through a special API, located in the [Storages](xref:StockSharp.Algo.Storages) section. There is the [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry), interface in this section, which created to describe all the possible actions with the storage and contains such properties as [Securities](xref:StockSharp.Algo.Storages.IEntityRegistry.Securities), [Positions](xref:StockSharp.Algo.Storages.IEntityRegistry.Positions) etc. Through these properties, it is possible to get all the previously saved trading objects, for example, instruments. All processes run as with regular collection using the [IStorageEntityList\`1](xref:StockSharp.Algo.Storages.IStorageEntityList`1) interface. If you want to save the trading object in the storage (for example, a new order appeared or previously registered order updated), you should use the [IStorageEntityList\`1.Save](xref:StockSharp.Algo.Storages.IStorageEntityList`1.Save) method.

The default implementation of the [IStorageRegistry](xref:StockSharp.Algo.Storages.IStorageRegistry) interface is the [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) class. It interacts with the data through a low\-level system interface [IStorage](xref:Ecng.Serialization.IStorage). This interface provides a transparent work with [databases](StoragesDatabase.md), hiding details.

The work with market data, such as tick trades or order books, takes place through the certain interface [IMarketDataStorage\`1](xref:StockSharp.Algo.Storages.IMarketDataStorage`1), which is obtained based on the information about the instrument through [IStorageRegistry.GetTradeStorage](xref:StockSharp.Algo.Storages.IStorageRegistry.GetTradeStorage) mathod. [IStorageRegistry.GetMarketDepthStorage](xref:StockSharp.Algo.Storages.IStorageRegistry.GetMarketDepthStorage) for tick trades or order books accordingly.

If the [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) used, the implementation of methods with market data does not depend on [StorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.StorageRegistry.DefaultDrive), since the data is always stored in the file. This is the internal format of the [S\#](StockSharpAbout.md), and it is organized in such a way that the trades or order books takes a minimum of disk space. The path to the directory where the market data will be stored (or read) indicated by the [LocalMarketDataDrive.Path](xref:StockSharp.Algo.Storages.LocalMarketDataDrive.Path) property of the [IStorageRegistry.DefaultDrive](xref:StockSharp.Algo.Storages.IStorageRegistry.DefaultDrive) storage.

The folders with the names equal [instruments identifiers](SecurityId.md) (separate folder for each instrument) will be created at this path.

Subfolders, indicating the date of the market data will be created inside each of these folders. For example, if you save the tick trades for a period of 3 days, then three separate folders with dates will be created. The format of the folder name is always fixed as yyyy\_MM\_dd.

Files with the bin extension are inside each folder with dates. Trades are stored in the **trades.bin** file, order books in the **quotes.bin** file. Also the *candles\_XXX.bin* files may be present, which store various types of [candles](Candles.md) (filename indicates the type and parameter of candles) and the *orderLog.bin* files where the order log stored.

## Example of working with market data storage

1. The SampleStorage example, located in the [S\#](StockSharpAbout.md) installation package, shows how to save and load the trades through the [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry)class. In the beginning an instrument created and its basic properties are initialized \- [Id](xref:StockSharp.BusinessEntities.Security.Id) (to determine location on disk), [StepPrice](xref:StockSharp.BusinessEntities.Security.StepPrice) and [Decimals](xref:StockSharp.BusinessEntities.Security.Decimals) (for decimal value compression in the **trades.bin** file):

   ```cs
   var security = new Security
   {
   	Id = "TestId",
   	MinStepSize = 0.1,
   	Decimals = 1,
   };
   					
   ```
2. Further, as initial data, a list of the 1,000 undefined trades created (in a real application, it will be those trades that are get from external sources or trade application):

   ```cs
   var trades = new List<Trade>();
   // generating 1000 random ticks
   //
   var tradeGenerator = new RandomWalkTradeGenerator(security, 99)
   {
   	IdGenerator = new IdGenerator
   	{
   		Current = DateTime.Now.Ticks
   	}
   };
   // generator initialization
   tradeGenerator.Init();
   for (var i = 0; i < 1000; i++)
   {
   	var t = tradeGenerator.Generate(DateTime.Today + TimeSpan.FromMinutes(i));
   	t.Id = i + 1;
   	trades.Add(t);
   }
   					
   ```
3. The [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) itself created on the next step:

   ```cs
   var storage = new StorageRegistry();
   					
   ```
4. The market data storage obtained via the trade objects store. The tick trades storage, which is obtained via the [StorageRegistry.GetTradeStorage](xref:StockSharp.Algo.Storages.StorageRegistry.GetTradeStorage) method, used in example:

   ```cs
   var tradeStorage = storage.GetTradeStorage(security);
   					
   ```
5. The saving of trades:

   ```cs
   tradeStorage.Save(trades);
   					
   ```
6. The loading of trades, which were saved in the previous step:

   ```cs
   var loadedTrades = tradeStorage.Load(DateTime.Today, DateTime.Today + TimeSpan.FromMinutes(1000));
    	  
   foreach (var trade in loadedTrades)
   {
   	Console.WriteLine("Tick № {0}: {1}", trade.Id, trade);
   }
   					
   ```
7. Since the trades are stored in the file, the next time you run the sample, they will be present there, and you will get 2000 trades instead of 1000. For correct work of example, it is necessary to remove the trades at the end of the work:

   ```cs
   tradeStorage.Delete(DateTime.Today, DateTime.Today + TimeSpan.FromMinutes(1000));
   					
   ```

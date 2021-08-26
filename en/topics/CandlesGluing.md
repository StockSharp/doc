# Gluing candles, history + real\-time

In order to glue historical candles with a real\-time, it is necessary to initialize the corresponding storage of trade objects [CsvEntityRegistry](../api/StockSharp.Algo.Storages.Csv.CsvEntityRegistry.html), storage of market data [StorageRegistry](../api/StockSharp.Algo.Storages.StorageRegistry.html) and registry of snapshot storage [SnapshotRegistry](../api/StockSharp.Algo.Storages.SnapshotRegistry.html). 

Consider the following example of gluing [TimeFrameCandle](../api/StockSharp.Algo.Candles.TimeFrameCandle.html) using the tick data saved with [S\#.Data](Hydra.md):

```cs
private Connector \_connector;
private Security security;
private CandleSeries \_series;
private readonly CsvEntityRegistry \_csvEntityRegistry;
private readonly StorageRegistry \_storageRegistry;
private readonly SnapshotRegistry \_snapshotRegistry;
private const string \_historyPath \= @"e:\\DataServer\\";
...
public MainWindow()
		{
			InitializeComponent();     
            ....   
            \_csvEntityRegistry \= new CsvEntityRegistry(\_historyPath);
            \_storageRegistry \= new StorageRegistry
            {
            	DefaultDrive \= new LocalMarketDataDrive(\_historyPath),
            };
            \_snapshotRegistry \= new SnapshotRegistry("Snapshots");
            \_connector.InitializeStorage(\_entityRegistry, \_storageRegistry, \_snapshotRegistry);
            \_connector.StorageAdapter.DaysLoad \= TimeSpan.FromDays(30);
		}
...
\_connector.CandleSeriesProcessing +\= Connector\_CandleSeriesProcessing;
...
\_candleSeries \=
	new CandleSeries(CandleSettingsEditor.Settings.CandleType, security, CandleSettingsEditor.Settings.Arg)
	{
		BuildCandlesMode \= MarketDataBuildModes.Build,
		BuildCandlesFrom \= MarketDataTypes.Trades,
		IsCalcVolumeProfile \= true,
	};
  
\_connector.SubscribeCandles(\_candleSeries, DateTime.Today.Subtract(TimeSpan.FromDays(30)), DateTime.Now);
		
```

After that, candles will start to arrive to the CandleSeriesProcessing event handler, starting from the first historical and until the current real\-time:

```cs
        
		private void Connector\_CandleSeriesProcessing(CandleSeries candleSeries, Candle candle)
		{
			Chart.Draw(\_candleElement, candle);
		}
		
```

If the [Connector](../api/StockSharp.Algo.Connector.html), is originally created, then no need to initialize StorageAdapter, enough to pass the storage of trade objects [CsvEntityRegistry](../api/StockSharp.Algo.Storages.Csv.CsvEntityRegistry.html), storage of market data [StorageRegistry](../api/StockSharp.Algo.Storages.StorageRegistry.html) and registry of snapshot storage [SnapshotRegistry](../api/StockSharp.Algo.Storages.SnapshotRegistry.html) to the [Connector](../api/StockSharp.Algo.Connector.html) constructor:

```cs
...
\_csvEntityRegistry \= new CsvEntityRegistry(\_historyPath);
\_storageRegistry \= new StorageRegistry
{
   DefaultDrive \= new LocalMarketDataDrive(\_historyPath),
};
\_snapshotRegistry \= new SnapshotRegistry("Snapshots");
\_connector \= new Connector(\_entityRegistry, \_storageRegistry, \_snapshotRegistry, supportOffline: true, supportSubscriptionTracking: true);
\_connector.StorageAdapter.DaysLoad \= TimeSpan.FromDays(3);
...
		
```

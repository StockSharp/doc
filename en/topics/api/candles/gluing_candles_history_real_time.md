# Merging candles, history + real\-time

In order to glue historical candles with a real\-time, it is necessary to initialize the corresponding storage of trade objects [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), storage of market data [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) and registry of snapshot storage [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry). 

Consider the following example of gluing [TimeFrameCandle](xref:StockSharp.Algo.Candles.TimeFrameCandle) using the tick data saved with [Hydra](../../hydra.md):

```cs
private Connector _connector;
private Security security;
private CandleSeries _series;
private readonly CsvEntityRegistry _csvEntityRegistry;
private readonly StorageRegistry _storageRegistry;
private readonly SnapshotRegistry _snapshotRegistry;
private const string _historyPath = @"e:\DataServer\";
...
public MainWindow()
		{
			InitializeComponent();     
            ....   
            _csvEntityRegistry = new CsvEntityRegistry(_historyPath);
            _storageRegistry = new StorageRegistry
            {
            	DefaultDrive = new LocalMarketDataDrive(_historyPath),
            };
            _snapshotRegistry = new SnapshotRegistry("Snapshots");
            _connector.InitializeStorage(_entityRegistry, _storageRegistry, _snapshotRegistry);
            _connector.StorageAdapter.DaysLoad = TimeSpan.FromDays(30);
		}
...
_connector.CandleSeriesProcessing += Connector_CandleSeriesProcessing;
...
_candleSeries =
	new CandleSeries(CandleSettingsEditor.Settings.CandleType, security, CandleSettingsEditor.Settings.Arg)
	{
		BuildCandlesMode = MarketDataBuildModes.Build,
		BuildCandlesFrom = MarketDataTypes.Trades,
		IsCalcVolumeProfile = true,
	};
  
_connector.SubscribeCandles(_candleSeries, DateTime.Today.Subtract(TimeSpan.FromDays(30)), DateTime.Now);
		
```

After that, candles will start to arrive to the CandleSeriesProcessing event handler, starting from the first historical and until the current real\-time:

```cs
        
		private void Connector_CandleSeriesProcessing(CandleSeries candleSeries, Candle candle)
		{
			Chart.Draw(_candleElement, candle);
		}
		
```

If the [Connector](xref:StockSharp.Algo.Connector), is originally created, then no need to initialize StorageAdapter, enough to pass the storage of trade objects [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), storage of market data [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) and registry of snapshot storage [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) to the [Connector](xref:StockSharp.Algo.Connector) constructor:

```cs
...
_csvEntityRegistry = new CsvEntityRegistry(_historyPath);
_storageRegistry = new StorageRegistry
{
   DefaultDrive = new LocalMarketDataDrive(_historyPath),
};
_snapshotRegistry = new SnapshotRegistry("Snapshots");
_connector = new Connector(_entityRegistry, _storageRegistry, _snapshotRegistry, supportOffline: true, supportSubscriptionTracking: true);
_connector.StorageAdapter.DaysLoad = TimeSpan.FromDays(3);
...
		
```

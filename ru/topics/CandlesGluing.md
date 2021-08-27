# Склеивание свечей, история + реал\-тайм

Для того, чтобы склеить исторические свечи с реал\-тайм, надо инициализировать соответствующие хранилище торговых объектов [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), хранилище маркет\-данных [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) и реестр хранилищ\-снэпшотов [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry). 

Рассмотрим склеивание на примере [TimeFrameCandle](xref:StockSharp.Algo.Candles.TimeFrameCandle) с использованием тиковых данных, сохраненных с помощью [S\#.Data](Hydra.md):

```cs
private Connector _connector;
private Security security;
private CandleSeries _series;
//объявляем хранилище торговых объектов CsvEntityRegistry и хранилище маркет-данных StorageRegistry
private readonly CsvEntityRegistry _csvEntityRegistry;
private readonly StorageRegistry _storageRegistry;
private readonly SnapshotRegistry _snapshotRegistry;
// Путь к данным истории
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
            //Устанавливаем количество дней для загрузки данных.
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

После этого, в обработчик события CandleSeriesProcessing начнут поступать свечи, начиная с первой исторической и по текущую реал\-тайм:

```cs
        
		private void Connector_CandleSeriesProcessing(CandleSeries candleSeries, Candle candle)
		{
			Chart.Draw(_candleElement, candle);
		}
		
```

Если изначально создаётся [Connector](xref:StockSharp.Algo.Connector), то в инициализации StorageAdapter нет необходимости, достаточно передать хранилище торговых объектов [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), хранилище маркет\-данных [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) и реестр хранилищ\-снэпшотов [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) в конструктор [Connector](xref:StockSharp.Algo.Connector)

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

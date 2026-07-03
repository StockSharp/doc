# 合并K线：历史数据与实时数据

要将历史K线与实时数据衔接起来，需要初始化相应的存储：用于交易对象的 [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry)、用于市场数据的 [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry)，以及快照存储注册表 [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry)。

下面以 Samples/Candles/CombineHistoryRealtime 项目为例进行说明：

## 配置存储和连接器

```cs
public partial class MainWindow
{
	private readonly Connector _connector;
	private const string _connectorFile = "ConnectorFile.json";
	
	// Path to historical data
	private readonly string _pathHistory = Paths.HistoryDataPath;

	private readonly IFileSystem _fileSystem = Paths.FileSystem;
	
	private Subscription _subscription;
	private ChartCandleElement _candleElement;

	private readonly ChannelExecutor _executor;
	
	public MainWindow()
	{
		InitializeComponent();

		_executor = TimeSpan.FromSeconds(1).CreateExecutorAndRun(ex => ex.LogError());
		
		// Initialize storages
		var entityRegistry = new CsvEntityRegistry(_fileSystem, _pathHistory, _executor);
		var storageRegistry = new StorageRegistry
		{
			DefaultDrive = new LocalMarketDataDrive(_fileSystem, _pathHistory)
		};
		
		// Create connector with configured storages
		_connector = new Connector(
			entityRegistry.Securities, 
			entityRegistry.PositionStorage, 
			new InMemoryExchangeInfoProvider(), 
			storageRegistry, 
			new SnapshotRegistry(_fileSystem, "SnapshotRegistry"));
		
		// Register message adapter provider
		ConfigManager.RegisterService<IMessageAdapterProvider>(
			new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));
		
		// Load connector settings if file exists
		if (_fileSystem.FileExists(_connectorFile))
		{
			_connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}
		
		// Set default candle data type (5-minute)
		CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
	}
}
```

## 连接设置

```cs
// Method for configuring connection parameters
private void Setting_Click(object sender, RoutedEventArgs e)
{
	// Call connector configuration window
	if (_connector.Configure(this))
	{
		// Save settings to file
		_connector.Save().Serialize(_fileSystem, _connectorFile);
	}
}

// Method for connecting to trading system
private void Connect_Click(object sender, RoutedEventArgs e)
{
	// Set connector as data source for instrument selection
	SecurityPicker.SecurityProvider = _connector;
	
	// Subscribe to candle reception event
	_connector.CandleReceived += Connector_CandleReceived;
	
	// Connect
	_connector.Connect();
}
```

## 处理K线并将其显示在图表上

```cs
// Handler for candle reception event
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Draw candle on chart
	Chart.Draw(_candleElement, candle);
}
```

## 创建K线订阅

```cs
// Method called when an instrument is selected
private void SecurityPicker_SecuritySelected(Security security)
{
	// Check if instrument is selected
	if (security == null) 
		return;
	
	// Unsubscribe from previous subscription if it exists
	if (_subscription != null) 
		_connector.UnSubscribe(_subscription);
	
	// Create new subscription for selected instrument
	_subscription = new(CandleDataTypeEdit.DataType, security)
	{
		MarketData =
		{
			// Request historical data for last 720 days
			From = DateTime.Today.AddDays(-720),
			
			// Mode: load historical data and build in real-time
			BuildMode = MarketDataBuildModes.LoadAndBuild,
		}
	};
	
	// Configure chart
	Chart.ClearAreas();
	
	// Create chart area and element for displaying candles
	var area = new ChartArea();
	_candleElement = new ChartCandleElement();
	
	// Add area and element to chart
	Chart.AddArea(area);
	
	// Link chart element with subscription for automatic drawing
	Chart.AddElement(area, _candleElement, _subscription);
	
	// Start subscription
	_connector.Subscribe(_subscription);
}
```

## 完整 MainWindow 类示例

```cs
namespace StockSharp.Samples.Candles.CombineHistoryRealtime;

using System;
using System.Windows;

using Ecng.Common;
using Ecng.Serialization;
using Ecng.Configuration;
using Ecng.ComponentModel;
using Ecng.Logging;
using Ecng.IO;

using StockSharp.Configuration;
using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Algo.Storages.Csv;
using StockSharp.BusinessEntities;
using StockSharp.Xaml;
using StockSharp.Messages;
using StockSharp.Xaml.Charting;
using StockSharp.Charting;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
	private readonly Connector _connector;
	private const string _connectorFile = "ConnectorFile.json";

	private readonly string _pathHistory = Paths.HistoryDataPath;
	private readonly IFileSystem _fileSystem = Paths.FileSystem;

	private Subscription _subscription;
	private ChartCandleElement _candleElement;

	private readonly ChannelExecutor _executor;
	
	public MainWindow()
	{
		InitializeComponent();

		_executor = TimeSpan.FromSeconds(1).CreateExecutorAndRun(ex => ex.LogError());

		var entityRegistry = new CsvEntityRegistry(_fileSystem, _pathHistory, _executor);
		var storageRegistry = new StorageRegistry
		{
			DefaultDrive = new LocalMarketDataDrive(_fileSystem, _pathHistory)
		};
		_connector = new Connector(
			entityRegistry.Securities, 
			entityRegistry.PositionStorage, 
			new InMemoryExchangeInfoProvider(), 
			storageRegistry, 
			new SnapshotRegistry(_fileSystem, "SnapshotRegistry"));

		// registering all connectors
		ConfigManager.RegisterService<IMessageAdapterProvider>(
			new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));

		if (_fileSystem.FileExists(_connectorFile))
		{
			_connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}

		CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
	}

	protected override void OnClosed(EventArgs e)
	{
		AsyncHelper.Run(_executor.DisposeAsync);

		base.OnClosed(e);
	}

	private void Setting_Click(object sender, RoutedEventArgs e)
	{
		if (_connector.Configure(this))
		{
			_connector.Save().Serialize(_fileSystem, _connectorFile);
		}
	}

	private void Connect_Click(object sender, RoutedEventArgs e)
	{
		SecurityPicker.SecurityProvider = _connector;
		_connector.CandleReceived += Connector_CandleReceived;
		_connector.Connect();
	}

	private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
	{
		Chart.Draw(_candleElement, candle);
	}

	private void SecurityPicker_SecuritySelected(Security security)
	{
		if (security == null) return;
		if (_subscription != null) _connector.UnSubscribe(_subscription);

		_subscription = new(CandleDataTypeEdit.DataType, security)
		{
			MarketData =
			{
				From = DateTime.Today.AddDays(-720),
				BuildMode = MarketDataBuildModes.LoadAndBuild,
			}
		};

		//-----------------Chart--------------------------------
		Chart.ClearAreas();

		var area = new ChartArea();
		_candleElement = new ChartCandleElement();

		Chart.AddArea(area);
		Chart.AddElement(area, _candleElement, _subscription);

		_connector.Subscribe(_subscription);
	}
}
```

## 示例的主要功能

> [!IMPORTANT]
> 所有使用文件系统的类（[CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry)、[LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive)、[SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry)）都需要在构造函数中传入 `IFileSystem` 实例。标准实现可以使用 `Paths.FileSystem`。`CsvEntityRegistry` 还需要 `ChannelExecutor` 来同步磁盘访问。序列化方法（`Serialize`、`Deserialize`）也接受 `IFileSystem` 作为参数。

1. **创建存储**：
   - [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry) 用于保存实体
   - [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) 配置市场数据存储路径
   - [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) 用于处理快照

2. **创建订阅**：
   - 使用 [Subscription](xref:StockSharp.BusinessEntities.Subscription) 类
   - MarketData 中的 From 参数指定历史数据加载的开始日期
   - 将 BuildMode 设置为 MarketDataBuildModes.LoadAndBuild，自动衔接历史数据和实时数据

3. **图表显示**：
   - 使用 Chart.AddElement 方法将图表元素与订阅关联
   - 收到新K线后自动更新图表

4. **事件处理**：
   - 订阅 CandleReceived 事件以处理接收到的K线
   - 所选交易品种发生变化时取消之前的订阅

## 扩展功能

可以通过添加以下功能扩展此示例：

### 跟踪向实时模式的切换

```cs
// Subscription to the event of transition to real-time mode
_connector.SubscriptionOnline += OnSubscriptionOnline;

// Event handler
private void OnSubscriptionOnline(Subscription subscription)
{
	if (subscription == _subscription)
	{
		this.GuiAsync(() => StatusLabel.Content = "Online mode");
	}
}
```

### 配置历史数据加载时段

```cs
// Setting history loading period
private void SetHistoryPeriod(int days)
{
	if (_subscription != null)
	{
		_connector.UnSubscribe(_subscription);
		
		_subscription.MarketData.From = DateTime.Today.AddDays(-days);
		
		_connector.Subscribe(_subscription);
	}
}
```


### 进一步处理K线

```cs
// Extended candle processing with information output
private void ExtendedCandleProcessing(Subscription subscription, ICandleMessage candle)
{
	// Draw candle on chart
	Chart.Draw(_candleElement, candle);
	
	// Output information about candle to logs
	this.GuiAsync(() => 
	{
		var status = subscription.State == SubscriptionStates.Online ? "Real-time" : "History";
		LogControl.LogMessage($"{status}: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice}");
	});
}
```

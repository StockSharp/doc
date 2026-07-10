# 合并K线：历史数据与实时数据

要将历史K线与实时数据衔接起来，需要初始化相应的存储：用于交易对象的 [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry)、用于市场数据的 [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry)，以及快照存储注册表 [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry)。

下面以 Samples/Candles/CombineHistoryRealtime 项目为例进行说明：

## 配置存储和连接器

```cs
public partial class MainWindow
{
	private readonly Connector _connector;
	private const string _connectorFile = "ConnectorFile.json";
	
	// 历史数据路径
	private readonly string _pathHistory = Paths.HistoryDataPath;

	private readonly IFileSystem _fileSystem = Paths.FileSystem;
	
	private Subscription _subscription;
	private ChartCandleElement _candleElement;

	private readonly ChannelExecutor _executor;
	
	public MainWindow()
	{
		InitializeComponent();

		_executor = TimeSpan.FromSeconds(1).CreateExecutorAndRun(ex => ex.LogError());
		
		// 初始化存储
		var entityRegistry = new CsvEntityRegistry(_fileSystem, _pathHistory, _executor);
		var storageRegistry = new StorageRegistry
		{
			DefaultDrive = new LocalMarketDataDrive(_fileSystem, _pathHistory)
		};
		
		// 使用已配置的存储创建连接器
		_connector = new Connector(
			entityRegistry.Securities, 
			entityRegistry.PositionStorage, 
			new InMemoryExchangeInfoProvider(), 
			storageRegistry, 
			new SnapshotRegistry(_fileSystem, "SnapshotRegistry"));
		
		// 注册消息适配器提供者
		ConfigManager.RegisterService<IMessageAdapterProvider>(
			new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));
		
		// 如果文件存在，则加载连接器设置
		if (_fileSystem.FileExists(_connectorFile))
		{
			_connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}
		
		// 设置默认K线数据类型（5分钟）
		CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
	}
}
```

## 连接设置

```cs
// 配置连接参数的方法
private void Setting_Click(object sender, RoutedEventArgs e)
{
	// 调用连接器配置窗口
	if (_connector.Configure(this))
	{
		// 将设置保存到文件
		_connector.Save().Serialize(_fileSystem, _connectorFile);
	}
}

// 连接到交易系统的方法
private void Connect_Click(object sender, RoutedEventArgs e)
{
	// 将连接器设置为交易品种选择的数据源
	SecurityPicker.SecurityProvider = _connector;
	
	// 订阅 K线接收事件
	_connector.CandleReceived += Connector_CandleReceived;
	
	// Connect
	_connector.Connect();
}
```

## 处理K线并将其显示在图表上

```cs
// K线接收事件处理器
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 在图表上绘制 K线
	Chart.Draw(_candleElement, candle);
}
```

## 创建K线订阅

```cs
// 选择交易品种时调用的方法
private void SecurityPicker_SecuritySelected(Security security)
{
	// 检查是否已选择交易品种
	if (security == null) 
		return;
	
	// 如果存在上一个订阅，则取消订阅
	if (_subscription != null) 
		_connector.UnSubscribe(_subscription);
	
	// 为所选交易品种创建新订阅
	_subscription = new(CandleDataTypeEdit.DataType, security)
	{
		MarketData =
		{
			// 请求最近 720 天的历史数据
			From = DateTime.Today.AddDays(-720),
			
			// 模式：加载历史数据并实时构建
			BuildMode = MarketDataBuildModes.LoadAndBuild,
		}
	};
	
	// 配置图表
	Chart.ClearAreas();
	
	// 创建图表区域和用于显示 K线的元素
	var area = new ChartArea();
	_candleElement = new ChartCandleElement();
	
	// 向图表添加区域和元素
	Chart.AddArea(area);
	
	// 将图表元素与订阅关联以自动绘制
	Chart.AddElement(area, _candleElement, _subscription);
	
	// 启动订阅
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
/// MainWindow.xaml 的交互逻辑
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

		// 注册所有连接器
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

		// -----------------图表--------------------------------
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
// 订阅切换到实时模式的事件
_connector.SubscriptionOnline += OnSubscriptionOnline;

// 事件处理器
private void OnSubscriptionOnline(Subscription subscription)
{
	if (subscription == _subscription)
	{
		this.GuiAsync(() => StatusLabel.Content = "在线模式");
	}
}
```

### 配置历史数据加载时段

```cs
// 设置历史加载期间
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
// 带信息输出的扩展 K线处理
private void ExtendedCandleProcessing(Subscription subscription, ICandleMessage candle)
{
	// 在图表上绘制 K线
	Chart.Draw(_candleElement, candle);
	
	// 将 K线信息输出到日志
	this.GuiAsync(() => 
	{
		var status = subscription.State == SubscriptionStates.Online ? "Real-time" : "History";
		LogControl.LogMessage($"{status}: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice}");
	});
}
```

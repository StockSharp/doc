# Verbindung von Candles: Historie + Echtzeit

Um historische Candles mit Echtzeitdaten zu kombinieren, müssen Sie die entsprechenden Speicher initialisieren: Speicher für Handelsobjekte [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), Speicher für Marktdaten [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) und Snapshot-Speicherregister [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry).

Das Projekt `Samples/Candles/CombineHistoryRealtime` zeigt diese Einrichtung in der Praxis:

## Einrichtung von Speichern und Connector

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

## Verbindungseinrichtung

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

## Verarbeitung von Candles und Anzeige im Chart

```cs
// Handler for candle reception event
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Draw candle on chart
	Chart.Draw(_candleElement, candle);
}
```

## Erstellung eines Candle-Abonnements

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

## Vollständiges Beispiel der MainWindow-Klasse

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

## Merkmale des Beispiels

> [!IMPORTANT]
> Alle Klassen, die mit dem Dateisystem arbeiten ([CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive), [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry)), benötigen eine `IFileSystem`-Instanz im Konstruktor. Verwenden Sie `Paths.FileSystem` für die Standardimplementierung. `CsvEntityRegistry` benötigt außerdem `ChannelExecutor` zur Synchronisierung des Festplattenzugriffs. Die Serialisierungsmethoden (`Serialize`, `Deserialize`) akzeptieren ebenfalls `IFileSystem` als Parameter.

1. **Erstellung von Speichern**:
   - [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry) wird zum Speichern von Entitäten verwendet und benötigt `IFileSystem` und `ChannelExecutor`
   - [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) wird mit dem Pfad zum Speicher konfiguriert
   - [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) wird für die Arbeit mit Snapshots erstellt und benötigt `IFileSystem`

2. **Erstellung des Abonnements**:
   - Die Klasse [Subscription](xref:StockSharp.BusinessEntities.Subscription) wird verwendet
   - Der Parameter From in MarketData gibt das Startdatum für das Laden der Historie an
   - BuildMode = MarketDataBuildModes.LoadAndBuild wird für die automatische Kombination von Historie und Echtzeit gesetzt

3. **Chart-Anzeige**:
   - Die Methode Chart.AddElement wird verwendet, um das Chart-Element mit dem Abonnement zu verknüpfen
   - Das Chart wird automatisch aktualisiert, wenn neue Candles empfangen werden

4. **Ereignisbehandlung**:
   - Abonnement des Ereignisses CandleReceived zur Verarbeitung empfangener Candles
   - Kündigung des vorherigen Abonnements, wenn das ausgewählte Instrument geändert wird

## Erweiterte Funktionen

Sie können dieses Beispiel um folgende Funktionen erweitern:

### Verfolgung des Übergangs in den Echtzeitmodus

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

### Konfiguration des Zeitraums für das Laden der Historie

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

### Zusätzliche Candle-Verarbeitung

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

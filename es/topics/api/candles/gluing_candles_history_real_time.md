# Combinación de velas: historial + tiempo real

Para combinar velas históricas con datos en tiempo real, es necesario inicializar los almacenamientos correspondientes: almacenamiento para objetos de trading [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), almacenamiento para datos de mercado [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) y el registro de almacenamiento de instantáneas [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry).

El proyecto `Samples/Candles/CombineHistoryRealtime` muestra esta configuración en la práctica:

## Configuración de almacenamientos y conector

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

## Configuración de la conexión

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

## Procesamiento de velas y visualización en el gráfico

```cs
// Handler for candle reception event
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Draw candle on chart
	Chart.Draw(_candleElement, candle);
}
```

## Creación de la suscripción a velas

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

## Ejemplo completo de la clase MainWindow

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

## Características del ejemplo

> [!IMPORTANT]
> Todas las clases que trabajan con el sistema de archivos ([CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive), [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry)) requieren una instancia de `IFileSystem` en el constructor. Use `Paths.FileSystem` para la implementación estándar. `CsvEntityRegistry` también requiere `ChannelExecutor` para sincronizar el acceso al disco. Los métodos de serialización (`Serialize`, `Deserialize`) también aceptan `IFileSystem` como parámetro.

1. **Creación de almacenamientos**:
   - [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry) se utiliza para almacenar entidades y requiere `IFileSystem` y `ChannelExecutor`
   - [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) se configura con la ruta al almacenamiento
   - [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) se crea para trabajar con instantáneas y requiere `IFileSystem`

2. **Creación de la suscripción**:
   - Se utiliza la clase [Subscription](xref:StockSharp.BusinessEntities.Subscription)
   - El parámetro From en MarketData especifica la fecha inicial para cargar el historial
   - Se establece BuildMode = MarketDataBuildModes.LoadAndBuild para la combinación automática de historial y tiempo real

3. **Visualización en el gráfico**:
   - Se utiliza el método Chart.AddElement para vincular el elemento del gráfico con la suscripción
   - El gráfico se actualiza automáticamente cuando se reciben nuevas velas

4. **Manejo de eventos**:
   - Suscripción al evento CandleReceived para procesar las velas recibidas
   - Cancelación de la suscripción anterior cuando cambia el instrumento seleccionado

## Capacidades ampliadas

Puede ampliar este ejemplo con las siguientes funciones:

### Seguimiento de la transición al modo en tiempo real

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

### Configuración del período de carga del historial

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

### Procesamiento adicional de velas

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

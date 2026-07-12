# Combinación de velas: historial + tiempo real

Para combinar velas históricas con datos en tiempo real, es necesario inicializar los almacenamientos correspondientes: almacenamiento para objetos de trading [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), almacenamiento para datos de mercado [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) y el registro de almacenamiento de instantáneas [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry).

El proyecto `Samples/Candles/CombineHistoryRealtime` muestra esta configuración en la práctica:

## Configuración de almacenamientos y conector

```cs
public partial class MainWindow
{
	private readonly Connector _connector;
	private const string _connectorFile = "ConnectorFile.json";

	// Ruta a datos históricos
	private readonly string _pathHistory = Paths.HistoryDataPath;

	private readonly IFileSystem _fileSystem = Paths.FileSystem;

	private Subscription _subscription;
	private ChartCandleElement _candleElement;

	private readonly ChannelExecutor _executor;

	public MainWindow()
	{
		InitializeComponent();

		_executor = TimeSpan.FromSeconds(1).CreateExecutorAndRun(ex => ex.LogError());

		// Inicializar almacenamientos
		var entityRegistry = new CsvEntityRegistry(_fileSystem, _pathHistory, _executor);
		var storageRegistry = new StorageRegistry
		{
			DefaultDrive = new LocalMarketDataDrive(_fileSystem, _pathHistory)
		};

		// Crear conector con almacenamientos configurados
		_connector = new Connector(
			entityRegistry.Securities,
			entityRegistry.PositionStorage,
			new InMemoryExchangeInfoProvider(),
			storageRegistry,
			new SnapshotRegistry(_fileSystem, "SnapshotRegistry"));

		// Registrar proveedor de adaptadores de mensajes
		ConfigManager.RegisterService<IMessageAdapterProvider>(
			new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));

		// Cargar ajustes del conector si el archivo existe
		if (_fileSystem.FileExists(_connectorFile))
		{
			_connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}

		// Establecer tipo de datos de vela predeterminado (5 minutos)
		CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
	}
}
```

## Configuración de la conexión

```cs
// Método para configurar parámetros de conexión
private void Setting_Click(object sender, RoutedEventArgs e)
{
	// Abrir ventana de configuración del conector
	if (_connector.Configure(this))
	{
		// Guardar ajustes en archivo
		_connector.Save().Serialize(_fileSystem, _connectorFile);
	}
}

// Método para conectarse al sistema de trading
private void Connect_Click(object sender, RoutedEventArgs e)
{
	// Establecer el conector como fuente de datos para seleccionar instrumentos
	SecurityPicker.SecurityProvider = _connector;

	// Suscribirse al evento de recepción de velas
	_connector.CandleReceived += Connector_CandleReceived;

	// Conectar
	_connector.Connect();
}
```

## Procesamiento de velas y visualización en el gráfico

```cs
// Controlador del evento de recepción de velas
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Dibujar la vela en el gráfico
	Chart.Draw(_candleElement, candle);
}
```

## Creación de la suscripción a velas

```cs
// Método llamado al seleccionar un instrumento
private void SecurityPicker_SecuritySelected(Security security)
{
	// Comprobar si se seleccionó un instrumento
	if (security == null)
		return;

	// Cancelar la suscripción anterior si existe
	if (_subscription != null)
		_connector.UnSubscribe(_subscription);

	// Crear nueva suscripción para el instrumento seleccionado
	_subscription = new(CandleDataTypeEdit.DataType, security)
	{
		MarketData =
		{
			// Solicitar datos históricos de los últimos 720 días
			From = DateTime.Today.AddDays(-720),

			// Modo: cargar datos históricos y construir en tiempo real
			BuildMode = MarketDataBuildModes.LoadAndBuild,
		}
	};

	// Configurar gráfico
	Chart.ClearAreas();

	// Crear área del gráfico y elemento para mostrar velas
	var area = new ChartArea();
	_candleElement = new ChartCandleElement();

	// Añadir área y elemento al gráfico
	Chart.AddArea(area);

	// Vincular elemento del gráfico con la suscripción para dibujo automático
	Chart.AddElement(area, _candleElement, _subscription);

	// Iniciar suscripción
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
/// Lógica de interacción para MainWindow.xaml
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

		// registro de todos los conectores
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

		// -----------------Gráfico--------------------------------
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
// Suscripción al evento de transición al modo de tiempo real
_connector.SubscriptionOnline += OnSubscriptionOnline;

// Controlador de evento
private void OnSubscriptionOnline(Subscription subscription)
{
	if (subscription == _subscription)
	{
		this.GuiAsync(() => StatusLabel.Content = "Modo en línea");
	}
}
```

### Configuración del período de carga del historial

```cs
// Establecer periodo de carga de historial
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
// Procesamiento extendido de velas con salida de información
private void ExtendedCandleProcessing(Subscription subscription, ICandleMessage candle)
{
	// Dibujar la vela en el gráfico
	Chart.Draw(_candleElement, candle);

	// Enviar información de la vela a los registros
	this.GuiAsync(() =>
	{
		var status = subscription.State == SubscriptionStates.Online ? "Tiempo real" : "Historial";
		LogControl.LogMessage($"{status}: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice}");
	});
}
```

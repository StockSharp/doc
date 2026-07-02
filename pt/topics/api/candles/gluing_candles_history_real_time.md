# Colagem de Candles: Histórico + Tempo Real

Para combinar candles históricos com dados em tempo real, você precisa inicializar os armazenamentos apropriados: armazenamento para objetos de negociação [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), armazenamento para dados de mercado [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry), e o registro de armazenamento de snapshots [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry).

O projeto `Samples/Candles/CombineHistoryRealtime` mostra essa configuração na prática:

## Configurando Armazenamentos e Conector

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

## Configuração da Conexão

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

## Processamento de Candles e Exibição no Gráfico

```cs
// Handler for candle reception event
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Draw candle on chart
	Chart.Draw(_candleElement, candle);
}
```

## Criando a Assinatura de Candles

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

## Exemplo Completo da Classe MainWindow

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

## Características do Exemplo

> [!IMPORTANT]
> Todas as classes que trabalham com o sistema de arquivos ([CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive), [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry)) requerem uma instância de `IFileSystem` no construtor. Use `Paths.FileSystem` para a implementação padrão. `CsvEntityRegistry` também requer `ChannelExecutor` para sincronizar o acesso ao disco. Os métodos de serialização (`Serialize`, `Deserialize`) também aceitam `IFileSystem` como parâmetro.

1. **Criação de Armazenamentos**:
   - [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry) é usado para armazenar entidades e requer `IFileSystem` e `ChannelExecutor`
   - [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) é configurado com o caminho para o armazenamento
   - [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) é criado para trabalhar com snapshots e requer `IFileSystem`

2. **Criação da Assinatura**:
   - A classe [Subscription](xref:StockSharp.BusinessEntities.Subscription) é utilizada
   - O parâmetro From em MarketData especifica a data inicial para o carregamento do histórico
   - BuildMode = MarketDataBuildModes.LoadAndBuild é definido para a combinação automática de histórico e tempo real

3. **Exibição no Gráfico**:
   - O método Chart.AddElement é usado para vincular o elemento do gráfico à assinatura
   - O gráfico é atualizado automaticamente quando novos candles são recebidos

4. **Tratamento de Eventos**:
   - Assinatura do evento CandleReceived para processar os candles recebidos
   - Cancelamento da assinatura anterior quando o instrumento selecionado muda

## Recursos Estendidos

Você pode estender este exemplo com as seguintes funções:

### Rastreamento da Transição para o Modo Tempo Real

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

### Configurando o Período de Carregamento do Histórico

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

### Processamento Adicional de Candles

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

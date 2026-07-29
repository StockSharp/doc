# Combinação de velas: histórico + tempo real

Para combinar velas históricas com dados em tempo real, é necessário inicializar os armazenamentos apropriados: armazenamento para objetos de negociação [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), armazenamento para dados de mercado [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry), e o registo de armazenamento de instantâneos [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry).

O projeto `Samples/Candles/CombineHistoryRealtime` mostra essa configuração na prática:

## Configurando Armazenamentos e Conector

```cs
public partial class MainWindow
{
	private readonly Connector _connector;
	private const string _connectorFile = "ConnectorFile.json";

	// Caminho para dados históricos
	private readonly string _pathHistory = Paths.HistoryDataPath;

	private readonly IFileSystem _fileSystem = Paths.FileSystem;

	private Subscription _subscription;
	private ChartCandleElement _candleElement;

	private readonly ChannelExecutor _executor;

	public MainWindow()
	{
		InitializeComponent();

		_executor = TimeSpan.FromSeconds(1).CreateExecutorAndRun(ex => ex.LogError());

		// Inicializar armazenamentos
		var entityRegistry = new CsvEntityRegistry(_fileSystem, _pathHistory, _executor);
		var storageRegistry = new StorageRegistry
		{
			DefaultDrive = new LocalMarketDataDrive(_fileSystem, _pathHistory)
		};

		// Criar conector com armazenamentos configurados
		_connector = new Connector(
			entityRegistry.Securities,
			entityRegistry.PositionStorage,
			new InMemoryExchangeInfoProvider(),
			storageRegistry,
			new SnapshotRegistry(_fileSystem, "SnapshotRegistry"));

		// Registrar provedor de adaptadores de mensagens
		ConfigManager.RegisterService<IMessageAdapterProvider>(
			new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));

		// Carregar configurações do conector se o ficheiro existir
		if (_fileSystem.FileExists(_connectorFile))
		{
			_connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}

		// Definir tipo de dados de vela predefinido (5 minutos)
		CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
	}
}
```

## Configuração da Ligação

```cs
// Método para configurar parâmetros de ligação
private void Setting_Click(object sender, RoutedEventArgs e)
{
	// Abrir janela de configuração do conector
	if (_connector.Configure(this))
	{
		// Guardar configurações num ficheiro
		_connector.Save().Serialize(_fileSystem, _connectorFile);
	}
}

// Método para ligar ao sistema de negociação
private void Connect_Click(object sender, RoutedEventArgs e)
{
	// Definir o conector como fonte de dados para seleção de instrumentos
	SecurityPicker.SecurityProvider = _connector;

	// Assinar o evento de recebimento de velas
	_connector.CandleReceived += Connector_CandleReceived;

	// Ligar
	_connector.Connect();
}
```

## Processamento de velas e exibição no gráfico

```cs
// Manipulador do evento de recebimento de velas
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Desenhar a vela no gráfico
	Chart.Draw(_candleElement, candle);
}
```

## Criar a subscrição de velas

```cs
// Método chamado quando um instrumento é selecionado
private void SecurityPicker_SecuritySelected(Security security)
{
	// Verificar se um instrumento foi selecionado
	if (security == null)
		return;

	// Cancelar a assinatura anterior, se existir
	if (_subscription != null)
		_connector.UnSubscribe(_subscription);

	// Criar nova assinatura para o instrumento selecionado
	_subscription = new(CandleDataTypeEdit.DataType, security)
	{
		MarketData =
		{
			// Solicitar dados históricos dos últimos 720 dias
			From = DateTime.Today.AddDays(-720),

			// Modo: carregar dados históricos e construir em tempo real
			BuildMode = MarketDataBuildModes.LoadAndBuild,
		}
	};

	// Configurar gráfico
	Chart.ClearAreas();

	// Criar área do gráfico e elemento para exibir velas
	var area = new ChartArea();
	_candleElement = new ChartCandleElement();

	// Adicionar área e elemento ao gráfico
	Chart.AddArea(area);

	// Vincular elemento do gráfico à assinatura para desenho automático
	Chart.AddElement(area, _candleElement, _subscription);

	// Iniciar assinatura
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
/// Lógica de interação para MainWindow.xaml
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

		// registro de todos os conectores
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

## Características do Exemplo

> [!IMPORTANT]
> Todas as classes que trabalham com o sistema de ficheiros ([CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), [LocalMarketDataDrive](xref:StockSharp.Algo.Storages.LocalMarketDataDrive), [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry)) requerem uma instância de `IFileSystem` no construtor. Use `Paths.FileSystem` para a implementação padrão. `CsvEntityRegistry` também requer `ChannelExecutor` para sincronizar o acesso ao disco. Os métodos de serialização (`Serialize`, `Deserialize`) também aceitam `IFileSystem` como parâmetro.

1. **Criação de Armazenamentos**:
   - [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry) é usado para armazenar entidades e requer `IFileSystem` e `ChannelExecutor`
   - [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) é configurado com o caminho para o armazenamento
   - [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) é criado para trabalhar com instantâneos e requer `IFileSystem`

2. **Criação da Assinatura**:
   - A classe [Subscription](xref:StockSharp.BusinessEntities.Subscription) é utilizada
   - O parâmetro From em MarketData especifica a data inicial para o carregamento do histórico
   - BuildMode = MarketDataBuildModes.LoadAndBuild é definido para a combinação automática de histórico e tempo real

3. **Exibição no Gráfico**:
   - O método Chart.AddElement é usado para vincular o elemento do gráfico à assinatura
   - O gráfico é atualizado automaticamente quando novas velas são recebidas

4. **Tratamento de Eventos**:
- Assinatura do evento CandleReceived para processar as velas recebidas
   - Cancelamento da assinatura anterior quando o instrumento selecionado muda

## Recursos Estendidos

Pode estender este exemplo com as seguintes funções:

### Rastreamento da Transição para o Modo Tempo Real

```cs
// Assinatura do evento de transição para o modo em tempo real
_connector.SubscriptionOnline += OnSubscriptionOnline;

// Manipulador de evento
private void OnSubscriptionOnline(Subscription subscription)
{
	if (subscription == _subscription)
	{
		this.GuiAsync(() => StatusLabel.Content = "Modo online");
	}
}
```

### Configurando o Período de Carregamento do Histórico

```cs
// Definir período de carregamento do histórico
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

### Processamento adicional de velas

```cs
// Processamento estendido de velas com saída de informações
private void ExtendedCandleProcessing(Subscription subscription, ICandleMessage candle)
{
	// Desenhar a vela no gráfico
	Chart.Draw(_candleElement, candle);

	// Enviar informações da vela para os registos
	this.GuiAsync(() =>
	{
		var status = subscription.State == SubscriptionStates.Online ? "Tempo real" : "Histórico";
		LogControl.LogMessage($"{status}: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice}");
	});
}
```

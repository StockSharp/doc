# Verbindung von Kerzen: Historie + Echtzeit

Um historische Kerzen mit Echtzeitdaten zu kombinieren, müssen Sie die entsprechenden Speicher initialisieren: Speicher für Handelsobjekte [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), Speicher für Marktdaten [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) und Momentaufnahmen-Speicherregister [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry).

Das Projekt `Samples/Candles/CombineHistoryRealtime` zeigt diese Einrichtung in der Praxis:

## Einrichtung von Speichern und Connector

```cs
public partial class MainWindow
{
	private readonly Connector _connector;
	private const string _connectorFile = "ConnectorFile.json";

	// Pfad zu historischen Daten
	private readonly string _pathHistory = Paths.HistoryDataPath;

	private readonly IFileSystem _fileSystem = Paths.FileSystem;

	private Subscription _subscription;
	private ChartCandleElement _candleElement;

	private readonly ChannelExecutor _executor;

	public MainWindow()
	{
		InitializeComponent();

		_executor = TimeSpan.FromSeconds(1).CreateExecutorAndRun(ex => ex.LogError());

		// Speicher initialisieren
		var entityRegistry = new CsvEntityRegistry(_fileSystem, _pathHistory, _executor);
		var storageRegistry = new StorageRegistry
		{
			DefaultDrive = new LocalMarketDataDrive(_fileSystem, _pathHistory)
		};

		// Connector mit konfigurierten Speichern erstellen
		_connector = new Connector(
			entityRegistry.Securities,
			entityRegistry.PositionStorage,
			new InMemoryExchangeInfoProvider(),
			storageRegistry,
			new SnapshotRegistry(_fileSystem, "SnapshotRegistry"));

		// Provider für Nachrichtenadapter registrieren
		ConfigManager.RegisterService<IMessageAdapterProvider>(
			new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));

		// Connector-Einstellungen laden, falls die Datei existiert
		if (_fileSystem.FileExists(_connectorFile))
		{
			_connector.Load(_connectorFile.Deserialize<SettingsStorage>(_fileSystem));
		}

		// Standard-Kerzdatentyp festlegen (5 Minuten)
		CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
	}
}
```

## Verbindungseinrichtung

```cs
// Methode zum Konfigurieren der Verbindungsparameter
private void Setting_Click(object sender, RoutedEventArgs e)
{
	// Connector-Konfigurationsfenster öffnen
	if (_connector.Configure(this))
	{
		// Einstellungen in Datei speichern
		_connector.Save().Serialize(_fileSystem, _connectorFile);
	}
}

// Methode zum Verbinden mit dem Handelssystem
private void Connect_Click(object sender, RoutedEventArgs e)
{
	// Connector als Datenquelle für die Instrumentenauswahl festlegen
	SecurityPicker.SecurityProvider = _connector;

	// Ereignis zum Empfang von Kerzen abonnieren
	_connector.CandleReceived += Connector_CandleReceived;

	// Verbinden
	_connector.Connect();
}
```

## Verarbeitung von Kerzen und Anzeige im Chart

```cs
// Handler für das Kerzenempfangsereignis
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Kerze im Diagramm zeichnen
	Chart.Draw(_candleElement, candle);
}
```

## Erstellung eines Kerzenabonnements

```cs
// Methode, die bei Auswahl eines Instruments aufgerufen wird
private void SecurityPicker_SecuritySelected(Security security)
{
	// Prüfen, ob ein Instrument ausgewählt ist
	if (security == null)
		return;

	// Vorheriges Abonnement abbestellen, falls vorhanden
	if (_subscription != null)
		_connector.UnSubscribe(_subscription);

	// Neues Abonnement für das ausgewählte Instrument erstellen
	_subscription = new(CandleDataTypeEdit.DataType, security)
	{
		MarketData =
		{
			// Historische Daten für die letzten 720 Tage anfordern
			From = DateTime.Today.AddDays(-720),

			// Modus: historische Daten laden und in Echtzeit erstellen
			BuildMode = MarketDataBuildModes.LoadAndBuild,
		}
	};

	// Diagramm konfigurieren
	Chart.ClearAreas();

	// Diagrammbereich und Element zur Anzeige von Kerzen erstellen
	var area = new ChartArea();
	_candleElement = new ChartCandleElement();

	// Bereich und Element zum Diagramm hinzufügen
	Chart.AddArea(area);

	// Diagrammelement für automatisches Zeichnen mit Abonnement verknüpfen
	Chart.AddElement(area, _candleElement, _subscription);

	// Abonnement starten
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
/// Interaktionslogik für MainWindow.xaml
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

		// alle Connectoren registrieren
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

		// -----------------Diagramm--------------------------------
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
   - [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) wird für die Arbeit mit Momentaufnahmen erstellt und benötigt `IFileSystem`

2. **Erstellung des Abonnements**:
   - Die Klasse [Subscription](xref:StockSharp.BusinessEntities.Subscription) wird verwendet
   - Der Parameter From in MarketData gibt das Startdatum für das Laden der Historie an
   - BuildMode = MarketDataBuildModes.LoadAndBuild wird für die automatische Kombination von Historie und Echtzeit gesetzt

3. **Chart-Anzeige**:
   - Die Methode Chart.AddElement wird verwendet, um das Chart-Element mit dem Abonnement zu verknüpfen
   - Das Chart wird automatisch aktualisiert, wenn neue Kerzen empfangen werden

4. **Ereignisbehandlung**:
   - Abonnement des Ereignisses CandleReceived zur Verarbeitung empfangener Kerzen
   - Aufheben des vorherigen Abonnements, wenn das ausgewählte Instrument geändert wird

## Erweiterte Funktionen

Sie können dieses Beispiel um folgende Funktionen erweitern:

### Verfolgung des Übergangs in den Echtzeitmodus

```cs
// Abonnement für das Ereignis des Übergangs in den Echtzeitmodus
_connector.SubscriptionOnline += OnSubscriptionOnline;

// Ereignishandler
private void OnSubscriptionOnline(Subscription subscription)
{
	if (subscription == _subscription)
	{
		this.GuiAsync(() => StatusLabel.Content = "Online-Modus");
	}
}
```

### Konfiguration des Zeitraums für das Laden der Historie

```cs
// Zeitraum zum Laden der Historie festlegen
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

### Zusätzliche Kerzenverarbeitung

```cs
// Erweiterte Kerzenverarbeitung mit Informationsausgabe
private void ExtendedCandleProcessing(Subscription subscription, ICandleMessage candle)
{
	// Kerze im Diagramm zeichnen
	Chart.Draw(_candleElement, candle);

	// Informationen zur Kerze in Protokolle ausgeben
	this.GuiAsync(() =>
	{
		var status = subscription.State == SubscriptionStates.Online ? "Echtzeit" : "Historie";
		LogControl.LogMessage($"{status}: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice}");
	});
}
```

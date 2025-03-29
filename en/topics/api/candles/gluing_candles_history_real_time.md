# Gluing Candles: History + Real-Time

To combine historical candles with real-time data, you need to initialize the appropriate storages: storage for trading objects [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry), storage for market data [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry), and snapshot storage registry [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry).

Let's look at an example from the Samples/Candles/CombineHistoryRealtime project:

## Setting Up Storages and Connector

```cs
public partial class MainWindow
{
    private readonly Connector _connector;
    private const string _connectorFile = "ConnectorFile.json";
    
    // Path to historical data
    private readonly string _pathHistory = Paths.HistoryDataPath;
    
    private Subscription _subscription;
    private ChartCandleElement _candleElement;
    
    public MainWindow()
    {
        InitializeComponent();
        
        // Initialize storages
        var entityRegistry = new CsvEntityRegistry(_pathHistory);
        var storageRegistry = new StorageRegistry
        {
            DefaultDrive = new LocalMarketDataDrive(_pathHistory)
        };
        
        // Create connector with configured storages
        _connector = new Connector(
            entityRegistry.Securities, 
            entityRegistry.PositionStorage, 
            new InMemoryExchangeInfoProvider(), 
            storageRegistry, 
            new SnapshotRegistry("SnapshotRegistry"));
        
        // Register message adapter provider
        ConfigManager.RegisterService<IMessageAdapterProvider>(
            new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));
        
        // Load connector settings if file exists
        if (File.Exists(_connectorFile))
        {
            _connector.Load(_connectorFile.Deserialize<SettingsStorage>());
        }
        
        // Set default candle data type (5-minute)
        CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
    }
}
```

## Connection Setup

```cs
// Method for configuring connection parameters
private void Setting_Click(object sender, RoutedEventArgs e)
{
    // Call connector configuration window
    if (_connector.Configure(this))
    {
        // Save settings to file
        _connector.Save().Serialize(_connectorFile);
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

## Processing Candles and Displaying on Chart

```cs
// Handler for candle reception event
private void Connector_CandleReceived(Subscription subscription, ICandleMessage candle)
{
    // Draw candle on chart
    Chart.Draw(_candleElement, candle);
}
```

## Creating Candle Subscription

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

## Complete MainWindow Class Example

```cs
namespace StockSharp.Samples.Candles.CombineHistoryRealtime;

using System;
using System.Windows;
using System.IO;

using Ecng.Common;
using Ecng.Serialization;
using Ecng.Configuration;

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

    private Subscription _subscription;
    private ChartCandleElement _candleElement;
    
    public MainWindow()
    {
        InitializeComponent();
        var entityRegistry = new CsvEntityRegistry(_pathHistory);
        var storageRegistry = new StorageRegistry
        {
            DefaultDrive = new LocalMarketDataDrive(_pathHistory)
        };
        _connector = new Connector(
            entityRegistry.Securities, 
            entityRegistry.PositionStorage, 
            new InMemoryExchangeInfoProvider(), 
            storageRegistry, 
            new SnapshotRegistry("SnapshotRegistry"));

        // registering all connectors
        ConfigManager.RegisterService<IMessageAdapterProvider>(
            new InMemoryMessageAdapterProvider(_connector.Adapter.InnerAdapters));

        if (File.Exists(_connectorFile))
        {
            _connector.Load(_connectorFile.Deserialize<SettingsStorage>());
        }

        CandleDataTypeEdit.DataType = TimeSpan.FromMinutes(5).TimeFrame();
    }

    private void Setting_Click(object sender, RoutedEventArgs e)
    {
        if (_connector.Configure(this))
        {
            _connector.Save().Serialize(_connectorFile);
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

## Example Features

1. **Creating Storages**:
   - [CsvEntityRegistry](xref:StockSharp.Algo.Storages.Csv.CsvEntityRegistry) is used for storing entities
   - [StorageRegistry](xref:StockSharp.Algo.Storages.StorageRegistry) is configured with path to storage
   - [SnapshotRegistry](xref:StockSharp.Algo.Storages.SnapshotRegistry) is created for working with snapshots

2. **Creating Subscription**:
   - The class [Subscription](xref:StockSharp.BusinessEntities.Subscription) is used
   - The From parameter in MarketData specifies the initial date for loading history
   - BuildMode = MarketDataBuildModes.LoadAndBuild is set for automatic combination of history and real-time

3. **Chart Display**:
   - Chart.AddElement method is used to link chart element with subscription
   - The chart is automatically updated when new candles are received

4. **Event Handling**:
   - Subscription to CandleReceived event for processing received candles
   - Unsubscribing from previous subscription when selected instrument changes

## Extended Capabilities

This example can be extended by adding the following functions:

### Tracking Transition to Real-Time Mode

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

### Configuring History Loading Period

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

### Saving Received Data

```cs
// Method for saving received data
private void SaveReceivedData()
{
    if (_connector.StorageAdapter != null)
    {
        // Force save cached data to disk
        _connector.StorageAdapter.Flush();
    }
}
```

### Additional Candle Processing

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
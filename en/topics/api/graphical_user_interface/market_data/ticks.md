# Tick Trades

![GUI TradeGrid](../../../../images/gui_tradegrid.png)

[TradeGrid](xref:StockSharp.Xaml.TradeGrid) - a trade table.

**Main properties**

- [TradeGrid.Trades](xref:StockSharp.Xaml.TradeGrid.Trades) - list of trades.
- [TradeGrid.SelectedTrade](xref:StockSharp.Xaml.TradeGrid.SelectedTrade) - selected trade.
- [TradeGrid.SelectedTrades](xref:StockSharp.Xaml.TradeGrid.SelectedTrades) - selected trades.

Below are code fragments demonstrating its usage:

```xaml
<Window x:Class="Sample.TradesWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
    xmlns:xaml="http://schemas.stocksharp.com/xaml"
    Title="{x:Static loc:LocalizedStrings.Str985}" Height="284" Width="544">
    <xaml:TradeGrid x:Name="TradeGrid" x:FieldModifier="public" />
</Window>
```

```cs
public class TradesWindow
{
    private readonly Connector _connector;
    private readonly Security _security;
    private Subscription _tickSubscription;
    
    public TradesWindow(Connector connector, Security security)
    {
        InitializeComponent();
        
        _connector = connector;
        _security = security;
        
        // Subscribe to tick trade reception event
        _connector.TickTradeReceived += OnTickReceived;
        
        // Create a subscription to tick trades
        _tickSubscription = new Subscription(DataType.Ticks, security);
        
        // Start subscription
        _connector.Subscribe(_tickSubscription);
    }
    
    // Handler for tick trade reception event
    private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
    {
        // Check if the trade belongs to our subscription
        if (subscription != _tickSubscription)
            return;
            
        // Add the trade to TradeGrid in the user interface thread
        this.GuiAsync(() => TradeGrid.Trades.Add(tick));
    }
    
    // Method for unsubscribing when the window is closed
    public void Unsubscribe()
    {
        if (_tickSubscription != null)
        {
            _connector.TickTradeReceived -= OnTickReceived;
            _connector.UnSubscribe(_tickSubscription);
            _tickSubscription = null;
        }
    }
}
```

### Displaying own trades

```cs
public class MyTradesWindow
{
    private readonly Connector _connector;
    
    public MyTradesWindow(Connector connector)
    {
        InitializeComponent();
        
        _connector = connector;
        
        // Subscribe to own trade reception event
        _connector.OwnTradeReceived += OnOwnTradeReceived;
        
        // Create a subscription to transaction data
        var myTradesSubscription = new Subscription(DataType.Transactions, null);
        
        // Start subscription
        _connector.Subscribe(myTradesSubscription);
    }
    
    // Handler for own trade reception event
    private void OnOwnTradeReceived(Subscription subscription, MyTrade myTrade)
    {
        // Add own trade to TradeGrid in the user interface thread
        this.GuiAsync(() => TradeGrid.Trades.Add(myTrade));
    }
}
```

### Getting historical tick trades

```cs
// Method for getting historical tick trades
public void LoadHistoricalTicks(Security security, DateTime from, DateTime to)
{
    // Clear current trades
    TradeGrid.Trades.Clear();
    
    // Create a subscription to historical tick trades
    var historySubscription = new Subscription(DataType.Ticks, security)
    {
        MarketData =
        {
            // Specify time period for historical data
            From = from,
            To = to
        }
    };
    
    // Subscribe to tick trade reception event
    _connector.TickTradeReceived += OnHistoricalTickReceived;
    
    // Start subscription
    _connector.Subscribe(historySubscription);
}

// Handler for historical tick trade reception event
private void OnHistoricalTickReceived(Subscription subscription, ITickTradeMessage tick)
{
    // Add tick to TradeGrid in the user interface thread
    this.GuiAsync(() => 
    {
        TradeGrid.Trades.Add(tick);
        
        // Update statistics
        UpdateTradeStatistics();
    });
}

// Method for updating trade statistics
private void UpdateTradeStatistics()
{
    int totalTrades = TradeGrid.Trades.Count;
    decimal totalVolume = TradeGrid.Trades.Sum(t => t.Volume);
    decimal averagePrice = TradeGrid.Trades.Any() 
        ? TradeGrid.Trades.Average(t => t.Price)
        : 0;
    
    // Update interface statistics elements
    TotalTradesLabel.Content = $"Total trades: {totalTrades}";
    TotalVolumeLabel.Content = $"Total volume: {totalVolume}";
    AveragePriceLabel.Content = $"Average price: {averagePrice:F2}";
}
```

### Filtering trades by volume

```cs
// Method for filtering trades by minimum volume
public void FilterTicksByVolume(decimal minVolume)
{
    // Save filter value
    _minVolumeFilter = minVolume;
    
    // Update tick trade reception event handler
    _connector.TickTradeReceived -= OnTickReceived;
    _connector.TickTradeReceived += OnFilteredTickReceived;
}

// Handler for tick trade reception event with volume filtering
private void OnFilteredTickReceived(Subscription subscription, ITickTradeMessage tick)
{
    // Check if the trade belongs to the selected instrument
    if (tick.SecurityId != _security.ToSecurityId())
        return;
        
    // Apply volume filter
    if (tick.Volume < _minVolumeFilter)
        return;
        
    // Add the trade to TradeGrid in the user interface thread
    this.GuiAsync(() => TradeGrid.Trades.Add(tick));
    
    // If it's a large trade, you can highlight it or send a notification
    if (tick.Volume >= _largeVolumeThreshold)
    {
        NotifyLargeVolumeTrade(tick);
    }
}

// Method for large trade notification
private void NotifyLargeVolumeTrade(ITickTradeMessage tick)
{
    // Output information about the large trade
    Console.WriteLine($"Large trade: {tick.SecurityId}, {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
    
    // You can add sound or visual notification
    this.GuiAsync(() => 
    {
        // Example of visual highlighting in the list
        var tradeItem = TradeGrid.Trades.LastOrDefault();
        if (tradeItem != null)
        {
            TradeGrid.SelectedTrade = tradeItem;
            HighlightTrade(tradeItem);
        }
    });
}
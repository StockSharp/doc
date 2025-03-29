# Order Log

![GUI orderlog](../../../../images/gui_orderlog.png)

[OrderLogGrid](xref:StockSharp.Xaml.OrderLogGrid) - a graphical component for displaying the order log ([OrderLogItem](xref:StockSharp.BusinessEntities.OrderLogItem)).

**Main properties and methods**

- [OrderLogGrid.LogItems](xref:StockSharp.Xaml.OrderLogGrid.LogItems) - list of order log items.
- [OrderLogGrid.SelectedLogItem](xref:StockSharp.Xaml.OrderLogGrid.SelectedLogItem) - selected order log item.
- [OrderLogGrid.SelectedLogItems](xref:StockSharp.Xaml.OrderLogGrid.SelectedLogItems) - selected order log items.

Below are code fragments demonstrating its usage:

```xaml
<Window x:Class="SampleITCH.OrdersLogWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
        xmlns:xaml="http://schemas.stocksharp.com/xaml"
        Title="{x:Static loc:LocalizedStrings.OrderLog}" Height="750" Width="900">
    <xaml:OrderLogGrid x:Name="OrderLogGrid" x:FieldModifier="public" />
</Window>
```

```cs
public class OrderLogWindow
{
    private readonly Connector _connector;
    private readonly Security _security;
    private Subscription _orderLogSubscription;
    
    public OrderLogWindow(Connector connector, Security security)
    {
        InitializeComponent();
        
        _connector = connector;
        _security = security;
        
        // Subscribe to order log item reception event
        _connector.OrderLogItemReceived += OnOrderLogItemReceived;
        
        // Create a subscription to order log
        _orderLogSubscription = new Subscription(DataType.OrderLog, security);
        
        // Start subscription
        _connector.Subscribe(_orderLogSubscription);
    }
    
    // Handler for order log item reception event
    private void OnOrderLogItemReceived(Subscription subscription, OrderLogItem item)
    {
        // Check if the log item belongs to our subscription
        if (subscription != _orderLogSubscription)
            return;
            
        // Add the item to OrderLogGrid in the user interface thread
        this.GuiAsync(() => OrderLogGrid.LogItems.Add(item));
    }
    
    // Method for unsubscribing when the window is closed
    public void Unsubscribe()
    {
        if (_orderLogSubscription != null)
        {
            _connector.OrderLogItemReceived -= OnOrderLogItemReceived;
            _connector.UnSubscribe(_orderLogSubscription);
            _orderLogSubscription = null;
        }
    }
}
```

### Order log filtering

```cs
// Creating a subscription to order log with filtering
public void SubscribeOrderLog(Security security, DateTime from, DateTime to)
{
    // Create a subscription to order log
    var orderLogSubscription = new Subscription(DataType.OrderLog, security)
    {
        MarketData =
        {
            // Specify time period for historical data
            From = from,
            To = to
        }
    };
    
    // Subscribe to order log item reception event
    _connector.OrderLogItemReceived += OnFilteredOrderLogItemReceived;
    
    // Start subscription
    _connector.Subscribe(orderLogSubscription);
}

// Handler for order log item reception event with filtering
private void OnFilteredOrderLogItemReceived(Subscription subscription, OrderLogItem item)
{
    // Check subscription type
    if (subscription.DataType != DataType.OrderLog)
        return;
        
    // Filter by price (example)
    if (item.Price < _minPrice || item.Price > _maxPrice)
        return;
        
    // Add the item to OrderLogGrid in the user interface thread
    this.GuiAsync(() => 
    {
        OrderLogGrid.LogItems.Add(item);
        
        // Limit the number of displayed items
        while (OrderLogGrid.LogItems.Count > _maxItems)
            OrderLogGrid.LogItems.RemoveAt(0);
    });
}
```

### Order log dynamics analysis

```cs
// Class for analyzing order log dynamics
public class OrderLogAnalyzer
{
    private readonly Connector _connector;
    private readonly Security _security;
    private readonly OrderLogGrid _orderLogGrid;
    
    // Counters for analysis
    private int _buyCount = 0;
    private int _sellCount = 0;
    private decimal _buyVolume = 0;
    private decimal _sellVolume = 0;
    
    public OrderLogAnalyzer(Connector connector, Security security, OrderLogGrid orderLogGrid)
    {
        _connector = connector;
        _security = security;
        _orderLogGrid = orderLogGrid;
        
        // Subscribe to order log item reception event
        _connector.OrderLogItemReceived += OnOrderLogItemReceived;
        
        // Create a subscription to order log
        var subscription = new Subscription(DataType.OrderLog, security);
        
        // Start subscription
        _connector.Subscribe(subscription);
    }
    
    // Handler for order log item reception event
    private void OnOrderLogItemReceived(Subscription subscription, OrderLogItem item)
    {
        if (item.SecurityId != _security.ToSecurityId())
            return;
            
        // Analyze order log item
        if (item.Side == Sides.Buy)
        {
            _buyCount++;
            _buyVolume += item.Volume;
        }
        else if (item.Side == Sides.Sell)
        {
            _sellCount++;
            _sellVolume += item.Volume;
        }
        
        // Update interface with analysis results
        this.GuiAsync(() => 
        {
            // Add item to OrderLogGrid
            _orderLogGrid.LogItems.Add(item);
            
            // Update statistics
            UpdateStatistics();
        });
    }
    
    // Update statistics
    private void UpdateStatistics()
    {
        BuyCountLabel.Content = $"Buys: {_buyCount}";
        SellCountLabel.Content = $"Sells: {_sellCount}";
        BuyVolumeLabel.Content = $"Buy volume: {_buyVolume}";
        SellVolumeLabel.Content = $"Sell volume: {_sellVolume}";
        
        // Calculate imbalance
        var volumeImbalance = _buyVolume - _sellVolume;
        var imbalancePercent = (_buyVolume + _sellVolume) > 0 
            ? volumeImbalance / (_buyVolume + _sellVolume) * 100 
            : 0;
            
        ImbalanceLabel.Content = $"Imbalance: {volumeImbalance:F2} ({imbalancePercent:F2}%)";
    }
}
```
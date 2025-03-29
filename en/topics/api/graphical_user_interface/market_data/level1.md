# Level1

![GUI Leve1Grid](../../../../images/gui_leve1grid.png)

[Level1Grid](xref:StockSharp.Xaml.Level1Grid) - a table for displaying Level1 fields. This table uses data in the form of [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) messages.

**Main properties**

- [Level1Grid.MaxCount](xref:StockSharp.Xaml.Level1Grid.MaxCount) - maximum number of messages to display.
- [Level1Grid.Messages](xref:StockSharp.Xaml.Level1Grid.Messages) - list of messages added to the table.
- [Level1Grid.SelectedMessage](xref:StockSharp.Xaml.Level1Grid.SelectedMessage) - selected message.
- [Level1Grid.SelectedMessages](xref:StockSharp.Xaml.Level1Grid.SelectedMessages) - selected messages.

Below are code fragments demonstrating its usage:

```xaml
<Window x:Class="Membrane02.Level1Window"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:sx="http://schemas.stocksharp.com/xaml"
        xmlns:local="clr-namespace:Membrane02"
        mc:Ignorable="d"
        Title="Level1Window" Height="300" Width="300" Closing="Window_Closing">
    <Grid>
        <sx:Level1Grid x:Name="Level1Grid" />
    </Grid>
</Window>
```

```cs
public Level1Window()
{
    InitializeComponent();
    _connector = MainWindow.This.Connector;
    
    // Subscribe to Level1 data reception event
    _connector.Level1Received += OnLevel1Received;
    
    // Create a subscription to Level1 data if not already subscribed
    var security = MainWindow.This.SelectedSecurity;
    if (!_connector.Subscriptions.Any(s => 
            s.DataType == DataType.Level1 && 
            s.SecurityId == security.ToSecurityId()))
    {
        var subscription = new Subscription(DataType.Level1, security);
        _connector.Subscribe(subscription);
    }
}

private void OnLevel1Received(Subscription subscription, Level1ChangeMessage level1Message)
{
    // Check if the message belongs to the selected instrument
    if (level1Message.SecurityId != MainWindow.This.SelectedSecurity.ToSecurityId())
        return;
        
    // Add the message to Level1Grid
    this.GuiAsync(() => Level1Grid.Messages.Add(level1Message));
}

private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
{
    // Unsubscribe from events when the window is closing
    if (_connector != null)
        _connector.Level1Received -= OnLevel1Received;
}
```

### Recommended way to process Level1 data

```cs
// Creating a subscription to Level1 using multiple instruments
public void SubscribeToLevel1(IEnumerable<Security> securities)
{
    foreach (var security in securities)
    {
        var subscription = new Subscription(DataType.Level1, security);
        _connector.Subscribe(subscription);
    }
    
    // Subscribe to Level1 data reception event
    _connector.Level1Received += OnLevel1Received;
}

// Handler for Level1 data reception event
private void OnLevel1Received(Subscription subscription, Level1ChangeMessage level1Message)
{
    // Get the instrument corresponding to the received message
    var security = _connector.LookupById(level1Message.SecurityId);
    if (security == null)
        return;
        
    // Check if we need to process this particular message
    if (IsSecurityNeeded(security))
    {
        // Update GUI in the user interface thread
        this.GuiAsync(() => 
        {
            // Add the message to Level1Grid
            Level1Grid.Messages.Add(level1Message);
            
            // Process changes in Level1 fields
            foreach (var change in level1Message.Changes)
            {
                switch (change.Key)
                {
                    case Level1Fields.LastTradePrice:
                        // Process last trade price change
                        var lastPrice = (decimal)change.Value;
                        Console.WriteLine($"Last price {security.Code}: {lastPrice}");
                        break;
                        
                    case Level1Fields.BestBidPrice:
                        // Process best bid price change
                        var bestBid = (decimal)change.Value;
                        Console.WriteLine($"Best bid {security.Code}: {bestBid}");
                        break;
                        
                    case Level1Fields.BestAskPrice:
                        // Process best ask price change
                        var bestAsk = (decimal)change.Value;
                        Console.WriteLine($"Best ask {security.Code}: {bestAsk}");
                        break;
                }
            }
        });
    }
}
```
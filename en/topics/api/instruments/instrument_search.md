# Instrument Search

Most connectors to US stock exchanges (for example, [Interactive Brokers](../connectors/stock_market/interactive_brokers.md), [PolygonIO](../connectors/stock_market/polygonio.md), and others) do not transfer all available instruments to the client after establishing a connection through the [IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect) method. This is due to the large number of instruments traded on American exchanges and is done to reduce the load on broker servers and data sources.

## Instrument Search Basics

For searching instruments in S#, a subscription mechanism is used, similar to receiving market data. This approach allows using uniform code for all types of data, including instruments.

### Creating a Subscription for Instrument Search

To search for instruments, you need to create an instance of the [Subscription](xref:StockSharp.BusinessEntities.Subscription) class based on the [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) message, which contains filtering parameters:

```csharp
// Create a filter object for search
var lookupMessage = new SecurityLookupMessage
{
    // Set search criteria
    SecurityId = new SecurityId
    {
        // Search by instrument code (you can use a mask like "AAPL*")
        SecurityCode = "AAPL",
        // Optionally, you can specify the board code
        BoardCode = "NASDAQ"
    },
    // You can specify the instrument type
    SecurityType = SecurityTypes.Stock,
    // Set transaction ID
    TransactionId = Connector.TransactionIdGenerator.GetNextId()
};

// Create a subscription for instrument search
var subscription = new Subscription(lookupMessage);
```

### Possible Filtering Parameters

The [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) message allows setting the following search criteria:

- **SecurityId** — instrument identifier, containing:
  - **SecurityCode** — code or mask of the instrument code (for example, "AAPL" or "MS*")
  - **BoardCode** — exchange board code (for example, [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq))
- **SecurityType** — instrument type ([SecurityTypes.Stock](xref:StockSharp.Messages.SecurityTypes.Stock), [SecurityTypes.Future](xref:StockSharp.Messages.SecurityTypes.Future), etc.)
- **SecurityTypes** — array of instrument types for advanced search
- **Currency** — trading currency of the instrument
- **ExpiryDate** — expiration date (for derivatives)
- **Strike** — strike price (for options)
- **OptionType** — option type (for options)
- **Name** — instrument name or part of it
- **Class** — instrument class

### Processing Search Results

After creating the subscription, you need to subscribe to events for receiving instruments and send the request:

```csharp
// Handler for instrument receiving event
private void OnSecurityReceived(Subscription subscription, Security security)
{
    if (subscription.SubscriptionMessage is not SecurityLookupMessage)
        return;
        
    Console.WriteLine($"Found instrument: {security.Id} - {security.Name}, Type: {security.Type}");
    
    // Here you can add the instrument to a collection or perform other actions
    Securities.Add(security);
}

// Handler for search completion event
private void OnSubscriptionFinished(Subscription subscription)
{
    if (subscription.SubscriptionMessage is not SecurityLookupMessage)
        return;
        
    Console.WriteLine($"Search completed. Instruments found: {Securities.Count}");
}

// Subscription error handler
private void OnSubscriptionFailed(Subscription subscription, Exception error, bool isSubscribe)
{
    if (subscription.SubscriptionMessage is not SecurityLookupMessage)
        return;
        
    Console.WriteLine($"Instrument search error: {error.Message}");
}

// Subscribe to events
Connector.SecurityReceived += OnSecurityReceived;
Connector.SubscriptionFinished += OnSubscriptionFinished;
Connector.SubscriptionFailed += OnSubscriptionFailed;

// Send the instrument search request
Connector.Subscribe(subscription);
```

### Complete Example of Instrument Search

Below is a complete example of a method for searching instruments:

```csharp
public void FindSecurities(string searchCode, SecurityTypes? securityType = null)
{
    // Create an object for instrument search
    var lookupMessage = new SecurityLookupMessage
    {
        SecurityId = new SecurityId
        {
            SecurityCode = searchCode,
            // If you need to search on a specific board
            // BoardCode = ExchangeBoard.Nyse.Code,
        },
        SecurityType = securityType,
        TransactionId = Connector.TransactionIdGenerator.GetNextId()
    };
    
    // Create a subscription
    var subscription = new Subscription(lookupMessage);
    
    // Clear the collection for search results
    _searchResults.Clear();
    
    // Temporary collection for accumulating results
    var foundSecurities = new List<Security>();
    
    // Subscription for receiving instruments
    void OnSecurityReceived(Subscription sub, Security security)
    {
        if (sub != subscription)
            return;
            
        // Add the found instrument to the collection
        foundSecurities.Add(security);
        Console.WriteLine($"Found: {security.Id}, {security.Name}");
    }
    
    // Subscription for search completion
    void OnSubscriptionFinished(Subscription sub)
    {
        if (sub != subscription)
            return;
            
        // Copy results to the main collection
        _searchResults.AddRange(foundSecurities);
        
        Console.WriteLine($"Search completed. Instruments found: {foundSecurities.Count}");
        
        // Unsubscribe from events
        Connector.SecurityReceived -= OnSecurityReceived;
        Connector.SubscriptionFinished -= OnSubscriptionFinished;
        Connector.SubscriptionFailed -= OnSubscriptionFailed;
    }
    
    // Handling subscription errors
    void OnSubscriptionFailed(Subscription sub, Exception error, bool isSubscribe)
    {
        if (sub != subscription)
            return;
            
        Console.WriteLine($"Instrument search error: {error.Message}");
        
        // Unsubscribe from events
        Connector.SecurityReceived -= OnSecurityReceived;
        Connector.SubscriptionFinished -= OnSubscriptionFinished;
        Connector.SubscriptionFailed -= OnSubscriptionFailed;
    }
    
    // Subscribe to events
    Connector.SecurityReceived += OnSecurityReceived;
    Connector.SubscriptionFinished += OnSubscriptionFinished;
    Connector.SubscriptionFailed += OnSubscriptionFailed;
    
    // Send the search request
    Connector.Subscribe(subscription);
}
```

### Example of Usage in a WPF Application

In a graphical application, instrument search is often called from a button click handler:

```csharp
private void FindButton_Click(object sender, RoutedEventArgs e)
{
    // Get search criteria from the text field
    var searchText = SearchTextBox.Text;
    
    if (string.IsNullOrWhiteSpace(searchText))
    {
        MessageBox.Show("Enter a search criterion");
        return;
    }
    
    // Create and send a search subscription
    var lookupMessage = new SecurityLookupMessage
    {
        SecurityId = new SecurityId { SecurityCode = searchText },
        // If a type is selected in the interface
        SecurityType = SecurityTypeComboBox.SelectedItem as SecurityTypes?
    };
    
    var subscription = new Subscription(lookupMessage);
    
    // Here you can show a loading indicator
    LoadingIndicator.Visibility = Visibility.Visible;
    
    // Send the request
    Connector.Subscribe(subscription);
}
```

## Using SecurityLookupWindow

StockSharp also provides a ready-made dialog for instrument search — [SecurityLookupWindow](xref:StockSharp.Xaml.SecurityLookupWindow):

```csharp
private void ShowSecurityLookupWindow_Click(object sender, RoutedEventArgs e)
{
    var lookupWindow = new SecurityLookupWindow
    {
        // Specify the ability to search for all instruments
        // (if the connector supports this function)
        ShowAllOption = Connector.Adapter.IsSupportSecuritiesLookupAll(),
        
        // Set initial search criteria
        CriteriaMessage = new SecurityLookupMessage
        {
            SecurityId = new SecurityId { SecurityCode = "AAPL" },
            SecurityType = SecurityTypes.Stock
        }
    };
    
    // Show the window as a modal dialog
    if (lookupWindow.ShowModal(this))
    {
        // If the user confirmed the selection, send the request
        Connector.Subscribe(new Subscription(lookupWindow.CriteriaMessage));
    }
}
```

## Conclusion

The subscription mechanism in StockSharp provides a unified way to obtain data, including instrument search. This allows using the same approach for working with different connectors and data types, which significantly simplifies the development of trading applications.
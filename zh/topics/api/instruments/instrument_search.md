# 乐器搜索

大多数与美国股票交易所的连接器（例如 [Interactive Brokers](../connectors/stock_market/interactive_brokers.md)、[PolygonIO](../connectors/stock_market/polygonio.md) 等）在通过 [IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect) 方法建立连接后，并不会向客户端传输所有可用的交易工具。这是由于美国交易所交易的工具数量庞大，因此这样做是为了减轻经纪商服务器和数据源的负载。

## 仪器搜索基础

在 S# 中搜索工具时，使用订阅机制，类似于接收市场数据。这种方法允许对所有类型的数据，包括工具，使用统一的代码。

### 为乐器搜索创建订阅

要搜索工具，您需要基于包含过滤参数的 [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) 消息创建 [Subscription](xref:StockSharp.BusinessEntities.Subscription) 类的实例：

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

### 可能的过滤参数

[SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) 消息允许设置以下搜索条件：

- **SecurityId** — 工具标识符，包含：
  - **SecurityCode** — 证券代码或代码掩码（例如，“AAPL”或“MS*”）
  - **BoardCode** — 交易板代码（例如，[ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq)）
- **SecurityType** — 工具类型 ([SecurityTypes.Stock](xref:StockSharp.Messages.SecurityTypes.Stock), [SecurityTypes.Future](xref:StockSharp.Messages.SecurityTypes.Future) 等)
- **SecurityTypes** — 用于高级搜索的工具类型数组
- **货币** — 该工具的交易货币
- **ExpiryDate** — 到期日（用于衍生品）
- **行权价** — 期权的行权价
- **OptionType** — 期权类型（适用于期权）
- **名称** — 乐器名称或其部分
- **类** — 乐器类别

### 正在处理搜索结果

创建订阅后，您需要订阅事件以接收工具并发送请求：

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

### 乐器搜索完整示例

下面是一个用于搜索仪器的方法的完整示例：

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

### 在 WPF 应用中的使用示例

在图形应用程序中，仪器搜索通常由按钮点击处理程序调用：

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

## 使用安全查找窗口

StockSharp 还提供了一个现成的工具搜索对话框 — [SecurityLookupWindow](xref:StockSharp.Xaml.SecurityLookupWindow)：

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

## 结论

StockSharp 中的订阅机制提供了一种获取数据的统一方式，包括工具搜索。这允许对不同的连接器和数据类型使用相同的方法，从而显著简化了交易应用程序的开发。
# 交易品种搜索

大多数与美国股票交易所的连接器（例如 [Interactive Brokers](../connectors/stock_market/interactive_brokers.md)、[PolygonIO](../connectors/stock_market/polygonio.md) 等）在通过 [IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect) 方法建立连接后，并不会向客户端传输所有可用的交易品种。这是由于美国交易所交易的工具数量庞大，因此这样做是为了减轻经纪商服务器和数据源的负载。

## 交易品种搜索基础

在 S# 中搜索工具时，使用订阅机制，类似于接收市场数据。这种方法允许对所有类型的数据，包括工具，使用统一的代码。

### 为交易品种搜索创建订阅

要搜索工具，您需要创建 [Subscription](xref:StockSharp.BusinessEntities.Subscription) 类的实例，并以包含过滤参数的 [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) 消息为基础：

```csharp
// 创建搜索过滤对象
var lookupMessage = new SecurityLookupMessage
{
	// 设置搜索条件
	SecurityId = new SecurityId
	{
		// Search by instrument code (you can use a mask like "AAPL*")
		SecurityCode = "AAPL",
		// 可选指定交易板代码
		BoardCode = "NASDAQ"
	},
	// 可以指定交易品种类型
	SecurityType = SecurityTypes.Stock,
	// 设置事务 ID
	TransactionId = Connector.TransactionIdGenerator.GetNextId()
};

// 创建交易品种搜索订阅
var subscription = new Subscription(lookupMessage);
```

### 可能的过滤参数

[SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) 消息允许设置以下搜索条件：

- **SecurityId** — 工具标识符，包含：
  - **SecurityCode** — 交易品种代码或代码掩码（例如，“AAPL”或“MS*”）
  - **BoardCode** — 交易板代码（例如，[ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq)）
- **SecurityType** — 工具类型 ([SecurityTypes.Stock](xref:StockSharp.Messages.SecurityTypes.Stock), [SecurityTypes.Future](xref:StockSharp.Messages.SecurityTypes.Future) 等)
- **SecurityTypes** — 用于高级搜索的工具类型数组
- **货币** — 该工具的交易货币
- **ExpiryDate** — 到期日（用于衍生品）
- **行权价** — 期权的行权价
- **OptionType** — 期权类型（适用于期权）
- **名称** — 交易品种名称或其部分
- **类** — 交易品种类别

### 正在处理搜索结果

创建订阅后，您需要订阅事件以接收工具并发送请求：

```csharp
// 交易品种接收事件处理器
private void OnSecurityReceived(Subscription subscription, Security security)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Found instrument: {security.Id} - {security.Name}, Type: {security.Type}");
	
	// 这里可以将交易品种添加到集合或执行其他操作
	Securities.Add(security);
}

// 搜索完成事件处理器
private void OnSubscriptionFinished(Subscription subscription)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Search completed. Instruments found: {Securities.Count}");
}

// 订阅错误处理器
private void OnSubscriptionFailed(Subscription subscription, Exception error, bool isSubscribe)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Instrument search error: {error.Message}");
}

// 订阅事件
Connector.SecurityReceived += OnSecurityReceived;
Connector.SubscriptionFinished += OnSubscriptionFinished;
Connector.SubscriptionFailed += OnSubscriptionFailed;

// 发送交易品种搜索请求
Connector.Subscribe(subscription);
```

### 交易品种搜索完整示例

下面是一个用于搜索交易品种的方法的完整示例：

```csharp
public void FindSecurities(string searchCode, SecurityTypes? securityType = null)
{
	// 创建交易品种搜索对象
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
	
	// 创建订阅
	var subscription = new Subscription(lookupMessage);
	
	// 清空搜索结果集合
	_searchResults.Clear();
	
	// 用于累积结果的临时集合
	var foundSecurities = new List<Security>();
	
	// 用于接收交易品种的订阅
	void OnSecurityReceived(Subscription sub, Security security)
	{
		if (sub != subscription)
			return;
			
		// 将找到的交易品种添加到集合
		foundSecurities.Add(security);
		Console.WriteLine($"Found: {security.Id}, {security.Name}");
	}
	
	// 搜索完成订阅
	void OnSubscriptionFinished(Subscription sub)
	{
		if (sub != subscription)
			return;
			
		// 将结果复制到主集合
		_searchResults.AddRange(foundSecurities);
		
		Console.WriteLine($"Search completed. Instruments found: {foundSecurities.Count}");
		
		// 取消事件订阅
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// 处理订阅错误
	void OnSubscriptionFailed(Subscription sub, Exception error, bool isSubscribe)
	{
		if (sub != subscription)
			return;
			
		Console.WriteLine($"Instrument search error: {error.Message}");
		
		// 取消事件订阅
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// 订阅事件
	Connector.SecurityReceived += OnSecurityReceived;
	Connector.SubscriptionFinished += OnSubscriptionFinished;
	Connector.SubscriptionFailed += OnSubscriptionFailed;
	
	// 发送搜索请求
	Connector.Subscribe(subscription);
}
```

### 在 WPF 应用中的使用示例

在图形应用程序中，交易品种搜索通常由按钮点击处理程序调用：

```csharp
private void FindButton_Click(object sender, RoutedEventArgs e)
{
	// 从文本字段获取搜索条件
	var searchText = SearchTextBox.Text;
	
	if (string.IsNullOrWhiteSpace(searchText))
	{
		MessageBox.Show("Enter a search criterion");
		return;
	}
	
	// 创建并发送搜索订阅
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId { SecurityCode = searchText },
		// If a type is selected in the interface
		SecurityType = SecurityTypeComboBox.SelectedItem as SecurityTypes?
	};
	
	var subscription = new Subscription(lookupMessage);
	
	// 这里可以显示加载指示器
	LoadingIndicator.Visibility = Visibility.Visible;
	
	// 发送请求
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
		// 指定可搜索所有交易品种
		// (if the connector supports this function)
		ShowAllOption = Connector.Adapter.IsSupportSecuritiesLookupAll(),
		
		// 设置初始搜索条件
		CriteriaMessage = new SecurityLookupMessage
		{
			SecurityId = new SecurityId { SecurityCode = "AAPL" },
			SecurityType = SecurityTypes.Stock
		}
	};
	
	// 以模态对话框显示窗口
	if (lookupWindow.ShowModal(this))
	{
		// If the user confirmed the selection, send the request
		Connector.Subscribe(new Subscription(lookupWindow.CriteriaMessage));
	}
}
```

## 结论

StockSharp 中的订阅机制提供了一种获取数据的统一方式，包括工具搜索。这允许对不同的连接器和数据类型使用相同的方法，从而显著简化了交易应用程序的开发。

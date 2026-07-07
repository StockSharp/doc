# Notícias

[NewsGrid](xref:StockSharp.Xaml.NewsGrid) - uma tabela para apresentar notícias.

**Propriedades principais**

- [NewsGrid.News](xref:StockSharp.Xaml.NewsGrid.News) - lista de notícias.
- [NewsGrid.FirstSelectedNews](xref:StockSharp.Xaml.NewsGrid.FirstSelectedNews) - notícia selecionada.
- [NewsGrid.SelectedNews](xref:StockSharp.Xaml.NewsGrid.SelectedNews) - notícias selecionadas.
- [NewsGrid.SubscriptionProvider](xref:StockSharp.Xaml.NewsGrid.SubscriptionProvider) - fornecedor de notícias.

Abaixo encontram-se fragmentos de código que demonstram a sua utilização:

```xaml
<Window	x:Class="SampleAlfa.NewsWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
		xmlns:xaml="http://schemas.stocksharp.com/xaml"
		Title="{x:Static loc:LocalizedStrings.News}" Height="300" Width="1050">
	<xaml:NewsPanel x:Name="NewsPanel"/>
</Window>
```

```cs
private readonly Connector _connector = new Connector();
private void ConnectClick(object sender, RoutedEventArgs e)
{
	// Other connection actions
	
	// Set news provider
	_newsWindow.NewsPanel.SubscriptionProvider = _connector;
	
	// Subscribe to news reception event
	_connector.NewsReceived += OnNewsReceived;
	
	// Create a subscription to news
	var newsSubscription = new Subscription(DataType.News);
	_connector.Subscribe(newsSubscription);
	
	// Perform connection
	_connector.Connect();
}

// Handler for news reception event
private void OnNewsReceived(Subscription subscription, News news)
{
	// Add news to NewsGrid in user interface thread
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
}
```

### Filtragem de notícias

```cs
// Creating a subscription to news with filtering
public void SubscribeToFilteredNews(string source = null, DateTime? from = null)
{
	// Create a subscription to news
	var newsSubscription = new Subscription(DataType.News)
	{
		MarketData =
		{
			// Set starting date for historical news
			From = from ?? DateTime.Today.AddDays(-7),
			
			// Optionally set news source
			NewsSource = source
		}
	};
	
	// Subscribe to news reception event
	_connector.NewsReceived += OnFilteredNewsReceived;
	
	// Start the subscription
	_connector.Subscribe(newsSubscription);
}

// Handler for filtered news reception events
private void OnFilteredNewsReceived(Subscription subscription, News news)
{
	// Check source filter
	if (subscription.MarketData.NewsSource != null && 
		!string.Equals(news.Source, subscription.MarketData.NewsSource, StringComparison.OrdinalIgnoreCase))
		return;
		
	// Add news to NewsGrid
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
	
	// Output news information
	Console.WriteLine($"News: {news.Headline}");
	Console.WriteLine($"Source: {news.Source}");
	Console.WriteLine($"Time: {news.ServerTime}");
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Text: {news.Story}");
}
```

### Pesquisa de notícias por palavras-chave

```cs
// Method for filtering news by keywords
public void FilterNewsByKeywords(IEnumerable<string> keywords)
{
	var keywordsList = keywords.ToList();
	
	// If already subscribed to news,
	// just set the handler
	_connector.NewsReceived += (subscription, news) =>
	{
		// Check if the news headline contains any of the keywords
		bool containsKeyword = keywordsList.Any(keyword => 
			news.Headline.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
			
		if (containsKeyword)
		{
			// Add news to NewsGrid
			this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
			
			// Display notification
			ShowNotification($"New news on topic: {news.Headline}");
		}
	};
}
```

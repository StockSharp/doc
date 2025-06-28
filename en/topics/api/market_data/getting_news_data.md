# Getting News Data

StockSharp API allows you to receive news data from various sources. News can be an important source of information when making trading decisions or for market analysis.

> [!NOTE]
> Note that not all data sources provide news. Some cryptocurrency exchanges, including Binance, do not have a built-in news feed through their API. In such cases, it is recommended to use specialized news sources or RSS feeds.

## Subscribing to News Data

To start receiving news, you need to create a subscription to news data and then handle news reception events:

```cs
// Create a subscription to news
var newsSubscription = new Subscription(DataType.News);

// Subscribe to the news received event
_connector.NewsReceived += OnNewsReceived;

// Start the subscription
_connector.Subscribe(newsSubscription);

// Event handler for receiving news
private void OnNewsReceived(Subscription subscription, News news)
{
	if (subscription != newsSubscription)
		return;
		
	// Process the received news
	Console.WriteLine($"News: {news.Id}");
	Console.WriteLine($"Headline: {news.Headline}");
	Console.WriteLine($"Source: {news.Source}");
	Console.WriteLine($"Time: {news.ServerTime}");
	Console.WriteLine($"URL: {news.Url}");
	
	// If there is news text
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Story: {news.Story}");
	
	// If the news is related to specific instruments
	if (news.SecurityId != null)
		Console.WriteLine($"Instrument: {news.SecurityId}");
}
```

## Filtering News

When subscribing to news, you can specify filtering parameters to receive only the news you are interested in:

```cs
// Create a subscription to news with filtering
var filteredNewsSubscription = new Subscription(DataType.News)
{
	MarketData = 
	{
		// Specify the period for which to get news
		From = DateTime.Now.Subtract(TimeSpan.FromHours(24)),
		
		// You can specify a specific news source
		// For example, we use an RSS source
		NewsSource = "CryptoNews"
	}
};

_connector.Subscribe(filteredNewsSubscription);
```

## Displaying News in the User Interface

StockSharp provides a special visual component [NewsPanel](xref:StockSharp.Xaml.NewsPanel) for displaying news:

```cs
// Create and configure a news panel
var newsPanel = new NewsPanel();

// Subscribe to the news received event and add news to the panel
_connector.NewsReceived += (subscription, news) => 
{
	// To update UI elements
	// you need to use the GuiAsync or GuiSync method
	this.GuiAsync(() => newsPanel.NewsGrid.News.Add(news));
};
```

In XAML code:

```xaml
<sx:NewsPanel x:Name="NewsPanel" Grid.Row="1" />
```

## Historical News

To get historical news for a specific period, you can use the same subscription mechanism with a specified time range:

```cs
// Create a subscription to historical news
var historicalNewsSubscription = new Subscription(DataType.News)
{
	MarketData = 
	{
		// Specify the period for which to get news
		From = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

_connector.Subscribe(historicalNewsSubscription);
```

## Connecting to RSS for News

If you are working with connectors that do not provide news feeds (for example, Binance), you can add an additional news source via RSS:

```cs
// Create a Connector instance
var connector = new Connector();

// Add the main adapter for connecting to Binance
var binanceAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API Key>",
	Secret = "<Your Secret Key>",
};
connector.Adapter.InnerAdapters.Add(binanceAdapter);

// Add an adapter for receiving news via RSS
var rssAdapter = new RssMessageAdapter(connector.TransactionIdGenerator)
{
	Address = "https://news-source.com/feed",
	IsEnabled = true
};
connector.Adapter.InnerAdapters.Add(rssAdapter);

// Subscribe to the news received event
connector.NewsReceived += OnNewsReceived;

// Connect
connector.Connect();
```

## Notes

- Not all connectors support receiving news. For example, Binance does not provide a news feed through the API.
- For cryptocurrency market news, it is recommended to use specialized RSS sources.
- For news related to specific instruments, additional subscription configuration may be required.
- When working with a graphical interface, remember to update UI elements in the user interface thread using the `GuiAsync` or `GuiSync` methods.

## See Also

- [Subscriptions](subscriptions.md)
- [Graphical Components](../graphical_user_interface.md)
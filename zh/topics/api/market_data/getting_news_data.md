# 获取新闻数据

StockSharp API 允许您从各种来源接收新闻数据。新闻在做出交易决策或进行市场分析时可能是一个重要的信息来源。

> [!NOTE]
> 请注意，并非所有数据源都提供新闻。一些加密货币交易所，包括币安，通过其 API 并没有内置的新闻推送。在这种情况下，建议使用专业的新闻来源或 RSS 订阅源。

## 订阅新闻数据

要开始接收新闻，您需要创建新闻数据订阅，然后处理新闻接收事件：

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

## 筛选新闻

在订阅新闻时，您可以指定过滤参数，只接收您感兴趣的新闻：

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

## 在用户界面中显示新闻

StockSharp 提供了一个用于显示新闻的特殊视觉组件 [NewsPanel](xref:StockSharp.Xaml.NewsPanel)：

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

在 XAML 代码中：

```xaml
<sx:NewsPanel x:Name="NewsPanel" Grid.Row="1" />
```

## 历史新闻

要获取特定时间段的历史新闻，您可以使用相同的订阅机制并指定时间范围：

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

## 正在连接 RSS 获取新闻

如果你正在使用不提供新闻源的连接器（例如 Binance），你可以通过 RSS 添加额外的新闻来源：

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

## 笔记

- 并非所有连接器都支持接收新闻。例如，Binance 并不通过 API 提供新闻源。
- 对于加密货币市场新闻，建议使用专业的 RSS 来源。
- 对于与特定工具相关的新闻，可能需要额外的订阅配置。
- 在使用图形界面时，请记住使用 `GuiAsync` 或 `GuiSync` 方法在用户界面线程中更新 UI 元素。

## 另请参阅

- [订阅](subscriptions.md)
- [图形组件](../graphical_user_interface.md)

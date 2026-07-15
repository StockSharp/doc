# 获取新闻数据

StockSharp API 允许您从各种来源接收新闻数据。新闻在做出交易决策或进行市场分析时可能是一个重要的信息来源。

> [!NOTE]
> 请注意，并非所有数据源都提供新闻。一些加密货币交易所，包括Binance，通过其 API 并没有内置的新闻推送。在这种情况下，建议使用专业的新闻来源或 RSS 订阅源。

## 订阅新闻数据

要开始接收新闻，您需要创建新闻数据订阅，然后处理新闻接收事件：

```cs
// 创建新闻订阅
var newsSubscription = new Subscription(DataType.News);

// 订阅新闻接收事件
_connector.NewsReceived += OnNewsReceived;

// 启动订阅
_connector.Subscribe(newsSubscription);

// 新闻接收事件处理器
private void OnNewsReceived(Subscription subscription, News news)
{
	if (subscription != newsSubscription)
		return;

	// 处理收到的新闻
	Console.WriteLine($"新闻: {news.Id}");
	Console.WriteLine($"标题: {news.Headline}");
	Console.WriteLine($"来源: {news.Source}");
	Console.WriteLine($"时间: {news.ServerTime}");
	Console.WriteLine($"链接: {news.Url}");

	// 如果存在新闻文本
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"正文: {news.Story}");

	// 如果新闻与特定交易品种相关
	if (news.SecurityId != null)
		Console.WriteLine($"交易品种: {news.SecurityId}");
}
```

## 筛选新闻

在订阅新闻时，您可以指定过滤参数，只接收您感兴趣的新闻：

```cs
// 创建带过滤的新闻订阅
var filteredNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// 指定获取新闻的期间
		From = DateTime.Now.Subtract(TimeSpan.FromHours(24)),

		// 可以指定特定新闻来源
		// 例如，这里使用 RSS 源
		NewsSource = "CryptoNews"
	}
};

_connector.Subscribe(filteredNewsSubscription);
```

## 在用户界面中显示新闻

StockSharp 提供了一个用于显示新闻的特殊视觉组件 [NewsPanel](xref:StockSharp.Xaml.NewsPanel)：

```cs
// 创建并配置新闻面板
var newsPanel = new NewsPanel();

// 订阅新闻接收事件并将新闻添加到面板
_connector.NewsReceived += (subscription, news) =>
{
	// 要更新 UI 元素
	// 需要使用 GuiAsync 或 GuiSync 方法
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
// 创建历史新闻订阅
var historicalNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// 指定获取新闻的期间
		From = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

_connector.Subscribe(historicalNewsSubscription);
```

## 正在连接 RSS 获取新闻

如果你正在使用不提供新闻源的连接器（例如 Binance），你可以通过 RSS 添加额外的新闻来源：

```cs
// 创建 Connector 实例
var connector = new Connector();

// 添加用于连接 Binance 的主适配器
var binanceAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 API 访问密钥>",
	Secret = "<您的秘密密钥>",
};
connector.Adapter.InnerAdapters.Add(binanceAdapter);

// 添加通过 RSS 接收新闻的适配器
var rssAdapter = new RssMessageAdapter(connector.TransactionIdGenerator)
{
	Address = "https://news-source.com/feed",
	IsEnabled = true
};
connector.Adapter.InnerAdapters.Add(rssAdapter);

// 订阅新闻接收事件
connector.NewsReceived += OnNewsReceived;

// 连接
connector.Connect();
```

## 笔记

- 并非所有连接器都支持接收新闻。例如，Binance 并不通过 API 提供新闻源。
- 对于加密货币市场新闻，建议使用专业的 RSS 来源。
- 对于与特定交易品种相关的新闻，可能需要额外的订阅配置。
- 在使用图形界面时，请记住使用 `GuiAsync` 或 `GuiSync` 方法在用户界面线程中更新 UI 元素。

## 另请参阅

- [订阅](subscriptions.md)
- [图形组件](../graphical_user_interface.md)

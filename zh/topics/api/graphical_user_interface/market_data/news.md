# 新闻

[NewsGrid](xref:StockSharp.Xaml.NewsGrid) - 一个用于显示新闻的表格。

**主要属性**

- [NewsGrid.News](xref:StockSharp.Xaml.NewsGrid.News) - 新闻列表。
- [NewsGrid.FirstSelectedNews](xref:StockSharp.Xaml.NewsGrid.FirstSelectedNews) - 已选择的新闻项目。
- [NewsGrid.SelectedNews](xref:StockSharp.Xaml.NewsGrid.SelectedNews) - 精选新闻项目。
- [NewsGrid.SubscriptionProvider](xref:StockSharp.Xaml.NewsGrid.SubscriptionProvider) - 新闻提供者。

以下是展示其用法的代码片段：

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
	// 其他连接操作
	
	// 设置新闻提供者
	_newsWindow.NewsPanel.SubscriptionProvider = _connector;
	
	// 订阅新闻接收事件
	_connector.NewsReceived += OnNewsReceived;
	
	// 创建新闻订阅
	var newsSubscription = new Subscription(DataType.News);
	_connector.Subscribe(newsSubscription);
	
	// 执行连接
	_connector.Connect();
}

// 新闻接收事件处理器
private void OnNewsReceived(Subscription subscription, News news)
{
	// 在用户界面线程中将新闻添加到 NewsGrid
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
}
```

### 新闻筛选

```cs
// 创建带过滤的新闻订阅
public void SubscribeToFilteredNews(string source = null, DateTime? from = null)
{
	// 创建新闻订阅
	var newsSubscription = new Subscription(DataType.News)
	{
		MarketData =
		{
			// 设置历史新闻的开始日期
			From = from ?? DateTime.Today.AddDays(-7),
			
			// 可选设置新闻来源
			NewsSource = source
		}
	};
	
	// 订阅新闻接收事件
	_connector.NewsReceived += OnFilteredNewsReceived;
	
	// 启动订阅
	_connector.Subscribe(newsSubscription);
}

// 过滤新闻接收事件处理器
private void OnFilteredNewsReceived(Subscription subscription, News news)
{
	// 检查来源过滤器
	if (subscription.MarketData.NewsSource != null && 
		!string.Equals(news.Source, subscription.MarketData.NewsSource, StringComparison.OrdinalIgnoreCase))
		return;
		
	// 将新闻添加到 NewsGrid
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
	
	// 输出新闻信息
	Console.WriteLine($"News: {news.Headline}");
	Console.WriteLine($"Source: {news.Source}");
	Console.WriteLine($"Time: {news.ServerTime}");
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Text: {news.Story}");
}
```

### 按关键词搜索新闻

```cs
// 按关键字过滤新闻的方法
public void FilterNewsByKeywords(IEnumerable<string> keywords)
{
	var keywordsList = keywords.ToList();
	
	// 如果已订阅新闻，
	// 只需设置处理器
	_connector.NewsReceived += (subscription, news) =>
	{
		// 检查新闻标题是否包含任一关键字
		bool containsKeyword = keywordsList.Any(keyword => 
			news.Headline.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
			
		if (containsKeyword)
		{
			// 将新闻添加到 NewsGrid
			this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
			
			// 显示通知
			ShowNotification($"New news on topic: {news.Headline}");
		}
	};
}
```
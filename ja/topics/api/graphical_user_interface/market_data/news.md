# ニュース

[NewsGrid](xref:StockSharp.Xaml.NewsGrid) は、ニュースを表示するためのテーブルです。

**主なプロパティ**

- [NewsGrid.News](xref:StockSharp.Xaml.NewsGrid.News) - ニュースのリスト。
- [NewsGrid.FirstSelectedNews](xref:StockSharp.Xaml.NewsGrid.FirstSelectedNews) - 選択されたニュース項目。
- [NewsGrid.SelectedNews](xref:StockSharp.Xaml.NewsGrid.SelectedNews) - 選択されたニュース項目群。
- [NewsGrid.SubscriptionProvider](xref:StockSharp.Xaml.NewsGrid.SubscriptionProvider) - ニュースプロバイダー。

以下は、その使用方法を示すコード断片です。

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
	// その他の接続処理
	
	// ニュースプロバイダーを設定します
	_newsWindow.NewsPanel.SubscriptionProvider = _connector;
	
	// ニュース受信イベントを購読します
	_connector.NewsReceived += OnNewsReceived;
	
	// ニュースへの購読を作成します
	var newsSubscription = new Subscription(DataType.News);
	_connector.Subscribe(newsSubscription);
	
	// 接続を実行します
	_connector.Connect();
}

// ニュース受信イベントのハンドラー
private void OnNewsReceived(Subscription subscription, News news)
{
	// ユーザーインターフェイススレッドでニュースを NewsGrid に追加します
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
}
```

### ニュースのフィルタリング

```cs
// フィルタリング付きでニュースへの購読を作成します
public void SubscribeToFilteredNews(string source = null, DateTime? from = null)
{
	// ニュースへの購読を作成します
	var newsSubscription = new Subscription(DataType.News)
	{
		MarketData =
		{
			// 履歴ニュースの開始日を設定します
			From = from ?? DateTime.Today.AddDays(-7),
			
			// 必要に応じてニュースソースを設定します
			NewsSource = source
		}
	};
	
	// ニュース受信イベントを購読します
	_connector.NewsReceived += OnFilteredNewsReceived;
	
	// 購読を開始します
	_connector.Subscribe(newsSubscription);
}

// フィルタリングされたニュース受信イベントのハンドラー
private void OnFilteredNewsReceived(Subscription subscription, News news)
{
	// ソースフィルターを確認します
	if (subscription.MarketData.NewsSource != null && 
		!string.Equals(news.Source, subscription.MarketData.NewsSource, StringComparison.OrdinalIgnoreCase))
		return;
		
	// ニュースを NewsGrid に追加します
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
	
	// ニュース情報を出力します
	Console.WriteLine($"ニュース: {news.Headline}");
	Console.WriteLine($"ソース: {news.Source}");
	Console.WriteLine($"時刻: {news.ServerTime}");
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"本文: {news.Story}");
}
```

### キーワードによるニュース検索

```cs
// キーワードでニュースをフィルタリングするメソッド
public void FilterNewsByKeywords(IEnumerable<string> keywords)
{
	var keywordsList = keywords.ToList();
	
	// すでにニュースを購読している場合は、
	// ハンドラーを設定するだけです
	_connector.NewsReceived += (subscription, news) =>
	{
		// ニュース見出しにいずれかのキーワードが含まれているか確認します
		bool containsKeyword = keywordsList.Any(keyword => 
			news.Headline.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
			
		if (containsKeyword)
		{
			// ニュースを NewsGrid に追加します
			this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
			
			// 通知を表示します
			ShowNotification($"トピックの新しいニュース: {news.Headline}");
		}
	};
}
```

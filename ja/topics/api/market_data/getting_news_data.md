# ニュースデータの取得

StockSharp API では、さまざまなソースからニュースデータを受信できます。ニュースは、取引判断や市場分析を行う際の重要な情報源になり得ます。

> [!NOTE]
> すべてのデータソースがニュースを提供しているわけではないことに注意してください。Binance を含む一部の暗号資産取引所には、API 経由の組み込みニュースフィードがありません。このような場合は、専用のニュースソースまたは RSS フィードを使用することを推奨します。

## ニュースデータのサブスクライブ

ニュースの受信を開始するには、ニュースデータへのサブスクリプションを作成し、ニュース受信イベントを処理する必要があります。

```cs
// ニュースへのサブスクリプションを作成します
var newsSubscription = new Subscription(DataType.News);

// ニュース受信イベントを購読します
_connector.NewsReceived += OnNewsReceived;

// サブスクリプションを開始します
_connector.Subscribe(newsSubscription);

// ニュース受信用イベントハンドラー
private void OnNewsReceived(Subscription subscription, News news)
{
	if (subscription != newsSubscription)
		return;

	// 受信したニュースを処理します
	Console.WriteLine($"News: {news.Id}");
	Console.WriteLine($"Headline: {news.Headline}");
	Console.WriteLine($"Source: {news.Source}");
	Console.WriteLine($"Time: {news.ServerTime}");
	Console.WriteLine($"URL: {news.Url}");

	// ニュース本文がある場合
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Story: {news.Story}");

	// ニュースが特定の銘柄に関連している場合
	if (news.SecurityId != null)
		Console.WriteLine($"銘柄: {news.SecurityId}");
}
```

## ニュースのフィルタリング

ニュースをサブスクライブするときは、関心のあるニュースのみを受信するためのフィルタリングパラメーターを指定できます。

```cs
// フィルタリング付きのニュースサブスクリプションを作成します
var filteredNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// ニュースを取得する期間を指定します
		From = DateTime.Now.Subtract(TimeSpan.FromHours(24)),

		// 特定のニュースソースを指定できます
		// たとえば、RSS ソースを使用します
		NewsSource = "CryptoNews"
	}
};

_connector.Subscribe(filteredNewsSubscription);
```

## ユーザーインターフェイスでのニュース表示

StockSharp は、ニュースを表示するための専用ビジュアルコンポーネント [NewsPanel](xref:StockSharp.Xaml.NewsPanel) を提供しています。

```cs
// ニュースパネルを作成して設定します
var newsPanel = new NewsPanel();

// ニュース受信イベントを購読し、ニュースをパネルに追加します
_connector.NewsReceived += (subscription, news) =>
{
	// UI 要素を更新するには
	// GuiAsync または GuiSync メソッドを使用する必要があります
	this.GuiAsync(() => newsPanel.NewsGrid.News.Add(news));
};
```

XAML コードでは次のようになります。

```xaml
<sx:NewsPanel x:Name="NewsPanel" Grid.Row="1" />
```

## ヒストリカルニュース

特定期間のヒストリカルニュースを取得するには、指定した時間範囲で同じサブスクリプションメカニズムを使用できます。

```cs
// ヒストリカルニュースへのサブスクリプションを作成します
var historicalNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// ニュースを取得する期間を指定します
		From = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

_connector.Subscribe(historicalNewsSubscription);
```

## ニュース用 RSS への接続

ニュースフィードを提供しないコネクター（たとえば Binance）を使用している場合は、RSS 経由で追加のニュースソースを追加できます。

```cs
// Connector インスタンスを作成します
var connector = new Connector();

// Binance に接続するためのメインアダプターを追加します
var binanceAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API Key>",
	Secret = "<Your Secret Key>",
};
connector.Adapter.InnerAdapters.Add(binanceAdapter);

// RSS 経由でニュースを受信するためのアダプターを追加します
var rssAdapter = new RssMessageAdapter(connector.TransactionIdGenerator)
{
	Address = "https://news-source.com/feed",
	IsEnabled = true
};
connector.Adapter.InnerAdapters.Add(rssAdapter);

// ニュース受信イベントを購読します
connector.NewsReceived += OnNewsReceived;

// 接続します
connector.Connect();
```

## 注意事項

- すべてのコネクターがニュース受信をサポートしているわけではありません。たとえば、Binance は API 経由のニュースフィードを提供していません。
- 暗号資産市場ニュースには、専用の RSS ソースを使用することを推奨します。
- 特定の銘柄に関連するニュースでは、追加のサブスクリプション設定が必要になる場合があります。
- グラフィカルインターフェイスを使用する場合は、`GuiAsync` または `GuiSync` メソッドを使用してユーザーインターフェイススレッドで UI 要素を更新することを忘れないでください。

## 関連項目

- [サブスクリプション](subscriptions.md)
- [グラフィカルコンポーネント](../graphical_user_interface.md)


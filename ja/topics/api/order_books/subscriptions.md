# サブスクリプション

## 板情報へのサブスクライブ

StockSharp で板情報をサブスクライブするには、次の手順を実行する必要があります。

1. 板情報を受信するイベント [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) にサブスクライブし、[IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) インターフェイスのオブジェクトを処理します。

```cs
// イベントハンドラー
private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// ここで板情報データを処理できます。たとえば、画面に表示したり、取引戦略で使用したりできます。
	Console.WriteLine($"Received order book for {orderBook.SecurityId}. Best buy price: {orderBook.GetBestBid()?.Price}, Best sell price: {orderBook.GetBestAsk()?.Price}");
}

// イベントにサブスクライブ
connector.OrderBookReceived += OnOrderBookReceived;
```

板情報のサブスクリプションリクエストを送信する**前に**、[Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) イベントにサブスクライブすることが重要です。これにより、サブスクリプションリクエスト送信後すぐに板情報が非常に速く到着し始めた場合でも、データを取り逃がさないようにできます。

2. [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)) メソッドを使用してサブスクリプションリクエストを送信します。

```cs
var security = GetSecurity(); // サブスクライブしたい Security オブジェクトを取得
				
// 板情報にサブスクライブ
var subscription = new Subscription(DataType.MarketDepth, security);
connector.Subscribe(subscription);
```

## 板情報のサブスクライブ解除

板情報のサブスクライブを解除するには、[Connector.UnSubscribe](xref:StockSharp.Algo.Connector.UnSubscribe(StockSharp.BusinessEntities.Subscription)) メソッドを呼び出します。

```cs
connector.UnSubscribe(subscription);
```

## 板情報の受信に関する補足

[Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) イベントを扱う際は、このイベントを通じて届く板情報が、すでに構築済みで使用可能な状態であることを理解しておくことが重要です。これは、ソースによるデータ送信方式が差分データ（板情報内の変更のみ）であっても、板情報の完全なスナップショットであっても、StockSharp プラットフォームがそのデータを処理し、トレーダーが完全で更新済みの板情報を受け取れるようにすることを意味します。

プラットフォームは、[Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) イベントを呼び出す前に、変更を板情報へ自動的に統合し、その内容を現在の状態に更新します。これにより、トレーダーは差分データを独自に処理したり、連続したスナップショットから板情報を構築したりする必要がないため、データの取り扱いが簡素化されます。したがって、イベントハンドラーで受信したデータが、そのイベント時点における板情報の最新状態を反映していると確信できます。

これにより、トレーダーは板情報データの構築と処理に関する技術的な側面に時間を費やすことなく、戦略のロジックに直接集中できるため、取引戦略と市場分析の開発が大幅に簡素化されます。

## 使用例

板情報の使用例は、[GitHub](https://github.com/StockSharp/StockSharp/) 上の *Samples\/01\_Basic\/02\_MarketDepths* プロジェクト、または [Installer](../../installer.md) を通じて取得できる StockSharp API アーカイブで利用できます。これらの例は、取引システムへの接続、フィルター済み板情報へのサブスクライブ、受信データの処理について実践的な説明を提供し、独自の取引戦略を開発するための良い出発点になります。

## 関連項目

[サブスクリプション](../market_data/subscriptions.md)

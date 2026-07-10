# フィルター済み板情報

フィルター済み板情報は、トレーダーと自動売買戦略が自身の注文を考慮対象から除外して市場で動作できるようにする、StockSharp の専用ツールです。複数の戦略を並行して使用する場合、ある戦略が板情報内の数量を別の市場参加者によるもの、または並行実行中の別戦略の操作結果だと認識できずに、別の戦略と「取引」し始める状況を防ぐため、これは非常に重要です。

## フィルター済み板情報の利点

- **自己取引の回避:** 並行実行時に、戦略が自分自身または互いの注文に対して約定することがありません。
- **分析の純度:** 戦略が自身の注文による歪みを受けず、外部注文のみに基づいて市場状況を分析できます。
- **執行効率:** 自身の注文が市場価格へ与える影響を最小化することで、注文執行の品質向上に役立ちます。

## サブスクリプション例

フィルター済み板情報を扱う方法は、[通常の板情報へのサブスクライブ](subscriptions.md) と同じメソッドを使用しますが、異なる [DataType](xref:StockSharp.Messages.DataType) 値を使用します。以下は、特定の銘柄についてフィルター済み板情報をサブスクライブする例です。

1. **板情報更新イベントへのサブスクライブ:** 板情報更新を受信するための [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived)。このイベントは、通常の板情報とフィルター済み板情報の両方で使用されます。

    イベントを処理するときは、イベントに関連付けられた `subscription` オブジェクト内の [Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType) を確認します。[Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType) が [DataType](xref:StockSharp.Messages.DataType.FilteredMarketDepth) の場合、受信した板情報がフィルター済みであることを示します。

    ```cs
    connector.OrderBookReceived += (sender, subscription, orderBook) =>
    {
        if (subscription.DataType == DataType.FilteredMarketDepth)
        {
            // フィルター済み板情報の処理ロジック
            Console.WriteLine($"{orderBook.SecurityId} のフィルター済み板を受信しました。");
        }
    };
    ```

2. **サブスクリプションの送信:** [Subscription](xref:StockSharp.BusinessEntities.Subscription) オブジェクトを作成し、コネクターに送信します。

    ```cs
    var subscription = new Subscription(DataType.FilteredMarketDepth, security);
    connector.Subscribe(subscription);
    
    // または次のようにします
    //var subscription = connector.SubscribeFilteredMarketDepth(security);
    ```

## まとめ

StockSharp でフィルター済み板情報を使用すると、トレーダーと戦略開発者は市場分析のための柔軟なツールを利用できます。これにより、同時に実行されている戦略間の望ましくない自己相互作用を回避し、市場注文データに基づく意思決定を簡素化できます。

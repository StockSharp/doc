# サブスクリプションのライフサイクル

StockSharp のサブスクリプションは、特定のライフサイクル段階を通過します。[ISubscriptionProvider](xref:StockSharp.BusinessEntities.ISubscriptionProvider) インターフェイスは、各段階を追跡するためのイベントを提供します。

## ライフサイクルイベント

### SubscriptionStarted

```cs
event Action<Subscription> SubscriptionStarted;
```

サブスクリプションが正常に開始されたとき、つまりアダプターがリクエストを受け付け、データの送信を開始したときに呼び出されます。履歴サブスクリプションの場合、これはデータ読み込みが開始されたことを意味します。ライブサブスクリプションの場合は、サーバーがリクエストを受け付けたことを意味します。

### SubscriptionOnline

```cs
event Action<Subscription> SubscriptionOnline;
```

サブスクリプションがリアルタイムモードへ移行したときに呼び出されます。ライブサブスクリプションの場合、これは履歴データの追い付き処理がある場合はそれが完了し、データがリアルタイムで到着していることを意味します。これは、インジケーターの「ウォームアップ」が完了し、取引を開始できることをストラテジーが理解するための重要なシグナルです。

### SubscriptionStopped

```cs
event Action<Subscription, Exception> SubscriptionStopped;
```

サブスクリプションが終了したときに呼び出されます。`Exception` パラメーターには停止理由が含まれます。
- `null` -- 正常完了（ユーザーがサブスクライブ解除した、または履歴データが終了した）。
- 例外オブジェクト -- エラー（接続断、サーバー側エラーなど）。

### SubscriptionFailed

```cs
event Action<Subscription, Exception, bool> SubscriptionFailed;
```

サブスクリプションエラー時に呼び出されます。3 番目の `bool` パラメーターは、これがサブスクライブ操作（`true`）だったのか、サブスクライブ解除操作（`false`）だったのかを示します。

## イベントの順序

ライブサブスクリプションの典型的なシーケンス:

1. `Subscribe(subscription)` を呼び出す
2. `SubscriptionStarted` -- サブスクリプションが受け付けられた
3. データが到着する（キャンドル、オーダーブック、約定など）
4. `SubscriptionOnline` -- リアルタイムモードへ移行
5. リアルタイムでデータ到着が継続する
6. `UnSubscribe(subscription)` を呼び出す、または接続断
7. `SubscriptionStopped` -- サブスクリプションが終了した

履歴サブスクリプション（指定された日付範囲あり）の場合:

1. `Subscribe(subscription)` を呼び出す
2. `SubscriptionStarted` -- サブスクリプションが受け付けられた
3. 履歴データが到着する
4. `null` 付きの `SubscriptionStopped` -- すべてのデータを受信した

## SubscriptionsOnConnect

[Connector.SubscriptionsOnConnect](xref:StockSharp.Algo.Connector) プロパティは、接続時に自動的に送信されるサブスクリプションのセットを定義します。

```cs
ISet<Subscription> SubscriptionsOnConnect { get; }
```

既定では、銘柄検索、ポートフォリオ検索、注文検索のサブスクリプションが含まれます。

```cs
SubscriptionsOnConnect.Add(SecurityLookup);
SubscriptionsOnConnect.Add(PortfolioLookup);
SubscriptionsOnConnect.Add(OrderLookup);
```

各接続時に自動的に開始される独自のサブスクリプションを追加できます。

```cs
// Level1 データの自動サブスクリプションを追加
var l1Sub = new Subscription(DataType.Level1, security);
connector.SubscriptionsOnConnect.Add(l1Sub);

// 接続時の自動注文検索を削除
connector.SubscriptionsOnConnect.Remove(connector.OrderLookup);
```

## アダプターごとの接続イベント

複数の接続（複数のアダプター）を扱う場合、どの特定のアダプターが接続または切断されたかを示すイベントが役立ちます。

### ConnectedEx

```cs
event Action<IMessageAdapter> ConnectedEx;
```

特定のアダプターの接続が成功したときに呼び出されます。パラメーターはイベントを開始したアダプターです。

### DisconnectedEx

```cs
event Action<IMessageAdapter> DisconnectedEx;
```

特定のアダプターの切断時に呼び出されます。

### ConnectionErrorEx

```cs
event Action<IMessageAdapter, Exception> ConnectionErrorEx;
```

特定のアダプターで接続エラーが発生したときに呼び出されます。

集約イベント `Connected`、`Disconnected`、`ConnectionError` も利用でき、これらは特定のアダプターを指定せずに発火します。

## 例

```cs
private readonly Connector _connector = new();

public void SetupSubscriptionTracking()
{
    // サブスクリプションのライフサイクルを追跡
    _connector.SubscriptionStarted += subscription =>
    {
        Console.WriteLine($"Subscription started: {subscription.DataType}, " +
            $"Security: {subscription.SecurityId}");
    };

    _connector.SubscriptionOnline += subscription =>
    {
        Console.WriteLine($"Subscription online: {subscription.DataType}");
    };

    _connector.SubscriptionStopped += (subscription, error) =>
    {
        if (error == null)
            Console.WriteLine($"Subscription completed: {subscription.DataType}");
        else
            Console.WriteLine($"Subscription interrupted: {subscription.DataType}, " +
                $"Error: {error.Message}");
    };

    // 個別のアダプター接続を追跡
    _connector.ConnectedEx += adapter =>
    {
        Console.WriteLine($"Adapter connected: {adapter.Name}");
    };

    _connector.DisconnectedEx += adapter =>
    {
        Console.WriteLine($"Adapter disconnected: {adapter.Name}");
    };

    _connector.ConnectionErrorEx += (adapter, error) =>
    {
        Console.WriteLine($"Adapter connection error {adapter.Name}: {error.Message}");
    };

    // 接続
    _connector.Connect();

    // 接続後 -- サブスクリプションを作成
    _connector.Connected += () =>
    {
        var subscription = new Subscription(DataType.Ticks, security);
        _connector.Subscribe(subscription);
    };
}
```

## 関連項目

[接続](../connectors.md)

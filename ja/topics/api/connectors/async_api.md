# 非同期注文操作

[Connector](xref:StockSharp.Algo.Connector) クラスは、すべての注文操作の非同期版を提供します。非同期メソッドは呼び出し元スレッドのブロックを避け、`CancellationToken` によるキャンセルをサポートします。

## メソッド

### RegisterOrderAsync

新しい注文の非同期登録:

```cs
public async ValueTask RegisterOrderAsync(Order order, CancellationToken cancellationToken = default)
```

このメソッドは注文を検証し (数量を確認し、注文タイプをリミットまたは成行として自動判定します)、トランザクションを初期化して、登録コマンドをアダプターへ送信します。エラーが発生した場合は、登録エラーイベントが生成されます。

同期版の [RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)) は内部で `RegisterOrderAsync` を呼び出します。

### CancelOrderAsync

既存注文の非同期キャンセル:

```cs
public async ValueTask CancelOrderAsync(Order order, CancellationToken cancellationToken = default)
```

このメソッドはキャンセル操作用に新しいトランザクション識別子を作成し、注文取消コマンドをアダプターへ送信します。注文は事前に登録されている必要があります。

同期版: [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order))。

### EditOrderAsync

アクティブな注文の非同期編集 (キャンセルせずに価格や数量を変更):

```cs
public async ValueTask EditOrderAsync(Order order, Order changes, CancellationToken cancellationToken = default)
```

パラメーター:
- `order` -- 編集対象の元の注文。
- `changes` -- 新しいフィールド値 (価格、数量など) を持つ [Order](xref:StockSharp.BusinessEntities.Order) オブジェクト。

呼び出し前に、編集がサポートされているか確認することを推奨します。

```cs
if (connector.IsOrderEditable(order) == true)
{
    var changes = order.CreateOrder();
    changes.Price = newPrice;
    await connector.EditOrderAsync(order, changes);
}
```

同期版: [EditOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.EditOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))。

### ReRegisterOrderAsync

注文の非同期再登録 (古い注文のキャンセルと新しい注文の登録を単一操作で実行):

```cs
public async ValueTask ReRegisterOrderAsync(Order oldOrder, Order newOrder, CancellationToken cancellationToken = default)
```

取引所が注文編集をサポートしていない一方で、アトミックな置換をサポートしている場合に使用します。サポート状況は `IsOrderReplaceable` で確認できます。

```cs
if (connector.IsOrderReplaceable(order) == true)
{
    var newOrder = order.CreateOrder();
    newOrder.Price = newPrice;
    await connector.ReRegisterOrderAsync(order, newOrder);
}
```

## 非同期メソッドを使用する場面

**非同期メソッドを使用する** 場面:
- コードが `async` コンテキストで実行される場合 (例: ASP.NET ハンドラー、クロスプラットフォームアプリケーション)。
- `CancellationToken` による操作キャンセルをサポートする必要がある場合。
- UI スレッドのブロックを避ける必要がある場合。

**同期メソッドを使用する** 場面:
- コードがストラテジー (`Strategy`) 内で実行され、そのストラテジーが内部でスレッドを管理している場合。
- 非同期が不要な単純なスクリプトやコンソールアプリケーションの場合。

同期メソッド (`RegisterOrder`、`CancelOrder`、`EditOrder`) は内部で `AsyncHelper.Run` を介して対応する非同期メソッドを呼び出すため、機能面では完全に同等です。

## 例

```cs
private readonly Connector _connector = new();

public async Task PlaceAndManageOrderAsync(Security security, Portfolio portfolio, CancellationToken cancellationToken)
{
    // 注文を作成
    var order = new Order
    {
        Security = security,
        Portfolio = portfolio,
        Direction = Sides.Buy,
        Volume = 1,
        Price = security.BestBid?.Price ?? 100m,
        Type = OrderTypes.Limit,
    };

    // 非同期登録
    await _connector.RegisterOrderAsync(order, cancellationToken);

    // ... 市場条件の変化を待機 ...

    // 非同期価格編集 (サポートされている場合)
    if (_connector.IsOrderEditable(order) == true)
    {
        var changes = order.CreateOrder();
        changes.Price = order.Price - 0.01m;
        await _connector.EditOrderAsync(order, changes, cancellationToken);
    }

    // 非同期キャンセル
    await _connector.CancelOrderAsync(order, cancellationToken);
}
```

## 関連項目

[注文](../orders_management.md)

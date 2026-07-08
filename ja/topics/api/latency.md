# レイテンシ測定

[S#](../api.md) は、[LatencyManager](xref:StockSharp.Algo.Latency.LatencyManager) を通じて注文登録と注文キャンセルのレイテンシを測定します。このマネージャーは、注文の送信から取引所からの確認受信までにどれだけの時間が経過したかを判定します。

## ILatencyManager インターフェイス

[ILatencyManager](xref:StockSharp.Algo.Latency.ILatencyManager) インターフェイスは、基本契約を定義します。

- **LatencyRegistration** — すべての注文にわたる合計登録レイテンシ（TimeSpan）。
- **LatencyCancellation** — すべての注文にわたる合計キャンセルレイテンシ（TimeSpan）。
- **Reset()** — マネージャーの状態をリセットします。
- **ProcessMessage(Message)** — メッセージを処理します。指定された操作のレイテンシ、または `null` を返します。

## 動作の仕組み

レイテンシマネージャーは「リクエスト-レスポンス」の原則で動作します。

### 1. 注文登録

[OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) を受信すると、マネージャーは注文が送信された瞬間を表すペア（`TransactionId`, `LocalTime`）を保存します。

### 2. 注文キャンセル

[OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) を受信すると、マネージャーはキャンセルが送信された瞬間を表すペア（`TransactionId`, `LocalTime`）を保存します。

### 3. 注文置換

[OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) を受信すると、マネージャーはキャンセル（旧注文）と登録（新注文）の両方を同時に登録します。

### 4. 確認

注文情報を持つ [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage)（`Pending` 状態ではなく、`Failed` でもない）を受信すると、マネージャーはレイテンシを計算します。

```
Latency = ExecutionMessage.LocalTime - StoredLocalTime
```

結果は、操作タイプに応じて `LatencyRegistration` または `LatencyCancellation` に追加されます。

## 状態: ILatencyManagerState

[ILatencyManagerState](xref:StockSharp.Algo.Latency.ILatencyManagerState) インターフェイスは、マネージャーの内部状態を保存します。

- 保留中の登録: `AddRegistration(transactionId, localTime)` / `TryGetAndRemoveRegistration(transactionId, out localTime)`
- 保留中のキャンセル: `AddCancellation(transactionId, localTime)` / `TryGetAndRemoveCancellation(transactionId, out localTime)`
- 蓄積されたレイテンシ: `LatencyRegistration`, `LatencyCancellation`
- 追加メソッド: `AddLatencyRegistration(TimeSpan)`, `AddLatencyCancellation(TimeSpan)`

既定の実装は [LatencyManagerState](xref:StockSharp.Algo.Latency.LatencyManagerState) です。

## エラー処理

注文が失敗した場合（`OrderState == Failed`）、レイテンシはカウントされません。レコードは状態ストアから単に削除されます。

## Adapter による統合

[LatencyMessageAdapter](xref:StockSharp.Algo.Latency.LatencyMessageAdapter) クラスは内部アダプターをラップし、すべての注文操作についてレイテンシを自動的に測定します。

## Strategy との統合

ストラテジー（[Strategy](xref:StockSharp.Algo.Strategies.Strategy)）は、レイテンシを追跡するための `Latency` プロパティを公開します。

## 使用例

```cs
// 状態ストアを持つマネージャーを作成します
var manager = new LatencyManager(new LatencyManagerState());

// 注文登録を処理します（送信時刻を保存します）
manager.ProcessMessage(orderRegisterMsg);

// 確認を処理します（レイテンシを計算します）
TimeSpan? latency = manager.ProcessMessage(executionMsg);
if (latency != null)
{
    Console.WriteLine($"Latency: {latency.Value.TotalMilliseconds} ms");
}

// 合計レイテンシ
Console.WriteLine($"Registration latency: {manager.LatencyRegistration.TotalMilliseconds} ms");
Console.WriteLine($"Cancellation latency: {manager.LatencyCancellation.TotalMilliseconds} ms");
```

## 状態のリセット

`Reset()` メソッドは、すべての保留中レコードをクリアし、蓄積されたレイテンシをゼロにリセットします。

```cs
manager.Reset();
```

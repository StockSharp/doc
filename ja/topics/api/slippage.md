# スリッページ測定

[S#](../api.md) は、[SlippageManager](xref:StockSharp.Algo.Slippage.SlippageManager) を通じてスリッページを計算します。スリッページとは、注文の期待約定価格と実際の取引価格との差です。

## ISlippageManager インターフェイス

[ISlippageManager](xref:StockSharp.Algo.Slippage.ISlippageManager) インターフェイスは、基本契約を定義します。

- **スリッページ** — 累積スリッページ合計（decimal）。
- **Reset()** — マネージャーの状態をリセットします。
- **ProcessMessage(Message)** — メッセージを処理します。指定された約定のスリッページを返すか、`null` を返します。

## 仕組み

スリッページマネージャーは 3 段階で動作します。

### 1. 市場価格の更新

[Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) または [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) を受信すると、マネージャーは各銘柄の最良 Bid/Ask 価格を保存します。

### 2. 予定価格の保存

[OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) を受信すると、マネージャーは「予定価格」、つまり注文登録時点の最良市場価格を保存します。

- 買い（`Buy`）の場合は、最良 Ask が使用されます。
- 売り（`Sell`）の場合は、最良 Bid が使用されます。

### 3. スリッページの計算

取引を含む [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) を受信すると、マネージャーはスリッページを計算します。

- 買いの場合: `(TradePrice - PlannedPrice) * TradeVolume`
- 売りの場合: `(PlannedPrice - TradePrice) * TradeVolume`

正の値は不利なスリッページ（期待より悪い価格）を意味し、負の値は有利なスリッページ（期待より良い価格）を意味します。

## 状態: ISlippageManagerState

[ISlippageManagerState](xref:StockSharp.Algo.Slippage.ISlippageManagerState) インターフェイスは、マネージャーの内部状態を保存します。

- 各銘柄（[SecurityId](xref:StockSharp.Messages.SecurityId)）の最良 Bid/Ask 価格。
- 各トランザクション（`TransactionId`）の予定価格と方向。
- 累積スリッページ合計。

デフォルト実装は [SlippageManagerState](xref:StockSharp.Algo.Slippage.SlippageManagerState) です。

## 設定

| プロパティ | デフォルト | 説明 |
|----------|:-------:|-------------|
| `CalculateNegative` | `true` | 有利なスリッページを考慮します。`false` の場合、負の値はゼロに置き換えられます。 |

## アダプター経由の統合

[SlippageMessageAdapter](xref:StockSharp.Algo.Slippage.SlippageMessageAdapter) クラスは内部アダプターをラップし、すべての取引についてスリッページを自動的に計算します。

## 戦略との統合

戦略（[Strategy](xref:StockSharp.Algo.Strategies.Strategy)）は、全体的なスリッページを追跡するための `Slippage` プロパティを公開します。

## 使用例

```cs
// 状態ストアを持つマネージャーを作成
var manager = new SlippageManager(new SlippageManagerState());

// 不利なスリッページのみを考慮
manager.CalculateNegative = false;

// 市場データの処理（最良価格の更新）
manager.ProcessMessage(level1Msg);
manager.ProcessMessage(quoteChangeMsg);

// 注文登録の処理（予定価格の保存）
manager.ProcessMessage(orderRegisterMsg);

// 取引の処理（スリッページの計算）
decimal? slippage = manager.ProcessMessage(executionMsg);
if (slippage != null)
{
    Console.WriteLine($"スリッページ: {slippage.Value}");
}

// 累積スリッページ合計
Console.WriteLine($"合計スリッページ: {manager.Slippage}");
```

## 状態のリセット

`Reset()` メソッドは、最良価格、予定価格、累積スリッページを含む内部状態を完全にクリアします。

```cs
manager.Reset();
```

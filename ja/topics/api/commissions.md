# コミッションシステム

[S#](../api.md) は、[CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager) を通じて柔軟なコミッション計算システムを実装しています。このマネージャーは注文メッセージと取引メッセージを受け取り、設定されたルールに基づいてコミッションを計算します。

## ICommissionManager インターフェイス

[ICommissionManager](xref:StockSharp.Algo.Commissions.ICommissionManager) インターフェイスは、基本契約を定義します。

- **Rules** — コミッション計算用の [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule) ルールのコレクション。
- **Commission** — 累積されたコミッション総額 (decimal)。
- **Reset()** — マネージャーとすべてのルールの状態をリセットします。
- **Process(Message)** — メッセージを処理します。指定されたメッセージのコミッション、または `null` を返します。

## ICommissionRule インターフェイス

各ルールは [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule) を実装します。

- **Title** — ルールのタイトル。
- **Value** — コミッション値 ([Unit](xref:Ecng.ComponentModel.Unit))。絶対値またはパーセンテージベースにできます。
- **Process(ExecutionMessage)** — 特定のメッセージに対するコミッションを計算します。

基底クラス [CommissionRule](xref:StockSharp.Algo.Commissions.CommissionRule) には、ヘルパーメソッド `GetValue(price, volume)` が含まれています。

- **絶対値** の場合、`Value` をそのまま返します。
- **パーセンテージ** 値の場合、`(price * volume * Value) / 100` を計算します。

## ルールの種類

### 注文ルール

| クラス | 説明 |
|-------|-------------|
| [CommissionOrderRule](xref:StockSharp.Algo.Commissions.CommissionOrderRule) | 注文ごとのコミッション (注文価格と数量に基づく)。 |
| [CommissionOrderVolumeRule](xref:StockSharp.Algo.Commissions.CommissionOrderVolumeRule) | 注文数量に基づくコミッション。絶対値の場合: `Value * volume`。 |
| [CommissionOrderCountRule](xref:StockSharp.Algo.Commissions.CommissionOrderCountRule) | N 件の注文ごとのコミッション。`Count` プロパティがしきい値を設定します。 |

### 取引ルール

| クラス | 説明 |
|-------|-------------|
| [CommissionTradeRule](xref:StockSharp.Algo.Commissions.CommissionTradeRule) | 取引ごとのコミッション (取引価格と数量に基づく)。 |
| [CommissionTradeVolumeRule](xref:StockSharp.Algo.Commissions.CommissionTradeVolumeRule) | 取引数量に基づくコミッション。 |
| [CommissionTradePriceRule](xref:StockSharp.Algo.Commissions.CommissionTradePriceRule) | コミッション: `price * volume * Value`。 |
| [CommissionTradeCountRule](xref:StockSharp.Algo.Commissions.CommissionTradeCountRule) | N 件の取引ごとのコミッション。`Count` プロパティがしきい値を設定します。 |
| [CommissionTurnOverRule](xref:StockSharp.Algo.Commissions.CommissionTurnOverRule) | 各売買代金しきい値に対するコミッション。`TurnOver` プロパティがしきい値を設定します。 |

### フィルタールール

| クラス | 説明 |
|-------|-------------|
| [CommissionSecurityIdRule](xref:StockSharp.Algo.Commissions.CommissionSecurityIdRule) | 特定の銘柄に対してのみ適用されるコミッション。`Security` プロパティ。 |
| [CommissionBoardCodeRule](xref:StockSharp.Algo.Commissions.CommissionBoardCodeRule) | 特定の取引所ボードに対してのみ適用されるコミッション。`Board` プロパティ。 |
| [CommissionSecurityTypeRule](xref:StockSharp.Algo.Commissions.CommissionSecurityTypeRule) | 特定の銘柄タイプに対してのみ適用されるコミッション。`SecurityType` プロパティ。 |

## アダプター経由の統合

[CommissionMessageAdapter](xref:StockSharp.Algo.Commissions.CommissionMessageAdapter) クラスは内部アダプターをラップし、受信および送信される [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) のコミッションを自動的に計算します。メッセージに `Commission` フィールドが設定されていない場合、アダプターはマネージャーから取得した値を設定します。

## ストラテジーとの統合

ストラテジー ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) は `Commission` プロパティを公開しており、これを通じて累積コミッションを追跡できます。

## 使用例

```cs
var manager = new CommissionManager();

// 取引ごとの固定コミッション 1.5
manager.Rules.Add(new CommissionTradeRule { Value = 1.5m });

// 先物の売買代金に対する 0.1%
manager.Rules.Add(new CommissionSecurityTypeRule
{
    SecurityType = SecurityTypes.Future,
    Value = new Unit(0.1m, UnitTypes.Percent)
});

// 100 件の注文ごとに 50 のコミッション
manager.Rules.Add(new CommissionOrderCountRule
{
    Count = 100,
    Value = 50m
});

// 売買代金 1,000,000 ごとに 10 のコミッション
manager.Rules.Add(new CommissionTurnOverRule
{
    TurnOver = 1_000_000m,
    Value = 10m
});

// メッセージを処理
decimal? commission = manager.Process(executionMsg);
if (commission != null)
{
    Console.WriteLine($"Commission for message: {commission.Value}");
}

// 累積コミッション総額
Console.WriteLine($"Total commission: {manager.Commission}");
```

## 状態のリセット

`Reset()` メソッドはコミッション総額をゼロにリセットし、各ルールで `Reset()` を呼び出します。これにより、内部カウンター (注文数、現在の売買代金など) がクリアされます。

```cs
manager.Reset();
```

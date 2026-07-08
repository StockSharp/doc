# 損益管理

[S#](../api.md) は、[PnLManager](xref:StockSharp.Algo.PnL.PnLManager) を通じて損益（PnL）計算を実装しています。このマネージャーはメッセージ（取引、市場データ）のストリームを処理し、実現損益と未実現損益を計算します。

## IPnLManager インターフェイス

[IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager) インターフェイスは、基本契約を定義します。

- **RealizedPnL** — 実現損益（decimal）。ポジションがクローズされると累積されます。
- **UnrealizedPnL** — 未実現損益（decimal）。現在の市場価格に基づいて再計算されます。
- **Reset()** — マネージャーの状態をリセットします。
- **UpdateSecurity(Level1ChangeMessage)** — 銘柄パラメーター（価格ステップ、ステップ価格、ロット乗数）を更新します。
- **ProcessMessage(Message, ICollection\<PortfolioPnLManager\>)** — メッセージを処理します。ポジションがクローズされた場合は [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) を返し、それ以外の場合は `null` を返します。

## アーキテクチャ

PnL システムは 3 階層の構造を持ちます。

```
PnLManager
  └── PortfolioPnLManager (by portfolio name)
        └── PnLQueue (by SecurityId)
```

- [PnLManager](xref:StockSharp.Algo.PnL.PnLManager) — 最上位レベルで、ポートフォリオマネージャーの辞書を管理します。
- [PortfolioPnLManager](xref:StockSharp.Algo.PnL.PortfolioPnLManager) — 特定のポートフォリオ用の PnL マネージャーで、銘柄別にキューを管理します。
- [PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) — 単一銘柄の取引をマッチングする FIFO キューです。

### PnLQueue — 計算キュー

[PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) は、オープン取引とクローズ取引のマッチングを担当します。

- **PriceStep** — 銘柄の価格ステップ。
- **StepPrice** — 価格ステップのコスト（先物用）。
- **Leverage** — レバレッジ。
- **LotMultiplier** — ロット乗数。

利益乗数は次の式で計算されます。

```
Multiplier = (StepPrice / PriceStep) * Leverage * LotMultiplier
```

通常の株式（`StepPrice` が設定されていない場合）では、乗数は `1 * Leverage * LotMultiplier` と等しくなります。

## PnLInfo — 取引処理結果

[PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) クラスには、ポジションをクローズした結果が含まれます。

- **ServerTime** — 取引時刻。
- **ClosedVolume** — クローズされたポジションの数量。
- **PnL** — この取引からの実現利益。

たとえば、ポジションが +2 で、-5 契約の取引が到着した場合、`ClosedVolume = 2`（ポジションから 2 契約がクローズされた）になります。

## データソースの構成

[PnLManager](xref:StockSharp.Algo.PnL.PnLManager) では、未実現損益計算に使用する市場データソースを選択できます。

| プロパティ | デフォルト | 説明 |
|----------|:-------:|-------------|
| `UseTick` | `true` | ティック取引を使用します。 |
| `UseOrderBook` | `false` | 板情報（最良 Bid/Ask）を使用します。 |
| `UseLevel1` | `false` | Level1 データを使用します。 |
| `UseOrderLog` | `false` | 注文ログを使用します。 |
| `UseCandles` | `true` | ローソク足（終値）を使用します。 |

## アダプター経由の統合

[PnLMessageAdapter](xref:StockSharp.Algo.PnL.PnLMessageAdapter) クラスは内部アダプターをラップし、PnL 計算のためにすべてのメッセージを自動的に処理します。

## 戦略との統合

戦略（[Strategy](xref:StockSharp.Algo.Strategies.Strategy)）は次を提供します。

- `PnLManager` プロパティ — マネージャーインスタンス。
- `PnL` プロパティ — 総損益（`RealizedPnL + UnrealizedPnL`）。
- `PnLChanged` イベント — 損益変化の通知。
- `PnLReceived2` イベント — 新しい PnL データを受信したときの通知。

## 使用例

```cs
var pnlManager = new PnLManager
{
    UseTick = true,
    UseOrderBook = true,
    UseCandles = true
};

// メッセージの処理
var info = pnlManager.ProcessMessage(executionMsg);
if (info != null)
{
    Console.WriteLine($"Closed: {info.ClosedVolume}, PnL: {info.PnL}");
}

// 総損益
var realizedPnL = pnlManager.RealizedPnL;
var unrealizedPnL = pnlManager.UnrealizedPnL;
var totalPnL = realizedPnL + unrealizedPnL;

Console.WriteLine($"Realized PnL: {realizedPnL}");
Console.WriteLine($"Unrealized PnL: {unrealizedPnL}");
Console.WriteLine($"Total PnL: {totalPnL}");
```

## 状態のリセット

`Reset()` メソッドは、すべてのポートフォリオマネージャーと計算キューをクリアし、実現 PnL をゼロにリセットします。

```cs
pnlManager.Reset();
```

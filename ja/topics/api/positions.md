# ポジション管理

StockSharp は、ポジションの現在の状態を追跡し、注文または取引に基づいてポジションを計算し、ライフサイクル履歴（オープン、クローズ、反転）を保持できる柔軟なポジション管理システムを提供します。

## PositionManager

[PositionManager](xref:StockSharp.Algo.Positions.PositionManager) クラスは [IPositionManager](xref:StockSharp.Algo.Positions.IPositionManager) インターフェイスを実装し、受信メッセージに基づいて現在のポジションを計算する主要コンポーネントとして機能します。

### マネージャーの作成

コンストラクターは 2 つのパラメーターを受け取ります。

```cs
var state = new PositionManagerState();
var manager = new PositionManager(byOrders: false, state);
```

- `byOrders = true` -- ポジションは注文残高の変化に基づいて計算されます。取引システムが注文状態の更新を受信する一方で、個別の取引を受信しない場合に適しています。
- `byOrders = false` -- ポジションは取引数量に基づいて計算されます（推奨モード）。約定済み操作をより正確に会計処理できます。

### メッセージの処理

`ProcessMessage` メソッドは受信メッセージ（[Message](xref:StockSharp.Messages.Message)）を受け取り、ポジションが変化した場合は [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) を返し、ポジションが変化していない場合は `null` を返します。

```cs
var posChange = manager.ProcessMessage(executionMsg);

if (posChange != null)
{
    Console.WriteLine($"ポジション: {posChange.CurrentValue}");
}
```

## IPositionManagerState

[IPositionManagerState](xref:StockSharp.Algo.Positions.IPositionManagerState) インターフェイスは、ポジションマネージャーの内部状態を記述します。[PositionManagerState](xref:StockSharp.Algo.Positions.PositionManagerState) 実装は、現在の注文とポジションに関する情報を保存します。

### 主なメソッド

| メソッド | 説明 |
|--------|-------------|
| `AddOrGetOrder` | 新しい注文を登録するか、`transactionId` によって既存の注文を返します |
| `TryGetOrder` | 注文パラメーター（銘柄、ポートフォリオ、方向、残高）を取得します |
| `UpdateOrderBalance` | 部分約定後に現在の注文残高を更新します |
| `RemoveOrder` | 完了した注文を追跡対象から削除します |
| `UpdatePosition` | 銘柄とポートフォリオ別にポジションを更新し、新しい値を返します |
| `Clear` | すべてのマネージャー状態をリセットします |

### 状態を扱う例

```cs
var state = new PositionManagerState();

// 注文を登録
state.AddOrGetOrder(
    transactionId: 12345,
    securityId: secId,
    portfolioName: "MyPortfolio",
    side: Sides.Buy,
    volume: 100,
    balance: 100
);

// 部分約定後に更新
state.UpdateOrderBalance(12345, newBalance: 60);

// ポジションを直接更新
var newPosition = state.UpdatePosition(secId, "MyPortfolio", diff: 40);
Console.WriteLine($"現在のポジション: {newPosition}");

// クリア
state.Clear();
```

## PositionLifecycleTracker

[PositionLifecycleTracker](xref:StockSharp.Algo.Positions.PositionLifecycleTracker) クラスは、オープンからクローズまで（往復取引）のポジションの完全なライフサイクルを追跡します。これは、個別取引の分析、各ポジションの利益計算、レポート生成に役立ちます。

### 主な特徴

- **履歴**: `History` プロパティ（`IReadOnlyList<ReportPosition>`）には、完了したすべての往復ポジションが含まれます。
- **`RoundTripClosed` イベント**: ポジションがクローズされた（値がゼロに達した）場合、または反転した（ポジションの符号が変化した）場合に発生します。
- **`ProcessPosition` メソッド**: [Position](xref:StockSharp.BusinessEntities.Position) オブジェクトを受け取り、内部状態を更新します。

### 検出される状態

| 状態 | 説明 |
|-------|-------------|
| オープン | ポジションがゼロから非ゼロの値へ移行します |
| クローズ | ポジション値がゼロに達します |
| 反転 | ポジションの符号が変化します（例: ロングからショートへ） |

### 使用例

```cs
var tracker = new PositionLifecycleTracker();

tracker.RoundTripClosed += report =>
{
    Console.WriteLine($"ラウンドトリップ完了:");
    Console.WriteLine($"  開始: {report.OpenTime}");
    Console.WriteLine($"  終了: {report.CloseTime}");
};

// ポジション更新を処理
tracker.ProcessPosition(position);

// 履歴を表示
foreach (var report in tracker.History)
{
    Console.WriteLine($"  {report.OpenTime} -> {report.CloseTime}");
}
```

## PositionMessageAdapter

[PositionMessageAdapter](xref:StockSharp.Algo.Positions.PositionMessageAdapter) クラスは、メッセージストリームからポジションを自動計算するメッセージアダプターのラッパーです。内部コネクターインフラストラクチャ内で使用されます。

### 動作の仕組み

```cs
var innerAdapter = connector.Adapter;
var posManager = new PositionManager(byOrders: false, new PositionManagerState());
var posAdapter = new PositionMessageAdapter(innerAdapter, posManager);
```

このアダプターは注文約定メッセージと取引メッセージをインターセプトし、`PositionManager.ProcessMessage` を呼び出して、上流ハンドラー向けの対応する `PositionChangeMessage` インスタンスを生成します。

## 戦略内のポジション

[Strategy](xref:StockSharp.Algo.Strategies.Strategy) クラスでは、現在のポジションは `Position` プロパティを通じてアクセスされます。

```cs
// プライマリ銘柄の現在ポジション
decimal currentPosition = Position;

// ポジションをクローズ
if (Position > 0)
    SellMarket(Math.Abs(Position));
else if (Position < 0)
    BuyMarket(Math.Abs(Position));

// または組み込みメソッドを使用
ClosePosition();
```

戦略内の取引操作について詳しくは、[取引操作](strategies/trading_operations.md) セクションを参照してください。

## 関連項目

- [取引操作](strategies/trading_operations.md)
- [ポジション保護](strategies/take_profit_and_stop_loss.md)
- [目標ポジション管理](strategies/target_position_management.md)
- [レポート](strategies/reporting.md)

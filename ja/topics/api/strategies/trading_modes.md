# ストラテジーの取引モード

## 概要

`TradingMode` プロパティを使用すると、ストラテジーを完全に停止せずに取引アクティビティを制限できます。これはリスク管理に有用です。たとえば、新規ポジションのオープンを禁止し、既存ポジションのクローズだけを許可したり、注文送信を完全にブロックしたりできます。

モードは `StrategyTradingModes` 列挙体を使用して設定し、ストラテジーの実行中にも変更できます。

## StrategyTradingModes 列挙体

| 値 | 説明 |
|-------|-------------|
| `Full` | 完全な取引アクセス。注文に制限はありません。既定値です。 |
| `Disabled` | 取引は完全に禁止されます。すべての注文発注の試行は拒否されます。 |
| `CancelOrdersOnly` | 注文のキャンセルのみ許可されます。新規注文および既存注文の変更は禁止されます。 |
| `ReducePositionOnly` | 現在のポジションを減少させる注文のみ許可されます。新規ポジションのオープンおよび既存ポジションの増加は禁止されます。 |
| `LongOnly` | ロングポジションのみ許可されます。売りは既存のロングポジションをクローズする場合にのみ許可されます（売り数量は現在のポジションを超えることはできません）。ショートポジションのオープンは禁止されます。 |

## モードの設定

```csharp
// ストラテジー作成時
var strategy = new MyStrategy();
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// 稼働中の動的な変更
strategy.TradingMode = StrategyTradingModes.Disabled;
```

## モード確認ロジック

注文を登録しようとすると、ストラテジーは現在のモードを確認します。

- **`Disabled`** -- 注文は「取引は禁止されています」という理由で拒否されます。
- **`ReducePositionOnly`** -- 現在のポジションがゼロの場合、注文方向がポジション方向と一致する場合、または注文数量がポジションの絶対値を超える場合、注文は拒否されます。
- **`LongOnly`** -- 売り注文は、現在のポジションが正でない場合、または売り数量が現在のポジションを超える場合に拒否されます。
- **`Full`** -- 制限はありません。
- **`CancelOrdersOnly`** -- 注文のキャンセルのみ許可されます。

## IsFormedAndOnlineAndAllowTrading メソッド

`IsFormedAndOnlineAndAllowTrading` 拡張メソッドは、ストラテジーが形成済み（`IsFormed`）であり、オンライン状態（`IsOnline`）であり、取引モードが必要なアクションを許可していることを確認します。

```csharp
// 完全な取引の権限を確認（既定）
if (!IsFormedAndOnlineAndAllowTrading())
    return;

// 注文キャンセルのみの権限を確認
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
    CancelActiveOrders();

// ポジション削減の権限を確認
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly))
    return;
```

`required` パラメーターを指定して呼び出した場合の許可ロジック:

| 現在の TradingMode \ 必須 | `Full` | `CancelOrdersOnly` | `ReducePositionOnly` |
|-------------------------------|--------|---------------------|---------------------|
| `Full` | はい | はい | はい |
| `Disabled` | いいえ | いいえ | いいえ |
| `CancelOrdersOnly` | いいえ | はい | いいえ |
| `ReducePositionOnly` | いいえ | はい | はい |
| `LongOnly` | いいえ | はい | はい |

## 使用例

```csharp
public class TradingModeStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TradingModeStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        // ストラテジーが完全な取引を行う準備ができていることを確認
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.ClosePrice > candle.OpenPrice)
        {
            BuyMarket(Volume);
        }
        else if (candle.ClosePrice < candle.OpenPrice)
        {
            SellMarket(Volume);
        }
    }
}

// 制限付きでストラテジーを開始 -- ロングポジションのみ
var strategy = new TradingModeStrategy();
strategy.TradingMode = StrategyTradingModes.LongOnly;
strategy.Start();

// 後で -- ポジションクローズモードに切り替え
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// 取引を完全にブロック
strategy.TradingMode = StrategyTradingModes.Disabled;
```

この例では、ストラテジーは最初に `LongOnly` モードで動作し、買いおよびロングポジションのクローズのみを許可します。市場状況が変化したら、段階的にポジションをクローズするためにモードを `ReducePositionOnly` に切り替え、その後、取引アクティビティを完全に停止するために `Disabled` に切り替えることができます。

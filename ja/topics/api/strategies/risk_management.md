# リスク管理

## 概要

StockSharp のすべてのストラテジーには、取引リスクを自動的に制御できる組み込みの `RiskManager` があります。ルールがトリガーされると、マネージャーはポジションをクローズしたり、アクティブな注文をキャンセルしたり、ストラテジーの取引を完全に停止したりできます。

リスク マネージャーは、ストラテジーを通過するすべての取引メッセージを処理し、ルール条件が満たされると、ストラテジー ロジックに追加コードを記述しなくても指定されたアクションを自動的に実行します。

## ストラテジー プロパティ

### RiskManager

ルールのリストを管理する `IRiskManager` オブジェクトです。ストラテジーの初期化時に自動的に作成されます。

```csharp
// リスク マネージャーへアクセスする
IRiskManager manager = strategy.RiskManager;
```

### RiskRules

ルールのリストを読み取り、設定するための便利なプロパティです。

```csharp
// 現在のルールを読み取る
IEnumerable<IRiskRule> rules = strategy.RiskRules;

// 新しいルールを設定する
strategy.RiskRules = new IRiskRule[]
{
    new RiskPnLRule { PnL = -1000m, Action = RiskActions.StopTrading },
    new RiskPositionSizeRule { Position = 100m, Action = RiskActions.ClosePositions },
};
```

## トリガー アクション

`RiskActions` 列挙体は、実行可能なアクションを定義します。

| アクション | 説明 |
|--------|-------------|
| `ClosePositions` | すべてのオープン ポジションを成行注文でクローズする |
| `StopTrading` | ストラテジーの取引をブロックする |
| `CancelOrders` | すべてのアクティブ注文をキャンセルする |

ルールがトリガーされると、ストラテジーはルール名、そのパラメーター、実行されたアクションを含む警告をログに記録します。

## 利用可能なルール

### RiskPnLRule -- 損益管理

指定された P&L レベルに到達するとトリガーされます。正の値は利益を制御し、負の値は損失を制御します。

```csharp
// 5000 を超える損失で取引を停止する
new RiskPnLRule
{
    PnL = -5000m,
    Action = RiskActions.StopTrading
}
```

### RiskPositionSizeRule -- ポジション サイズ管理

指定されたポジション サイズに到達するとトリガーされます。

```csharp
// ポジション >= 100 のとき注文をキャンセルする
new RiskPositionSizeRule
{
    Position = 100m,
    Action = RiskActions.CancelOrders
}
```

### RiskOrderFreqRule -- 注文頻度管理

指定された間隔内の注文数が上限を超えるとトリガーされます。

```csharp
// 1 分あたり 50 件を超える注文で停止する
new RiskOrderFreqRule
{
    Count = 50,
    Interval = TimeSpan.FromMinutes(1),
    Action = RiskActions.StopTrading
}
```

### RiskOrderVolumeRule -- 注文数量管理

単一注文の数量が上限を超えるとトリガーされます。

### RiskOrderPriceRule -- 注文価格管理

注文価格が設定された境界を超えるとトリガーされます。

### RiskTradeVolumeRule -- 約定数量管理

単一約定の数量が上限を超えるとトリガーされます。

### RiskTradePriceRule -- 約定価格管理

約定価格が設定された境界を超えるとトリガーされます。

### RiskTradeFreqRule -- 約定頻度管理

間隔内の約定数が上限を超えるとトリガーされます。

### RiskPositionTimeRule -- ポジション保有時間管理

ポジションが設定された時間より長く保有されるとトリガーされます。

### RiskCommissionRule -- 手数料管理

総手数料が上限を超えるとトリガーされます。

### RiskSlippageRule -- スリッページ管理

スリッページ レベルが上限を超えるとトリガーされます。

### RiskErrorRule -- エラー管理

一定数のエラーが蓄積されるとトリガーされます。

### RiskOrderErrorRule -- 注文登録エラー管理

注文登録エラーが蓄積されるとトリガーされます。

## 使用例

```csharp
public class RiskAwareStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public RiskAwareStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // リスク管理ルールを設定する
        RiskRules = new IRiskRule[]
        {
            // 10000 を超える損失でポジションをクローズする
            new RiskPnLRule
            {
                PnL = -10000m,
                Action = RiskActions.ClosePositions
            },

            // ポジションが 500 コントラクトを超えたとき取引を停止する
            new RiskPositionSizeRule
            {
                Position = 500m,
                Action = RiskActions.StopTrading
            },

            // 頻度が 1 分あたり 100 件を超えたとき注文をキャンセルする
            new RiskOrderFreqRule
            {
                Count = 100,
                Interval = TimeSpan.FromMinutes(1),
                Action = RiskActions.CancelOrders
            },
        };

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // 取引ロジック
        if (candle.OpenPrice < candle.ClosePrice && Position <= 0)
            BuyMarket(Volume + Math.Abs(Position));
        else if (candle.OpenPrice > candle.ClosePrice && Position >= 0)
            SellMarket(Volume + Math.Abs(Position));
    }
}
```

この例では、ストラテジー開始時にリスク管理ルールが設定されます。10000 の損失に到達すると、ポジションは自動的にクローズされます。ポジション サイズが 500 コントラクトを超えると、取引はブロックされます。注文が頻繁に発注されすぎると、アクティブ注文はキャンセルされます。これらすべてのチェックは、`ProcessCandle` メソッドに追加コードを記述しなくても自動的に実行されます。

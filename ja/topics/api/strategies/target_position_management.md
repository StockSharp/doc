# 目標ポジション管理

## 概要

目標ポジション管理システムを使用すると、戦略は望ましいポジションサイズを宣言的に指定でき、プラットフォームはその水準に到達するために必要な注文を自動的に発注します。取引の数量と方向を手動で計算する代わりに、単に `SetTargetPosition(10)` を呼び出します。すると、マネージャーが買うべきか売るべきか、どの数量で行うべきかを判断します。

主要コンポーネントは `PositionTargetManager` クラスで、次を自動的に行います。

- 現在ポジションと目標ポジションの差を計算
- 注文の方向と数量を決定
- 注文の執行、キャンセル、エラーを処理
- 失敗時の再試行をサポート

## 戦略メソッド

### SetTargetPosition

目標ポジションを設定します。2 つの呼び出しバリアントを利用できます。

```csharp
// 戦略のメイン銘柄とポートフォリオ用
SetTargetPosition(decimal target);

// 任意の銘柄とポートフォリオ用
SetTargetPosition(Security security, Portfolio portfolio, decimal target);
```

`target` が現在ポジションより大きい場合、マネージャーは買い注文を発注します。小さい場合は売り注文を発注します。ポジションがすでに目標と等しい場合 (`PositionTolerance` を考慮)、何も行われません。

### CancelTargetPosition

以前に設定した目標ポジションをキャンセルし、関連するすべてのアクティブ注文を停止します。

```csharp
// 戦略のメイン銘柄とポートフォリオ用
CancelTargetPosition();

// 任意の銘柄とポートフォリオ用
CancelTargetPosition(Security security, Portfolio portfolio);
```

### GetTargetPosition

現在の目標ポジション値を返します。目標が設定されていない場合は `null` を返します。

```csharp
decimal? target = GetTargetPosition();
decimal? target = GetTargetPosition(security, portfolio);
```

## TargetPositionManager プロパティ

`TargetPositionManager` プロパティは、微調整のために `PositionTargetManager` オブジェクトへの直接アクセスを提供します。

```csharp
// 注文エラー時の最大再試行回数 (既定値は 3)
TargetPositionManager.MaxRetries = 5;

// 目標ポジションに到達したかどうかを判断する許容差
TargetPositionManager.PositionTolerance = 0.01m;

// 注文タイプ (既定値は Market)
TargetPositionManager.OrderType = OrderTypes.Market;
```

マネージャーは次のイベントを生成します。

- `TargetReached` -- 目標ポジションに到達しました
- `Error` -- 注文執行中にエラーが発生しました
- `OrderRegistered` -- マネージャーが注文を登録しました

## TargetAlgoFactory プロパティ

`TargetAlgoFactory` プロパティでは、ポジション変更アルゴリズムのファクトリーを設定できます。既定では、成行注文を作成する `MarketOrderAlgo` が使用されます。

```csharp
// 成行注文の代わりにカスタムアルゴリズムを使用
TargetAlgoFactory = (side, volume) => new MyCustomAlgo(side, volume);
```

## 使用例

```csharp
public class TargetPositionStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TargetPositionStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // 目標ポジションマネージャーを設定
        TargetPositionManager.MaxRetries = 5;
        TargetPositionManager.TargetReached += (sec, pf) =>
        {
            this.AddInfoLog("目標ポジションに到達しました: {0}, {1}", sec, pf);
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

        if (candle.OpenPrice < candle.ClosePrice)
        {
            // 陽線 -- 買い用の目標ポジションを設定
            SetTargetPosition(Volume);
        }
        else if (candle.OpenPrice > candle.ClosePrice)
        {
            // 陰線 -- 売り用の目標ポジションを設定
            SetTargetPosition(-Volume);
        }
    }
}
```

この例では、戦略は数量と方向の手動計算を扱いません。単に望ましいポジションサイズを宣言し、`PositionTargetManager` がすべての注文発注処理を担当します。


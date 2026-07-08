# 高レベルサブスクリプション

## 概要

`Strategy` クラスは、高レベルのマーケットデータ購読メソッドとして `SubscribeCandles`、`SubscribeTicks`、`SubscribeLevel1`、`SubscribeOrderBook` を提供します。これらのメソッドは `ISubscriptionHandler<T>` オブジェクトを返し、データハンドラーやインジケーターを便利な fluent スタイルでバインドできます。

`Subscription` オブジェクトを手動で作成して `Subscribe()` を呼び出す方法とは異なり、高レベルメソッドには次の特徴があります。

- 適切なパラメーターでサブスクリプションを自動作成する
- ハンドラーをバインドするための型付き `ISubscriptionHandler<T>` を提供する
- `Bind` メソッドを介してインジケーターシステムと統合する
- チャートへの自動描画をサポートする
- サブスクリプションのライフサイクルを適切に管理する

## サブスクリプションメソッド

### SubscribeCandles

ローソク足を購読します。タイムフレームまたは `DataType` を受け取ります。

```csharp
// タイムフレームで購読
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    TimeSpan tf,
    bool isFinishedOnly = true,
    Security security = default);

// DataType で購読 (すべてのローソク足タイプをサポート)
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    DataType dt,
    bool isFinishedOnly = true,
    Security security = default);

// 作成済みの Subscription オブジェクトで購読
ISubscriptionHandler<ICandleMessage> SubscribeCandles(Subscription subscription);
```

`isFinishedOnly` パラメーターはデフォルトで `true` です。この場合、ハンドラーは完成済みのローソク足のみを受け取ります。

### SubscribeTicks

ティック約定を購読します。

```csharp
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Security security = null);
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Subscription subscription);
```

### SubscribeLevel1

Level1 データ (最良買気配/売気配価格、直近約定、その他のフィールド) を購読します。

```csharp
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Security security = null);
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Subscription subscription);
```

### SubscribeOrderBook

板情報を購読します。

```csharp
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Security security = null);
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Subscription subscription);
```

`security` パラメーターを指定しない場合は、ストラテジーの `Security` が使用されます。

## ISubscriptionHandler インターフェイス

`ISubscriptionHandler<T>` オブジェクトは、次のメソッドを提供します。

### Start / Stop

サブスクリプションの開始と停止:

```csharp
handler.Start();   // Subscribe を呼び出す
handler.Stop();    // UnSubscribe を呼び出す
```

### Bind (インジケーターなし)

単純なデータハンドラーをバインドします。

```csharp
handler.Bind(Action<T> callback);
```

### Bind (インジケーターあり)

1 つ以上のインジケーターを持つハンドラーをバインドします。インジケーターは受信データを自動的に処理し、ハンドラーは計算済みの値を受け取ります。

```csharp
// 1 つのインジケーター -- decimal 値
handler.Bind(IIndicator indicator, Action<T, decimal> callback);

// 2 つのインジケーター
handler.Bind(IIndicator ind1, IIndicator ind2, Action<T, decimal, decimal> callback);

// 最大 8 つのインジケーター
handler.Bind(ind1, ind2, ind3, ..., callback);

// インジケーターの配列
handler.Bind(IIndicator[] indicators, Action<T, decimal[]> callback);
```

`Bind` を使用したハンドラーは、すべてのインジケーターが空でない値を返した場合にのみ呼び出されます。

### BindWithEmpty

`Bind` と似ていますが、インジケーターが空の値を返した場合でもハンドラーが呼び出されます。値は `decimal?` として表されます。

```csharp
handler.BindWithEmpty(IIndicator indicator, Action<T, decimal?> callback);
```

### BindEx

抽出された `decimal` ではなく、完全な `IIndicatorValue` オブジェクトへアクセスできます。

```csharp
handler.BindEx(IIndicator indicator, Action<T, IIndicatorValue> callback, bool allowEmpty = false);
```

## 例: インジケーターを使用するストラテジー

```csharp
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<int> _shortPeriod;
    private readonly StrategyParam<int> _longPeriod;
    private readonly StrategyParam<DataType> _candleType;

    public int ShortPeriod
    {
        get => _shortPeriod.Value;
        set => _shortPeriod.Value = value;
    }

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public SmaStrategy()
    {
        _shortPeriod = Param(nameof(ShortPeriod), 10);
        _longPeriod = Param(nameof(LongPeriod), 20);
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var shortSma = new SimpleMovingAverage { Length = ShortPeriod };
        var longSma = new SimpleMovingAverage { Length = LongPeriod };

        var subscription = SubscribeCandles(CandleType);

        // 2 つのインジケーターをバインド -- 両方のインジケーターが
        // 形成されたときにハンドラーが呼び出される
        subscription
            .Bind(shortSma, longSma, (candle, shortValue, longValue) =>
            {
                if (!IsFormedAndOnlineAndAllowTrading())
                    return;

                if (shortValue > longValue && Position <= 0)
                    BuyMarket(Volume + Math.Abs(Position));
                else if (shortValue < longValue && Position >= 0)
                    SellMarket(Volume + Math.Abs(Position));
            })
            .Start();

        // チャート設定
        var area = CreateChartArea();
        if (area != null)
        {
            DrawCandles(area, subscription);
            DrawIndicator(area, shortSma);
            DrawIndicator(area, longSma);
            DrawOwnTrades(area);
        }
    }
}
```

## 例: ティックサブスクリプション

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeTicks()
        .Bind(tick =>
        {
            if (!IsFormedAndOnlineAndAllowTrading())
                return;

            this.AddInfoLog("Tick: price={0}, volume={1}", tick.Price, tick.Volume);
        })
        .Start();
}
```

## 例: 板情報サブスクリプション

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeOrderBook()
        .Bind(book =>
        {
            var bestBid = book.GetBestBid();
            var bestAsk = book.GetBestAsk();

            if (bestBid != null && bestAsk != null)
            {
                var spread = bestAsk.Price - bestBid.Price;
                this.AddInfoLog("Spread: {0}", spread);
            }
        })
        .Start();
}
```

## 手動でのサブスクリプション作成との違い

| 観点 | 手動サブスクリプション | 高レベルメソッド |
|--------|-------------------|-------------------|
| 作成 | `new Subscription(DataType, Security)` | `SubscribeCandles(tf)` |
| データ処理 | コネクターイベントへの購読 | `Bind(callback)` |
| インジケーター | 手動での `indicator.Process()` 呼び出し | `Bind(indicator, callback)` により自動 |
| インジケーター登録 | `Indicators` への手動追加 | `Bind` 時に自動 |
| チャート描画 | `IChart` との手動統合 | `DrawCandles`, `DrawIndicator` |

高レベルメソッドは、コード量を大幅に削減し、エラーの可能性を下げるため、ほとんどのストラテジーでの使用が推奨されます。

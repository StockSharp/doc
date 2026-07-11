# ストラテジー レポート

## 概要

StockSharp は、ストラテジーの取引結果に対するレポート生成システムを提供します。このシステムは 2 つの主要コンポーネントで構成されています。

- **`IReportSource`** -- レポートのデータ ソース (ストラテジー パラメーター、注文、約定、ポジション、統計) を記述するインターフェイス。
- **`IReportGenerator`** -- さまざまな形式 (CSV、JSON、XML、Excel) をサポートするレポート ジェネレーター インターフェイス。

`Strategy` クラスは `IReportSource` インターフェイスを実装しているため、ストラテジーをレポート ジェネレーターへ直接渡すことができます。

## IReportSource インターフェイス

`IReportSource` インターフェイスは、レポート生成に必要なすべてのデータを提供します。

| プロパティ | 型 | 説明 |
|----------|------|-------------|
| `Name` | `string` | ストラテジー名 |
| `TotalWorkingTime` | `TimeSpan` | 総稼働時間 |
| `Commission` | `decimal?` | 総手数料 |
| `Position` | `decimal` | 現在のポジション |
| `PnL` | `decimal` | 総損益 |
| `Slippage` | `decimal?` | 総スリッページ |
| `Latency` | `TimeSpan?` | 総レイテンシ |
| `Parameters` | `IEnumerable<(string, object)>` | ストラテジー パラメーター |
| `StatisticParameters` | `IEnumerable<(string, object)>` | 統計パラメーター |
| `Orders` | `IEnumerable<ReportOrder>` | 注文 |
| `OwnTrades` | `IEnumerable<ReportTrade>` | 自己約定 |
| `Positions` | `IEnumerable<ReportPosition>` | ポジションのラウンドトリップ |

データを読み取る前に、ソースの内部状態を同期するために `Prepare()` メソッドが呼び出されます。

## ReportSource クラス

`ReportSource` は、`Strategy` クラスに結び付かない `IReportSource` の独立した実装です。レポート用のデータ ソースを手動で構築できます。

```csharp
var source = new ReportSource();
source.Name = "自分の戦略";
source.PnL = 15000m;
source.TotalWorkingTime = TimeSpan.FromHours(8);

source.AddParameter("時間枠", "5 分");
source.AddStatisticParameter("シャープレシオ", 1.85);

source.AddOrder(new ReportOrder(
    Id: 123,
    TransactionId: 456,
    SecurityId: securityId,
    Side: Sides.Buy,
    Time: DateTime.UtcNow,
    Price: 100m,
    State: OrderStates.Done,
    Balance: 0,
    Volume: 10,
    Type: OrderTypes.Limit
));
```

### データ集計

注文や約定の数が多い場合、`ReportSource` はレポート サイズを小さくするためにデータを自動的に集計します。

```csharp
// 自動集計しきい値 (既定値は 10000)
source.MaxOrdersBeforeAggregation = 5000;
source.MaxTradesBeforeAggregation = 5000;

// グループ化間隔 (既定値は 1 時間)
source.AggregationInterval = TimeSpan.FromMinutes(30);

// 手動集計
source.AggregateOrders(TimeSpan.FromHours(1));
source.AggregateTrades(TimeSpan.FromHours(1));
```

集計時には、注文と約定は時間間隔、銘柄、方向でグループ化されます。数量は合計され、価格は加重平均として計算されます。

## PositionLifecycleTracker

`PositionLifecycleTracker` はポジションのライフサイクルを追跡し、ラウンドトリップ、つまりポジションの開始と終了の記録を生成します。

ラウンドトリップは次の場合に記録されます。
- ポジションが完全にクローズされた (値がゼロになる)
- ポジション反転が発生した (符号が変わる)

`Strategy` クラスでは、トラッカーは自動的に統合されます。完了したラウンドトリップは、`RoundTripClosed` イベントを通じて `ReportSource` に追加されます。

```csharp
var tracker = new PositionLifecycleTracker();

// ラウンドトリップ クローズ時のイベント
tracker.RoundTripClosed += roundTrip =>
{
    Console.WriteLine($"ポジション終了: {roundTrip.SecurityId}, " +
        $"開始: {roundTrip.OpenTime} 価格 {roundTrip.OpenPrice}, " +
        $"終了: {roundTrip.CloseTime} 価格 {roundTrip.ClosePrice}, " +
        $"最大数量: {roundTrip.MaxPosition}");
};

// ポジション更新を処理する
tracker.ProcessPosition(position);

// ラウンドトリップ履歴へアクセスする
IReadOnlyList<ReportPosition> history = tracker.History;
```

## レポート ジェネレーター

次のジェネレーターを利用できます。

| ジェネレーター | 形式 | 説明 |
|-----------|--------|-------------|
| `CsvReportGenerator` | CSV | 区切り文字付きのテキスト形式 |
| `JsonReportGenerator` | JSON | 構造化 JSON |
| `XmlReportGenerator` | XML | XML 形式 |
| `ExcelReportGenerator` | Excel | Excel 形式 (`IExcelWorkerProvider` が必要) |

すべてのジェネレーターは `BaseReportGenerator` から継承され、含めるセクションの設定をサポートします。

```csharp
var generator = new CsvReportGenerator();

// レポート セクションを設定する
generator.IncludeOrders = true;
generator.IncludeTrades = true;
generator.IncludePositions = true;
generator.Encoding = Encoding.UTF8;
```

## ストラテジーからレポートを生成する

`Strategy` は `IReportSource` を実装しているため、レポートを直接生成できます。

```csharp
// ストラテジー自体がデータ ソース
var generator = new JsonReportGenerator();

using var stream = File.Create("report.json");
await generator.Generate(strategy, stream, CancellationToken.None);
```

別のデータ ソースを使う場合:

```csharp
var source = new ReportSource();
source.Name = strategy.Name;
source.PnL = strategy.PnL;
source.TotalWorkingTime = strategy.TotalWorkingTime;

// トラッカーからポジションを追加する
source.AddPositions(tracker.History);

var generator = new CsvReportGenerator();
using var stream = File.Create("report.csv");
await generator.Generate(source, stream, CancellationToken.None);
```

## 例: 停止時にレポートを生成するストラテジー

```csharp
public class ReportingStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public ReportingStrategy()
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
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // 取引ロジック...
    }

    protected override void OnStopped()
    {
        // ストラテジー停止時にレポートを生成する
        var generator = new CsvReportGenerator();

        using var stream = File.Create($"report_{Name}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        generator.Generate(this, stream, CancellationToken.None).AsTask().Wait();

        base.OnStopped();
    }
}
```

この例では、ストラテジーは停止時に CSV レポートを自動的に作成します。レポートには、ストラテジー パラメーター、統計、注文、約定、ポジションのラウンドトリップが含まれます。

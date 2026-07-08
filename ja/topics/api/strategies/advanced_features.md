# 高度な戦略機能

## 概要

`Strategy` クラスは、動作を細かく調整するための追加プロパティを多数提供します。注文コメントの自動設定、取引スケジュール、統計用のリスクフリーレート、指標のデータソース、履歴期間管理などです。

## CommentMode -- 注文コメント

`CommentMode` プロパティは、戦略が送信するすべての注文について、`Order.Comment` フィールドの自動入力を制御します。これにより、どの戦略が注文を作成したかを識別できます。これは、同じ口座で複数の戦略を同時に実行している場合に特に有用です。

### StrategyCommentModes 列挙型

| 値 | 説明 |
|-------|-------------|
| `Disabled` | コメントは自動入力されません。デフォルト値です。 |
| `Id` | コメントは `Strategy.Id`（一意の GUID 識別子）に設定されます。 |
| `Name` | コメントは `Strategy.Name`（戦略名）に設定されます。 |

### 例

```csharp
public class CommentStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // すべての注文に戦略名をタグ付けします
        CommentMode = StrategyCommentModes.Name;

        // または正確な紐付けのために識別子を使用します
        // CommentMode = StrategyCommentModes.Id;
    }
}
```

`Name` 値を使用し、戦略名が "SMA Crossover" の場合、すべての注文のコメントは "SMA Crossover" になります。これにより、取引ジャーナルでこの戦略の注文をフィルターできます。

## WorkingTime -- 稼働スケジュール

`WorkingTime` プロパティは、戦略がアクティブになるスケジュールを設定します。指定された時間間隔外では、戦略は自動的に活動を制限できます。

```csharp
public class ScheduledStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // 稼働時間を構成
        WorkingTime = new WorkingTime
        {
            Periods = new List<WorkingTimePeriod>
            {
                new WorkingTimePeriod
                {
                    Till = DateTime.MaxValue,
                    Times = new List<Range<TimeSpan>>
                    {
                        // 10:00 から 18:00 まで取引
                        new Range<TimeSpan>(
                            TimeSpan.FromHours(10),
                            TimeSpan.FromHours(18))
                    }
                }
            }
        };
    }
}
```

`TotalWorkingTime` プロパティ（読み取り専用）は、開始以降の戦略の総稼働時間を示します。これは戦略の停止および再起動時に自動的に計算されます。

## RiskFreeRate -- リスクフリーレート

`RiskFreeRate` プロパティは、統計計算で使用される年率のリスクフリーレートを設定します。主にシャープレシオとソルティノレシオに使用されます。

```csharp
var strategy = new MyStrategy();

// 年率 5% のリスクフリーレート
strategy.RiskFreeRate = 0.05m;
```

この値は、戦略の統計マネージャーが初期化されるとき、`IRiskFreeRateStatisticParameter` を実装するすべての統計パラメーターへ自動的に渡されます。

## IndicatorSource -- 指標データソース

`IndicatorSource` プロパティは、明示的にソースが指定されていないすべての戦略指標について、`IIndicator.Source` プロパティのデフォルト値を設定します。指標の入力データとして使用する `Level1Fields` フィールドを定義します。

```csharp
var strategy = new MyStrategy();

// すべての指標はデフォルトで最終取引価格を使用します
strategy.IndicatorSource = Level1Fields.LastTradePrice;

// または平均価格
// strategy.IndicatorSource = Level1Fields.AveragePrice;
```

プロパティが `null`（デフォルト値）の場合、指標はそれぞれ独自のデータソースを使用します。

## HistoryCalculated -- 計算された履歴期間

仮想プロパティ `HistoryCalculated` により、戦略は指標のウォームアップに必要な履歴データ期間をプログラムで決定できます。これは `TimeSpan?`、つまり履歴期間の長さ、または期間が指定されていない場合は `null` を返します。

```csharp
public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _longPeriod;

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public SmaCrossStrategy()
    {
        _longPeriod = Param(nameof(LongPeriod), 50);
    }

    // 必要な履歴期間の自動計算
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(LongPeriod * 2);
}
```

`HistoryCalculated` は、`HistorySize` プロパティのコード計算版です。違いは、`HistorySize` はユーザーが戦略パラメーターとして設定するのに対し、`HistoryCalculated` は戦略パラメーター（たとえば指標期間）に基づいてプログラムで計算される点です。

## 例: すべての高度な設定を持つ戦略

```csharp
public class AdvancedStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<int> _smaPeriod;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public int SmaPeriod
    {
        get => _smaPeriod.Value;
        set => _smaPeriod.Value = value;
    }

    public AdvancedStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _smaPeriod = Param(nameof(SmaPeriod), 20);
    }

    // 履歴期間の自動計算
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(SmaPeriod * 2);

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // 注文コメント -- 戦略名
        CommentMode = StrategyCommentModes.Name;

        // Sharpe 計算用のリスクフリーレート
        RiskFreeRate = 0.05m;

        // 指標のデータソース
        IndicatorSource = Level1Fields.LastTradePrice;

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
}
```

この例では、戦略は説明したすべての機能を使用しています。注文へ自動的にコメントを付け、統計用のリスクフリーレートを設定し、指標のデータソースを確立し、必要な履歴期間を計算します。

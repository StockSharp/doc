# タイマーシステム

## 概要

StockSharp の戦略は、指定した時間間隔でアクションを実行できる組み込みタイマーシステムをサポートしています。タイマーはコネクターの `WhenIntervalElapsed` 機構に基づいており、ライブ取引中とバックテスト中 (エミュレーターの仮想時刻を使用) の両方で正しく動作します。

タイマーは次の用途に便利です。

- 定期的な市場状況のチェック
- 固定間隔でのポートフォリオリバランス
- 時間によるポジションのクローズ
- 戦略状態の監視

## ITimerHandler インターフェイス

タイマーは `ITimerHandler` インターフェイスを通じて管理され、次のメンバーを提供します。

| メンバー | 説明 |
|--------|-------------|
| `Start()` | タイマーを開始します。メソッドチェーン用に `ITimerHandler` を返します |
| `Stop()` | タイマーを停止します。メソッドチェーン用に `ITimerHandler` を返します |
| `Interval` | タイマーの発火間隔 (読み取りおよび書き込み) |
| `IsStarted` | タイマーが実行中かどうかを示します |
| `Dispose()` | リソースを解放し、タイマーを停止します |

## タイマー作成メソッド

### CreateTimer

タイマーを作成しますが、開始はしません。開始は別途 `Start()` 呼び出しで行います。

```csharp
ITimerHandler CreateTimer(TimeSpan interval, Action callback);
```

### StartTimer

タイマーを作成し、ただちに開始します。`CreateTimer(...).Start()` と同等です。

```csharp
ITimerHandler StartTimer(TimeSpan interval, Action callback);
```

間隔は正の値 (`> TimeSpan.Zero`) でなければならず、それ以外の場合は例外がスローされます。

## タイマー管理

タイマーは停止して再開できます。

```csharp
var timer = CreateTimer(TimeSpan.FromMinutes(1), MyCallback);

// 開始
timer.Start();

// 停止
timer.Stop();

// 間隔を変更
timer.Interval = TimeSpan.FromMinutes(5);

// 再度開始
timer.Start();

// リソースを解放
timer.Dispose();
```

## 使用例

```csharp
public class TimerStrategy : Strategy
{
    private ITimerHandler _checkTimer;
    private ITimerHandler _closeTimer;

    private readonly StrategyParam<TimeSpan> _checkInterval;
    private readonly StrategyParam<TimeSpan> _maxHoldTime;

    public TimeSpan CheckInterval
    {
        get => _checkInterval.Value;
        set => _checkInterval.Value = value;
    }

    public TimeSpan MaxHoldTime
    {
        get => _maxHoldTime.Value;
        set => _maxHoldTime.Value = value;
    }

    public TimerStrategy()
    {
        _checkInterval = Param(nameof(CheckInterval), TimeSpan.FromMinutes(1));
        _maxHoldTime = Param(nameof(MaxHoldTime), TimeSpan.FromHours(4));
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // 定期的な市場状況チェック用タイマー
        _checkTimer = StartTimer(CheckInterval, OnCheckTimer);

        // 強制ポジションクローズ用タイマー (作成するが開始しない)
        _closeTimer = CreateTimer(MaxHoldTime, OnCloseTimer);

        var subscription = SubscribeCandles(TimeSpan.FromMinutes(5));

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void OnCheckTimer()
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        this.AddInfoLog("Checking market conditions at {0}", CurrentTime);

        // 定期的なポジション状態チェック
        if (Position != 0)
        {
            this.AddInfoLog("Current position: {0}", Position);
        }
    }

    private void OnCloseTimer()
    {
        if (Position != 0)
        {
            this.AddInfoLog("Position hold time expired, closing");
            ClosePosition();

            // 発火後にクローズタイマーを停止
            _closeTimer.Stop();
        }
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.OpenPrice < candle.ClosePrice && Position == 0)
        {
            BuyMarket();

            // 強制クローズタイマーを開始
            _closeTimer.Start();
        }
    }
}
```

この例では、2 つのタイマーを使用しています。1 つは定期的な状態チェック用 (`StartTimer` -- 作成してただちに開始)、もう 1 つはポジション保有時間を制限するためのもの (`CreateTimer` -- 作成するが、ポジションに入ったときにのみ開始) です。

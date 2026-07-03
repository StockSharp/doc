# 计时器系统

## 概览

StockSharp 中的策略支持内置的计时器系统，可在指定的时间间隔执行操作。计时器基于连接器的 `WhenIntervalElapsed` 机制，并且在实时交易和回测（使用模拟器的虚拟时间）中都能正常工作。

计时器的用途包括：

- 定期市场状况检查
- 固定间隔的投资组合再平衡
- 按时间平仓
- 策略状态监控

## ITimerHandler 接口

计时器通过 `ITimerHandler` 接口管理，该接口提供以下成员：

| 成员 | 描述 |
|--------|-------------|
| `Start()` | 启动计时器。返回 `ITimerHandler` 以进行方法链 |
| `Stop()` | 停止计时器。返回 `ITimerHandler` 以进行方法链 |
| `Interval` | 定时器触发间隔（读写） |
| `IsStarted` | 指示计时器是否正在运行 |
| `Dispose()` | 释放资源并停止计时器 |

## 计时器创建方法

### 创建计时器

创建一个计时器但不启动它。启动由另一个 `Start()` 调用完成：

```csharp
ITimerHandler CreateTimer(TimeSpan interval, Action callback);
```

### 开始计时器

创建并立即启动计时器。等同于 `CreateTimer(...).Start()`：

```csharp
ITimerHandler StartTimer(TimeSpan interval, Action callback);
```

间隔必须为正值 (`> TimeSpan.Zero`)，否则将抛出异常。

## 定时器管理

计时器可以停止并重新启动：

```csharp
var timer = CreateTimer(TimeSpan.FromMinutes(1), MyCallback);

// Start
timer.Start();

// Stop
timer.Stop();

// Change interval
timer.Interval = TimeSpan.FromMinutes(5);

// Start again
timer.Start();

// Release resources
timer.Dispose();
```

## 使用示例

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

        // Timer for periodic market condition checks
        _checkTimer = StartTimer(CheckInterval, OnCheckTimer);

        // Timer for forced position closing (created but not started)
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

        // Periodic position state check
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

            // Stop the close timer after it fires
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

            // Start the forced close timer
            _closeTimer.Start();
        }
    }
}
```

在此示例中，使用了两个计时器：一个用于定期状态检查（`StartTimer` —— 已创建并立即启动），另一个用于限制持仓时间（`CreateTimer` —— 已创建但仅在进入持仓时启动）。

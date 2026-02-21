# Timer System

## Overview

Strategies in StockSharp support a built-in timer system that allows executing actions at specified time intervals. Timers are based on the connector's `WhenIntervalElapsed` mechanism and work correctly both in live trading and during backtesting (using the emulator's virtual time).

Timers are convenient for:

- Periodic market condition checks
- Portfolio rebalancing at fixed intervals
- Closing positions by time
- Strategy state monitoring

## ITimerHandler Interface

The timer is managed through the `ITimerHandler` interface, which provides the following members:

| Member | Description |
|--------|-------------|
| `Start()` | Starts the timer. Returns `ITimerHandler` for method chaining |
| `Stop()` | Stops the timer. Returns `ITimerHandler` for method chaining |
| `Interval` | Timer trigger interval (read and write) |
| `IsStarted` | Indicates whether the timer is running |
| `Dispose()` | Releases resources and stops the timer |

## Timer Creation Methods

### CreateTimer

Creates a timer but does not start it. Starting is done by a separate `Start()` call:

```csharp
ITimerHandler CreateTimer(TimeSpan interval, Action callback);
```

### StartTimer

Creates and immediately starts a timer. Equivalent to `CreateTimer(...).Start()`:

```csharp
ITimerHandler StartTimer(TimeSpan interval, Action callback);
```

The interval must be a positive value (`> TimeSpan.Zero`), otherwise an exception will be thrown.

## Timer Management

A timer can be stopped and restarted:

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

## Usage Example

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

In this example, two timers are used: one for periodic state checking (`StartTimer` -- created and immediately started), and another for limiting the position hold time (`CreateTimer` -- created but started only upon entering a position).

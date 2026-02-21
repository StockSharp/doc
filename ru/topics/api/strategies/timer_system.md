# Система таймеров

## Обзор

Стратегии в StockSharp поддерживают встроенную систему таймеров, позволяющую выполнять действия через определенные интервалы времени. Таймеры работают на основе механизма `WhenIntervalElapsed` коннектора и корректно функционируют как в реальной торговле, так и при бэктестинге (используя виртуальное время эмулятора).

Таймеры удобны для:

- Периодической проверки условий рынка
- Ребалансировки портфеля через фиксированные интервалы
- Закрытия позиций по времени
- Мониторинга состояния стратегии

## Интерфейс ITimerHandler

Таймер управляется через интерфейс `ITimerHandler`, который предоставляет следующие члены:

| Член | Описание |
|------|----------|
| `Start()` | Запускает таймер. Возвращает `ITimerHandler` для цепочки вызовов |
| `Stop()` | Останавливает таймер. Возвращает `ITimerHandler` для цепочки вызовов |
| `Interval` | Интервал срабатывания таймера (чтение и запись) |
| `IsStarted` | Признак того, что таймер запущен |
| `Dispose()` | Освобождает ресурсы и останавливает таймер |

## Методы создания таймеров

### CreateTimer

Создает таймер, но не запускает его. Запуск производится отдельным вызовом `Start()`:

```csharp
ITimerHandler CreateTimer(TimeSpan interval, Action callback);
```

### StartTimer

Создает и сразу запускает таймер. Эквивалентно `CreateTimer(...).Start()`:

```csharp
ITimerHandler StartTimer(TimeSpan interval, Action callback);
```

Интервал должен быть положительным значением (`> TimeSpan.Zero`), иначе будет выброшено исключение.

## Управление таймером

Таймер можно останавливать и запускать повторно:

```csharp
var timer = CreateTimer(TimeSpan.FromMinutes(1), MyCallback);

// Запустить
timer.Start();

// Остановить
timer.Stop();

// Изменить интервал
timer.Interval = TimeSpan.FromMinutes(5);

// Запустить снова
timer.Start();

// Освободить ресурсы
timer.Dispose();
```

## Пример использования

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

        // Таймер для периодической проверки условий рынка
        _checkTimer = StartTimer(CheckInterval, OnCheckTimer);

        // Таймер для принудительного закрытия позиции (создаем, но не запускаем)
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

        this.AddInfoLog("Проверка условий рынка в {0}", CurrentTime);

        // Периодическая проверка состояния позиции
        if (Position != 0)
        {
            this.AddInfoLog("Текущая позиция: {0}", Position);
        }
    }

    private void OnCloseTimer()
    {
        if (Position != 0)
        {
            this.AddInfoLog("Время удержания позиции истекло, закрытие");
            ClosePosition();

            // Остановить таймер закрытия после срабатывания
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

            // Запустить таймер принудительного закрытия
            _closeTimer.Start();
        }
    }
}
```

В этом примере используются два таймера: один для периодической проверки состояния (`StartTimer` -- создан и сразу запущен), второй для ограничения времени удержания позиции (`CreateTimer` -- создан, но запускается только при входе в позицию).

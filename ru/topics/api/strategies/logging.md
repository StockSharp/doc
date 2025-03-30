# Логирование в стратегии

В StockSharp класс [Strategy](xref:StockSharp.Algo.Strategies.Strategy) унаследован от [BaseLogReceiver](xref:Ecng.Logging.BaseLogReceiver), что позволяет встроенными средствами вести логирование всех действий и событий, происходящих в процессе работы торговой стратегии.

## Уровни логирования

В StockSharp поддерживаются следующие уровни логирования (перечислены в порядке увеличения важности):

1. Verbose (подробный) - самый детальный уровень логирования для трассировки
2. Debug (отладка) - сообщения для отладки
3. Info (информация) - обычные информационные сообщения
4. Warning (предупреждение) - предупреждения о потенциальных проблемах
5. Error (ошибка) - сообщения об ошибках

## Методы логирования в стратегии

Стратегия предоставляет следующие методы для записи сообщений в лог:

### LogVerbose

Метод [LogVerbose](xref:Ecng.Logging.BaseLogReceiver.LogVerbose(System.String,System.Object[])) предназначен для записи подробных сообщений при трассировке:

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);
    
    LogVerbose("Стратегия запущена с параметрами: Long SMA={0}, Short SMA={1}", LongSmaLength, ShortSmaLength);
    
    // ...
}
```

### LogDebug

Метод [LogDebug](xref:Ecng.Logging.BaseLogReceiver.LogDebug(System.String,System.Object[])) используется для отладочных сообщений:

```cs
private void ProcessCandle(ICandleMessage candle)
{
    LogDebug("Обработка свечи: {0}, Open={1}, Close={2}, High={3}, Low={4}, Volume={5}", 
        candle.OpenTime, candle.OpenPrice, candle.ClosePrice, candle.HighPrice, candle.LowPrice, candle.TotalVolume);
    
    // ...
}
```

### LogInfo

Метод [LogInfo](xref:Ecng.Logging.BaseLogReceiver.LogInfo(System.String,System.Object[])) предназначен для обычных информационных сообщений:

```cs
private void CalculateSignal(decimal shortSma, decimal longSma)
{
    bool isShortGreaterThanLong = shortSma > longSma;
    
    LogInfo("Сигнал: {0}, Short SMA={1}, Long SMA={2}", 
        isShortGreaterThanLong ? "Покупка" : "Продажа", shortSma, longSma);
    
    // ...
}
```

### LogWarning

Метод [LogWarning](xref:Ecng.Logging.BaseLogReceiver.LogWarning(System.String,System.Object[])) используется для записи предупреждений:

```cs
public void RegisterOrder(Order order)
{
    if (order.Volume <= 0)
    {
        LogWarning("Попытка зарегистрировать заявку с некорректным объемом: {0}", order.Volume);
        return;
    }
    
    // ...
}
```

### LogError

Метод [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.String,System.Object[])) предназначен для записи сообщений об ошибках:

```cs
try
{
    // Какие-то действия
}
catch (Exception ex)
{
    LogError("Ошибка при выполнении операции: {0}", ex.Message);
    Stop();
}
```

Также есть перегрузка [LogError](xref:Ecng.Logging.BaseLogReceiver.LogError(System.Exception)), принимающая исключение напрямую:

```cs
try
{
    // Какие-то действия
}
catch (Exception ex)
{
    LogError(ex);
    Stop();
}
```

## Настройка уровня логирования

Класс [Strategy](xref:StockSharp.Algo.Strategies.Strategy) содержит свойство [LogLevel](xref:Ecng.Logging.ILogSource.LogLevel), которое определяет, какие сообщения будут записываться в лог:

```cs
// Устанавливаем уровень логирования для стратегии
strategy.LogLevel = LogLevels.Info;
```

При выбранном уровне логирования будут записываться только сообщения этого уровня и более высоких уровней. Например, если установлен уровень `LogLevels.Info`, то сообщения уровней Verbose и Debug будут игнорироваться.

## Параметр LogLevel

Для удобной настройки уровня логирования в конструкторе стратегии можно добавить параметр:

```cs
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<LogLevels> _logLevel;
    
    public SmaStrategy()
    {
        _logLevel = Param(nameof(LogLevel), LogLevels.Info)
                    .SetDisplay("Уровень логирования", "Уровень детализации сообщений в логе", "Настройки логирования");
    }
    
    public override LogLevels LogLevel
    {
        get => _logLevel.Value;
        set => _logLevel.Value = value;
    }
    
    // ...
}
```

## Примеры использования в реальной стратегии

### Логирование запуска и остановки стратегии

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);
    
    LogInfo("Стратегия {0} запущена в {1}. Инструмент: {2}, Портфель: {3}", 
        Name, time, Security?.Code, Portfolio?.Name);
    
    // ...
}

protected override void OnStopped()
{
    LogInfo("Стратегия {0} остановлена. Позиция: {1}, P&L: {2}", 
        Name, Position, PnL);
    
    base.OnStopped();
}
```

### Логирование сделок

```cs
protected override void OnNewMyTrade(MyTrade trade)
{
    LogInfo("{0} {1} {2} по цене {3}. Объем: {4}", 
        trade.Order.Direction == Sides.Buy ? "Куплено" : "Продано",
        trade.Order.Security.Code,
        trade.Order.Type,
        trade.Trade.Price,
        trade.Trade.Volume);
    
    base.OnNewMyTrade(trade);
}
```

### Логирование ошибок при работе с заявками

```cs
protected override void OnOrderRegisterFailed(OrderFail fail, bool calcRisk)
{
    LogError("Ошибка регистрации заявки {0}: {1}", 
        fail.Order.TransactionId, fail.Error.Message);
    
    base.OnOrderRegisterFailed(fail, calcRisk);
}
```

## Просмотр логов

Сообщения, записанные в лог стратегии, можно просматривать:

1. В программе [Designer](../../designer.md) на панели "Логи"
2. В файлах лога, если настроен [FileLogListener](xref:Ecng.Logging.FileLogListener)
3. В пользовательском интерфейсе через [LogControl](xref:StockSharp.Xaml.LogControl), если используется [GuiLogListener](xref:StockSharp.Xaml.GuiLogListener)

## См. также

[Логирование](../logging.md)
[Компонент LogControl](../graphical_user_interface/logging/log_panel.md)

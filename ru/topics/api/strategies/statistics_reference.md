# Полный справочник статистик

Менеджер статистик [StatisticManager](xref:StockSharp.Algo.Statistics.StatisticManager) управляет коллекцией экземпляров [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter). Каждый параметр отслеживает определенную метрику во время выполнения стратегии. Все доступные параметры создаются при помощи реестра [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry).

Общий обзор работы со статистикой стратегий см. в разделе [Статистика](statistics.md).

## Интерфейсы

Система статистик построена на иерархии интерфейсов. Каждый интерфейс определяет источник данных для расчета параметра:

| Интерфейс | Описание |
|---|---|
| [IStatisticParameter](xref:StockSharp.Algo.Statistics.IStatisticParameter) | Базовый интерфейс: свойства `Name`, `Type`, `Value`, `DisplayName`, `Description`, `Category`, `Order`; метод `Reset()` |
| [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) | Параметры на основе прибыли/убытков: метод `Add(marketTime, pnl, commission)` |
| [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter) | Параметры на основе сделок: метод `Add(PnLInfo)` |
| [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) | Параметры на основе заявок: методы `New(order)`, `Changed(order)`, `RegisterFailed(fail)`, `CancelFailed(fail)` |
| [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter) | Параметры на основе позиций: метод `Add(marketTime, position)` |
| [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter) | Параметры с безрисковой ставкой: свойство `RiskFreeRate` |
| [IBeginValueStatisticParameter](xref:StockSharp.Algo.Statistics.IBeginValueStatisticParameter) | Параметры с начальным значением: свойство `BeginValue` |

## Параметры прибыли и убытков (P&L)

Все параметры данной группы реализуют интерфейс [IPnLStatisticParameter](xref:StockSharp.Algo.Statistics.IPnLStatisticParameter) и наследуют от [BasePnLStatisticParameter](xref:StockSharp.Algo.Statistics.BasePnLStatisticParameter`1). Они получают данные при каждом обновлении значения P&L стратегии.

| Класс | Описание | Тип значения |
|---|---|---|
| [NetProfitParameter](xref:StockSharp.Algo.Statistics.NetProfitParameter) | Чистая прибыль за весь период. Устанавливается равной текущему значению P&L | `decimal` |
| [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) | Чистая прибыль в процентах. Требует установки `BeginValue` (начальный капитал). Формула: `pnl * 100 / BeginValue` | `decimal` |
| [MaxProfitParameter](xref:StockSharp.Algo.Statistics.MaxProfitParameter) | Максимальная прибыль (максимальное значение P&L за весь период) | `decimal` |
| [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) | Максимальная прибыль в процентах. Требует `BeginValue`. Формула: `MaxProfit * 100 / BeginValue` | `decimal` |
| [MaxProfitDateParameter](xref:StockSharp.Algo.Statistics.MaxProfitDateParameter) | Дата достижения максимальной прибыли | `DateTime` |
| [MaxDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownParameter) | Максимальная абсолютная просадка. Разница между пиком и минимумом кривой эквити | `decimal` |
| [MaxDrawdownPercentParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownPercentParameter) | Максимальная просадка в процентах. Формула: `MaxDrawdown * 100 / MaxEquity` | `decimal` |
| [MaxDrawdownDateParameter](xref:StockSharp.Algo.Statistics.MaxDrawdownDateParameter) | Дата максимальной просадки | `DateTime` |
| [MaxRelativeDrawdownParameter](xref:StockSharp.Algo.Statistics.MaxRelativeDrawdownParameter) | Максимальная относительная просадка. Вычисляется как отношение просадки к пиковому значению эквити | `decimal` |
| [ReturnParameter](xref:StockSharp.Algo.Statistics.ReturnParameter) | Относительная доходность за весь период. Максимальный рост от минимума к текущему значению в относительном выражении | `decimal` |
| [CommissionParameter](xref:StockSharp.Algo.Statistics.CommissionParameter) | Общая уплаченная комиссия. Накапливает все значения комиссий | `decimal` |
| [AverageDrawdownParameter](xref:StockSharp.Algo.Statistics.AverageDrawdownParameter) | Средняя просадка. Среднее арифметическое всех завершенных и текущей просадок | `decimal` |
| [RecoveryFactorParameter](xref:StockSharp.Algo.Statistics.RecoveryFactorParameter) | Фактор восстановления. Формула: `NetProfit / MaxDrawdown` | `decimal` |
| [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) | Коэффициент Шарпа. Формула: `(годовая доходность - безрисковая ставка) / годовое стандартное отклонение` | `decimal` |
| [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) | Коэффициент Сортино. Аналогичен Шарпу, но учитывает только нисходящие отклонения | `decimal` |
| [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) | Коэффициент Калмара. Формула: `NetProfit / MaxDrawdown` | `decimal` |
| [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) | Коэффициент Стерлинга. Формула: `NetProfit / AverageDrawdown` | `decimal` |

### Коэффициенты риска

Коэффициенты [SharpeRatioParameter](xref:StockSharp.Algo.Statistics.SharpeRatioParameter) и [SortinoRatioParameter](xref:StockSharp.Algo.Statistics.SortinoRatioParameter) наследуют от базового класса [RiskAdjustedRatioParameter](xref:StockSharp.Algo.Statistics.RiskAdjustedRatioParameter) и реализуют интерфейс [IRiskFreeRateStatisticParameter](xref:StockSharp.Algo.Statistics.IRiskFreeRateStatisticParameter).

Они поддерживают следующие настройки:

- **RiskFreeRate** -- годовая безрисковая ставка (например, `0.03m` = 3%)
- **Period** -- период расчета доходности (по умолчанию `TimeSpan.FromDays(1)`)

Коэффициенты [CalmarRatioParameter](xref:StockSharp.Algo.Statistics.CalmarRatioParameter) и [SterlingRatioParameter](xref:StockSharp.Algo.Statistics.SterlingRatioParameter) зависят от других параметров (`NetProfitParameter`, `MaxDrawdownParameter`, `AverageDrawdownParameter`) и автоматически связываются при создании через [StatisticParameterRegistry](xref:StockSharp.Algo.Statistics.StatisticParameterRegistry).

## Параметры сделок (Trades)

Все параметры данной группы реализуют интерфейс [ITradeStatisticParameter](xref:StockSharp.Algo.Statistics.ITradeStatisticParameter). Они получают данные через объект [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) при каждом совершении сделки.

| Класс | Описание | Тип значения |
|---|---|---|
| [TradeCountParameter](xref:StockSharp.Algo.Statistics.TradeCountParameter) | Общее количество сделок (учитываются только сделки с `ClosedVolume > 0`) | `int` |
| [WinningTradesParameter](xref:StockSharp.Algo.Statistics.WinningTradesParameter) | Количество прибыльных сделок (`ClosedVolume > 0` и `PnL > 0`) | `int` |
| [LossingTradesParameter](xref:StockSharp.Algo.Statistics.LossingTradesParameter) | Количество убыточных сделок (`ClosedVolume > 0` и `PnL < 0`) | `int` |
| [RoundtripCountParameter](xref:StockSharp.Algo.Statistics.RoundtripCountParameter) | Количество завершенных раундтрипов (закрывающих сделок с `ClosedVolume > 0`) | `int` |
| [AverageTradeProfitParameter](xref:StockSharp.Algo.Statistics.AverageTradeProfitParameter) | Средняя прибыль на сделку. Формула: `SumPnL / Count` | `decimal` |
| [AverageWinTradeParameter](xref:StockSharp.Algo.Statistics.AverageWinTradeParameter) | Средняя прибыль прибыльных сделок. Учитываются только сделки с `PnL > 0` | `decimal` |
| [AverageLossTradeParameter](xref:StockSharp.Algo.Statistics.AverageLossTradeParameter) | Средний убыток убыточных сделок. Учитываются только сделки с `PnL < 0` | `decimal` |
| [ProfitFactorParameter](xref:StockSharp.Algo.Statistics.ProfitFactorParameter) | Фактор прибыли. Формула: `GrossProfit / GrossLoss` | `decimal` |
| [ExpectancyParameter](xref:StockSharp.Algo.Statistics.ExpectancyParameter) | Математическое ожидание. Формула: `P(win) * AvgWin + P(loss) * AvgLoss` | `decimal` |
| [PerMonthTradeParameter](xref:StockSharp.Algo.Statistics.PerMonthTradeParameter) | Среднее количество сделок в месяц | `decimal` |
| [PerDayTradeParameter](xref:StockSharp.Algo.Statistics.PerDayTradeParameter) | Среднее количество сделок в день | `decimal` |
| [GrossProfitParameter](xref:StockSharp.Algo.Statistics.GrossProfitParameter) | Валовая прибыль. Сумма P&L всех прибыльных сделок (`PnL > 0`) | `decimal` |
| [GrossLossParameter](xref:StockSharp.Algo.Statistics.GrossLossParameter) | Валовый убыток. Сумма P&L всех убыточных сделок (`PnL < 0`, значение отрицательное) | `decimal` |

## Параметры позиций

Параметры данной группы реализуют интерфейс [IPositionStatisticParameter](xref:StockSharp.Algo.Statistics.IPositionStatisticParameter). Они получают данные при каждом изменении позиции.

| Класс | Описание | Тип значения |
|---|---|---|
| [MaxLongPositionParameter](xref:StockSharp.Algo.Statistics.MaxLongPositionParameter) | Максимальная длинная позиция. Наибольшее положительное значение позиции | `decimal` |
| [MaxShortPositionParameter](xref:StockSharp.Algo.Statistics.MaxShortPositionParameter) | Максимальная короткая позиция. Наибольшее по модулю отрицательное значение позиции | `decimal` |

## Параметры заявок (Orders)

Все параметры данной группы реализуют интерфейс [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter) и наследуют от [BaseOrderStatisticParameter](xref:StockSharp.Algo.Statistics.BaseOrderStatisticParameter`1). Они получают данные при регистрации, изменении и ошибках заявок.

| Класс | Описание | Тип значения |
|---|---|---|
| [OrderCountParameter](xref:StockSharp.Algo.Statistics.OrderCountParameter) | Общее количество зарегистрированных заявок | `int` |
| [OrderRegisterErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderRegisterErrorCountParameter) | Количество ошибок регистрации заявок | `int` |
| [OrderInsufficientFundErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderInsufficientFundErrorCountParameter) | Количество ошибок "недостаточно средств" (тип `InsufficientFundException`) | `int` |
| [OrderCancelErrorCountParameter](xref:StockSharp.Algo.Statistics.OrderCancelErrorCountParameter) | Количество ошибок отмены заявок | `int` |

## Параметры задержки (Latency)

Параметры задержки также реализуют [IOrderStatisticParameter](xref:StockSharp.Algo.Statistics.IOrderStatisticParameter), но отслеживают временные характеристики обработки заявок.

| Класс | Описание | Тип значения |
|---|---|---|
| [MaxLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyRegistrationParameter) | Максимальная задержка регистрации заявки (свойство `Order.LatencyRegistration`) | `TimeSpan` |
| [MinLatencyRegistrationParameter](xref:StockSharp.Algo.Statistics.MinLatencyRegistrationParameter) | Минимальная задержка регистрации заявки | `TimeSpan` |
| [MaxLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MaxLatencyCancellationParameter) | Максимальная задержка отмены заявки (свойство `Order.LatencyCancellation`) | `TimeSpan` |
| [MinLatencyCancellationParameter](xref:StockSharp.Algo.Statistics.MinLatencyCancellationParameter) | Минимальная задержка отмены заявки | `TimeSpan` |

## Использование

### Доступ к статистике стратегии

```cs
var strategy = new MyStrategy();

// Доступ к статистике после выполнения
foreach (var param in strategy.StatisticManager.Parameters)
{
    Console.WriteLine($"{param.DisplayName}: {param.Value}");
}
```

### Получение конкретного параметра

```cs
// Получить значение чистой прибыли
var netProfit = strategy.StatisticManager.Parameters
    .OfType<NetProfitParameter>()
    .First();

Console.WriteLine($"Чистая прибыль: {netProfit.Value}");
```

### Настройка безрисковой ставки для коэффициентов

Коэффициенты Шарпа и Сортино требуют установки безрисковой ставки для корректного расчета:

```cs
// Установить безрисковую ставку 3% для всех коэффициентов
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IRiskFreeRateStatisticParameter>())
{
    param.RiskFreeRate = 0.03m;
}
```

### Настройка начального капитала для процентных параметров

Параметры [NetProfitPercentParameter](xref:StockSharp.Algo.Statistics.NetProfitPercentParameter) и [MaxProfitPercentParameter](xref:StockSharp.Algo.Statistics.MaxProfitPercentParameter) требуют установки начального значения капитала:

```cs
// Установить начальный капитал для процентных расчетов
foreach (var param in strategy.StatisticManager.Parameters
    .OfType<IBeginValueStatisticParameter>())
{
    param.BeginValue = 1_000_000m; // 1 000 000
}
```

### Сброс статистики

```cs
// Сбросить все статистические параметры
strategy.StatisticManager.Reset();
```

### Сохранение и загрузка состояния

Все параметры поддерживают сериализацию через интерфейс `IPersistable`:

```cs
// Сохранение
var storage = new SettingsStorage();
strategy.StatisticManager.Save(storage);

// Загрузка
strategy.StatisticManager.Load(storage);
```

## См. также

- [Статистика стратегий](statistics.md)
- [Графический компонент статистики](../graphical_user_interface/strategies/statistics.md)

# Отчеты стратегий

## Обзор

StockSharp предоставляет систему генерации отчетов по результатам работы стратегий. Система построена на двух ключевых компонентах:

- **`IReportSource`** -- интерфейс, описывающий источник данных для отчета (параметры стратегии, заявки, сделки, позиции, статистика).
- **`IReportGenerator`** -- интерфейс генератора отчетов, поддерживающий различные форматы (CSV, JSON, XML, Excel).

Класс `Strategy` реализует интерфейс `IReportSource`, поэтому стратегию можно напрямую передать в генератор отчетов.

## Интерфейс IReportSource

Интерфейс `IReportSource` предоставляет все данные, необходимые для формирования отчета:

| Свойство | Тип | Описание |
|----------|-----|----------|
| `Name` | `string` | Название стратегии |
| `TotalWorkingTime` | `TimeSpan` | Общее время работы |
| `Commission` | `decimal?` | Суммарная комиссия |
| `Position` | `decimal` | Текущая позиция |
| `PnL` | `decimal` | Суммарная прибыль/убыток |
| `Slippage` | `decimal?` | Суммарное проскальзывание |
| `Latency` | `TimeSpan?` | Суммарная задержка |
| `Parameters` | `IEnumerable<(string, object)>` | Параметры стратегии |
| `StatisticParameters` | `IEnumerable<(string, object)>` | Статистические параметры |
| `Orders` | `IEnumerable<ReportOrder>` | Заявки |
| `OwnTrades` | `IEnumerable<ReportTrade>` | Собственные сделки |
| `Positions` | `IEnumerable<ReportPosition>` | Раунд-трипы позиций |

Перед чтением данных вызывается метод `Prepare()`, который синхронизирует внутреннее состояние источника.

## Класс ReportSource

`ReportSource` -- самостоятельная реализация `IReportSource`, не привязанная к классу `Strategy`. Позволяет вручную формировать источник данных для отчета:

```csharp
var source = new ReportSource();
source.Name = "Моя стратегия";
source.PnL = 15000m;
source.TotalWorkingTime = TimeSpan.FromHours(8);

source.AddParameter("Таймфрейм", "5 минут");
source.AddStatisticParameter("Sharpe Ratio", 1.85);

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

### Агрегация данных

При большом количестве заявок и сделок `ReportSource` автоматически агрегирует данные для уменьшения объема отчета:

```csharp
// Порог автоматической агрегации (по умолчанию 10000)
source.MaxOrdersBeforeAggregation = 5000;
source.MaxTradesBeforeAggregation = 5000;

// Интервал группировки (по умолчанию 1 час)
source.AggregationInterval = TimeSpan.FromMinutes(30);

// Ручная агрегация
source.AggregateOrders(TimeSpan.FromHours(1));
source.AggregateTrades(TimeSpan.FromHours(1));
```

При агрегации заявки и сделки группируются по временному интервалу, инструменту и направлению. Объемы суммируются, цены рассчитываются как средневзвешенные.

## PositionLifecycleTracker

`PositionLifecycleTracker` отслеживает жизненный цикл позиций и формирует раунд-трипы (round-trips) -- записи об открытии и закрытии позиции.

Раунд-трип фиксируется, когда:
- Позиция полностью закрывается (значение становится равным 0)
- Происходит разворот позиции (смена знака)

В классе `Strategy` трекер автоматически интегрирован: замкнутые раунд-трипы добавляются в `ReportSource` через событие `RoundTripClosed`.

```csharp
var tracker = new PositionLifecycleTracker();

// Событие при закрытии раунд-трипа
tracker.RoundTripClosed += roundTrip =>
{
    Console.WriteLine($"Позиция закрыта: {roundTrip.SecurityId}, " +
        $"Открытие: {roundTrip.OpenTime} по {roundTrip.OpenPrice}, " +
        $"Закрытие: {roundTrip.CloseTime} по {roundTrip.ClosePrice}, " +
        $"Макс. объем: {roundTrip.MaxPosition}");
};

// Обработка обновления позиции
tracker.ProcessPosition(position);

// Доступ к истории раунд-трипов
IReadOnlyList<ReportPosition> history = tracker.History;
```

## Генераторы отчетов

Доступны следующие генераторы:

| Генератор | Формат | Описание |
|-----------|--------|----------|
| `CsvReportGenerator` | CSV | Текстовый формат с разделителями |
| `JsonReportGenerator` | JSON | Структурированный JSON |
| `XmlReportGenerator` | XML | Формат XML |
| `ExcelReportGenerator` | Excel | Формат Excel (требует `IExcelWorkerProvider`) |

Все генераторы наследуются от `BaseReportGenerator` и поддерживают настройку включаемых секций:

```csharp
var generator = new CsvReportGenerator();

// Настройка секций отчета
generator.IncludeOrders = true;
generator.IncludeTrades = true;
generator.IncludePositions = true;
generator.Encoding = Encoding.UTF8;
```

## Генерация отчета из стратегии

Так как `Strategy` реализует `IReportSource`, отчет можно сгенерировать напрямую:

```csharp
// Стратегия сама является источником данных
var generator = new JsonReportGenerator();

using var stream = File.Create("report.json");
await generator.Generate(strategy, stream, CancellationToken.None);
```

Для отдельного источника данных:

```csharp
var source = new ReportSource();
source.Name = strategy.Name;
source.PnL = strategy.PnL;
source.TotalWorkingTime = strategy.TotalWorkingTime;

// Добавить позиции из трекера
source.AddPositions(tracker.History);

var generator = new CsvReportGenerator();
using var stream = File.Create("report.csv");
await generator.Generate(source, stream, CancellationToken.None);
```

## Пример: стратегия с генерацией отчета при остановке

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

        // Торговая логика...
    }

    protected override void OnStopped()
    {
        // Генерация отчета при остановке стратегии
        var generator = new CsvReportGenerator();

        using var stream = File.Create($"report_{Name}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        generator.Generate(this, stream, CancellationToken.None).AsTask().Wait();

        base.OnStopped();
    }
}
```

В этом примере стратегия автоматически создает CSV-отчет при остановке. Отчет включает параметры стратегии, статистику, заявки, сделки и раунд-трипы позиций.

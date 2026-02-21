# Дополнительные возможности стратегий

## Обзор

Класс `Strategy` предоставляет ряд дополнительных свойств для тонкой настройки поведения: автоматическое комментирование заявок, расписание торговли, безрисковая ставка для статистики, источник данных для индикаторов и управление историческим периодом.

## CommentMode -- комментарии к заявкам

Свойство `CommentMode` управляет автоматическим заполнением поля `Order.Comment` для всех заявок, отправляемых стратегией. Это позволяет идентифицировать, какая стратегия создала заявку, что особенно полезно при одновременной работе нескольких стратегий на одном счете.

### Перечисление StrategyCommentModes

| Значение | Описание |
|----------|----------|
| `Disabled` | Комментарий не заполняется автоматически. Значение по умолчанию. |
| `Id` | В комментарий записывается `Strategy.Id` (уникальный идентификатор GUID). |
| `Name` | В комментарий записывается `Strategy.Name` (имя стратегии). |

### Пример

```csharp
public class CommentStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Все заявки будут помечены именем стратегии
        CommentMode = StrategyCommentModes.Name;

        // Или идентификатором для точной привязки
        // CommentMode = StrategyCommentModes.Id;
    }
}
```

При значении `Name` и имени стратегии "SMA Crossover" каждая заявка получит комментарий "SMA Crossover", что позволит отфильтровать заявки этой стратегии в журнале сделок.

## WorkingTime -- расписание работы

Свойство `WorkingTime` задает расписание, в рамках которого стратегия активна. За пределами указанных временных интервалов стратегия может автоматически ограничивать свою активность.

```csharp
public class ScheduledStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Настройка рабочего времени
        WorkingTime = new WorkingTime
        {
            Periods = new List<WorkingTimePeriod>
            {
                new WorkingTimePeriod
                {
                    Till = DateTime.MaxValue,
                    Times = new List<Range<TimeSpan>>
                    {
                        // Торговля с 10:00 до 18:00
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

Свойство `TotalWorkingTime` (только чтение) показывает суммарное время работы стратегии с момента запуска. Оно автоматически рассчитывается при остановке и перезапуске стратегии.

## RiskFreeRate -- безрисковая ставка

Свойство `RiskFreeRate` задает годовую безрисковую ставку, используемую в расчетах статистических показателей -- в первую очередь коэффициента Шарпа и коэффициента Сортино.

```csharp
var strategy = new MyStrategy();

// Безрисковая ставка 5% годовых
strategy.RiskFreeRate = 0.05m;
```

Значение автоматически передается всем параметрам статистики, реализующим `IRiskFreeRateStatisticParameter`, при инициализации менеджера статистики стратегии.

## IndicatorSource -- источник данных для индикаторов

Свойство `IndicatorSource` задает значение по умолчанию для свойства `IIndicator.Source` для всех индикаторов стратегии, у которых источник не указан явно. Определяет, какое поле `Level1Fields` использовать в качестве входных данных для индикатора.

```csharp
var strategy = new MyStrategy();

// Все индикаторы по умолчанию будут использовать цену последней сделки
strategy.IndicatorSource = Level1Fields.LastTradePrice;

// Или среднюю цену
// strategy.IndicatorSource = Level1Fields.AveragePrice;
```

Если свойство равно `null` (значение по умолчанию), индикаторы используют свой собственный источник данных.

## HistoryCalculated -- вычисляемый исторический период

Виртуальное свойство `HistoryCalculated` позволяет стратегии программно определить необходимый период исторических данных для прогрева индикаторов. Возвращает `TimeSpan?` -- длительность исторического периода, или `null`, если период не задан.

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

    // Автоматический расчет необходимого исторического периода
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(LongPeriod * 2);
}
```

`HistoryCalculated` является вычисляемой из кода версией свойства `HistorySize`. Разница в том, что `HistorySize` задается пользователем как параметр стратегии, а `HistoryCalculated` рассчитывается программно на основе параметров стратегии (например, периодов индикаторов).

## Пример: стратегия со всеми дополнительными настройками

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

    // Автоматический расчет исторического периода
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(SmaPeriod * 2);

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Комментарии к заявкам -- имя стратегии
        CommentMode = StrategyCommentModes.Name;

        // Безрисковая ставка для расчета Sharpe
        RiskFreeRate = 0.05m;

        // Источник данных для индикаторов
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

        // Торговая логика...
    }
}
```

В этом примере стратегия использует все описанные возможности: автоматически комментирует заявки, задает безрисковую ставку для статистики, устанавливает источник данных для индикаторов и рассчитывает необходимый исторический период.

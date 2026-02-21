# Высокоуровневые подписки

## Обзор

Класс `Strategy` предоставляет набор высокоуровневых методов подписки на рыночные данные: `SubscribeCandles`, `SubscribeTicks`, `SubscribeLevel1` и `SubscribeOrderBook`. Эти методы возвращают объект `ISubscriptionHandler<T>`, который позволяет привязывать обработчики данных и индикаторы в удобном fluent-стиле.

В отличие от ручного создания объекта `Subscription` и вызова `Subscribe()`, высокоуровневые методы:

- Автоматически создают подписку с правильными параметрами
- Предоставляют типизированный `ISubscriptionHandler<T>` для привязки обработчиков
- Интегрируются с системой индикаторов через методы `Bind`
- Поддерживают автоматическую отрисовку на графике
- Корректно управляют жизненным циклом подписки

## Методы подписки

### SubscribeCandles

Подписка на свечи. Принимает таймфрейм или `DataType`:

```csharp
// Подписка по таймфрейму
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    TimeSpan tf,
    bool isFinishedOnly = true,
    Security security = default);

// Подписка по DataType (поддерживает все типы свечей)
ISubscriptionHandler<ICandleMessage> SubscribeCandles(
    DataType dt,
    bool isFinishedOnly = true,
    Security security = default);

// Подписка с готовым объектом Subscription
ISubscriptionHandler<ICandleMessage> SubscribeCandles(Subscription subscription);
```

Параметр `isFinishedOnly` по умолчанию `true` -- обработчик получает только завершенные свечи.

### SubscribeTicks

Подписка на тиковые сделки:

```csharp
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Security security = null);
ISubscriptionHandler<ITickTradeMessage> SubscribeTicks(Subscription subscription);
```

### SubscribeLevel1

Подписка на данные Level1 (лучшие цены bid/ask, последняя сделка и другие поля):

```csharp
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Security security = null);
ISubscriptionHandler<Level1ChangeMessage> SubscribeLevel1(Subscription subscription);
```

### SubscribeOrderBook

Подписка на стакан котировок:

```csharp
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Security security = null);
ISubscriptionHandler<IOrderBookMessage> SubscribeOrderBook(Subscription subscription);
```

Если параметр `security` не указан, используется `Security` стратегии.

## Интерфейс ISubscriptionHandler

Объект `ISubscriptionHandler<T>` предоставляет следующие методы:

### Start / Stop

Запуск и остановка подписки:

```csharp
handler.Start();   // вызывает Subscribe
handler.Stop();    // вызывает UnSubscribe
```

### Bind (без индикаторов)

Привязка простого обработчика данных:

```csharp
handler.Bind(Action<T> callback);
```

### Bind (с индикаторами)

Привязка обработчика с одним или несколькими индикаторами. Индикатор автоматически обрабатывает входящие данные, и обработчик получает уже рассчитанное значение:

```csharp
// Один индикатор -- значение типа decimal
handler.Bind(IIndicator indicator, Action<T, decimal> callback);

// Два индикатора
handler.Bind(IIndicator ind1, IIndicator ind2, Action<T, decimal, decimal> callback);

// До восьми индикаторов
handler.Bind(ind1, ind2, ind3, ..., callback);

// Массив индикаторов
handler.Bind(IIndicator[] indicators, Action<T, decimal[]> callback);
```

Обработчик с `Bind` вызывается только тогда, когда все индикаторы вернули непустое значение.

### BindWithEmpty

Аналогичен `Bind`, но обработчик вызывается даже если индикатор вернул пустое значение. Значения представлены как `decimal?`:

```csharp
handler.BindWithEmpty(IIndicator indicator, Action<T, decimal?> callback);
```

### BindEx

Предоставляет доступ к полному объекту `IIndicatorValue` вместо извлеченного `decimal`:

```csharp
handler.BindEx(IIndicator indicator, Action<T, IIndicatorValue> callback, bool allowEmpty = false);
```

## Пример: стратегия с индикаторами

```csharp
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<int> _shortPeriod;
    private readonly StrategyParam<int> _longPeriod;
    private readonly StrategyParam<DataType> _candleType;

    public int ShortPeriod
    {
        get => _shortPeriod.Value;
        set => _shortPeriod.Value = value;
    }

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public SmaStrategy()
    {
        _shortPeriod = Param(nameof(ShortPeriod), 10);
        _longPeriod = Param(nameof(LongPeriod), 20);
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var shortSma = new SimpleMovingAverage { Length = ShortPeriod };
        var longSma = new SimpleMovingAverage { Length = LongPeriod };

        var subscription = SubscribeCandles(CandleType);

        // Привязка двух индикаторов -- обработчик вызывается,
        // когда оба индикатора сформированы
        subscription
            .Bind(shortSma, longSma, (candle, shortValue, longValue) =>
            {
                if (!IsFormedAndOnlineAndAllowTrading())
                    return;

                if (shortValue > longValue && Position <= 0)
                    BuyMarket(Volume + Math.Abs(Position));
                else if (shortValue < longValue && Position >= 0)
                    SellMarket(Volume + Math.Abs(Position));
            })
            .Start();

        // Настройка графика
        var area = CreateChartArea();
        if (area != null)
        {
            DrawCandles(area, subscription);
            DrawIndicator(area, shortSma);
            DrawIndicator(area, longSma);
            DrawOwnTrades(area);
        }
    }
}
```

## Пример: подписка на тики

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeTicks()
        .Bind(tick =>
        {
            if (!IsFormedAndOnlineAndAllowTrading())
                return;

            this.AddInfoLog("Тик: цена={0}, объем={1}", tick.Price, tick.Volume);
        })
        .Start();
}
```

## Пример: подписка на стакан

```csharp
protected override void OnStarted2(DateTime time)
{
    base.OnStarted2(time);

    SubscribeOrderBook()
        .Bind(book =>
        {
            var bestBid = book.GetBestBid();
            var bestAsk = book.GetBestAsk();

            if (bestBid != null && bestAsk != null)
            {
                var spread = bestAsk.Price - bestBid.Price;
                this.AddInfoLog("Спред: {0}", spread);
            }
        })
        .Start();
}
```

## Отличия от ручного создания подписки

| Аспект | Ручная подписка | Высокоуровневый метод |
|--------|-----------------|----------------------|
| Создание | `new Subscription(DataType, Security)` | `SubscribeCandles(tf)` |
| Обработка данных | Подписка на события коннектора | `Bind(callback)` |
| Индикаторы | Ручной вызов `indicator.Process()` | Автоматически через `Bind(indicator, callback)` |
| Регистрация индикаторов | Ручное добавление в `Indicators` | Автоматически при `Bind` |
| Отрисовка на графике | Ручная интеграция с `IChart` | `DrawCandles`, `DrawIndicator` |

Высокоуровневые методы рекомендуется использовать в большинстве стратегий, так как они значительно сокращают объем кода и уменьшают вероятность ошибок.

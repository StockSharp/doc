# Cтратегии

## Введение

StockSharp предоставляет мощный инструментарий для создания торговых стратегий. В этом руководстве мы рассмотрим процесс создания стратегии на примере SMA (Simple Moving Average) стратегии.

## Основы создания стратегии

### Наследование от базового класса

Для создания стратегии необходимо создать класс, наследующий от [Strategy](xref:StockSharp.Algo.Strategies.Strategy).

```cs
public class SmaStrategy : Strategy
{
    // Объявление класса стратегии, наследующего от базового класса Strategy
    // Это позволяет использовать все базовые функции стратегий StockSharp
}
```

### Параметры стратегии

Параметры стратегии определяются с помощью [StrategyParam](xref:StockSharp.Algo.Strategies.StrategyParam`1). Это позволяет легко настраивать стратегию без изменения кода.

```cs
// Объявление параметров стратегии
private readonly StrategyParam<DataType> _candleTypeParam;
private readonly StrategyParam<int> _long;
private readonly StrategyParam<int> _short;

public DataType CandleType
{
    get => _candleTypeParam.Value;
    set => _candleTypeParam.Value = value;
}

public int Long
{
    get => _long.Value;
    set => _long.Value = value;
}

public int Short
{
    get => _short.Value;
    set => _short.Value = value;
}

// Эти параметры позволяют легко настраивать стратегию без изменения кода
// CandleType определяет тип используемых свечей
// Long и Short задают длины для длинной и короткой SMA
```

## Инициализация стратегии

### Метод OnStarted

Метод [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) вызывается при запуске стратегии и используется для инициализации.

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);

    this.AddInfoLog(nameof(OnStarted));

    // Метод OnStarted вызывается при запуске стратегии
    // Здесь происходит основная инициализация
}
```

### Создание индикаторов

В этом методе создаются и настраиваются используемые индикаторы.

```cs
// Создание индикаторов
_longSma = new SimpleMovingAverage { Length = Long };
_shortSma = new SimpleMovingAverage { Length = Short };

Indicators.Add(_longSma);
Indicators.Add(_shortSma);

// Создаются два индикатора SMA с разными периодами
// Важно добавить их в коллекцию Indicators для правильной работы
```

### Подписка на данные

Здесь же происходит подписка на необходимые рыночные данные.

```cs
var subscription = new Subscription(CandleType, Security)
{
    MarketData =
    {
        IsFinishedOnly = true,
    }
};

subscription
    .WhenCandleReceived(this)
    .Do(ProcessCandle)
    .Apply(this);

Subscribe(subscription);

// Создается подписка на свечи выбранного типа
// IsFinishedOnly = true означает, что обрабатываются только завершенные свечи
// WhenCandleReceived подписывается на получение новых свечей
// ProcessCandle - метод, который будет вызываться для каждой новой свечи
```

Для подписки создается [правило](strategies/event_model.md), активизирующееся каждый раз, как приходит свеча.

## Обработка рыночных данных

### Метод ProcessCandle

Этот метод вызывается для каждой новой свечи и содержит основную логику стратегии.

```cs
private void ProcessCandle(ICandleMessage candle)
{
    // Метод ProcessCandle вызывается для каждой новой свечи
    // Здесь реализуется основная логика стратегии
}
```

### Торговая логика

В этом методе реализуется основная торговая логика стратегии, включая анализ индикаторов и принятие решений о входе в позицию или выходе из нее.

```cs
// Торговая логика
if (this.IsFormedAndOnlineAndAllowTrading())
{
    if (candle.State == CandleStates.Finished)
    {
        var isShortLessThenLong = shortValue.GetValue<decimal>() < longValue.GetValue<decimal>();

        if (_isShortLessThenLong == null)
        {
            _isShortLessThenLong = isShortLessThenLong;
        }
        else if (_isShortLessThenLong != isShortLessThenLong)
        {
            // Здесь реализуется основная торговая логика
            // Проверяется пересечение коротной и длинной SMA
            // На основе этого принимается решение о покупке или продаже
        }
    }
}

// Этот код проверяет, сформированы ли индикаторы и разрешена ли торговля
// Затем анализируется пересечение короткой и длинной SMA
// При пересечении генерируется сигнал на покупку или продажу
```

## Визуализация

StockSharp позволяет легко визуализировать работу стратегии с помощью графиков.

```cs
_chart = this.GetChart();

if (_chart != null)
{
    var area = _chart.AddArea();

    _chartCandlesElem = area.AddCandles();
    _chartTradesElem = area.AddTrades();
    _chartShortElem = area.AddIndicator(_shortSma);
    _chartLongElem = area.AddIndicator(_longSma);
}

// Код для создания и настройки графика
// Добавляются элементы для отображения свечей, сделок и индикаторов
```

## Управление позициями и заявками

### Создание и регистрация заявок

Для создания заявок используется вспомогательный метод [CreateOrder](xref:StockSharp.Algo.Strategies.Strategy.CreateOrder(StockSharp.Messages.Sides,System.Decimal,System.Decimal,System.String)).

```cs
var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
var price = candle.ClosePrice + ((direction == Sides.Buy ? priceStep : -priceStep) ?? 1);

RegisterOrder(this.CreateOrder(direction, price, volume));

// Код создания и регистрации заявки
// Определяется направление сделки (покупка или продажа)
// Рассчитывается объем и цена заявки
// Создается и регистрируется новая заявка
```

Созданные заявки регистрируются с помощью метода [RegisterOrder](xref:StockSharp.Algo.Strategies.Strategy.RegisterOrder(StockSharp.Messages.OrderRegisterMessage)).

## Работа с собственными сделками

Стратегия может отслеживать собственные сделки для анализа эффективности.

```cs
this
    .WhenNewMyTrade()
    .Do(_myTrades.Add)
    .Apply(this);

// Код для обработки собственных сделок
// При появлении новой сделки она добавляется в список _myTrades
```

## Логирование

Для отладки и мониторинга работы стратегии важно использовать логирование.

```cs
this.AddInfoLog(nameof(OnStarted));
this.AddInfoLog(LocalizedStrings.SmaNewCandleLog, candle.OpenTime, candle.OpenPrice, candle.HighPrice, candle.LowPrice, candle.ClosePrice, candle.TotalVolume, candle.SecurityId);

// Примеры использования логирования
// Логируются важные события, такие как старт стратегии и получение новой свечи
```
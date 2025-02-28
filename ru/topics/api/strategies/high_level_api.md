# Высокоуровневые API в стратегиях

StockSharp предоставляет набор высокоуровневых API для упрощения работы с типовыми задачами в торговых стратегиях. Эти интерфейсы позволяют писать более чистый код, концентрируясь на торговой логике, а не на технических деталях.

## Упрощенная работа с подписками

Высокоуровневые методы для работы с подписками скрывают сложности управления жизненным циклом подписок и обработки данных.

### Метод SubscribeCandles

Вместо ручного создания подписки и настройки обработчиков событий, можно использовать метод `SubscribeCandles`:

```cs
// Создание и настройка подписки на свечи одной строкой
var subscription = SubscribeCandles(CandleType);
```

Этот метод возвращает объект типа `ISubscriptionHandler<ICandleMessage>`, который предоставляет удобный интерфейс для дальнейшей настройки подписки.

### Автоматическое связывание индикаторов с подпиской

Высокоуровневый API позволяет легко связать индикаторы с подпиской на данные:

```cs
var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

subscription
    // Связываем индикаторы с подпиской на свечи
    .Bind(longSma, shortSma, OnProcess)
    // Запускаем обработку
    .Start();
```

Метод `Bind` устанавливает соединение между данными из подписки и индикаторами. При получении новой свечи:

1. Свеча автоматически отправляется на обработку в индикаторы
2. Результаты обработки передаются в указанный обработчик (в примере это метод `OnProcess`)
3. Весь код синхронизации и управления состоянием скрыт от разработчика

Обработчик получает уже готовые значения в виде простых типов `decimal`:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
    // Работаем напрямую с готовыми значениями индикаторов
    var isShortLessThenLong = shortValue < longValue;
    
    // Торговая логика использует чистые числовые значения
    // без необходимости извлекать их из IIndicatorValue
    // ...
}
```

Это значительно упрощает код и делает его более читаемым, так как разработчику не нужно:
- Вручную обрабатывать событие получения свечи
- Самостоятельно передавать данные в индикаторы
- Извлекать значения из результатов индикаторов

## Упрощенная работа с графиками

### Автоматическая визуализация

Высокоуровневое API предоставляет простые методы для привязки подписок и индикаторов к элементам графика:

```cs
var area = CreateChartArea();

// area может быть null при запуске без GUI
if (area != null)
{
    // Автоматическая привязка свечей к области графика
    DrawCandles(area, subscription);

    // Отрисовка индикаторов с настройкой цвета
    DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
    DrawIndicator(area, longSma);
    
    // Отрисовка собственных сделок
    DrawOwnTrades(area);
}
```

Метод `DrawCandles` автоматически связывает подписку на свечи с элементом отображения свечей на графике. Аналогично, методы `DrawIndicator` и `DrawOwnTrades` автоматически настраивают отображение индикаторов и сделок.

Преимущества такого подхода:
- Не требуется создавать объекты `ChartDrawData` вручную
- Не нужно управлять группировкой данных по времени
- Не нужно вызывать `chart.Draw()` для обновления графика

Система автоматически обновляет график при получении новых данных, что позволяет разработчику не отвлекаться на технические детали визуализации.

## Защита позиций

### Метод StartProtection

Для защиты открытых позиций StockSharp предоставляет высокоуровневый метод `StartProtection`:

```cs
// Запуск защиты позиций с указанием Take Profit и Stop Loss
StartProtection(TakeValue, StopValue);
```

Этот метод автоматически настраивает защиту для всех открытых позиций:
- Отслеживает изменения цены
- Автоматически создает заявки на закрытие позиций при достижении уровней Take Profit или Stop Loss
- Поддерживает различные типы единиц измерения (абсолютные значения, проценты, пункты)
- Может использовать трейлинг-стоп для адаптивной защиты позиций

Пример с дополнительными параметрами:

```cs
// Запуск защиты с трейлинг-стопом и рыночными заявками
StartProtection(
    takeProfit: new Unit(50, UnitTypes.Point),   // Take Profit в пунктах
    stopLoss: new Unit(2, UnitTypes.Percent),    // Stop Loss в процентах
    isStopTrailing: true,                        // Включение трейлинг-стопа
    useMarketOrders: true                        // Использование рыночных заявок
);
```

## Преимущества высокоуровневого API

Высокоуровневый API в стратегиях StockSharp предоставляет следующие преимущества:

1. **Сокращение объема кода** - выполнение типовых задач требует меньше строк кода

2. **Разделение ответственности** - торговая логика отделена от технических деталей обработки данных и визуализации

3. **Улучшение читаемости** - код становится более понятным и выразительным, сосредоточенным на бизнес-логике

4. **Уменьшение вероятности ошибок** - многие типичные ошибки исключаются благодаря автоматизации рутинных задач

5. **Работа с чистыми типами данных** - вместо работы со сложными объектами, можно оперировать простыми типами данных (например, `decimal`)

## Пример стратегии с использованием высокоуровневого API

Ниже приведен полный пример стратегии, демонстрирующий использование высокоуровневого API:

```cs
public class SmaStrategy : Strategy
{
    private bool? _isShortLessThenLong;

    public SmaStrategy()
    {
        _candleTypeParam = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
        _long = Param(nameof(Long), 80);
        _short = Param(nameof(Short), 30);
        _takeValue = Param(nameof(TakeValue), new Unit(0, UnitTypes.Absolute));
        _stopValue = Param(nameof(StopValue), new Unit(2, UnitTypes.Percent));
    }

    private readonly StrategyParam<DataType> _candleTypeParam;
    public DataType CandleType
    {
        get => _candleTypeParam.Value;
        set => _candleTypeParam.Value = value;
    }

    private readonly StrategyParam<int> _long;
    public int Long
    {
        get => _long.Value;
        set => _long.Value = value;
    }

    private readonly StrategyParam<int> _short;
    public int Short
    {
        get => _short.Value;
        set => _short.Value = value;
    }

    private readonly StrategyParam<Unit> _takeValue;
    public Unit TakeValue
    {
        get => _takeValue.Value;
        set => _takeValue.Value = value;
    }

    private readonly StrategyParam<Unit> _stopValue;
    public Unit StopValue
    {
        get => _stopValue.Value;
        set => _stopValue.Value = value;
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        // Создаем индикаторы
        var longSma = new SMA { Length = Long };
        var shortSma = new SMA { Length = Short };

        // Создаем подписку на свечи и связываем с индикаторами
        var subscription = SubscribeCandles(CandleType);
        subscription
            .Bind(longSma, shortSma, OnProcess)
            .Start();

        // Настраиваем визуализацию
        var area = CreateChartArea();
        if (area != null)
        {
            DrawCandles(area, subscription);
            DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
            DrawIndicator(area, longSma);
            DrawOwnTrades(area);
        }

        // Запускаем защиту позиций
        StartProtection(TakeValue, StopValue);
    }

    private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
    {
        // Обрабатываем только завершенные свечи
        if (candle.State != CandleStates.Finished)
            return;

        // Торговая логика на основе пересечения индикаторов
        var isShortLessThenLong = shortValue < longValue;

        if (_isShortLessThenLong == null)
        {
            _isShortLessThenLong = isShortLessThenLong;
        }
        else if (_isShortLessThenLong != isShortLessThenLong)
        {
            // Произошло пересечение
            var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
            var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
            var priceStep = GetSecurity().PriceStep ?? 1;
            var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

            // Выставляем заявку
            if (direction == Sides.Buy)
                BuyLimit(price, volume);
            else
                SellLimit(price, volume);

            // Сохраняем текущее положение индикаторов
            _isShortLessThenLong = isShortLessThenLong;
        }
    }
}
```

## Заключение

Высокоуровневый API в StockSharp значительно упрощает разработку торговых стратегий, позволяя разработчикам сосредоточиться на торговой логике, а не на технических деталях. Он особенно полезен для типовых сценариев использования, когда не требуется тонкая настройка обработки данных или визуализации.

В сочетании с системой параметров стратегии, событийной моделью и механизмами защиты позиций, высокоуровневый API делает StockSharp мощным и удобным инструментом для алгоритмической торговли, подходящим как для начинающих, так и для опытных разработчиков.

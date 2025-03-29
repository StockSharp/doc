# Высокоуровневые API в стратегиях

StockSharp предоставляет набор высокоуровневых API для упрощения работы с типовыми задачами в торговых стратегиях. Эти интерфейсы позволяют писать более чистый код, концентрируясь на торговой логике, а не на технических деталях.

## Упрощенная работа с подписками

Высокоуровневые методы для работы с подписками скрывают сложности управления жизненным циклом подписок и обработки данных.

### Метод SubscribeCandles

Вместо ручного создания подписки и настройки обработчиков событий, можно использовать метод [SubscribeCandles](xref:StockSharp.Algo.Strategies.Strategy.SubscribeCandles(System.TimeSpan,System.Boolean,StockSharp.BusinessEntities.Security)):

```cs
// Создание и настройка подписки на свечи одной строкой
var subscription = SubscribeCandles(CandleType);
```

Этот метод возвращает объект типа [ISubscriptionHandler\<ICandleMessage\>](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1), который предоставляет удобный интерфейс для дальнейшей настройки подписки.

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

#### Автоматическое добавление индикаторов в коллекцию Strategy.Indicators

Важно отметить, что при использовании метода [Bind](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.Bind(StockSharp.Algo.Indicators.IIndicator,StockSharp.Algo.Indicators.IIndicator,System.Action{`0,System.Decimal,System.Decimal})) для связывания индикаторов с подпиской, **не требуется** дополнительно добавлять эти индикаторы в коллекцию [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators), как это обычно делается в традиционном подходе (описанном в [документации по индикаторам](indicators.md)). Система автоматически:

1. Добавляет индикаторы в коллекцию [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators)
2. Отслеживает состояние формирования индикаторов
3. Обновляет состояние [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) стратегии

Это значительно упрощает код и уменьшает вероятность ошибок.

#### Использование BindEx для работы с сырыми значениями индикаторов

Если индикатор возвращает нестандартные значения (не просто числа), можно использовать метод [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue})), который предоставляет доступ к исходному объекту [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue):

```cs
subscription
    .BindEx(indicator, OnProcessWithRawValue)
    .Start();

// Обработчик получает исходное значение IIndicatorValue
private void OnProcessWithRawValue(ICandleMessage candle, IIndicatorValue value)
{
    // Доступ к свойствам IIndicatorValue
    if (value.IsFinal)
    {
        // Для индикаторов, возвращающих булевы значения
        var boolValue = value.GetValue<bool>();
        
        // Или другие типы данных, специфичные для конкретного индикатора
        // ...
    }
}
```

Метод [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue})) особенно полезен в следующих случаях:

- Работа с индикаторами, возвращающими логические значения (например, [Fractals](xref:StockSharp.Algo.Indicators.Fractals))
- Доступ к дополнительным свойствам типа индикатора (например, признак [IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal))
- Работа с индикаторами, возвращающими структурированные данные

#### Работа с комплексными индикаторами (IComplexIndicator)

Для сложных индикаторов, которые содержат несколько внутренних индикаторов (например, [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands), [MACD](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergence)), API предоставляет специальные перегрузки методов `Bind` и `BindEx`:

```cs
// Создаем комплексный индикатор
var bollinger = new BollingerBands 
{ 
    Length = 20, 
    Deviation = 2 
};

// Связываем комплексный индикатор с подпиской
subscription
    .Bind(bollinger, OnProcessBollinger)
    .Start();

// Обработчик получает значения верхней и нижней полосы Боллинджера
private void OnProcessBollinger(ICandleMessage candle, decimal middleBand, decimal upperBand)
{
    // Используем значения полос Боллинджера
    // middleBand - средняя полоса
    // upperBand - верхняя полоса
}
```

Также доступны перегрузки для комплексных индикаторов с тремя значениями:

```cs
subscription.Bind(bollinger, (candle, middle, upper, lower) => 
{
    // Обработка данных с доступом ко всем трем полосам
});
```

Для более гибкой работы можно использовать [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IComplexIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue[]})) с массивом значений:

```cs
subscription.BindEx(complexIndicator, (candle, values) => 
{
    // values содержит массив всех значений индикатора
    // в порядке их добавления в индикатор
});
```

Метод [Bind](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.Bind(StockSharp.Algo.Indicators.IComplexIndicator,System.Action{`0,System.Decimal,System.Decimal})) для комплексных индикаторов автоматически:

1. Обрабатывает входные данные через комплексный индикатор
2. Распаковывает значения внутренних индикаторов
3. Передает эти значения в указанный обработчик

Это позволяет работать с комплексными индикаторами более естественным образом, получая доступ к их составным частям напрямую.

### Метод `Bind` устанавливает соединение между данными из подписки и индикаторами. При получении новой свечи:

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
    
    // Отрисовка заявок
    DrawOrders(area);
}
```

#### Метод DrawCandles

Метод [DrawCandles](xref:StockSharp.Algo.Strategies.Strategy.DrawCandles(StockSharp.Charting.IChartArea,StockSharp.BusinessEntities.Subscription)) автоматически связывает подписку на свечи с элементом отображения свечей на графике:

```cs
// Создание элемента графика для отображения свечей
IChartCandleElement candles = DrawCandles(area, subscription);

// Можно настроить дополнительные параметры элемента
candles.DrawOpenClose = true;  // Отображать линии открытия/закрытия
candles.DrawHigh = true;       // Отображать максимумы
candles.DrawLow = true;        // Отображать минимумы
```

Метод возвращает элемент графика [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement), который можно дополнительно настраивать.

#### Метод DrawIndicator

Метод [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) создает и настраивает элемент графика для отображения значений индикатора:

```cs
// Простое добавление индикатора на график с цветом по умолчанию
IChartIndicatorElement smaElem = DrawIndicator(area, sma);

// Добавление индикатора с указанием основного цвета
IChartIndicatorElement rsiFast = DrawIndicator(area, rsi, System.Drawing.Color.Red);

// Добавление индикатора с указанием основного и дополнительного цветов
IChartIndicatorElement bollingerElem = DrawIndicator(
    area, 
    bollinger, 
    System.Drawing.Color.Blue,    // Основной цвет
    System.Drawing.Color.Gray     // Дополнительный цвет (для второй линии)
);

// Дополнительная настройка элемента
smaElem.DrawStyle = DrawStyles.Line;           // Стиль отрисовки: линия
rsiFast.DrawStyle = DrawStyles.Dot;            // Стиль отрисовки: точки
bollingerElem.DrawStyle = DrawStyles.Dashdot;  // Стиль отрисовки: штрихпунктир
```

Метод возвращает элемент графика [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement), который можно настраивать. Для индикаторов с несколькими значениями (например, [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands)) основной цвет применяется к первому значению, а дополнительный - ко второму.

#### Метод DrawOwnTrades

Метод [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)) создает элемент для отображения собственных сделок на графике:

```cs
// Создание элемента для отображения сделок
IChartTradeElement trades = DrawOwnTrades(area);

// Настройка элемента
trades.BuyColor = System.Drawing.Color.Green;   // Цвет для сделок на покупку
trades.SellColor = System.Drawing.Color.Red;    // Цвет для сделок на продажу
trades.FullTitle = "My Strategy Trades";        // Заголовок элемента
```

Данный метод автоматически настраивает отображение всех сделок, совершаемых стратегией. Сделки отображаются на графике в виде маркеров в местах их совершения, с учетом стороны сделки (покупка/продажа).

#### Метод DrawOrders

Метод [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) создает элемент для отображения заявок на графике:

```cs
// Создание элемента для отображения заявок
IChartOrderElement orders = DrawOrders(area);

// Настройка элемента
orders.BuyPendingColor = System.Drawing.Color.DarkGreen;   // Цвет для активных заявок на покупку
orders.SellPendingColor = System.Drawing.Color.DarkRed;    // Цвет для активных заявок на продажу
orders.BuyColor = System.Drawing.Color.Green;              // Цвет для исполненных заявок на покупку
orders.SellColor = System.Drawing.Color.Red;               // Цвет для исполненных заявок на продажу
orders.CancelColor = System.Drawing.Color.Gray;            // Цвет для отмененных заявок
```

Данный метод автоматически настраивает отображение всех заявок, выставляемых стратегией. Заявки отображаются в виде маркеров на уровнях их цен с различным цветовым кодированием для разных состояний заявок.

#### Метод CreateChartArea

Метод [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) создает новую область на графике стратегии:

```cs
// Создание первой области для свечей и индикаторов
var mainArea = CreateChartArea();
DrawCandles(mainArea, subscription);
DrawIndicator(mainArea, sma);

// Создание второй области для отдельных индикаторов (например, RSI)
var secondArea = CreateChartArea();
DrawIndicator(secondArea, rsi);
```

Разделение графика на области позволяет более наглядно отображать различные типы данных. Например, индикаторы, имеющие отличный от цены диапазон значений (RSI, стохастик и т.д.), лучше отображать в отдельных областях.

Преимущества высокоуровневых методов визуализации:
- Не требуется создавать объекты `ChartDrawData` вручную
- Не нужно управлять группировкой данных по времени
- Не нужно вызывать `chart.Draw()` для обновления графика
- Автоматическая синхронизация данных между подписками и элементами графика
- Упрощенное управление внешним видом графических элементов

Система автоматически обновляет график при получении новых данных, что позволяет разработчику не отвлекаться на технические детали визуализации.

## Защита позиций

### Метод StartProtection

Для защиты открытых позиций StockSharp предоставляет высокоуровневый метод [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)):

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
    takeProfit: new Unit(50, UnitTypes.Absolute),// Take Profit
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
        _candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
        _long = Param(nameof(Long), 80);
        _short = Param(nameof(Short), 30);
        _takeValue = Param(nameof(TakeValue), new Unit(50, UnitTypes.Absolute));
        _stopValue = Param(nameof(StopValue), new Unit(2, UnitTypes.Percent));
    }

    private readonly StrategyParam<DataType> _candleType;
    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
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
# Написание стратегии с ИИ

Пошаговое руководство по созданию торговой стратегии StockSharp с помощью ИИ-инструментов.

## Подготовка

### 1. Установите ИИ-инструмент

Выберите один из инструментов:
- **Claude Code** — `npm install -g @anthropic-ai/claude-code` (нужен Node.js)
- **Cursor** — скачайте с [cursor.com](https://cursor.com)
- **GitHub Copilot** — установите плагин в вашу IDE

### 2. Создайте проект

```bash
dotnet new console -n MyStrategy --framework net10.0
cd MyStrategy
dotnet add package StockSharp.Algo
dotnet add package StockSharp.Algo.Strategies
dotnet add package StockSharp.Algo.Indicators
dotnet add package StockSharp.Algo.Testing
dotnet add package StockSharp.Binance
```

### 3. Подготовьте контекст

Создайте файл `CLAUDE.md` (или `.cursorrules`) в корне проекта:

```markdown
# Правила проекта

- Фреймворк: StockSharp 5.x, .NET 10
- Стратегии наследуют от класса Strategy
- Для подписки на свечи: Connector.Subscribe(subscription)
- Для регистрации заявок: RegisterOrder(order)
- Логирование: this.AddInfoLog(), this.AddWarningLog(), this.AddErrorLog()
- Индикаторы: создавать через new и вызывать indicator.Process(candle)
- Обязательно обрабатывать connector.Error и стратегические ошибки
```

## Пошаговый пример: SMA-стратегия

### Шаг 1: Опишите задачу ИИ

Пример промпта:

```
Создай торговую стратегию на StockSharp, которая:
- Наследует от Strategy
- Использует две скользящие средние (SMA): быструю (период 10) и медленную (период 30)
- При пересечении быстрой SMA выше медленной — покупка
- При пересечении быстрой SMA ниже медленной — продажа
- Размер позиции: 1 лот
- Использует 5-минутные свечи
- Подписывается на свечи через OnStarted()
- Обрабатывает свечи в методе OnProcess() через подписку
```

### Шаг 2: Проверьте сгенерированный код

ИИ сгенерирует что-то вроде:

```csharp
using StockSharp.Algo;
using StockSharp.Algo.Indicators;
using StockSharp.Algo.Strategies;
using StockSharp.BusinessEntities;
using StockSharp.Messages;

public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _fastPeriod;
    private readonly StrategyParam<int> _slowPeriod;
    private readonly StrategyParam<DataType> _candleType;

    private SimpleMovingAverage _fastSma;
    private SimpleMovingAverage _slowSma;

    private bool _prevFastAbove;
    private bool _isFirstCandle = true;

    public SmaCrossStrategy()
    {
        _fastPeriod = Param(nameof(FastPeriod), 10);
        _slowPeriod = Param(nameof(SlowPeriod), 30);
        _candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(5)));
    }

    public int FastPeriod
    {
        get => _fastPeriod.Value;
        set => _fastPeriod.Value = value;
    }

    public int SlowPeriod
    {
        get => _slowPeriod.Value;
        set => _slowPeriod.Value = value;
    }

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        _fastSma = new SimpleMovingAverage { Length = FastPeriod };
        _slowSma = new SimpleMovingAverage { Length = SlowPeriod };
        _isFirstCandle = true;

        var subscription = new Subscription(CandleType, Security);

        this
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Subscribe(subscription);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        var fastValue = _fastSma.Process(candle);
        var slowValue = _slowSma.Process(candle);

        if (!_fastSma.IsFormed || !_slowSma.IsFormed)
            return;

        var fastAbove = fastValue.GetValue<decimal>() > slowValue.GetValue<decimal>();

        if (!_isFirstCandle)
        {
            if (fastAbove && !_prevFastAbove)
            {
                // Быстрая пересекла медленную вверх — покупка
                if (Position <= 0)
                    RegisterOrder(this.BuyAtMarket(Volume));
            }
            else if (!fastAbove && _prevFastAbove)
            {
                // Быстрая пересекла медленную вниз — продажа
                if (Position >= 0)
                    RegisterOrder(this.SellAtMarket(Volume));
            }
        }

        _prevFastAbove = fastAbove;
        _isFirstCandle = false;
    }
}
```

### Шаг 3: Что проверять

Пройдитесь по чек-листу:

- **Наследование**: класс наследует от `Strategy` ✓
- **Параметры**: используются `StrategyParam<T>` для оптимизации ✓
- **Подписка на свечи**: через `Subscribe(new Subscription(...))` ✓
- **Обработка свечей**: через правило `WhenCandlesFinished` ✓
- **Проверка IsFormed**: индикаторы проверяются на готовность ✓
- **Заявки**: через `RegisterOrder()` с `BuyAtMarket` / `SellAtMarket` ✓
- **Позиция**: проверяется `Position` перед выставлением заявки ✓

### Шаг 4: Попросите ИИ добавить бэктест

```
Добавь код для бэктестирования этой стратегии на исторических данных.
Используй HistoryEmulationConnector, загрузку данных из локального хранилища,
и выведи итоговую статистику (PnL, количество сделок, макс. просадку).
```

## Примеры промптов

### Стратегия по Bollinger Bands

```
Создай стратегию на StockSharp, которая торгует по Bollinger Bands:
- Покупка при касании нижней полосы
- Продажа при касании верхней полосы
- Период 20, множитель 2.0
- Стоп-лосс: 1% от цены входа
- Тейк-профит: 2% от цены входа
- Используй StrategyParam для всех параметров
```

### Арбитражная стратегия

```
Создай парную арбитражную стратегию на StockSharp:
- Два инструмента (указываются параметрами)
- Расчёт спреда между ценами
- Вход при отклонении спреда на 2 стандартных отклонения
- Выход при возврате спреда к среднему
- Нейтрализация по объёму (равные позиции в деньгах)
```

### Скальпинг по стакану

```
Создай скальпинг-стратегию на StockSharp:
- Подписка на стакан (MarketDepth) через Subscribe
- Анализ дисбаланса bid/ask
- Вход при сильном дисбалансе (> 3:1)
- Быстрый выход по тейк-профиту (5 тиков)
- Стоп-лосс: 3 тика
- Максимум 1 позиция одновременно
```

## Типичные ошибки ИИ

### 1. Устаревшие события

**Неправильно** (старый API):
```csharp
connector.NewSecurities += securities => { ... };
connector.CandleSeriesProcessing += (series, candle) => { ... };
```

**Правильно** (актуальный API):
```csharp
// Используйте подписки
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security);
connector.Subscribe(subscription);
```

### 2. Создание заявки без хелперов

**Неправильно**:
```csharp
var order = new Order
{
    Security = Security,
    Portfolio = Portfolio,
    Side = Sides.Buy,
    Type = OrderTypes.Market,
    Volume = 1,
};
```

**Правильно** (через хелперы стратегии):
```csharp
RegisterOrder(this.BuyAtMarket(Volume));
// или
RegisterOrder(this.SellAtLimit(price, Volume));
```

### 3. Отсутствие проверки IsFormed

**Неправильно**:
```csharp
var value = _sma.Process(candle);
// Сразу используем value — может быть неготовым
```

**Правильно**:
```csharp
var value = _sma.Process(candle);
if (!_sma.IsFormed)
    return;
```

## Советы

1. **Дайте ИИ документацию** — укажите ссылку на [doc.stocksharp.com](https://doc.stocksharp.com) или скопируйте примеры кода из `Samples/`
2. **Используйте CLAUDE.md** — файл с правилами проекта сильно снижает количество ошибок
3. **Начинайте с простого** — сначала базовая стратегия, потом добавляйте фильтры и риск-менеджмент
4. **Тестируйте на истории** — всегда прогоняйте бэктест перед реальной торговлей
5. **Склонируйте репозиторий** — если ИИ имеет доступ к исходникам StockSharp, он точнее использует API

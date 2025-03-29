# Алгоритм котирования

## Обзор

Алгоритм котирования — это механизм, который позволяет автоматически выставлять и обновлять заявки на рынке с целью достижения наилучшей цены исполнения. Вместо выставления агрессивных рыночных заявок, котирование использует лимитные заявки, что помогает минимизировать проскальзывание и снизить торговые издержки.

## Основные компоненты

StockSharp предоставляет два ключевых компонента для котирования:

1. **[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor)** — основной процессор, который анализирует рыночные данные и состояние заявок, чтобы рекомендовать действия по котированию.

2. **[IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior)** — интерфейс, определяющий поведение котирования, включая расчет оптимальной цены и определение необходимости обновления заявок.

## Примеры стратегий с котированием

В документации представлены примеры стратегий, которые демонстрируют использование механизма котирования:

- **MqStrategy** — пример стратегии, которая использует механизм котирования для управления позицией на рынке.
- **MqSpreadStrategy** — пример стратегии, создающей спред на рынке путем одновременного выставления котировок на покупку и продажу.
- **StairsCountertrendStrategy** — пример контртрендовой стратегии с использованием котирования для более точного входа в рынок.

Эти примеры помогают понять, как интегрировать механизм котирования в собственные торговые алгоритмы.

## Поведения котирования

StockSharp поддерживает различные поведения котирования:

- **[MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)** — котирование на основе рыночной цены с настраиваемым смещением и типом.
- **[BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior)** — котирование на основе лучшей цены с настраиваемым смещением.
- **[LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior)** — котирование по фиксированной цене.
- **[BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior)** — котирование на основе лучшей цены по объему.
- **[LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior)** — котирование на основе указанного уровня в стакане.
- **[LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior)** — котирование на основе цены последней сделки.
- **[TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior)** — котирование опционов на основе теоретической цены.
- **[VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior)** — котирование опционов на основе волатильности.

## Использование в собственной стратегии

### Шаг 1: Создание поведения котирования

```csharp
// Создаем поведение для рыночного котирования
var behavior = new MarketQuotingBehavior(
    new Unit(0.01m), // Смещение цены от лучшей котировки
    new Unit(0.1m, UnitTypes.Percent), // Минимальное отклонение для обновления котировки
    MarketPriceTypes.Following // Тип рыночной цены для котирования
);
```

### Шаг 2: Создание и инициализация процессора

```csharp
// Создаем процессор котирования
_quotingProcessor = new QuotingProcessor(
    behavior,
    Security, // Инструмент
    Portfolio, // Портфель
    Sides.Buy, // Направление котирования
    Volume, // Объем котирования
    Volume, // Максимальный объем заявки
    TimeSpan.Zero, // Без таймаута
    this, // Стратегия реализует ISubscriptionProvider
    this, // Стратегия реализует IMarketRuleContainer
    this, // Стратегия реализует ITransactionProvider
    this, // Стратегия реализует ITimeProvider
    this, // Стратегия реализует IMarketDataProvider
    IsFormedAndOnlineAndAllowTrading, // Проверка разрешения торговли
    true, // Использовать цены стакана
    true  // Использовать цену последней сделки, если стакан пуст
)
{
    Parent = this
};
```

### Шаг 3: Подписка на события процессора

```csharp
// Подписываемся на события процессора для логирования и обработки
_quotingProcessor.OrderRegistered += order =>
    this.AddInfoLog($"Order {order.TransactionId} registered at price {order.Price}");

_quotingProcessor.OrderFailed += fail =>
    this.AddInfoLog($"Order failed: {fail.Error.Message}");

_quotingProcessor.OwnTrade += trade =>
    this.AddInfoLog($"Trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

_quotingProcessor.Finished += isOk => {
    this.AddInfoLog($"Quoting finished with success: {isOk}");
    _quotingProcessor?.Dispose();
    _quotingProcessor = null;
};
```

### Шаг 4: Запуск процессора

```csharp
// Запускаем процессор
_quotingProcessor.Start();
```

### Шаг 5: Освобождение ресурсов

Не забудьте очистить ресурсы процессора при остановке стратегии:

```csharp
protected override void OnStopped()
{
    // Освобождаем ресурсы текущего процессора
    _quotingProcessor?.Dispose();
    _quotingProcessor = null;
    
    base.OnStopped();
}
```

## Преимущества использования

1. **Снижение проскальзывания** — котирование помогает получить лучшую цену исполнения по сравнению с рыночными заявками.

2. **Гибкость настройки** — различные поведения котирования для разных рыночных ситуаций.

3. **Автоматическое обновление** — процессор автоматически отслеживает изменения рынка и обновляет заявки при необходимости.

4. **Полный контроль** — возможность настройки параметров котирования, включая смещение цены, минимальное отклонение и тип рыночной цены.

5. **Управление рисками** — возможность установки таймаута для ограничения времени исполнения.

## Прикладные сценарии использования

- **Маркет-мейкинг** — создание ликвидности на рынке путем выставления двусторонних котировок.
- **Алгоритмическая торговля** — улучшение цены исполнения в автоматизированных стратегиях.
- **Контртрендовая торговля** — более точный вход в рынок против тренда.
- **Арбитраж** — одновременное котирование на нескольких рынках для использования ценовых расхождений.

Внедрение механизма котирования в вашу стратегию может значительно улучшить исполнение заявок и повысить её эффективность.
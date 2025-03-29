# Стратегия котирования по спреду

## Обзор

`MqSpreadStrategy` - это стратегия, которая создает спред на рынке путем одновременного выставления котировок на покупку и продажу. Она использует два процессора котирования для управления заявками на обеих сторонах рынка.

## Основные компоненты

```cs
public class MqSpreadStrategy : Strategy
{
    private readonly StrategyParam<MarketPriceTypes> _priceType;
    private readonly StrategyParam<Unit> _priceOffset;
    private readonly StrategyParam<Unit> _bestPriceOffset;

    private QuotingProcessor _buyProcessor;
    private QuotingProcessor _sellProcessor;
}
```

## Параметры стратегии

Стратегия позволяет настраивать следующие параметры:

- **PriceType** - тип рыночной цены для котирования (по умолчанию Following)
- **PriceOffset** - смещение цены от рыночной цены
- **BestPriceOffset** - минимальное отклонение для обновления котировки (по умолчанию 0.1%)

## Инициализация стратегии

В методе [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted(System.DateTimeOffset)) стратегия подписывается на изменения рыночного времени:

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);

    // Подписка на изменения рыночного времени для обновления котировок
    Connector.CurrentTimeChanged += Connector_CurrentTimeChanged;
    Connector_CurrentTimeChanged(new TimeSpan());
}
```

## Управление процессорами котирования

Метод `Connector_CurrentTimeChanged` вызывается при изменении рыночного времени и управляет созданием и обновлением процессоров котирования:

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
    // Создаем новые процессоры только при нулевой позиции и если текущие остановлены
    if (Position != 0)
        return;

    if (_buyProcessor != null && _buyProcessor.LeftVolume > 0)
        return;

    if (_sellProcessor != null && _sellProcessor.LeftVolume > 0)
        return;

    // Освобождаем ресурсы существующих процессоров
    _buyProcessor?.Dispose();
    _buyProcessor = null;

    _sellProcessor?.Dispose();
    _sellProcessor = null;

    // Создаем поведения для рыночного котирования
    var buyBehavior = new MarketQuotingBehavior(
        PriceOffset,
        BestPriceOffset,
        PriceType
    );

    var sellBehavior = new MarketQuotingBehavior(
        PriceOffset,
        BestPriceOffset,
        PriceType
    );

    // Создаем процессор для покупки
    _buyProcessor = new QuotingProcessor(
        buyBehavior,
        Security,
        Portfolio,
        Sides.Buy,
        Volume,
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

    // Создаем процессор для продажи
    _sellProcessor = new QuotingProcessor(
        sellBehavior,
        Security,
        Portfolio,
        Sides.Sell,
        Volume,
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

    // Логируем создание новых процессоров котирования
    this.AddInfoLog($"Created buy/sell spread at {CurrentTime}");

    // Подписываемся на события процессора покупки для логирования
    _buyProcessor.OrderRegistered += order =>
        this.AddInfoLog($"Buy order {order.TransactionId} registered at price {order.Price}");

    _buyProcessor.OrderFailed += fail =>
        this.AddInfoLog($"Buy order failed: {fail.Error.Message}");

    _buyProcessor.OwnTrade += trade =>
        this.AddInfoLog($"Buy trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

    _buyProcessor.Finished += isOk => {
        this.AddInfoLog($"Buy quoting finished with success: {isOk}");
        _buyProcessor?.Dispose();
        _buyProcessor = null;
    };

    // Подписываемся на события процессора продажи для логирования
    _sellProcessor.OrderRegistered += order =>
        this.AddInfoLog($"Sell order {order.TransactionId} registered at price {order.Price}");

    _sellProcessor.OrderFailed += fail =>
        this.AddInfoLog($"Sell order failed: {fail.Error.Message}");

    _sellProcessor.OwnTrade += trade =>
        this.AddInfoLog($"Sell trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

    _sellProcessor.Finished += isOk => {
        this.AddInfoLog($"Sell quoting finished with success: {isOk}");
        _sellProcessor?.Dispose();
        _sellProcessor = null;
    };

    // Запускаем оба процессора
    _buyProcessor.Start();
    _sellProcessor.Start();
}
```

## Освобождение ресурсов

В методе [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped()) стратегия освобождает ресурсы:

```cs
protected override void OnStopped()
{
    // Отписываемся для предотвращения утечек памяти
    Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

    // Освобождаем ресурсы процессоров
    _buyProcessor?.Dispose();
    _buyProcessor = null;

    _sellProcessor?.Dispose();
    _sellProcessor = null;

    base.OnStopped();
}
```

## Логика торговли

- Стратегия реагирует на изменения рыночного времени
- При нулевой позиции и остановленных процессорах создаются два новых процессора:
  - Процессор для покупки (Buy)
  - Процессор для продажи (Sell)
- Оба процессора настроены на одинаковый объем и используют одинаковые настройки котирования
- Процессоры создают спред на рынке, выставляя одновременно заявки на покупку и продажу

## Особенности

- Использует современный процессор котирования [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) с поведением [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)
- Создает спред на рынке, выставляя одновременно заявки на покупку и продажу
- Работает только при нулевой позиции, предотвращая накопление нежелательного риска
- Поддерживает настройку различных параметров котирования (тип цены, смещение, минимальное отклонение)
- Включает подробное логирование событий процессоров котирования
- Корректно управляет ресурсами при остановке стратегии и создании новых процессоров
- Поддерживает работу с различными типами рыночных цен (Following, Best, Opposite и др.)
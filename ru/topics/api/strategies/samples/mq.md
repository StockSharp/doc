# Стратегия котирования

## Обзор

`MqStrategy` - это стратегия, которая использует механизм котирования для управления позицией на рынке. Она создает процессор котирования на основе текущей позиции, что позволяет адаптивно реагировать на изменения рыночных условий.

## Основные компоненты

```cs
public class MqStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _quotingProcessor;
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
	Connector_CurrentTimeChanged(default);
}
```

## Управление процессором котирования

Метод `Connector_CurrentTimeChanged` вызывается при изменении рыночного времени и управляет созданием и обновлением процессора котирования:

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// Создаем новый процессор только если текущий остановлен или не существует
	if (_quotingProcessor != null && _quotingProcessor.LeftVolume > 0)
		return;

	// Освобождаем ресурсы старого процессора, если он существует
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	// Определяем сторону котирования на основе текущей позиции
	var side = Position <= 0 ? Sides.Buy : Sides.Sell;

	// Создаем новое поведение котирования
	var behavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// Рассчитываем объем котирования
	var quotingVolume = Volume + Math.Abs(Position);

	// Создаем и инициализируем процессор
	_quotingProcessor = new QuotingProcessor(
		behavior,
		Security,
		Portfolio,
		side,
		quotingVolume,
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

	// Подписываемся на события процессора для логирования
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

	// Запускаем процессор
	_quotingProcessor.Start();
}
```

## Освобождение ресурсов

В методе [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped) стратегия освобождает ресурсы:

```cs
protected override void OnStopped()
{
	// Отписываемся для предотвращения утечек памяти
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// Освобождаем ресурсы текущего процессора, если он существует
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	base.OnStopped();
}
```

## Логика торговли

- Стратегия реагирует на изменения рыночного времени
- Направление котирования определяется на основе текущей позиции:
  - Если позиция <= 0, создается котирование на покупку (Buy)
  - Если позиция > 0, создается котирование на продажу (Sell)
- Объем котирования рассчитывается как базовый объем плюс модуль текущей позиции
- Для котирования используется [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) с поведением [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)

## Особенности

- Использует современный процессор котирования вместо устаревших стратегий котирования
- Адаптивно реагирует на изменения в позиции, меняя направление котирования
- Поддерживает настройку различных параметров котирования (тип цены, смещение, минимальное отклонение)
- Включает подробное логирование событий процессора котирования
- Корректно управляет ресурсами при остановке стратегии и создании новых процессоров
- Поддерживает работу с различными типами рыночных цен (Following, Best, Opposite и др.)
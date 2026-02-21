# Контртрендовая стратегия с котированием

## Обзор

`StairsCountertrendStrategy` - это контртрендовая торговая стратегия, которая открывает позиции против установившегося тренда определенной длины, используя механизм котирования для более точного входа в рынок.

## Основные компоненты

```cs
public class StairsCountertrendStrategy : Strategy
{
	private readonly StrategyParam<DataType> _candleDataType;
	private readonly StrategyParam<int> _length;
	private QuotingProcessor _quotingProcessor;

	private int _bullLength;
	private int _bearLength;
}
```

## Параметры стратегии

Стратегия позволяет настраивать следующие параметры:

- **CandleDataType** - тип свечей для работы (по умолчанию 1-минутные)
- **Length** - количество последовательных свечей одного направления для идентификации тренда (по умолчанию 5)

Параметр Length доступен для оптимизации в диапазоне от 2 до 10 с шагом 1.

## Инициализация стратегии

В методе [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) обнуляются счетчики, создается подписка на свечи и готовится визуализация:

```cs
protected override void OnStarted2(DateTime time)
{
	// Сброс счетчиков на старте
	_bullLength = 0;
	_bearLength = 0;

	// Создание подписки на свечи
	var subscription = SubscribeCandles(CandleDataType);

	subscription
		.Bind(ProcessCandle)
		.Start();

	// Настройка визуализации на графике
	var area = CreateChartArea();
	if (area != null)
	{
		DrawCandles(area, subscription);
		DrawOwnTrades(area);
	}

	base.OnStarted2(time);
}
```

## Обработка свечей

Метод `ProcessCandle` вызывается для каждой завершенной свечи и реализует логику определения тренда и управления процессором котирования:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	if (candle.State != CandleStates.Finished)
		return;

	// Идентификация бычьей или медвежьей свечи
	if (candle.OpenPrice < candle.ClosePrice)
	{
		_bullLength++;
		_bearLength = 0;

		this.AddInfoLog($"Bullish candle detected. Streak: {_bullLength}");
	}
	else if (candle.OpenPrice > candle.ClosePrice)
	{
		_bullLength = 0;
		_bearLength++;

		this.AddInfoLog($"Bearish candle detected. Streak: {_bearLength}");
	}

	// Остановка существующего процессора при необходимости смены направления
	if (_quotingProcessor != null)
	{
		// Проверка необходимости очистки процессора (изменение тренда или позиции)
		var shouldClearProcessor = false;

		// Нужно продавать, если бычий тренд и нет короткой позиции
		if (_bullLength >= Length && Position >= 0)
			shouldClearProcessor = true;
		// Нужно покупать, если медвежий тренд и нет длинной позиции
		else if (_bearLength >= Length && Position <= 0)
			shouldClearProcessor = true;

		if (shouldClearProcessor)
		{
			_quotingProcessor?.Dispose();
			_quotingProcessor = null;
		}
	}

	// Создание нового процессора котирования при необходимости
	if (_quotingProcessor == null && IsFormedAndOnlineAndAllowTrading())
	{
		if (_bullLength >= Length && Position >= 0)
		{
			// Бычий тренд - открываем короткую позицию
			CreateQuotingProcessor(Sides.Sell);
			this.AddInfoLog($"Starting SELL quoting after {_bullLength} bullish candles");
		}
		else if (_bearLength >= Length && Position <= 0)
		{
			// Медвежий тренд - открываем длинную позицию
			CreateQuotingProcessor(Sides.Buy);
			this.AddInfoLog($"Starting BUY quoting after {_bearLength} bearish candles");
		}
	}
}
```

## Создание процессора котирования

Метод `CreateQuotingProcessor` создает процессор котирования с указанным направлением:

```cs
private void CreateQuotingProcessor(Sides side)
{
	// Создание поведения для рыночного котирования
	var behavior = new MarketQuotingBehavior(
		0, // Нет смещения цены
		new Unit(0.1m, UnitTypes.Percent), // Используем 0.1% как минимальное отклонение
		MarketPriceTypes.Following // Следование за рыночной ценой
	);

	// Создание процессора котирования
	_quotingProcessor = new(
		behavior,
		Security,
		Portfolio,
		side,
		Volume, // Объем котирования
		Volume, // Максимальный объем заявки
		TimeSpan.Zero, // Нет таймаута
		this, // Стратегия реализует ISubscriptionProvider
		this, // Стратегия реализует IMarketRuleContainer
		this, // Стратегия реализует ITransactionProvider
		this, // Стратегия реализует ITimeProvider
		this, // Стратегия реализует IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Проверка разрешения торговли
		true, // Использовать цены стакана
		true // Использовать цену последней сделки, если стакан пуст
	)
	{
		Parent = this
	};

	// Подписка на события процессора
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Order {order.TransactionId} registered at price {order.Price}");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Order failed: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk =>
	{
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// Инициализация процессора
	_quotingProcessor.Start();
}
```

## Логика торговли

- **Сигнал на продажу**: `Length` последовательных бычьих свечей (цена закрытия выше цены открытия) при отсутствии короткой позиции
- **Сигнал на покупку**: `Length` последовательных медвежьих свечей (цена закрытия ниже цены открытия) при отсутствии длинной позиции
- Для входа в рынок используется процессор котирования, который следует за рыночной ценой

## Особенности

- Стратегия автоматически определяет инструменты для работы через метод `GetWorkingSecurities()`
- Стратегия работает только с завершенными свечами
- Вместо рыночных ордеров используется котирование для более эффективного входа в рынок
- Применяется контртрендовый подход, открывая позиции против установившегося тренда
- Реализовано логирование основных событий для отладки
- Процессор котирования автоматически очищается при смене направления тренда или при достижении целей
- Поддерживается визуализация свечей и сделок на графике
- Реализована оптимизация параметра длины последовательности для настройки стратегии
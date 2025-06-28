# Торговые операции в стратегиях

В StockSharp класс [Strategy](xref:StockSharp.Algo.Strategies.Strategy) предоставляет различные способы работы с заявками для максимального удобства при реализации торговых стратегий.

## Способы выставления заявок

Существует несколько способов выставления заявок в стратегиях StockSharp:

### 1. Использование высокоуровневых методов

Самый простой способ — использование встроенных методов, которые создают и регистрируют заявку за один вызов:

```cs
// Покупка по рыночной цене
BuyMarket(volume);

// Продажа по рыночной цене
SellMarket(volume);

// Покупка по лимитной цене
BuyLimit(price, volume);

// Продажа по лимитной цене
SellLimit(price, volume);

// Закрытие текущей позиции по рыночной цене
ClosePosition();
```

Эти методы обеспечивают максимальную простоту и читаемость кода. Они автоматически:
- Создают объект заявки с указанными параметрами
- Заполняют необходимые поля (инструмент, портфель и т.д.)
- Регистрируют заявку в торговой системе

### 2. Использование CreateOrder + RegisterOrder

Более гибкий подход заключается в раздельном создании и регистрации заявок:

```cs
// Создаем объект заявки
var order = CreateOrder(Sides.Buy, price, volume);

// Дополнительная настройка заявки
order.Comment = "My special order";
order.TimeInForce = TimeInForce.MatchOrCancel;

// Регистрация заявки
RegisterOrder(order);
```

Метод [CreateOrder](xref:StockSharp.Algo.Strategies.Strategy.CreateOrder(StockSharp.Messages.Sides,System.Decimal,System.Nullable{System.Decimal})) создает инициализированный объект заявки, который затем можно дополнительно настроить перед регистрацией.

### 3. Прямое создание и регистрация заявки

Для максимального контроля можно создать объект заявки напрямую и зарегистрировать его:

```cs
// Создаем объект заявки напрямую
var order = new Order
{
	Security = Security,
	Portfolio = Portfolio,
	Side = Sides.Buy,
	Type = OrderTypes.Limit,
	Price = price,
	Volume = volume,
	Comment = "Custom order"
};

// Регистрация заявки
RegisterOrder(order);
```

Подробнее о работе с заявками можно узнать в разделе [Заявки](../orders_management.md).

## Обработка событий заявок

После регистрации заявки важно отслеживать её состояние. Для этого в стратегии можно:

### 1. Использовать обработчики событий

```cs
// Подписка на событие изменения заявки
OrderChanged += OnOrderChanged;

// Подписка на событие исполнения заявки
OrderRegisterFailed += OnOrderRegisterFailed;

private void OnOrderChanged(Order order)
{
	if (order.State == OrderStates.Done)
	{
		// Заявка исполнена - выполняем соответствующую логику
	}
}

private void OnOrderRegisterFailed(OrderFail fail)
{
	// Обработка ошибки регистрации заявки
	LogError($"Ошибка регистрации заявки: {fail.Error}");
}
```

### 2. Использовать правила для заявок

Более мощный подход — использование [правил](event_model.md) для заявок:

```cs
// Создаем заявку
var order = BuyLimit(price, volume);

// Создаем правило, которое сработает при исполнении заявки
order
	.WhenMatched(this)
	.Do(() => {
		// Действия после исполнения заявки
		LogInfo($"Заявка {order.TransactionId} исполнена");
		
		// Например, выставляем стоп-заявку
		var stopOrder = SellLimit(price * 0.95, volume);
	})
	.Apply(this);

// Правило для обработки ошибки регистрации
order
	.WhenRegisterFailed(this)
	.Do(fail => {
		LogError($"Ошибка регистрации заявки: {fail.Error}");
		// Возможно, повторная попытка с другими параметрами
	})
	.Apply(this);
```

Подробные примеры использования правил с заявками можно найти в разделе [Примеры правил на заявки](event_model/samples/rule_order.md).

## Управление позицией

Стратегия также предоставляет методы для управления позицией:

```cs
// Получение текущей позиции
decimal currentPosition = Position;

// Закрытие текущей позиции
ClosePosition();

// Защита позиции через стоп-лосс и тейк-профит
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute),   // тейк-профит
	stopLoss: new Unit(20, UnitTypes.Absolute),     // стоп-лосс
	isStopTrailing: true,                        // трейлинг стоп
	useMarketOrders: true                        // использовать рыночные заявки
);
```

## Пример использования торговых операций в стратегии

## Состояние стратегии перед торговлей

Перед выполнением торговых операций важно убедиться, что стратегия находится в корректном состоянии. StockSharp предоставляет несколько свойств и методов для проверки готовности стратегии:

### Свойство IsFormed

Свойство [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) указывает, сформированы ли (прогреты) все индикаторы, используемые в стратегии. По умолчанию проверяет, чтобы все индикаторы, добавленные в коллекцию [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators), были в состоянии [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) = `true`.

Подробнее о работе с индикаторами в стратегии можно прочитать в разделе [Индикаторы в стратегии](indicators.md).

### Свойство IsOnline

Свойство [IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) показывает, находится ли стратегия в режиме реального времени. Оно становится `true` только когда стратегия запущена и все её подписки на маркет-данные перешли в состояние [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online).

Более подробно о подписках на маркет-данные в стратегиях можно узнать в разделе [Подписки на маркет-данные в стратегиях](subscriptions.md).

### Свойство TradingMode

Свойство [TradingMode](xref:StockSharp.Algo.Strategies.Strategy.TradingMode) определяет режим торговли для стратегии. Возможные значения:

- [StrategyTradingModes.Full](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Full) - разрешены все торговые операции (режим по умолчанию)
- [StrategyTradingModes.Disabled](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Disabled) - торговля полностью запрещена
- [StrategyTradingModes.CancelOrdersOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.CancelOrdersOnly) - разрешено только снятие заявок
- [StrategyTradingModes.ReducePositionOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.ReducePositionOnly) - разрешены только операции по уменьшению позиции

Это свойство можно настроить через параметры стратегии:

```cs
public SmaStrategy()
{
	_tradingMode = Param(nameof(TradingMode), StrategyTradingModes.Full)
					.SetDisplay("Режим торговли", "Разрешенные торговые операции", "Основные настройки");
}
```

### Вспомогательные методы проверки состояния

Для удобной проверки готовности стратегии к торговле, StockSharp предоставляет вспомогательные методы:

- [IsFormedAndOnline()](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnline) - проверяет, что стратегия находится в состоянии `IsFormed = true` и `IsOnline = true`

- [IsFormedAndOnlineAndAllowTrading(StrategyTradingModes)](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) - проверяет, что стратегия сформирована, находится в онлайн-режиме и имеет необходимые права на торговлю

Метод `IsFormedAndOnlineAndAllowTrading` принимает опциональный параметр `required` типа [StrategyTradingModes](xref:StockSharp.Algo.Strategies.StrategyTradingModes):

```cs
public bool IsFormedAndOnlineAndAllowTrading(StrategyTradingModes required = StrategyTradingModes.Full)
```

Этот параметр позволяет указать минимальный уровень прав на торговлю, необходимый для конкретной операции:

1. **StrategyTradingModes.Full** (значение по умолчанию) - возвращает `true`, только если стратегия находится в режиме полной торговли (`TradingMode = StrategyTradingModes.Full`). Используется для операций, которые могут увеличивать позицию.

2. **StrategyTradingModes.ReducePositionOnly** - возвращает `true`, если стратегия находится в режиме полной торговли или в режиме только уменьшения позиции. Используется для операций закрытия или частичного закрытия позиции.

3. **StrategyTradingModes.CancelOrdersOnly** - возвращает `true` при любом активном режиме торговли (кроме `Disabled`). Используется для операций отмены заявок.

Это позволяет избирательно разрешать или запрещать различные торговые операции в зависимости от текущего режима торговли:

```cs
// Для выставления новой заявки, увеличивающей позицию, требуется полный режим торговли
if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.Full))
{
	// Можем выставлять любые заявки
	RegisterOrder(CreateOrder(Sides.Buy, price, volume));
}
// Для закрытия позиции достаточно режима уменьшения позиции
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly) && Position != 0)
{
	// Можем только закрывать позицию
	ClosePosition();
}
// Для снятия активных заявок достаточно режима снятия заявок
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
{
	// Можем только снимать заявки
	CancelActiveOrders();
}
```

Таким образом, данный метод позволяет реализовать безопасный механизм контроля доступа к торговым функциям, при котором более критичные операции (например, открытие новых позиций) требуют более высокого уровня прав, а менее критичные (снятие заявок) выполняются даже при ограниченном режиме торговли.

Хорошей практикой является использование этих методов перед выполнением торговых операций:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Проверка, сформирована ли стратегия и в онлайн-режиме ли она,
	// и разрешена ли торговля
	if (!IsFormedAndOnlineAndAllowTrading())
		return;
	
	// Торговая логика
	// ...
}
```

Ниже приведен пример, демонстрирующий различные способы выставления заявок в стратегии и обработку их исполнения:

```cs
protected override void OnStarted(DateTimeOffset time)
{
	base.OnStarted(time);
	
	// Подписка на свечи
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);
	
	// Создание правила для обработки свечей
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);
	
	Connector.Subscribe(subscription);
}

private void ProcessCandle(ICandleMessage candle)
{
	// Проверка готовности стратегии к торговле
	if (!this.IsFormedAndOnlineAndAllowTrading())
		return;
	
	// Пример торговой логики на основе цены закрытия
	if (candle.ClosePrice > _previousClose * 1.01)
	{
		// Вариант 1: Использование высокоуровневого метода
		var order = BuyLimit(candle.ClosePrice, Volume);
		
		// Создаем правило для обработки исполнения заявки
		order
			.WhenMatched(this)
			.Do(() => {
				// При исполнении заявки выставляем стоп-лосс и тейк-профит
				StartProtection(
					takeProfit: new Unit(50, UnitTypes.Absolute),
					stopLoss: new Unit(20, UnitTypes.Absolute)
				);
			})
			.Apply(this);
	}
	else if (candle.ClosePrice < _previousClose * 0.99)
	{
		// Вариант 2: Раздельное создание и регистрация
		var order = CreateOrder(Sides.Sell, candle.ClosePrice, Volume);
		RegisterOrder(order);
		
		// Альтернативный способ обработки через событие
		OrderChanged += (o) => {
			if (o == order && o.State == OrderStates.Done)
			{
				// Действия после исполнения
			}
		};
	}
	
	_previousClose = candle.ClosePrice;
}
```

## См. также

- [Заявки](../orders_management.md)
- [Правила на заявки](event_model/samples/rule_order.md)
- [Событийная модель](event_model.md)
- [Защита позиций](take_profit_and_stop_loss.md)
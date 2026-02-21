# Подписки на маркет-данные в стратегиях

В StockSharp стратегии используют механизм подписок для получения маркет-данных. Этот подход является основным и предпочтительным способом получения данных в торговых стратегиях.

## Основы подписок

Подписки в стратегиях работают на базе общего механизма [подписок StockSharp](../market_data/subscriptions.md). Они обеспечивают централизованный и унифицированный способ получения всех типов маркет-данных.

## Создание подписки в стратегии

В методе [OnStarted](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) стратегии вы можете создать и запустить подписку на необходимые данные:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Создание подписки на 5-минутные свечи напрямую через DataType
	var subscription = new Subscription(
		TimeSpan.FromMinutes(5).TimeFrame(),
		Security);
	
	// Если требуется указать дополнительные параметры, можно настроить подписку
	subscription.From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(7));
	
	// Создание правила для обработки поступающих свечей
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);
	
	// Запуск подписки
	Subscribe(subscription);
}
```

В этом примере создается подписка на 5-минутные свечи с использованием удобного конструктора, принимающего `DataType` и `Security`. При необходимости можно дополнительно настроить параметры подписки, такие как период истории.

## Преимущества подписок в стратегиях

Использование подписок в стратегиях имеет ряд преимуществ по сравнению с прямой подпиской на события [Strategy.Connector](xref:StockSharp.Algo.Strategies.Strategy.Connector):

1. **Изолированность** - каждая подписка работает независимо, что позволяет получать разные типы данных для разных инструментов без взаимных помех. Также это защищает стратегию от получения данных, предназначенных для других стратегий, запущенных параллельно. При прямой подписке на события коннектора пришлось бы дополнительно фильтровать данные, чтобы исключить информацию от других стратегий.

2. **Управление состоянием** - подписки имеют четкие состояния ([SubscriptionStates](xref:StockSharp.Messages.SubscriptionStates)), которые позволяют точно определить, получаются ли в данный момент исторические данные или подписка уже перешла в онлайн-режим.

3. **Автоматический контроль состояния стратегии** - стратегия автоматически отслеживает состояние всех своих подписок и переходит в онлайн-режим ([IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline)) только когда все подписки перешли в онлайн.

4. **Единообразие кода** - подписки используют унифицированный подход, не зависящий от типа запрашиваемых данных.

5. **Интеграция с правилами** - подписки легко интегрируются с [событийной моделью](event_model.md) стратегий через правила.

6. **Автоматическое управление подписками** - при остановке стратегии все её подписки автоматически отменяются, освобождая ресурсы.

7. **Поддержка исторических данных** - возможность загрузить исторические данные перед переходом к данным реального времени.

## Мониторинг состояния подписок

Стратегия автоматически отслеживает состояние всех подписок для контроля своего режима работы:

```cs
private void CheckRefreshOnlineState()
{
	bool nowOnline = ProcessState == ProcessStates.Started;

	if (nowOnline)
		nowOnline = _subscriptions.CachedKeys
			.Where(s => !s.SubscriptionMessage.IsHistoryOnly())
			.All(s => s.State == SubscriptionStates.Online);
	
	// Обновляем состояние IsOnline стратегии
	IsOnline = nowOnline;
}
```

Свойство [Strategy.IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) будет `true` только когда все подписки стратегии перешли в состояние [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online). Это позволяет стратегии понять момент, когда она работает с актуальными данными рынка.

## Типы подписок

В стратегиях можно использовать подписки на различные типы маркет-данных:

```cs
// Подписка на свечи
var candleSubscription = new Subscription(
	TimeSpan.FromMinutes(1).TimeFrame(),
	Security);

// Подписка на стакан
var depthSubscription = new Subscription(
	DataType.MarketDepth,
	Security);

// Подписка на тиковые сделки
var tickSubscription = new Subscription(
	DataType.Ticks,
	Security);

// Подписка на Level1 (лучший бид/аск и другую базовую информацию)
var level1Subscription = new Subscription(
	DataType.Level1,
	Security);
```

## Обработка данных подписки через правила

Для обработки данных, поступающих через подписку, рекомендуется использовать [правила](event_model.md):

```cs
// Подписка на свечи
var subscription = new Subscription(TimeSpan.FromMinutes(5).TimeFrame(), Security);

// Создание правила для обработки поступающих свечей
Connector
	.WhenCandlesFinished(subscription)  // Активация правила при получении завершенной свечи
	.Do(ProcessCandle)                   // Вызов метода обработки
	.Apply(this);                        // Применение правила к стратегии

// Запуск подписки
Subscribe(subscription);
```

В примере выше создается правило, которое будет вызывать метод `ProcessCandle` при поступлении каждой завершенной свечи.

## Запрос исторических данных

Стратегия автоматически устанавливает период загрузки истории через свойство [Strategy.HistorySize](xref:StockSharp.Algo.Strategies.Strategy.HistorySize):

```cs
// Установка периода загрузки истории на 30 дней
strategy.HistorySize = TimeSpan.FromDays(30);
```

При создании подписки стратегия автоматически устанавливает параметр `From` для загрузки истории, если он не был указан явно.

## Отмена подписок

Подписки можно отменить вручную, вызвав метод [UnSubscribe](xref:StockSharp.BusinessEntities.ISubscriptionProvider.UnSubscribe(StockSharp.BusinessEntities.Subscription)):

```cs
// Отмена подписки
Connector.UnSubscribe(subscription);
```

При остановке стратегии, если параметр [UnsubscribeOnStop](xref:StockSharp.Algo.Strategies.Strategy.UnsubscribeOnStop) установлен в `true` (по умолчанию), все подписки будут автоматически отменены.

## См. также

- [Подписки на маркет-данные](../market_data/subscriptions.md)
- [Событийная модель](event_model.md)
- [Совместимость стратегий с платформами](compatibility.md)

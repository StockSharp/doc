# Подписки

**StockSharp API** предлагает модель получения данных, основанную на подписках. Это универсальный механизм для получения как маркет-данных, так и транзакционной информации. Данный подход обладает значительными преимуществами:

- **Изоляция подписок** — каждая подписка работает независимо, что позволяет параллельно запускать произвольное количество подписок с различными параметрами (с запросом истории или без).
- **Отслеживание состояния** — подписки имеют определенные состояния, которые позволяют контролировать, идут ли в данный момент исторические данные или подписка перешла в режим реального времени.
- **Универсальность** — код для работы с подписками одинаков независимо от запрашиваемых типов данных, что делает разработку более эффективной.

Для работы с подписками необходимо использовать класс [Subscription](xref:StockSharp.BusinessEntities.Subscription). Рассмотрим примеры использования подписок для получения различных типов данных.

## Пример подписки на свечи

```cs
// Создаем подписку на 5-минутные свечи
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
    // Настраиваем параметры подписки через свойство MarketData
    MarketData =
    {
        // Запрашиваем данные за последние 30 дней
        From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
        // null означает, что подписка после получения истории перейдет в режим реального времени
        To = null
    }
};

// Обработка полученных свечей
_connector.CandleReceived += (sub, candle) =>
{
    if (sub != subscription)
        return;
        
    // Обработка свечи
    Console.WriteLine($"Свеча: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
};

// Обработка перехода подписки в онлайн-режим
_connector.SubscriptionOnline += (sub) =>
{
    if (sub != subscription)
        return;
        
    Console.WriteLine("Подписка перешла в режим реального времени");
};

// Обработка ошибок подписки
_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
{
    if (sub != subscription)
        return;
        
    Console.WriteLine($"Ошибка подписки: {error}");
};

// Запуск подписки
_connector.Subscribe(subscription);
```

## Пример подписки на стакан

```cs
// Создаем подписку на стакан для выбранного инструмента
var depthSubscription = new Subscription(DataType.MarketDepth, security);

// Обработка полученных стаканов
_connector.OrderBookReceived += (sub, depth) =>
{
    if (sub != depthSubscription)
        return;
        
    // Обработка стакана
    Console.WriteLine($"Стакан: {depth.SecurityId}, Время: {depth.ServerTime}");
    Console.WriteLine($"Покупки (Bids): {depth.Bids.Count}, Продажи (Asks): {depth.Asks.Count}");
};

// Запуск подписки
_connector.Subscribe(depthSubscription);
```

## Пример подписки на тиковые сделки

```cs
// Создаем подписку на тиковые сделки для выбранного инструмента
var tickSubscription = new Subscription(DataType.Ticks, security);

// Обработка полученных тиков
_connector.TickTradeReceived += (sub, tick) =>
{
    if (sub != tickSubscription)
        return;
        
    // Обработка тика
    Console.WriteLine($"Тик: {tick.SecurityId}, Время: {tick.ServerTime}, Цена: {tick.Price}, Объем: {tick.Volume}");
};

// Запуск подписки
_connector.Subscribe(tickSubscription);
```

## Пример подписки с настройкой режима построения свечей

```cs
// Подписка на 5-минутные свечи, которые будут построены из тиков
var candleSubscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
    MarketData =
    {
        // Указываем режим построения и источник данных
        BuildMode = MarketDataBuildModes.Build,
        BuildFrom = DataType.Ticks,
        // Также можно включить построение профиля объема
        IsCalcVolumeProfile = true,
    }
};

_connector.Subscribe(candleSubscription);
```

## Пример подписки на Level1 (базовая информация по инструменту)

```cs
// Создание подписки на базовую информацию по инструменту
var level1Subscription = new Subscription(DataType.Level1, security);

// Обработка полученных данных Level1
_connector.Level1Received += (sub, level1) =>
{
    if (sub != level1Subscription)
        return;
    
    Console.WriteLine($"Level1: {level1.SecurityId}, Время: {level1.ServerTime}");
    
    // Вывод значений полей Level1
    foreach (var pair in level1.Changes)
    {
        Console.WriteLine($"Поле: {pair.Key}, Значение: {pair.Value}");
    }
};

// Запуск подписки
_connector.Subscribe(level1Subscription);
```

## Отписка от данных

Для прекращения получения данных используется метод `UnSubscribe`:

```cs
// Отписка от конкретной подписки
_connector.UnSubscribe(subscription);

// Или можно отписаться от всех подписок
foreach (var sub in _connector.Subscriptions)
{
    _connector.UnSubscribe(sub);
}
```

## Состояния подписок

Подписки могут находиться в следующих состояниях:

- [SubscriptionStates.Stopped](xref:StockSharp.Messages.SubscriptionStates.Stopped) — подписка неактивна (остановлена или не запускалась).
- [SubscriptionStates.Active](xref:StockSharp.Messages.SubscriptionStates.Active) — подписка активна и может передавать исторические данные до перехода в режим реального времени или завершения.
- [SubscriptionStates.Error](xref:StockSharp.Messages.SubscriptionStates.Error) — подписка неактивна и находится в состоянии ошибки.
- [SubscriptionStates.Finished](xref:StockSharp.Messages.SubscriptionStates.Finished) — подписка завершила свою работу (все данные получены).
- [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) — подписка перешла в режим реального времени и передает только актуальные данные.
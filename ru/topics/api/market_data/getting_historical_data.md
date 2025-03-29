# Получение исторических данных

StockSharp API предоставляет удобные механизмы для получения исторических данных, которые можно использовать как для тестирования торговых стратегий, так и для построения [индикаторов](../indicators.md).

## Получение исторических данных через Connector

### Настройка подключения

Для получения исторических данных необходимо сначала настроить подключение к торговой системе:

```cs
// Создаем экземпляр Connector
var connector = new Connector();

// Добавляем адаптер для подключения к Binance
var messageAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
    Key = "<Your API Key>",
    Secret = "<Your Secret Key>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);

// Выполняем подключение
connector.Connect();
```

Подключение также можно настроить с помощью графического интерфейса, как описано в разделе [Окно настройки подключений](../graphical_user_interface/connection_settings_window.md).

### Подписка на исторические свечи

Для получения исторических свечей необходимо создать подписку и указать параметры запрашиваемых данных:

```cs
// Создаем подписку на 5-минутные свечи для выбранного инструмента
var subscription = new Subscription(
    DataType.TimeFrame(TimeSpan.FromMinutes(5)), 
    security)
{
    MarketData =
    {
        // Указываем период, за который нужно получить исторические данные
        From = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
        To = DateTime.Now,
        // Устанавливаем флаг для получения только завершенных свечей
        IsFinishedOnly = true
    }
};

// Подписываемся на событие получения свечей
connector.CandleReceived += OnCandleReceived;

// Запускаем подписку
connector.Subscribe(subscription);

// Обработчик события получения свечей
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
    // Проверяем, что свеча относится к нашей подписке
    if (subscription != _subscription)
        return;
        
    // Обрабатываем полученную свечу
    Console.WriteLine($"Получена свеча: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}, V:{candle.TotalVolume}");
    
    // Для отображения на графике можно использовать:
    // Chart.Draw(_candleElement, candle);
}
```

### Использование свечей для графика

Полученные свечи можно отображать на графике с помощью встроенных графических компонентов StockSharp:

```cs
// Создаем и настраиваем элементы графика
var chart = new Chart();
var area = new ChartArea();
var candleElement = new ChartCandleElement();

// Добавляем область и элемент на график
chart.AddArea(area);
chart.AddElement(area, candleElement, subscription);

// В обработчике события CandleReceived отрисовываем свечи
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
    // Проверяем, что свеча относится к нашей подписке
    if (subscription != _subscription)
        return;
        
    // Если нужно отображать только завершенные свечи
    if (candle.State == CandleStates.Finished)
    {
        var chartData = new ChartDrawData();
        chartData.Group(candle.OpenTime).Add(candleElement, candle);
        chart.Draw(chartData);
    }
}
```

## Получение исторических данных напрямую через MessageAdapter

В некоторых случаях может потребоваться получить исторические данные напрямую через MessageAdapter:

```cs
// Создаем и настраиваем адаптер для Binance
var messageAdapter = new BinanceMessageAdapter(new IncrementalIdGenerator())
{
    Key = "<Your API Key>",
    Secret = "<Your Secret Key>",
};

// Если требуется, оборачиваем адаптер в SecurityNativeIdMessageAdapter
var securityAdapter = new SecurityNativeIdMessageAdapter(messageAdapter, new InMemoryNativeIdStorage());

// Получаем список инструментов (криптовалютных пар) с Binance
var securities = securityAdapter.GetSecurities(new SecurityLookupMessage
{
    SecurityId = new SecurityId
    {
        SecurityCode = "BTCUSDT"
    }
});

// Находим нужный инструмент
SecurityMessage btcusdt = null;
foreach (var security in securities)
{
    if (security.SecurityId.SecurityCode.CompareIgnoreCase("BTCUSDT"))
        btcusdt = security;
}

// Получаем исторические свечи для найденного инструмента
if (btcusdt != null)
{
    // Получаем часовые свечи за последние 30 дней
    var candles = securityAdapter.GetCandles(
        btcusdt.SecurityId, 
        TimeSpan.FromHours(1), 
        DateTimeOffset.Now.AddDays(-30), 
        DateTimeOffset.Now);
        
    foreach (var candle in candles)
    {
        Console.WriteLine($"{candle.OpenTime}: O={candle.OpenPrice}, H={candle.HighPrice}, L={candle.LowPrice}, C={candle.ClosePrice}, V={candle.TotalVolume}");
    }
}
```

## Получение других типов исторических данных

Аналогичным образом можно получать и другие типы исторических данных:

### Получение исторических тиков

```cs
var tickSubscription = new Subscription(DataType.Ticks, security)
{
    MarketData =
    {
        From = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
        To = DateTime.Now
    }
};

connector.TickTradeReceived += (subscription, tick) =>
{
    if (subscription == tickSubscription)
        Console.WriteLine($"Тик: {tick.ServerTime}, Цена: {tick.Price}, Объем: {tick.Volume}");
};

connector.Subscribe(tickSubscription);
```

### Получение исторических стаканов

```cs
var depthSubscription = new Subscription(DataType.MarketDepth, security)
{
    MarketData =
    {
        From = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
        To = DateTime.Now
    }
};

connector.OrderBookReceived += (subscription, depth) =>
{
    if (subscription == depthSubscription)
        Console.WriteLine($"Стакан: {depth.ServerTime}, Лучшая покупка: {depth.GetBestBid()?.Price}, Лучшая продажа: {depth.GetBestAsk()?.Price}");
};

connector.Subscribe(depthSubscription);
```

## См. также

- [Свечи](../candles.md)
- [Подписки](subscriptions.md)
- [Индикаторы](../indicators.md)
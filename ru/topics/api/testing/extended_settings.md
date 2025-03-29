# Настройки тестирования

В этом разделе описаны основные настройки [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) для тестирования торговых стратегий.

## Основные настройки эмулятора

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - интервал прихода события о смене времени. Если используются генераторы сделок, сделки будут генерироваться с этой периодичностью. По-умолчанию равно 1 минуте.
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - минимальное значение задержки выставляемых заявок. По-умолчанию равно TimeSpan.Zero, что означает мгновенное принятие биржей выставляемых заявок. 
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - удовлетворять заявки, если цена "коснулась" уровня (допущение иногда слишком "оптимистично" и для реалистичного тестирования следует выключить режим). Если режим выключен, то лимитные заявки будут удовлетворяться, если цена "прошла сквозь них" хотя бы на 1 шаг. Опция работает во всех режимах кроме ордер лога. По-умолчанию выключено.

## Подписки на рыночные данные

Для корректного тестирования стратегии необходимо настроить подписки на нужные типы рыночных данных. Даже если стратегия тестируется на свечах, для корректной эмуляции торговли необходимо подписаться на тиковые сделки:

```cs
// Создаем подписку на тиковые сделки
var tickSubscription = new [Subscription](xref:StockSharp.BusinessEntities.Subscription)([DataType](xref:StockSharp.Messages.DataType).[Ticks](xref:StockSharp.Messages.DataType.Ticks), security);
_connector.Subscribe(tickSubscription);
```

Если для стратегии требуются данные стакана:

```cs
// Создаем подписку на стаканы
var depthSubscription = new [Subscription](xref:StockSharp.BusinessEntities.Subscription)([DataType](xref:StockSharp.Messages.DataType).[MarketDepth](xref:StockSharp.Messages.DataType.MarketDepth), security);
_connector.Subscribe(depthSubscription);
```

## Генерация стаканов для тестирования

Если исторических стаканов нет, но они нужны для тестирования стратегии, можно включить генерацию стаканов:

```cs
// Создаем генератор стаканов с трендовым поведением
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// Отправляем сообщение о подписке на генератор
_connector.[MarketDataAdapter](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketDataAdapter).SendInMessage(new [GeneratorMessage](xref:StockSharp.Messages.GeneratorMessage)
{
    IsSubscribe = true,
    Generator = mdGenerator
});
```

### Настройка генератора стаканов

- Интервал обновления стакана ([MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval)) - обновление не может быть чаще, чем приходят тиковые сделки, т.к. стаканы генерируются перед каждой сделкой:

```cs
mdGenerator.Interval = TimeSpan.FromSeconds(1);
```

- Глубина стаканов ([MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) и [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth)) - чем больше, тем медленнее тестирование:

```cs
mdGenerator.MaxAsksDepth = 1; 
mdGenerator.MaxBidsDepth = 1;
```

- Для получения реалистичного объема уровня в стакане можно использовать опцию [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume), при этом объемы у [MarketDepth.BestBid](xref:StockSharp.BusinessEntities.MarketDepth.BestBid) и [MarketDepth.BestAsk](xref:StockSharp.BusinessEntities.MarketDepth.BestAsk) будут браться из объема сделки, по которой идет генерация:

```cs
mdGenerator.UseTradeVolume = true;
```

- Диапазон объема уровня ([MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) и [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume)):

```cs
mdGenerator.MinVolume = 1;
mdGenerator.MaxVolume = 1;
```

- Настройка спреда - минимальный генерируемый спред равен [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep). Рекомендуется не генерировать спред между [MarketDepth.BestBid](xref:StockSharp.BusinessEntities.MarketDepth.BestBid) и [MarketDepth.BestAsk](xref:StockSharp.BusinessEntities.MarketDepth.BestAsk) больше чем 5 шагов цены, чтобы при генерации из свечей не получалось слишком широкого спреда:

```cs
mdGenerator.MinSpreadStepCount = 1;
mdGenerator.MaxSpreadStepCount = 5;
```

## Комплексный пример настройки тестирования

```cs
// Создаем историческое подключение
var connector = new [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector)();

// Настраиваем основные параметры
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.[EmulationAdapter](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.EmulationAdapter).Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.[EmulationAdapter](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.EmulationAdapter).Emulator.Settings.MatchOnTouch = false;

// Загружаем исторические данные
var storage = new StorageRegistry();
var security = new Security { Id = "AAPL", PriceStep = 0.01m };

// Создаем подписку на свечи
var candleSubscription = new Subscription(
    DataType.TimeFrame(TimeSpan.FromMinutes(5)),
    security)
{
    MarketData =
    {
        From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
        To = DateTime.Today,
        BuildMode = MarketDataBuildModes.Load
    }
};
connector.Subscribe(candleSubscription);

// Создаем подписку на тики для корректной эмуляции
var tickSubscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(tickSubscription);

// Настраиваем генерацию стаканов
var mdGenerator = new [TrendMarketDepthGenerator](xref:StockSharp.Algo.Testing.TrendMarketDepthGenerator)(security.ToSecurityId())
{
    [Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval) = TimeSpan.FromSeconds(1),
    [MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth) = 5,
    [MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) = 5,
    [UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume) = true,
    [MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) = 1,
    [MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume) = 100,
    [MinSpreadStepCount](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MinSpreadStepCount) = 1,
    [MaxSpreadStepCount](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxSpreadStepCount) = 5
};

connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
    IsSubscribe = true,
    Generator = mdGenerator
});

// Подписываемся на получение данных
connector.[CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) += OnCandleReceived;
connector.[TickTradeReceived](xref:StockSharp.Algo.Connector.TickTradeReceived) += OnTickReceived;
connector.[OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) += OnDepthReceived;

// Запускаем тестирование
connector.Connect();
```

// Обработка событий

```cs
private void OnCandleReceived([Subscription](xref:StockSharp.BusinessEntities.Subscription) subscription, [ICandleMessage](xref:StockSharp.Messages.ICandleMessage) candle)
{
    // Обработка полученных свечей
    Console.WriteLine($"Свеча: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived([Subscription](xref:StockSharp.BusinessEntities.Subscription) subscription, [ITickTradeMessage](xref:StockSharp.Messages.ITickTradeMessage) tick)
{
    // Обработка полученных тиков
    Console.WriteLine($"Тик: {tick.ServerTime}, Цена: {tick.Price}, Объем: {tick.Volume}");
}

private void OnDepthReceived([Subscription](xref:StockSharp.BusinessEntities.Subscription) subscription, [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) depth)
{
    // Обработка полученных стаканов
    Console.WriteLine($"Стакан: {depth.ServerTime}, Лучшая покупка: {depth.GetBestBid()?.Price}, Лучшая продажа: {depth.GetBestAsk()?.Price}");
}
```
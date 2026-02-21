# Настройки тестирования

В этом разделе описаны основные настройки [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) для тестирования торговых стратегий.

## Основные настройки эмулятора

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - интервал прихода события о смене времени. Если используются генераторы сделок, сделки будут генерироваться с этой периодичностью. По-умолчанию равно 1 минуте.
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - минимальное значение задержки выставляемых заявок. По-умолчанию равно TimeSpan.Zero, что означает мгновенное принятие биржей выставляемых заявок. 
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - удовлетворять заявки, если цена "коснулась" уровня (допущение иногда слишком "оптимистично" и для реалистичного тестирования следует выключить режим). Если режим выключен, то лимитные заявки будут удовлетворяться, если цена "прошла сквозь них" хотя бы на 1 шаг. Опция работает во всех режимах кроме ордер лога. По-умолчанию включено.
- [MarketEmulatorSettings.CandlePrice](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CandlePrice) - цена свечи для исполнения заявок (Middle, Open, High, Low, Close). По-умолчанию Middle.
- [MarketEmulatorSettings.Failing](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Failing) - процент ошибок регистрации новых заявок. Значение от 0 (нет ошибок) до 100. По-умолчанию выключено (0).
- [MarketEmulatorSettings.InitialOrderId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialOrderId) - начальный номер, с которого эмулятор будет генерировать идентификаторы заявок.
- [MarketEmulatorSettings.InitialTradeId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialTradeId) - начальный номер, с которого эмулятор будет генерировать идентификаторы сделок.
- [MarketEmulatorSettings.SpreadSize](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.SpreadSize) - размер спреда в шагах цены. Используется при генерации стакана из тиковых сделок. По-умолчанию равно 2.
- [MarketEmulatorSettings.MaxDepth](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MaxDepth) - максимальная глубина стакана, генерируемого из тиков. По-умолчанию равно 5.
- [MarketEmulatorSettings.PortfolioRecalcInterval](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PortfolioRecalcInterval) - интервал пересчета данных портфеля. Если равен TimeSpan.Zero, пересчет не выполняется.
- [MarketEmulatorSettings.ConvertTime](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.ConvertTime) - конвертировать время заявок и сделок в биржевое время. По-умолчанию выключено.
- [MarketEmulatorSettings.TimeZone](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.TimeZone) - информация о часовом поясе биржи.
- [MarketEmulatorSettings.PriceLimitOffset](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PriceLimitOffset) - сдвиг цены от предыдущей сделки, определяющий границы максимальных и минимальных цен для следующей сессии. По-умолчанию 40%.
- [MarketEmulatorSettings.IncreaseDepthVolume](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.IncreaseDepthVolume) - добавлять дополнительный объем в стакан при регистрации заявок с большим объемом. По-умолчанию включено.
- [MarketEmulatorSettings.CheckTradingState](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradingState) - проверять состояние торговой сессии. По-умолчанию выключено.
- [MarketEmulatorSettings.CheckMoney](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckMoney) - проверять баланс денежных средств. По-умолчанию выключено.
- [MarketEmulatorSettings.CheckShortable](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckShortable) - проверять возможность коротких позиций. По-умолчанию выключено.
- [MarketEmulatorSettings.CheckTradableDates](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradableDates) - проверять, являются ли загружаемые даты торговыми. По-умолчанию выключено.
- [MarketEmulatorSettings.CommissionRules](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CommissionRules) - правила расчета комиссий.

## Подписки на рыночные данные

Для корректного тестирования стратегии необходимо настроить подписки на нужные типы рыночных данных. Даже если стратегия тестируется на свечах, для корректной эмуляции торговли необходимо подписаться на тиковые сделки:

```cs
// Создаем подписку на тиковые сделки
var tickSubscription = new Subscription(DataType.Ticks, security);
_connector.Subscribe(tickSubscription);
```

Если для стратегии требуются данные стакана:

```cs
// Создаем подписку на стаканы
var depthSubscription = new Subscription(DataType.MarketDepth, security);
_connector.Subscribe(depthSubscription);
```

## Генерация стаканов для тестирования

Если исторических стаканов нет, но они нужны для тестирования стратегии, можно включить генерацию стаканов:

```cs
// Создаем генератор стаканов с трендовым поведением
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// Отправляем сообщение о подписке на генератор
_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
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

- Для получения реалистичного объема уровня в стакане можно использовать опцию [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume), при этом объемы у лучших котировок будут браться из объема сделки, по которой идет генерация:

```cs
mdGenerator.UseTradeVolume = true;
```

- Диапазон объема уровня ([MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) и [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume)):

```cs
mdGenerator.MinVolume = 1;
mdGenerator.MaxVolume = 1;
```

- Настройка спреда - минимальный генерируемый спред равен [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep). Рекомендуется не генерировать спред между лучшими котировками больше чем 5 шагов цены, чтобы при генерации из свечей не получалось слишком широкого спреда:

```cs
mdGenerator.MinSpreadStepCount = 1;
mdGenerator.MaxSpreadStepCount = 5;
```

## Комплексный пример настройки тестирования

```cs
// Создаем историческое подключение
var connector = new HistoryEmulationConnector();

// Настраиваем основные параметры
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.EmulationAdapter.Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.EmulationAdapter.Emulator.Settings.MatchOnTouch = false;

// Загружаем исторические данные
var storage = new StorageRegistry();
var security = new Security { Id = "AAPL", PriceStep = 0.01m };

// Создаем подписку на свечи
var candleSubscription = new Subscription(
	TimeSpan.FromMinutes(5).TimeFrame(),
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
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId())
{
	Interval = TimeSpan.FromSeconds(1),
	MaxAsksDepth = 5,
	MaxBidsDepth = 5,
	UseTradeVolume = true,
	MinVolume = 1,
	MaxVolume = 100,
	MinSpreadStepCount = 1,
	MaxSpreadStepCount = 5
};

connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});

// Подписываемся на получение данных
connector.CandleReceived += OnCandleReceived;
connector.TickTradeReceived += OnTickReceived;
connector.OrderBookReceived += OnOrderBookReceived;

// Запускаем тестирование
connector.Connect();
```

## Обработка событий

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Обработка полученных свечей
	Console.WriteLine($"Свеча: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
{
	// Обработка полученных тиков
	Console.WriteLine($"Тик: {tick.ServerTime}, Цена: {tick.Price}, Объем: {tick.Volume}");
}

private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// Используем методы расширения для IOrderBookMessage
	var bestBid = orderBook.GetBestBid();
	var bestAsk = orderBook.GetBestAsk();
	var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);
	
	// Обработка полученных стаканов
	Console.WriteLine($"Стакан: {orderBook.ServerTime}, Лучшая покупка: {bestBid?.Price}, Лучшая продажа: {bestAsk?.Price}, Середина спреда: {spreadMiddle}");
	
	// Получение цены по стороне заявки
	var bidPrice = orderBook.GetPrice(Sides.Buy);
	var askPrice = orderBook.GetPrice(Sides.Sell);
	
	Console.WriteLine($"Цена покупки: {bidPrice}, Цена продажи: {askPrice}");
}
```

## Работа с данными стакана

При работе со стаканами в виде IOrderBookMessage можно использовать следующие методы расширения:

```cs
// Получение лучшей заявки на покупку
var bestBid = orderBook.GetBestBid();

// Получение лучшей заявки на продажу
var bestAsk = orderBook.GetBestAsk();

// Получение середины спреда
var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);

// Получение цены по стороне заявки
var price = orderBook.GetPrice(Sides.Buy); // или Sides.Sell, или null для середины спреда
```

При работе с данными Level1 также можно получить середину спреда:

```cs
// Получение середины спреда из сообщения Level1
var spreadMiddle = level1.GetSpreadMiddle(Security.PriceStep);
```

Эти методы расширения упрощают доступ к данным стакана и позволяют писать более чистый и понятный код при работе с биржевыми котировками.
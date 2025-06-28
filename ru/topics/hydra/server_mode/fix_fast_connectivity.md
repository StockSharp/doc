# Подключение через FIX протокол

[Hydra](../../hydra.md) может использоваться в серверном режиме, в данном режиме можно удаленно подключиться к [Hydra](../../hydra.md) и получить имеющиеся данные в хранилище. Включение серверного режима [Hydra](../../hydra.md) описано в пункте [Настройки](settings.md).

Для подключения через [FIX протокол](../../api/connectors/common/fix_protocol.md) необходимо создать и настроить Fix подключение ([Инициализация адаптера FIX](../../api/connectors/common/fix_protocol/adapter_initialization_fix.md)).

```cs
// Создаем экземпляр коннектора
private readonly Connector _connector = new Connector();

// Настраиваем адаптер для рыночных данных по FIX протоколу
var marketDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(marketDataAdapter);

// Настраиваем адаптер для транзакционных данных
var transactionDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(transactionDataAdapter);
```

Подписываемся на события и настраиваем обработчики данных:

```cs
// Событие успешного подключения
_connector.Connected += () =>
{
	Console.WriteLine("Соединение установлено");
	
	// Создаем подписку на поиск инструментов
	var lookupSubscription = new Subscription(DataType.Securities);
	_connector.Subscribe(lookupSubscription);
};

// Событие разрыва соединения
_connector.Disconnected += () =>
{
	Console.WriteLine("Соединение разорвано");
};

// Событие получения инструмента
_connector.SecurityReceived += (subscription, security) =>
{
	Console.WriteLine($"Получен инструмент: {security.Code}, {security.Id}");
	BufferSecurity.Add(security);
	
	// Если это искомый инструмент, подписываемся на его данные
	if (security.Id == targetSecurityId)
	{
		// Подписка на стакан
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);
		
		// Подписка на тиковые сделки
		var tradesSubscription = new Subscription(DataType.Ticks, security);
		_connector.Subscribe(tradesSubscription);
		
		// Подписка на свечи
		var candleSubscription = new Subscription(
			DataType.TimeFrame(TimeSpan.FromMinutes(5)),
			security)
		{
			MarketData =
			{
				From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
				To = DateTime.Now
			}
		};
		_connector.Subscribe(candleSubscription);
	}
};

// Событие получения тиковой сделки
_connector.TickTradeReceived += (subscription, trade) =>
{
	Console.WriteLine($"Получена сделка: {trade.Security.Code}, {trade.Time}, {trade.Price}, {trade.Volume}");
};

// Событие изменения стакана
_connector.OrderBookReceived += (subscription, depth) =>
{
	Console.WriteLine($"Получен стакан: {depth.SecurityId}, Покупка: {depth.BestBid()?.Price}, Продажа: {depth.BestAsk()?.Price}");
};

// Событие получения свечи
_connector.CandleReceived += (subscription, candle) =>
{
	Console.WriteLine($"Получена свеча: {candle.SecurityId}, {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
};

// Событие ошибки подключения
_connector.ConnectionError += error =>
{
	Console.WriteLine($"Ошибка подключения: {error.Message}");
};

// Событие общей ошибки
_connector.Error += error =>
{
	Console.WriteLine($"Ошибка: {error.Message}");
};

// Событие ошибки подписки на рыночные данные
_connector.SubscriptionFailed += (subscription, error) =>
{
	Console.WriteLine($"Ошибка подписки {subscription.DataType} для {subscription.SecurityId}: {error}");
};

// Подключаемся к серверу
_connector.Connect();
```

## Использование сервисов Hydra

Hydra в режиме сервера предоставляет доступ к различным типам данных. Рассмотрим примеры получения исторических данных:

```cs
// Получение исторических свечей
private void RequestHistoricalCandles(Security security, DateTime from, DateTime to)
{
	// Создаем подписку на исторические свечи
	var candleSubscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		security)
	{
		MarketData =
		{
			From = from,
			To = to
		}
	};
	
	// Подписываемся на обработку полученных свечей
	_connector.CandleReceived += OnCandleReceived;
	
	// Запускаем подписку
	_connector.Subscribe(candleSubscription);
}

private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Проверяем, что свеча относится к нашей подписке
	if (subscription.DataType != DataType.TimeFrame(TimeSpan.FromMinutes(5)))
		return;
		
	Console.WriteLine($"Историческая свеча: {candle.OpenTime}, O: {candle.OpenPrice}, H: {candle.HighPrice}, L: {candle.LowPrice}, C: {candle.ClosePrice}, V: {candle.TotalVolume}");
	
	// Обрабатываем полученные свечи, например, сохраняем в локальное хранилище
	// или используем для анализа/визуализации
}
```

## Отключение от сервера Hydra

```cs
// Корректное закрытие соединения
private void DisconnectFromServer()
{
	// Отписываемся от всех подписок
	foreach (var subscription in _connector.Subscriptions.ToArray())
	{
		_connector.UnSubscribe(subscription);
	}
	
	// Отключаемся от сервера
	_connector.Disconnect();
}
```
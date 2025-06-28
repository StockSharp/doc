# Поиск инструмента

Большинство коннекторов к фондовым биржам США (например, [Interactive Brokers](../connectors/stock_market/interactive_brokers.md), [PolygonIO](../connectors/stock_market/polygonio.md) и другие) не передают все доступные инструменты на клиент после установки соединения через метод [IConnector.Connect](xref:StockSharp.BusinessEntities.IConnector.Connect). Это обусловлено большим количеством инструментов, торгуемых на американских биржах, и сделано для снижения нагрузки на серверы брокеров и источников данных.

## Основы поиска инструментов

Для поиска инструментов в S# применяется механизм подписок, аналогичный получению маркет-данных. Этот подход позволяет использовать единообразный код для всех типов данных, включая инструменты.

### Создание подписки для поиска инструментов

Для поиска инструментов необходимо создать экземпляр класса [Subscription](xref:StockSharp.BusinessEntities.Subscription) на основе сообщения [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage), которое содержит параметры фильтрации:

```csharp
// Создаем объект фильтра для поиска
var lookupMessage = new SecurityLookupMessage
{
	// Установка критериев поиска
	SecurityId = new SecurityId
	{
		// Поиск по коду инструмента (можно использовать маску "AAPL*")
		SecurityCode = "AAPL",
		// Опционально можно указать код площадки
		BoardCode = "NASDAQ"
	},
	// Можно указать тип инструмента
	SecurityType = SecurityTypes.Stock,
	// Установка идентификатора транзакции
	TransactionId = Connector.TransactionIdGenerator.GetNextId()
};

// Создаем подписку на поиск инструментов
var subscription = new Subscription(lookupMessage);
```

### Возможные параметры фильтрации

Сообщение [SecurityLookupMessage](xref:StockSharp.Messages.SecurityLookupMessage) позволяет задать следующие критерии поиска:

- **SecurityId** — идентификатор инструмента, содержащий:
  - **SecurityCode** — код или маска кода инструмента (например, "AAPL" или "MS*")
  - **BoardCode** — код биржевой площадки (например, [ExchangeBoard.Nasdaq](xref:StockSharp.BusinessEntities.ExchangeBoard.Nasdaq))
- **SecurityType** — тип инструмента ([SecurityTypes.Stock](xref:StockSharp.Messages.SecurityTypes.Stock), [SecurityTypes.Future](xref:StockSharp.Messages.SecurityTypes.Future) и др.)
- **SecurityTypes** — массив типов инструментов для расширенного поиска
- **Currency** — валюта торгов инструмента
- **ExpiryDate** — дата экспирации (для деривативов)
- **Strike** — страйк (для опционов)
- **OptionType** — тип опциона (для опционов)
- **Name** — название инструмента или его часть
- **Class** — класс инструмента

### Обработка результатов поиска

После создания подписки необходимо подписаться на события получения инструментов и отправить запрос:

```csharp
// Обработчик события получения инструмента
private void OnSecurityReceived(Subscription subscription, Security security)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Найден инструмент: {security.Id} - {security.Name}, Тип: {security.Type}");
	
	// Здесь можно добавить инструмент в коллекцию или выполнить другие действия
	Securities.Add(security);
}

// Обработчик события окончания поиска
private void OnSubscriptionFinished(Subscription subscription)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Поиск завершен. Найдено инструментов: {Securities.Count}");
}

// Обработчик ошибок подписки
private void OnSubscriptionFailed(Subscription subscription, Exception error, bool isSubscribe)
{
	if (subscription.SubscriptionMessage is not SecurityLookupMessage)
		return;
		
	Console.WriteLine($"Ошибка поиска инструментов: {error.Message}");
}

// Подписываемся на события
Connector.SecurityReceived += OnSecurityReceived;
Connector.SubscriptionFinished += OnSubscriptionFinished;
Connector.SubscriptionFailed += OnSubscriptionFailed;

// Отправляем запрос на поиск инструментов
Connector.Subscribe(subscription);
```

### Полный пример поиска инструментов

Ниже представлен полный пример метода для поиска инструментов:

```csharp
public void FindSecurities(string searchCode, SecurityTypes? securityType = null)
{
	// Создаем объект для поиска инструментов
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId
		{
			SecurityCode = searchCode,
			// Если требуется искать на конкретной площадке
			// BoardCode = ExchangeBoard.Nyse.Code,
		},
		SecurityType = securityType,
		TransactionId = Connector.TransactionIdGenerator.GetNextId()
	};
	
	// Создаем подписку
	var subscription = new Subscription(lookupMessage);
	
	// Очищаем коллекцию для результатов поиска
	_searchResults.Clear();
	
	// Временная коллекция для накопления результатов
	var foundSecurities = new List<Security>();
	
	// Подписка на получение инструментов
	void OnSecurityReceived(Subscription sub, Security security)
	{
		if (sub != subscription)
			return;
			
		// Добавляем найденный инструмент в коллекцию
		foundSecurities.Add(security);
		Console.WriteLine($"Найден: {security.Id}, {security.Name}");
	}
	
	// Подписка на окончание поиска
	void OnSubscriptionFinished(Subscription sub)
	{
		if (sub != subscription)
			return;
			
		// Копируем результаты в основную коллекцию
		_searchResults.AddRange(foundSecurities);
		
		Console.WriteLine($"Поиск завершен. Найдено инструментов: {foundSecurities.Count}");
		
		// Отписываемся от событий
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// Обработка ошибок подписки
	void OnSubscriptionFailed(Subscription sub, Exception error, bool isSubscribe)
	{
		if (sub != subscription)
			return;
			
		Console.WriteLine($"Ошибка поиска инструментов: {error.Message}");
		
		// Отписываемся от событий
		Connector.SecurityReceived -= OnSecurityReceived;
		Connector.SubscriptionFinished -= OnSubscriptionFinished;
		Connector.SubscriptionFailed -= OnSubscriptionFailed;
	}
	
	// Подписываемся на события
	Connector.SecurityReceived += OnSecurityReceived;
	Connector.SubscriptionFinished += OnSubscriptionFinished;
	Connector.SubscriptionFailed += OnSubscriptionFailed;
	
	// Отправляем запрос на поиск
	Connector.Subscribe(subscription);
}
```

### Пример использования в WPF приложении

В графическом приложении поиск инструментов часто вызывается из обработчика нажатия кнопки:

```csharp
private void FindButton_Click(object sender, RoutedEventArgs e)
{
	// Получаем критерии поиска из текстового поля
	var searchText = SearchTextBox.Text;
	
	if (string.IsNullOrWhiteSpace(searchText))
	{
		MessageBox.Show("Введите критерий поиска");
		return;
	}
	
	// Создаем и отправляем подписку на поиск
	var lookupMessage = new SecurityLookupMessage
	{
		SecurityId = new SecurityId { SecurityCode = searchText },
		// Если выбран тип в интерфейсе
		SecurityType = SecurityTypeComboBox.SelectedItem as SecurityTypes?
	};
	
	var subscription = new Subscription(lookupMessage);
	
	// Здесь можно показать индикатор загрузки
	LoadingIndicator.Visibility = Visibility.Visible;
	
	// Отправляем запрос
	Connector.Subscribe(subscription);
}
```

## Использование SecurityLookupWindow

StockSharp также предоставляет готовый диалог для поиска инструментов — [SecurityLookupWindow](xref:StockSharp.Xaml.SecurityLookupWindow):

```csharp
private void ShowSecurityLookupWindow_Click(object sender, RoutedEventArgs e)
{
	var lookupWindow = new SecurityLookupWindow
	{
		// Указываем возможность поиска всех инструментов 
		// (если коннектор поддерживает эту функцию)
		ShowAllOption = Connector.Adapter.IsSupportSecuritiesLookupAll(),
		
		// Задаем начальные критерии поиска
		CriteriaMessage = new SecurityLookupMessage
		{
			SecurityId = new SecurityId { SecurityCode = "AAPL" },
			SecurityType = SecurityTypes.Stock
		}
	};
	
	// Показываем окно как модальный диалог
	if (lookupWindow.ShowModal(this))
	{
		// Если пользователь подтвердил выбор, отправляем запрос
		Connector.Subscribe(new Subscription(lookupWindow.CriteriaMessage));
	}
}
```

## Заключение

Механизм подписок в StockSharp предоставляет унифицированный способ получения данных, включая поиск инструментов. Это позволяет использовать один и тот же подход для работы с различными коннекторами и типами данных, что существенно упрощает разработку торговых приложений.
# Получение новостных данных

StockSharp API позволяет получать новостные данные от различных источников. Новости могут быть важным источником информации при принятии торговых решений или для анализа рынка.

> [!NOTE]
> Обратите внимание, что не все источники данных предоставляют новости. Некоторые криптовалютные биржи, включая Binance, не имеют встроенного потока новостей через API. В таких случаях рекомендуется использовать специализированные новостные источники или RSS-фиды.

## Подписка на новостные данные

Чтобы начать получать новости, необходимо создать подписку на новостные данные и затем обрабатывать события получения новостей:

```cs
// Создаем подписку на новости
var newsSubscription = new Subscription(DataType.News);

// Подписываемся на событие получения новостей
_connector.NewsReceived += OnNewsReceived;

// Запускаем подписку
_connector.Subscribe(newsSubscription);

// Обработчик события получения новостей
private void OnNewsReceived(Subscription subscription, News news)
{
	if (subscription != newsSubscription)
		return;

	// Обрабатываем полученную новость
	Console.WriteLine($"Новость: {news.Id}");
	Console.WriteLine($"Заголовок: {news.Headline}");
	Console.WriteLine($"Источник: {news.Source}");
	Console.WriteLine($"Время: {news.ServerTime}");
	Console.WriteLine($"Ссылка: {news.Url}");

	// Если есть текст новости
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Текст: {news.Story}");

	// Если новость связана с конкретными инструментами
	if (news.SecurityId != null)
		Console.WriteLine($"Инструмент: {news.SecurityId}");
}
```

## Фильтрация новостей

При подписке на новости можно указать параметры фильтрации для получения только интересующих вас новостей:

```cs
// Создаем подписку на новости с фильтрацией
var filteredNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// Указываем период, за который нужно получить новости
		From = DateTime.Now.Subtract(TimeSpan.FromHours(24)),
	}
};

_connector.Subscribe(filteredNewsSubscription);
```

## Отображение новостей в пользовательском интерфейсе

StockSharp предоставляет специальный визуальный компонент [NewsPanel](xref:StockSharp.Xaml.NewsPanel) для отображения новостей:

```cs
// Создаем и настраиваем панель новостей
var newsPanel = new NewsPanel();

// Подписываемся на событие получения новостей и добавляем их в панель
_connector.NewsReceived += (subscription, news) =>
{
	// Для обновления элементов пользовательского интерфейса
	// нужно использовать метод GuiAsync или GuiSync
	this.GuiAsync(() => newsPanel.NewsGrid.News.Add(news));
};
```

В XAML-коде:

```xaml
<sx:NewsPanel x:Name="NewsPanel" Grid.Row="1" />
```

## Исторические новости

Для получения исторических новостей за определенный период можно использовать тот же механизм подписок с указанием временного диапазона:

```cs
// Создаем подписку на исторические новости
var historicalNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// Указываем период, за который нужно получить новости
		From = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

_connector.Subscribe(historicalNewsSubscription);
```

## Подключение к RSS для получения новостей

Если вы работаете с коннекторами, которые не предоставляют новостные ленты (например, Binance), вы можете добавить дополнительный источник новостей через RSS:

```cs
// Создаем экземпляр Connector
var connector = new Connector();

// Добавляем основной адаптер для подключения к Binance
var binanceAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API Key>",
	Secret = "<Your Secret Key>",
};
connector.Adapter.InnerAdapters.Add(binanceAdapter);

// Добавляем адаптер для получения новостей через RSS
var rssAdapter = new RssMessageAdapter(connector.TransactionIdGenerator)
{
	Address = "https://news-source.com/feed",
	IsEnabled = true
};
connector.Adapter.InnerAdapters.Add(rssAdapter);

// Подписываемся на событие получения новостей
connector.NewsReceived += OnNewsReceived;

// Выполняем подключение
connector.Connect();
```

## Примечания

- Не все коннекторы поддерживают получение новостей. Например, Binance не предоставляет новостной ленты через API.
- Для получения новостей с криптовалютных рынков рекомендуется использовать специализированные RSS-источники.
- Для новостей, связанных с конкретными инструментами, может потребоваться дополнительная настройка подписки.
- При работе с графическим интерфейсом не забывайте обновлять элементы UI в потоке пользовательского интерфейса с помощью методов `GuiAsync` или `GuiSync`.

## См. также

- [Подписки](subscriptions.md)
- [Графические компоненты](../graphical_user_interface.md)

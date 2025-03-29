# Новости

[NewsGrid](xref:StockSharp.Xaml.NewsGrid) - таблица для отображения новостей. 

**Основные свойства**

- [NewsGrid.News](xref:StockSharp.Xaml.NewsGrid.News) - список новостей.
- [NewsGrid.FirstSelectedNews](xref:StockSharp.Xaml.NewsGrid.FirstSelectedNews) - выбранная новость.
- [NewsGrid.SelectedNews](xref:StockSharp.Xaml.NewsGrid.SelectedNews) - выбранные новости.
- [NewsGrid.NewsProvider](xref:StockSharp.Xaml.NewsGrid.NewsProvider) - поставщик новостей.

Ниже показаны фрагменты кода с его использованием:

```xaml
<Window	x:Class="SampleAlfa.NewsWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
        xmlns:xaml="http://schemas.stocksharp.com/xaml"
        Title="{x:Static loc:LocalizedStrings.News}" Height="300" Width="1050">
    <xaml:NewsPanel x:Name="NewsPanel"/>
</Window>
```

```cs
private readonly Connector _connector = new Connector();
private void ConnectClick(object sender, RoutedEventArgs e)
{
    // Другие действия при подключении
    
    // Устанавливаем поставщика новостей
    _newsWindow.NewsPanel.NewsProvider = _connector;
    
    // Подписываемся на событие получения новостей
    _connector.NewsReceived += OnNewsReceived;
    
    // Создаем подписку на новости
    var newsSubscription = new Subscription(DataType.News);
    _connector.Subscribe(newsSubscription);
    
    // Выполняем подключение
    _connector.Connect();
}

// Обработчик события получения новостей
private void OnNewsReceived(Subscription subscription, News news)
{
    // Добавляем новость в NewsGrid в потоке пользовательского интерфейса
    this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
}
```

### Фильтрация новостей

```cs
// Создание подписки на новости с фильтрацией
public void SubscribeToFilteredNews(string source = null, DateTime? from = null)
{
    // Создаем подписку на новости
    var newsSubscription = new Subscription(DataType.News)
    {
        MarketData =
        {
            // Устанавливаем начальную дату для получения исторических новостей
            From = from ?? DateTime.Today.AddDays(-7),
            
            // Опционально устанавливаем источник новостей
            NewsSource = source
        }
    };
    
    // Подписываемся на событие получения новостей
    _connector.NewsReceived += OnFilteredNewsReceived;
    
    // Запускаем подписку
    _connector.Subscribe(newsSubscription);
}

// Обработчик событий получения отфильтрованных новостей
private void OnFilteredNewsReceived(Subscription subscription, News news)
{
    // Проверяем наличие фильтра по источнику
    if (subscription.MarketData.NewsSource != null && 
        !string.Equals(news.Source, subscription.MarketData.NewsSource, StringComparison.OrdinalIgnoreCase))
        return;
        
    // Добавляем новость в NewsGrid
    this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
    
    // Выводим информацию о новости
    Console.WriteLine($"Новость: {news.Headline}");
    Console.WriteLine($"Источник: {news.Source}");
    Console.WriteLine($"Время: {news.ServerTime}");
    if (!string.IsNullOrEmpty(news.Story))
        Console.WriteLine($"Текст: {news.Story}");
}
```

### Поиск новостей по ключевым словам

```cs
// Метод для фильтрации новостей по ключевым словам
public void FilterNewsByKeywords(IEnumerable<string> keywords)
{
    var keywordsList = keywords.ToList();
    
    // Если ранее уже подписались на получение новостей,
    // достаточно установить обработчик
    _connector.NewsReceived += (subscription, news) =>
    {
        // Проверяем, содержит ли заголовок новости какое-либо из ключевых слов
        bool containsKeyword = keywordsList.Any(keyword => 
            news.Headline.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
            
        if (containsKeyword)
        {
            // Добавляем новость в NewsGrid
            this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
            
            // Отображаем уведомление
            ShowNotification($"Новая новость по теме: {news.Headline}");
        }
    };
}
```
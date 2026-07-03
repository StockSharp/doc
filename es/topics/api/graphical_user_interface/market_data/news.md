# Noticias

[NewsGrid](xref:StockSharp.Xaml.NewsGrid) - tabla para mostrar noticias.

**Propiedades principales**

- [NewsGrid.News](xref:StockSharp.Xaml.NewsGrid.News) - lista de noticias.
- [NewsGrid.FirstSelectedNews](xref:StockSharp.Xaml.NewsGrid.FirstSelectedNews) - elemento de noticias seleccionado.
- [NewsGrid.SelectedNews](xref:StockSharp.Xaml.NewsGrid.SelectedNews) - elementos de noticias seleccionados.
- [NewsGrid.SubscriptionProvider](xref:StockSharp.Xaml.NewsGrid.SubscriptionProvider) - proveedor de noticias.

A continuación se muestran fragmentos de código que demuestran su uso:

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
	// Otras acciones de conexión
	
	// Establecer proveedor de noticias
	_newsWindow.NewsPanel.SubscriptionProvider = _connector;
	
	// Suscribirse al evento de recepción de noticias
	_connector.NewsReceived += OnNewsReceived;
	
	// Crear una suscripción a noticias
	var newsSubscription = new Subscription(DataType.News);
	_connector.Subscribe(newsSubscription);
	
	// Realizar conexión
	_connector.Connect();
}

// Manejador del evento de recepción de noticias
private void OnNewsReceived(Subscription subscription, News news)
{
	// Agregar noticia a NewsGrid en el hilo de la interfaz de usuario
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
}
```

### Filtrado de noticias

```cs
// Crear una suscripción a noticias con filtrado
public void SubscribeToFilteredNews(string source = null, DateTime? from = null)
{
	// Crear una suscripción a noticias
	var newsSubscription = new Subscription(DataType.News)
	{
		MarketData =
		{
			// Establecer fecha inicial para noticias históricas
			From = from ?? DateTime.Today.AddDays(-7),
			
			// Establecer opcionalmente la fuente de noticias
			NewsSource = source
		}
	};
	
	// Suscribirse al evento de recepción de noticias
	_connector.NewsReceived += OnFilteredNewsReceived;
	
	// Iniciar la suscripción
	_connector.Subscribe(newsSubscription);
}

// Manejador para eventos de recepción de noticias filtradas
private void OnFilteredNewsReceived(Subscription subscription, News news)
{
	// Comprobar filtro de fuente
	if (subscription.MarketData.NewsSource != null && 
		!string.Equals(news.Source, subscription.MarketData.NewsSource, StringComparison.OrdinalIgnoreCase))
		return;
		
	// Agregar noticia a NewsGrid
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
	
	// Mostrar información de la noticia
	Console.WriteLine($"News: {news.Headline}");
	Console.WriteLine($"Source: {news.Source}");
	Console.WriteLine($"Time: {news.ServerTime}");
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Text: {news.Story}");
}
```

### Búsqueda de noticias por palabras clave

```cs
// Método para filtrar noticias por palabras clave
public void FilterNewsByKeywords(IEnumerable<string> keywords)
{
	var keywordsList = keywords.ToList();
	
	// Si ya existe una suscripción a noticias,
	// solo establecer el manejador
	_connector.NewsReceived += (subscription, news) =>
	{
		// Comprobar si el titular de la noticia contiene alguna palabra clave
		bool containsKeyword = keywordsList.Any(keyword => 
			news.Headline.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
			
		if (containsKeyword)
		{
			// Agregar noticia a NewsGrid
			this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
			
			// Mostrar notificación
			ShowNotification($"New news on topic: {news.Headline}");
		}
	};
}
```


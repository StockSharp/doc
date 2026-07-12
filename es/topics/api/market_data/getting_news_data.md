# Obtención de datos de noticias

StockSharp API permite recibir datos de noticias desde varias fuentes. Las noticias pueden ser una fuente importante de información al tomar decisiones de trading o para el análisis de mercado.

> [!NOTE]
> Tenga en cuenta que no todas las fuentes de datos proporcionan noticias. Algunos exchanges de criptomonedas, incluido Binance, no tienen un feed de noticias integrado mediante su API. En estos casos, se recomienda usar fuentes de noticias especializadas o feeds RSS.

## Suscripción a datos de noticias

Para empezar a recibir noticias, debe crear una suscripción a datos de noticias y después procesar los eventos de recepción de noticias:

```cs
// Crear una suscripción a noticias
var newsSubscription = new Subscription(DataType.News);

// Suscribirse al evento de recepción de noticias
_connector.NewsReceived += OnNewsReceived;

// Iniciar la suscripción
_connector.Subscribe(newsSubscription);

// Manejador del evento de recepción de noticias
private void OnNewsReceived(Subscription subscription, News news)
{
	if (subscription != newsSubscription)
		return;

	// Procesar la noticia recibida
	Console.WriteLine($"Noticia: {news.Id}");
	Console.WriteLine($"Titular: {news.Headline}");
	Console.WriteLine($"Fuente: {news.Source}");
	Console.WriteLine($"Hora: {news.ServerTime}");
	Console.WriteLine($"Enlace: {news.Url}");

	// Si hay texto de noticia
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Texto: {news.Story}");

	// Si la noticia está relacionada con instrumentos específicos
	if (news.SecurityId != null)
		Console.WriteLine($"Instrumento: {news.SecurityId}");
}
```

## Filtrado de noticias

Al suscribirse a noticias, puede especificar parámetros de filtrado para recibir solo las noticias que le interesan:

```cs
// Crear una suscripción a noticias con filtrado
var filteredNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// Especificar el período para el que se obtendrán noticias
		From = DateTime.Now.Subtract(TimeSpan.FromHours(24)),

		// Puede especificar una fuente de noticias concreta
		// Por ejemplo, usamos una fuente RSS
		NewsSource = "CryptoNews"
	}
};

_connector.Subscribe(filteredNewsSubscription);
```

## Visualización de noticias en la interfaz de usuario

StockSharp proporciona un componente visual especial [NewsPanel](xref:StockSharp.Xaml.NewsPanel) para mostrar noticias:

```cs
// Crear y configurar un panel de noticias
var newsPanel = new NewsPanel();

// Suscribirse al evento de recepción de noticias y agregar noticias al panel
_connector.NewsReceived += (subscription, news) =>
{
	// Para actualizar elementos de la UI
	// debe usar el método GuiAsync o GuiSync
	this.GuiAsync(() => newsPanel.NewsGrid.News.Add(news));
};
```

En código XAML:

```xaml
<sx:NewsPanel x:Name="NewsPanel" Grid.Row="1" />
```

## Noticias históricas

Para obtener noticias históricas de un período específico, puede usar el mismo mecanismo de suscripción con un rango de tiempo especificado:

```cs
// Crear una suscripción a noticias históricas
var historicalNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// Especificar el período para el que se obtendrán noticias
		From = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

_connector.Subscribe(historicalNewsSubscription);
```

## Conexión a RSS para noticias

Si trabaja con conectores que no proporcionan feeds de noticias (por ejemplo, Binance), puede agregar una fuente de noticias adicional mediante RSS:

```cs
// Crear una instancia de Connector
var connector = new Connector();

// Agregar el adaptador principal para conectar con Binance
var binanceAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Su clave API>",
	Secret = "<Su clave secreta>",
};
connector.Adapter.InnerAdapters.Add(binanceAdapter);

// Agregar un adaptador para recibir noticias mediante RSS
var rssAdapter = new RssMessageAdapter(connector.TransactionIdGenerator)
{
	Address = "https://news-source.com/feed",
	IsEnabled = true
};
connector.Adapter.InnerAdapters.Add(rssAdapter);

// Suscribirse al evento de recepción de noticias
connector.NewsReceived += OnNewsReceived;

// Conectar
connector.Connect();
```

## Notas

- No todos los conectores admiten recibir noticias. Por ejemplo, Binance no proporciona un feed de noticias mediante la API.
- Para noticias del mercado de criptomonedas, se recomienda usar fuentes RSS especializadas.
- Para noticias relacionadas con instrumentos específicos, puede requerirse configuración adicional de la suscripción.
- Al trabajar con una interfaz gráfica, recuerde actualizar los elementos de la UI en el hilo de la interfaz de usuario mediante los métodos `GuiAsync` o `GuiSync`.

## Véase también

- [Suscripciones](subscriptions.md)
- [Componentes gráficos](../graphical_user_interface.md)

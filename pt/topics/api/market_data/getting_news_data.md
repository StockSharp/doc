# Obter Dados de Notícias

A API StockSharp permite receber dados de notícias de várias fontes. As notícias podem ser uma fonte importante de informação ao tomar decisões de negociação ou para análise de mercado.

> [!NOTE]
> Tenha em atenção que nem todas as fontes de dados fornecem notícias. Algumas bolsas de criptomoedas, incluindo a Binance, não têm um feed de notícias incorporado através da sua API. Nestes casos, recomenda-se utilizar fontes de notícias especializadas ou feeds RSS.

## Subscrever Dados de Notícias

Para começar a receber notícias, tem de criar uma subscrição de dados de notícias e depois tratar os eventos de recepção de notícias:

```cs
// Criar uma subscrição de notícias
var newsSubscription = new Subscription(DataType.News);

// Subscrever o evento de notícias recebidas
_connector.NewsReceived += OnNewsReceived;

// Iniciar a subscrição
_connector.Subscribe(newsSubscription);

// Manipulador do evento para receber notícias
private void OnNewsReceived(Subscription subscription, News news)
{
	if (subscription != newsSubscription)
		return;

	// Processar a notícia recebida
	Console.WriteLine($"Notícia: {news.Id}");
	Console.WriteLine($"Título: {news.Headline}");
	Console.WriteLine($"Fonte: {news.Source}");
	Console.WriteLine($"Hora: {news.ServerTime}");
	Console.WriteLine($"Ligação: {news.Url}");

	// Se existir texto da notícia
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Texto: {news.Story}");

	// Se a notícia estiver relacionada com instrumentos específicos
	if (news.SecurityId != null)
		Console.WriteLine($"Instrumento: {news.SecurityId}");
}
```

## Filtrar Notícias

Ao subscrever notícias, pode especificar parâmetros de filtragem para receber apenas as notícias que lhe interessam:

```cs
// Criar uma subscrição de notícias com filtragem
var filteredNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// Especificar o período para o qual obter notícias
		From = DateTime.Now.Subtract(TimeSpan.FromHours(24)),

		// Pode especificar uma fonte de notícias específica
		// Por exemplo, utilizamos uma fonte RSS
		NewsSource = "CryptoNews"
	}
};

_connector.Subscribe(filteredNewsSubscription);
```

## Apresentar Notícias na Interface de Utilizador

O StockSharp fornece um componente visual especial [NewsPanel](xref:StockSharp.Xaml.NewsPanel) para apresentar notícias:

```cs
// Criar e configurar um painel de notícias
var newsPanel = new NewsPanel();

// Subscrever o evento de notícias recebidas e adicionar notícias ao painel
_connector.NewsReceived += (subscription, news) =>
{
	// Para actualizar elementos da UI
	// tem de utilizar o método GuiAsync ou GuiSync
	this.GuiAsync(() => newsPanel.NewsGrid.News.Add(news));
};
```

Em código XAML:

```xaml
<sx:NewsPanel x:Name="NewsPanel" Grid.Row="1" />
```

## Notícias Históricas

Para obter notícias históricas para um período específico, pode utilizar o mesmo mecanismo de subscrição com um intervalo de tempo especificado:

```cs
// Criar uma subscrição de notícias históricas
var historicalNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// Especificar o período para o qual obter notícias
		From = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

_connector.Subscribe(historicalNewsSubscription);
```

## Ligar a RSS para Notícias

Se estiver a trabalhar com conectores que não fornecem feeds de notícias (por exemplo, Binance), pode adicionar uma fonte de notícias adicional via RSS:

```cs
// Criar uma instância de Connector
var connector = new Connector();

// Adicionar o adaptador principal para ligação à Binance
var binanceAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<A sua chave de API>",
	Secret = "<A sua chave secreta>",
};
connector.Adapter.InnerAdapters.Add(binanceAdapter);

// Adicionar um adaptador para receber notícias via RSS
var rssAdapter = new RssMessageAdapter(connector.TransactionIdGenerator)
{
	Address = "https://news-source.com/feed",
	IsEnabled = true
};
connector.Adapter.InnerAdapters.Add(rssAdapter);

// Subscrever o evento de notícias recebidas
connector.NewsReceived += OnNewsReceived;

// Ligar
connector.Connect();
```

## Notas

- Nem todos os conectores suportam a recepção de notícias. Por exemplo, a Binance não fornece um feed de notícias através da API.
- Para notícias do mercado de criptomoedas, recomenda-se utilizar fontes RSS especializadas.
- Para notícias relacionadas com instrumentos específicos, pode ser necessária configuração adicional da subscrição.
- Ao trabalhar com uma interface gráfica, lembre-se de actualizar os elementos da UI na thread da interface de utilizador utilizando os métodos `GuiAsync` ou `GuiSync`.

## Ver Também

- [Subscrições](subscriptions.md)
- [Componentes Gráficos](../graphical_user_interface.md)

# Notícias

[NewsGrid](xref:StockSharp.Xaml.NewsGrid) - uma tabela para apresentar notícias.

**Propriedades principais**

- [NewsGrid.News](xref:StockSharp.Xaml.NewsGrid.News) - lista de notícias.
- [NewsGrid.FirstSelectedNews](xref:StockSharp.Xaml.NewsGrid.FirstSelectedNews) - notícia selecionada.
- [NewsGrid.SelectedNews](xref:StockSharp.Xaml.NewsGrid.SelectedNews) - notícias selecionadas.
- [NewsGrid.SubscriptionProvider](xref:StockSharp.Xaml.NewsGrid.SubscriptionProvider) - fornecedor de notícias.

Abaixo encontram-se fragmentos de código que demonstram a sua utilização:

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
	// Outras ações de conexão
	
	// Definir provedor de notícias
	_newsWindow.NewsPanel.SubscriptionProvider = _connector;
	
	// Assinar evento de recebimento de notícias
	_connector.NewsReceived += OnNewsReceived;
	
	// Criar uma assinatura de notícias
	var newsSubscription = new Subscription(DataType.News);
	_connector.Subscribe(newsSubscription);
	
	// Realizar conexão
	_connector.Connect();
}

// Manipulador do evento de recebimento de notícias
private void OnNewsReceived(Subscription subscription, News news)
{
	// Adicionar notícia ao NewsGrid na thread da interface
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
}
```

### Filtragem de notícias

```cs
// Criar assinatura de notícias com filtragem
public void SubscribeToFilteredNews(string source = null, DateTime? from = null)
{
	// Criar uma assinatura de notícias
	var newsSubscription = new Subscription(DataType.News)
	{
		MarketData =
		{
			// Definir data inicial para notícias históricas
			From = from ?? DateTime.Today.AddDays(-7),
			
			// Opcionalmente definir fonte de notícias
			NewsSource = source
		}
	};
	
	// Assinar evento de recebimento de notícias
	_connector.NewsReceived += OnFilteredNewsReceived;
	
	// Iniciar a assinatura
	_connector.Subscribe(newsSubscription);
}

// Manipulador de eventos de recebimento de notícias filtradas
private void OnFilteredNewsReceived(Subscription subscription, News news)
{
	// Verificar filtro de fonte
	if (subscription.MarketData.NewsSource != null && 
		!string.Equals(news.Source, subscription.MarketData.NewsSource, StringComparison.OrdinalIgnoreCase))
		return;
		
	// Adicionar notícia ao NewsGrid
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
	
	// Exibir informações da notícia
	Console.WriteLine($"News: {news.Headline}");
	Console.WriteLine($"Source: {news.Source}");
	Console.WriteLine($"Time: {news.ServerTime}");
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Text: {news.Story}");
}
```

### Pesquisa de notícias por palavras-chave

```cs
// Método para filtrar notícias por palavras-chave
public void FilterNewsByKeywords(IEnumerable<string> keywords)
{
	var keywordsList = keywords.ToList();
	
	// Se já existir subscrição de notícias,
	// basta definir o manipulador
	_connector.NewsReceived += (subscription, news) =>
	{
		// Verificar se o título da notícia contém alguma palavra-chave
		bool containsKeyword = keywordsList.Any(keyword => 
			news.Headline.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
			
		if (containsKeyword)
		{
			// Adicionar notícia ao NewsGrid
			this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
			
			// Exibir notificação
			ShowNotification($"New news on topic: {news.Headline}");
		}
	};
}
```

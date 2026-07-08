# Nachrichten

[NewsGrid](xref:StockSharp.Xaml.NewsGrid) ist eine Tabelle zur Anzeige von Nachrichten.

**Haupteigenschaften**

- [NewsGrid.News](xref:StockSharp.Xaml.NewsGrid.News) - Liste der Nachrichten.
- [NewsGrid.FirstSelectedNews](xref:StockSharp.Xaml.NewsGrid.FirstSelectedNews) - ausgewählte Nachricht.
- [NewsGrid.SelectedNews](xref:StockSharp.Xaml.NewsGrid.SelectedNews) - ausgewählte Nachrichten.
- [NewsGrid.SubscriptionProvider](xref:StockSharp.Xaml.NewsGrid.SubscriptionProvider) - Nachrichtenanbieter.

Unten sehen Sie Codefragmente, die die Verwendung demonstrieren:

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
	// Weitere Verbindungsaktionen

	// Nachrichtenanbieter setzen
	_newsWindow.NewsPanel.SubscriptionProvider = _connector;

	// Ereignis für Nachrichtenempfang abonnieren
	_connector.NewsReceived += OnNewsReceived;

	// Nachrichtenabonnement erstellen
	var newsSubscription = new Subscription(DataType.News);
	_connector.Subscribe(newsSubscription);

	// Verbindung herstellen
	_connector.Connect();
}

// Handler für das Ereignis zum Nachrichtenempfang
private void OnNewsReceived(Subscription subscription, News news)
{
	// Nachrichten im UI-Thread zu NewsGrid hinzufügen
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));
}
```

### Nachrichtenfilterung

```cs
// Nachrichtenabonnement mit Filterung erstellen
public void SubscribeToFilteredNews(string source = null, DateTime? from = null)
{
	// Nachrichtenabonnement erstellen
	var newsSubscription = new Subscription(DataType.News)
	{
		MarketData =
		{
			// Startdatum für historische Nachrichten setzen
			From = from ?? DateTime.Today.AddDays(-7),

			// Optional Nachrichtenquelle setzen
			NewsSource = source
		}
	};

	// Ereignis für Nachrichtenempfang abonnieren
	_connector.NewsReceived += OnFilteredNewsReceived;

	// Abonnement starten
	_connector.Subscribe(newsSubscription);
}

// Handler für gefilterte Nachrichtenempfangsereignisse
private void OnFilteredNewsReceived(Subscription subscription, News news)
{
	// Quellenfilter prüfen
	if (subscription.MarketData.NewsSource != null &&
		!string.Equals(news.Source, subscription.MarketData.NewsSource, StringComparison.OrdinalIgnoreCase))
		return;

	// Nachrichten zu NewsGrid hinzufügen
	this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));

	// Nachrichteninformationen ausgeben
	Console.WriteLine($"News: {news.Headline}");
	Console.WriteLine($"Source: {news.Source}");
	Console.WriteLine($"Time: {news.ServerTime}");
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Text: {news.Story}");
}
```

### Suche nach Nachrichten anhand von Schlüsselwörtern

```cs
// Methode zum Filtern von Nachrichten nach Schlüsselwörtern
public void FilterNewsByKeywords(IEnumerable<string> keywords)
{
	var keywordsList = keywords.ToList();

	// Wenn bereits ein Nachrichtenabonnement besteht,
	// nur den Handler setzen
	_connector.NewsReceived += (subscription, news) =>
	{
		// Prüfen, ob die Nachrichtenüberschrift eines der Schlüsselwörter enthält
		bool containsKeyword = keywordsList.Any(keyword =>
			news.Headline.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);

		if (containsKeyword)
		{
			// Nachrichten zu NewsGrid hinzufügen
			this.GuiAsync(() => _newsWindow.NewsPanel.NewsGrid.News.Add(news));

			// Benachrichtigung anzeigen
			ShowNotification($"New news on topic: {news.Headline}");
		}
	};
}
```

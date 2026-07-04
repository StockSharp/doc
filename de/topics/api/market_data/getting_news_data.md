# Abrufen von Nachrichtendaten

Die StockSharp API ermöglicht den Empfang von Nachrichtendaten aus verschiedenen Quellen. Nachrichten können eine wichtige Informationsquelle für Handelsentscheidungen oder Marktanalysen sein.

> [!NOTE]
> Beachten Sie, dass nicht alle Datenquellen Nachrichten bereitstellen. Einige Kryptowährungsbörsen, darunter Binance, haben keinen integrierten Newsfeed über ihre API. In solchen Fällen wird empfohlen, spezialisierte Nachrichtenquellen oder RSS-Feeds zu verwenden.

## Nachrichtendaten abonnieren

Um Nachrichten zu empfangen, müssen Sie ein Abonnement für Nachrichtendaten erstellen und anschließend Ereignisse für den Nachrichteneingang verarbeiten:

```cs
// Abonnement für Nachrichten erstellen
var newsSubscription = new Subscription(DataType.News);

// Ereignis für empfangene Nachrichten abonnieren
_connector.NewsReceived += OnNewsReceived;

// Abonnement starten
_connector.Subscribe(newsSubscription);

// Ereignishandler für den Empfang von Nachrichten
private void OnNewsReceived(Subscription subscription, News news)
{
	if (subscription != newsSubscription)
		return;

	// Empfangene Nachricht verarbeiten
	Console.WriteLine($"News: {news.Id}");
	Console.WriteLine($"Headline: {news.Headline}");
	Console.WriteLine($"Source: {news.Source}");
	Console.WriteLine($"Time: {news.ServerTime}");
	Console.WriteLine($"URL: {news.Url}");

	// Wenn Nachrichtentext vorhanden ist
	if (!string.IsNullOrEmpty(news.Story))
		Console.WriteLine($"Story: {news.Story}");

	// Wenn die Nachricht bestimmten Instrumenten zugeordnet ist
	if (news.SecurityId != null)
		Console.WriteLine($"Instrument: {news.SecurityId}");
}
```

## Nachrichten filtern

Beim Abonnieren von Nachrichten können Sie Filterparameter angeben, um nur die Nachrichten zu empfangen, die Sie interessieren:

```cs
// Abonnement für Nachrichten mit Filterung erstellen
var filteredNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// Zeitraum angeben, für den Nachrichten abgerufen werden sollen
		From = DateTime.Now.Subtract(TimeSpan.FromHours(24)),

		// Eine bestimmte Nachrichtenquelle kann angegeben werden
		// Beispiel: Wir verwenden eine RSS-Quelle
		NewsSource = "CryptoNews"
	}
};

_connector.Subscribe(filteredNewsSubscription);
```

## Anzeigen von Nachrichten in der Benutzeroberfläche

StockSharp stellt eine spezielle visuelle Komponente [NewsPanel](xref:StockSharp.Xaml.NewsPanel) zur Anzeige von Nachrichten bereit:

```cs
// NewsPanel erstellen und konfigurieren
var newsPanel = new NewsPanel();

// Ereignis für empfangene Nachrichten abonnieren und Nachrichten dem Panel hinzufügen
_connector.NewsReceived += (subscription, news) =>
{
	// Zum Aktualisieren von UI-Elementen
	// muss die Methode GuiAsync oder GuiSync verwendet werden
	this.GuiAsync(() => newsPanel.NewsGrid.News.Add(news));
};
```

Im XAML-Code:

```xaml
<sx:NewsPanel x:Name="NewsPanel" Grid.Row="1" />
```

## Historische Nachrichten

Um historische Nachrichten für einen bestimmten Zeitraum abzurufen, können Sie denselben Abonnementmechanismus mit einem angegebenen Zeitbereich verwenden:

```cs
// Abonnement für historische Nachrichten erstellen
var historicalNewsSubscription = new Subscription(DataType.News)
{
	MarketData =
	{
		// Zeitraum angeben, für den Nachrichten abgerufen werden sollen
		From = DateTime.Now.Subtract(TimeSpan.FromDays(7)),
		To = DateTime.Now
	}
};

_connector.Subscribe(historicalNewsSubscription);
```

## Verbindung zu RSS für Nachrichten

Wenn Sie mit Connectors arbeiten, die keine Newsfeeds bereitstellen (zum Beispiel Binance), können Sie eine zusätzliche Nachrichtenquelle über RSS hinzufügen:

```cs
// Connector-Instanz erstellen
var connector = new Connector();

// Hauptadapter für die Verbindung zu Binance hinzufügen
var binanceAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API Key>",
	Secret = "<Your Secret Key>",
};
connector.Adapter.InnerAdapters.Add(binanceAdapter);

// Adapter für den Empfang von Nachrichten über RSS hinzufügen
var rssAdapter = new RssMessageAdapter(connector.TransactionIdGenerator)
{
	Address = "https://news-source.com/feed",
	IsEnabled = true
};
connector.Adapter.InnerAdapters.Add(rssAdapter);

// Ereignis für empfangene Nachrichten abonnieren
connector.NewsReceived += OnNewsReceived;

// Verbinden
connector.Connect();
```

## Hinweise

- Nicht alle Connectors unterstützen den Empfang von Nachrichten. Binance stellt beispielsweise keinen Newsfeed über die API bereit.
- Für Nachrichten zum Kryptowährungsmarkt wird empfohlen, spezialisierte RSS-Quellen zu verwenden.
- Für Nachrichten zu bestimmten Instrumenten kann zusätzliche Abonnementkonfiguration erforderlich sein.
- Denken Sie bei der Arbeit mit einer grafischen Oberfläche daran, UI-Elemente im UI-Thread mit den Methoden `GuiAsync` oder `GuiSync` zu aktualisieren.

## Siehe auch

- [Subscriptions](subscriptions.md)
- [Graphical Components](../graphical_user_interface.md)


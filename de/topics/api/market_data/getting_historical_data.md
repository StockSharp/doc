# Abrufen historischer Daten

Die StockSharp API bietet komfortable Mechanismen zum Abrufen historischer Daten, die sowohl zum Testen von Handelsstrategien als auch zum Erstellen von [Indikatoren](../indicators.md) verwendet werden können.

## Abrufen historischer Daten über Connector

### Verbindung einrichten

Um historische Daten abzurufen, müssen Sie zunächst eine Verbindung zum Handelssystem konfigurieren:

```cs
// Connector-Instanz erstellen
var connector = new Connector();

// Adapter für die Verbindung zu Binance hinzufügen
var messageAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>",
	Secret = "<Ihr geheimer Schlüssel>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);

// Verbinden
connector.Connect();
```

Die Verbindung kann auch über die grafische Oberfläche konfiguriert werden, wie im Abschnitt [Fenster für Verbindungseinstellungen](../graphical_user_interface/connection_settings_window.md) beschrieben.

### Historische Candles abonnieren

Um historische Candles zu empfangen, müssen Sie ein Abonnement erstellen und die Parameter der angeforderten Daten angeben:

```cs
// Abonnement für 5-Minuten-Candles für das ausgewählte Instrument erstellen
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		// Zeitraum angeben, für den historische Daten abgerufen werden sollen
		From = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now,
		// Flag setzen, um nur abgeschlossene Candles zu empfangen
		IsFinishedOnly = true
	}
};

// Ereignis für empfangene Candles abonnieren
connector.CandleReceived += OnCandleReceived;

// Abonnement starten
connector.Subscribe(subscription);

// Ereignishandler für den Empfang von Candles
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Prüfen, dass die Candle zu unserem Abonnement gehört
	if (subscription != _subscription)
		return;

	// Empfangene Candle verarbeiten
	Console.WriteLine($"Kerze empfangen: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}, V:{candle.TotalVolume}");

	// Für die Anzeige im Chart können Sie verwenden:
	// Chart.Draw(_candleElement, candle);
}
```

### Candles für Charts verwenden

Die empfangenen Candles können mit den integrierten grafischen Komponenten von StockSharp in einem Chart angezeigt werden:

```cs
// Chart-Elemente erstellen und konfigurieren
var chart = new Chart();
var area = new ChartArea();
var candleElement = new ChartCandleElement();

// Bereich und Element zum Chart hinzufügen
chart.AddArea(area);
chart.AddElement(area, candleElement, subscription);

// Im CandleReceived-Ereignishandler Candles zeichnen
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Prüfen, dass die Candle zu unserem Abonnement gehört
	if (subscription != _subscription)
		return;

	// Wenn nur abgeschlossene Candles angezeigt werden sollen
	if (candle.State == CandleStates.Finished)
	{
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(candleElement, candle);
		chart.Draw(chartData);
	}
}
```

## Abrufen anderer Typen historischer Daten

Auf ähnliche Weise können Sie andere Arten historischer Daten abrufen:

### Historische Ticks abrufen

```cs
var tickSubscription = new Subscription(DataType.Ticks, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
		To = DateTime.Now
	}
};

connector.TickTradeReceived += (subscription, tick) =>
{
	if (subscription == tickSubscription)
		Console.WriteLine($"Tick: {tick.ServerTime}, Preis: {tick.Price}, Volumen: {tick.Volume}");
};

connector.Subscribe(tickSubscription);
```

### Historische Orderbücher abrufen

```cs
var depthSubscription = new Subscription(DataType.MarketDepth, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
		To = DateTime.Now
	}
};

connector.OrderBookReceived += (subscription, depth) =>
{
	if (subscription == depthSubscription)
		Console.WriteLine($"Orderbuch: {depth.ServerTime}, bestes Bid: {depth.GetBestBid()?.Price}, bestes Ask: {depth.GetBestAsk()?.Price}");
};

connector.Subscribe(depthSubscription);
```

## Siehe auch

- [Kerzen](../candles.md)
- [Abonnements](subscriptions.md)
- [Indikatoren](../indicators.md)

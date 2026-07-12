# Testeinstellungen

Dieser Abschnitt beschreibt die wichtigsten Einstellungen des [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) zum Testen von Handelsstrategien.

## Grundeinstellungen des Emulators

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - Intervall für das Eintreffen des Zeitänderungsereignisses. Wenn Trade-Generatoren verwendet werden, werden Trades mit dieser Frequenz erzeugt. Der Standardwert ist 1 Minute.
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - minimale Verzögerung für gesendete Orders. Der Standardwert ist TimeSpan.Zero, was eine sofortige Annahme gesendeter Orders durch die Börse bedeutet.
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - Orders ausführen, wenn der Preis das Level "berührt" (diese Annahme ist mitunter zu "optimistisch" und sollte für realistische Tests deaktiviert werden). Wenn die Option deaktiviert ist, werden Limit-Orders nur ausgeführt, wenn der Preis sie um mindestens 1 Schritt "durchläuft". Diese Option funktioniert in allen Modi außer dem Orderprotokollmodus. Standardmäßig ist sie deaktiviert.
- [MarketEmulatorSettings.CandlePrice](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CandlePrice) - Kerzenpreis, der für die Order-Ausführung verwendet wird: Middle, Open, High, Low oder Close. Standardwert ist Middle.
- [MarketEmulatorSettings.Failing](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Failing) - Prozentsatz der Fehlschläge bei der Registrierung neuer Orders. Wertebereich von 0 (keine Fehlschläge) bis 100. Standardmäßig deaktiviert (0).
- [MarketEmulatorSettings.InitialOrderId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialOrderId) - Anfangsnummer, ab der der Emulator Order-IDs erzeugt.
- [MarketEmulatorSettings.InitialTradeId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialTradeId) - Anfangsnummer, ab der der Emulator Trade-IDs erzeugt.
- [MarketEmulatorSettings.SpreadSize](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.SpreadSize) - Spread-Größe in Preisschritten. Wird beim Erzeugen eines Orderbuchs aus Tick-Trades verwendet. Der Standardwert ist 2.
- [MarketEmulatorSettings.MaxDepth](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MaxDepth) - maximale Tiefe des aus Ticks erzeugten Orderbuchs. Der Standardwert ist 5.
- [MarketEmulatorSettings.PortfolioRecalcInterval](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PortfolioRecalcInterval) - Intervall für die Neuberechnung von Portfoliodaten. Wenn der Wert TimeSpan.Zero ist, wird keine Neuberechnung durchgeführt.
- [MarketEmulatorSettings.ConvertTime](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.ConvertTime) - Order- und Trade-Zeitstempel in Börsenzeit umwandeln. Standardmäßig deaktiviert.
- [MarketEmulatorSettings.TimeZone](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.TimeZone) - Informationen zur Zeitzone der Börse.
- [MarketEmulatorSettings.PriceLimitOffset](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PriceLimitOffset) - Preisabstand zum vorherigen Trade, der die maximale und minimale Preisgrenze für die nächste Session definiert. Der Standardwert ist 40 %.
- [MarketEmulatorSettings.IncreaseDepthVolume](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.IncreaseDepthVolume) - zusätzliches Volumen zum Orderbuch hinzufügen, wenn Orders mit großem Volumen registriert werden. Standardmäßig aktiviert.
- [MarketEmulatorSettings.CheckTradingState](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradingState) - Status der Handelssitzung prüfen. Standardmäßig deaktiviert.
- [MarketEmulatorSettings.CheckMoney](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckMoney) - Kontostand prüfen. Standardmäßig deaktiviert.
- [MarketEmulatorSettings.CheckShortable](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckShortable) - prüfen, ob Short-Positionen erlaubt sind. Standardmäßig deaktiviert.
- [MarketEmulatorSettings.CheckTradableDates](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradableDates) - prüfen, ob die geladenen Daten Handelstage sind. Standardmäßig deaktiviert.
- [MarketEmulatorSettings.CommissionRules](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CommissionRules) - Regeln zur Kommissionsberechnung.

## Marktdatenabonnements

Für korrektes Strategietesting müssen Abonnements für die erforderlichen Marktdatentypen eingerichtet werden. Selbst wenn die Strategie anhand von Kerzen getestet wird, ist für eine korrekte Trade-Emulation ein Abonnement für Tick-Trades erforderlich:

```cs
// Abonnement für Tick-Trades erstellen
var tickSubscription = new Subscription(DataType.Ticks, security);
_connector.Subscribe(tickSubscription);
```

Wenn die Strategie Orderbuchdaten benötigt:

```cs
// Abonnement für Orderbücher erstellen
var depthSubscription = new Subscription(DataType.MarketDepth, security);
_connector.Subscribe(depthSubscription);
```

## Orderbuchgenerierung für Tests

Wenn keine historischen Orderbücher vorhanden sind, diese aber für das Strategietesting benötigt werden, können Sie die Orderbuchgenerierung aktivieren:

```cs
// Orderbuchgenerator mit Trendverhalten erstellen
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// Abonnementnachricht an den Generator senden
_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});
```

### Einstellungen des Orderbuchgenerators

- Aktualisierungsintervall des Orderbuchs ([MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval)) - Aktualisierungen können nicht häufiger erfolgen als das Eintreffen von Tick-Trades, da Orderbücher vor jedem Trade generiert werden:

```cs
mdGenerator.Interval = TimeSpan.FromSeconds(1);
```

- Orderbuchtiefe ([MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) und [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth)) - je tiefer, desto langsamer der Test:

```cs
mdGenerator.MaxAsksDepth = 1;
mdGenerator.MaxBidsDepth = 1;
```

- Um ein realistisches Level-Volumen im Orderbuch zu erhalten, können Sie die Option [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume) verwenden. Sie übernimmt die Volumina für die besten Quotes aus dem generierten Trade-Volumen:

```cs
mdGenerator.UseTradeVolume = true;
```

- Bereich des Level-Volumens ([MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) und [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume)):

```cs
mdGenerator.MinVolume = 1;
mdGenerator.MaxVolume = 1;
```

- Spread-Einstellungen - der minimal erzeugte Spread entspricht [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep). Es wird empfohlen, keinen Spread zwischen den besten Quotes zu erzeugen, der größer als 5 Preisschritte ist, damit der Spread beim Generieren aus Kerzen nicht zu breit wird:

```cs
mdGenerator.MinSpreadStepCount = 1;
mdGenerator.MaxSpreadStepCount = 5;
```

## Beispiel für eine vollständige Testkonfiguration

```cs
// Historische Verbindung erstellen
var connector = new HistoryEmulationConnector();

// Basisparameter konfigurieren
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.EmulationAdapter.Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.EmulationAdapter.Emulator.Settings.MatchOnTouch = false;

// Historische Daten laden
var storage = new StorageRegistry();
var security = new Security { Id = "AAPL", PriceStep = 0.01m };

// Abonnement für Kerzen erstellen
var candleSubscription = new Subscription(
	TimeSpan.FromMinutes(5).TimeFrame(),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,
		BuildMode = MarketDataBuildModes.Load
	}
};
connector.Subscribe(candleSubscription);

// Abonnement für Ticks zur korrekten Emulation erstellen
var tickSubscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(tickSubscription);

// Orderbuchgenerierung konfigurieren
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId())
{
	Interval = TimeSpan.FromSeconds(1),
	MaxAsksDepth = 5,
	MaxBidsDepth = 5,
	UseTradeVolume = true,
	MinVolume = 1,
	MaxVolume = 100,
	MinSpreadStepCount = 1,
	MaxSpreadStepCount = 5
};

connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});

// Empfang von Daten abonnieren
connector.CandleReceived += OnCandleReceived;
connector.TickTradeReceived += OnTickReceived;
connector.OrderBookReceived += OnOrderBookReceived;

// Test starten
connector.Connect();
```

## Ereignisverarbeitung

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Empfangene Kerzen verarbeiten
	Console.WriteLine($"Kerze: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
{
	// Empfangene Ticks verarbeiten
	Console.WriteLine($"Tick: {tick.ServerTime}, Preis: {tick.Price}, Volumen: {tick.Volume}");
}

private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// Erweiterungsmethoden für IOrderBookMessage verwenden
	var bestBid = orderBook.GetBestBid();
	var bestAsk = orderBook.GetBestAsk();
	var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);

	// Empfangene Orderbücher verarbeiten
	Console.WriteLine($"Orderbuch: {orderBook.ServerTime}, bestes Gebot: {bestBid?.Price}, bestes Angebot: {bestAsk?.Price}, Spread-Mitte: {spreadMiddle}");

	// Preis nach Order-Seite abrufen
	var bidPrice = orderBook.GetPrice(Sides.Buy);
	var askPrice = orderBook.GetPrice(Sides.Sell);

	Console.WriteLine($"Gebotspreis: {bidPrice}, Angebotspreis: {askPrice}");
}
```

## Arbeiten mit Orderbuchdaten

Wenn Sie mit Orderbüchern als IOrderBookMessage arbeiten, können Sie die folgenden Erweiterungsmethoden verwenden:

```cs
// Best Bid abrufen
var bestBid = orderBook.GetBestBid();

// Best Ask abrufen
var bestAsk = orderBook.GetBestAsk();

// Mitte des Spreads abrufen
var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);

// Preis nach Order-Seite abrufen
var price = orderBook.GetPrice(Sides.Buy); // oder Sides.Sell, oder null für die Mitte des Spreads
```

Bei der Arbeit mit Level1-Daten können Sie ebenfalls die Mitte des Spreads abrufen:

```cs
// Mitte des Spreads aus einer Level1-Nachricht abrufen
var spreadMiddle = level1.GetSpreadMiddle(Security.PriceStep);
```

Diese Erweiterungsmethoden vereinfachen den Zugriff auf Orderbuchdaten und ermöglichen es, saubereren und verständlicheren Code für die Arbeit mit Börsenquotes zu schreiben.

# Verbindung über das FIX-Protokoll

[Hydra](../../hydra.md) kann im Servermodus verwendet werden. Dadurch ist eine Remote-Verbindung zu [Hydra](../../hydra.md) möglich, um auf Daten im Speicher zuzugreifen. Das Aktivieren des Servermodus von [Hydra](../../hydra.md) ist im Abschnitt [Einstellungen](settings.md) beschrieben.

Für die Verbindung über das [FIX-Protokoll](../../api/connectors/common/fix_protocol.md) müssen Sie eine Fix-Verbindung erstellen und konfigurieren ([FIX-Adapterinitialisierung](../../api/connectors/common/fix_protocol/adapter_initialization_fix.md)).

```cs
// Connector-Instanz erstellen
private readonly Connector _connector = new Connector();

// Adapter für Marktdaten über das FIX-Protokoll konfigurieren
var marketDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(marketDataAdapter);

// Adapter für Transaktionsdaten konfigurieren
var transactionDataAdapter = new FixMessageAdapter(_connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5002),
	SenderCompId = "hydra_user",
	TargetCompId = "StockSharpHydraMD",
	Login = "hydra_user",
	Password = "qwerty".To<SecureString>(),
};
_connector.Adapter.InnerAdapters.Add(transactionDataAdapter);
```

Abonnieren Sie Ereignisse und richten Sie Datenhandler ein:

```cs
// Ereignis bei erfolgreicher Verbindung
_connector.Connected += () =>
{
	Console.WriteLine("Verbindung hergestellt");

	// Abonnement zur Instrumentensuche erstellen
	var lookupSubscription = new Subscription(DataType.Securities);
	_connector.Subscribe(lookupSubscription);
};

// Ereignis bei Verbindungsverlust
_connector.Disconnected += () =>
{
	Console.WriteLine("Verbindung verloren");
};

// Ereignis bei empfangenem Instrument
_connector.SecurityReceived += (subscription, security) =>
{
	Console.WriteLine($"Instrument empfangen: {security.Code}, {security.Id}");
	BufferSecurity.Add(security);

	// Wenn dies das Zielinstrument ist, seine Daten abonnieren
	if (security.Id == targetSecurityId)
	{
		// Orderbuch-Abonnement
		var depthSubscription = new Subscription(DataType.MarketDepth, security);
		_connector.Subscribe(depthSubscription);

		// Tick-Trade-Abonnement
		var tradesSubscription = new Subscription(DataType.Ticks, security);
		_connector.Subscribe(tradesSubscription);

		// Kerzen-Abonnement
		var candleSubscription = new Subscription(
			DataType.TimeFrame(TimeSpan.FromMinutes(5)),
			security)
		{
			MarketData =
			{
				From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
				To = DateTime.Now
			}
		};
		_connector.Subscribe(candleSubscription);
	}
};

// Ereignis bei empfangenem Tick-Trade
_connector.TickTradeReceived += (subscription, trade) =>
{
	Console.WriteLine($"Trade empfangen: {trade.Security.Code}, {trade.Time}, {trade.Price}, {trade.Volume}");
};

// Ereignis bei geändertem Orderbuch
_connector.OrderBookReceived += (subscription, depth) =>
{
	Console.WriteLine($"Orderbuch empfangen: {depth.SecurityId}, bestes Bid: {depth.BestBid()?.Price}, bestes Ask: {depth.BestAsk()?.Price}");
};

// Ereignis bei empfangener Kerze
_connector.CandleReceived += (subscription, candle) =>
{
	Console.WriteLine($"Kerze empfangen: {candle.SecurityId}, {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
};

// Verbindungsfehlerereignis
_connector.ConnectionError += error =>
{
	Console.WriteLine($"Verbindungsfehler: {error.Message}");
};

// Allgemeines Fehlerereignis
_connector.Error += error =>
{
	Console.WriteLine($"Fehler: {error.Message}");
};

// Fehlerereignis bei Marktdaten-Abonnement
_connector.SubscriptionFailed += (subscription, error) =>
{
	Console.WriteLine($"Abonnementfehler {subscription.DataType} für {subscription.SecurityId}: {error}");
};

// Verbindung zum Server herstellen
_connector.Connect();
```

## Hydra-Dienste verwenden

Hydra stellt im Servermodus Zugriff auf verschiedene Datentypen bereit. Betrachten wir Beispiele für das Abrufen historischer Daten:

```cs
// Historische Kerzen abrufen
private void RequestHistoricalCandles(Security security, DateTime from, DateTime to)
{
	// Abonnement für historische Kerzen erstellen
	var candleSubscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		security)
	{
		MarketData =
		{
			From = from,
			To = to
		}
	};

	// Abonnieren, um empfangene Kerzen zu verarbeiten
	_connector.CandleReceived += OnCandleReceived;

	// Abonnement starten
	_connector.Subscribe(candleSubscription);
}

private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Prüfen, ob die Kerze zu unserem Abonnement gehört
	if (subscription.DataType != DataType.TimeFrame(TimeSpan.FromMinutes(5)))
		return;

	Console.WriteLine($"Historische Kerze: {candle.OpenTime}, O: {candle.OpenPrice}, H: {candle.HighPrice}, L: {candle.LowPrice}, C: {candle.ClosePrice}, V: {candle.TotalVolume}");

	// Empfangene Kerzen verarbeiten, zum Beispiel lokal speichern
	// oder für Analyse/Visualisierung verwenden
}
```

## Verbindung zum Hydra-Server trennen

```cs
// Verbindung korrekt schließen
private void DisconnectFromServer()
{
	// Von allen Abonnements abmelden
	foreach (var subscription in _connector.Subscriptions.ToArray())
	{
		_connector.UnSubscribe(subscription);
	}

	// Verbindung zum Server trennen
	_connector.Disconnect();
}
```

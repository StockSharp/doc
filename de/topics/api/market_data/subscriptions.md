# Abonnements

**StockSharp API** bietet ein auf Abonnements basierendes Modell zur Datenerfassung. Dies ist ein universeller Mechanismus für den Empfang sowohl von Marktdaten als auch von Transaktionsinformationen. Dieser Ansatz hat wichtige Vorteile:

- **Abonnement-Isolation** - jedes Abonnement arbeitet unabhängig, wodurch beliebig viele Abonnements mit unterschiedlichen Parametern parallel ausgeführt werden können (mit oder ohne Historienanfrage).
- **Zustandsverfolgung** - Abonnements besitzen bestimmte Zustände, mit denen Sie kontrollieren können, ob gerade historische Daten fließen oder das Abonnement in den Echtzeitmodus gewechselt ist.
- **Universalität** - der Code für die Arbeit mit Abonnements ist unabhängig von den angeforderten Datentypen gleich, was die Entwicklung effizienter macht.

Für die Arbeit mit Abonnements verwenden Sie die Klasse [Subscription](xref:StockSharp.BusinessEntities.Subscription). Betrachten wir Beispiele zur Verwendung von Abonnements zum Abrufen verschiedener Datentypen.

## Beispiel für ein Candle-Abonnement

```cs
// Abonnement für 5-Minuten-Candles erstellen
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	// Abonnementparameter über die MarketData-Eigenschaft konfigurieren
	MarketData =
	{
		// Daten für die letzten 30 Tage anfordern
		From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
		// null bedeutet, dass das Abonnement nach Empfang der Historie in den Echtzeitmodus wechselt
		To = null
	}
};

// Empfangene Candles verarbeiten
_connector.CandleReceived += (sub, candle) =>
{
	if (sub != subscription)
		return;

	// Candle verarbeiten
	Console.WriteLine($"Candle: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
};

// Übergang des Abonnements in den Online-Modus behandeln
_connector.SubscriptionOnline += (sub) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine("Subscription switched to real-time mode");
};

// Abonnementfehler behandeln
_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine($"Abonnementfehler: {error}");
};

// Abonnement starten
_connector.Subscribe(subscription);
```

## Beispiel für ein Orderbuch-Abonnement

```cs
// Abonnement für das Orderbuch des ausgewählten Instruments erstellen
var depthSubscription = new Subscription(DataType.MarketDepth, security);

// Empfangene Orderbücher verarbeiten
_connector.OrderBookReceived += (sub, depth) =>
{
	if (sub != depthSubscription)
		return;

	// Orderbuch verarbeiten
	Console.WriteLine($"Orderbuch: {depth.SecurityId}, Zeit: {depth.ServerTime}");
	Console.WriteLine($"Bids: {depth.Bids.Count}, Asks: {depth.Asks.Count}");
};

// Abonnement starten
_connector.Subscribe(depthSubscription);
```

## Beispiel für ein Tick-Trade-Abonnement

```cs
// Abonnement für Tick-Trades des ausgewählten Instruments erstellen
var tickSubscription = new Subscription(DataType.Ticks, security);

// Empfangene Ticks verarbeiten
_connector.TickTradeReceived += (sub, tick) =>
{
	if (sub != tickSubscription)
		return;

	// Tick verarbeiten
	Console.WriteLine($"Tick: {tick.SecurityId}, Time: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
};

// Abonnement starten
_connector.Subscribe(tickSubscription);
```

## Beispiel für ein Abonnement mit Konfiguration des Candle-Erstellungsmodus

```cs
// Abonnement für 5-Minuten-Candles, die aus Ticks erstellt werden
var candleSubscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	MarketData =
	{
		// Erstellungsmodus und Datenquelle angeben
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		// Zusätzlich kann die Erstellung des Volume Profile aktiviert werden
		IsCalcVolumeProfile = true,
	}
};

_connector.Subscribe(candleSubscription);
```

## Beispiel für ein Level1-Abonnement (Basisinformationen zum Instrument)

```cs
// Abonnement für Basisinformationen zum Instrument erstellen
var level1Subscription = new Subscription(DataType.Level1, security);

// Empfangene Level1-Daten verarbeiten
_connector.Level1Received += (sub, level1) =>
{
	if (sub != level1Subscription)
		return;

	Console.WriteLine($"Level1: {level1.SecurityId}, Time: {level1.ServerTime}");

	// Level1-Feldwerte ausgeben
	foreach (var pair in level1.Changes)
	{
		Console.WriteLine($"Field: {pair.Key}, Value: {pair.Value}");
	}
};

// Abonnement starten
_connector.Subscribe(level1Subscription);
```

## Daten abbestellen

Um den Datenempfang zu stoppen, verwenden Sie die Methode `UnSubscribe`:

```cs
// Bestimmtes Abonnement abbestellen
_connector.UnSubscribe(subscription);

// Oder alle Abonnements abbestellen
foreach (var sub in _connector.Subscriptions)
{
	_connector.UnSubscribe(sub);
}
```

## Abonnementzustände

Abonnements können sich in folgenden Zuständen befinden:

- [SubscriptionStates.Stopped](xref:StockSharp.Messages.SubscriptionStates.Stopped) - das Abonnement ist inaktiv (gestoppt oder nicht gestartet).
- [SubscriptionStates.Active](xref:StockSharp.Messages.SubscriptionStates.Active) - das Abonnement ist aktiv und kann historische Daten übertragen, bis es in den Echtzeitmodus wechselt oder abgeschlossen wird.
- [SubscriptionStates.Error](xref:StockSharp.Messages.SubscriptionStates.Error) - das Abonnement ist inaktiv und befindet sich im Fehlerzustand.
- [SubscriptionStates.Finished](xref:StockSharp.Messages.SubscriptionStates.Finished) - das Abonnement hat seine Arbeit abgeschlossen (alle Daten wurden empfangen).
- [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) - das Abonnement ist in den Echtzeitmodus gewechselt und überträgt nur aktuelle Daten.

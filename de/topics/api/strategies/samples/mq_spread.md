# Spread-Quoting-Strategie

## Überblick

`MqSpreadStrategy` ist eine Strategie, die im Markt einen Spread erzeugt, indem sie gleichzeitig Quotes für Kauf und Verkauf platziert. Sie verwendet zwei Quoting-Prozessoren, um Orders auf beiden Marktseiten zu verwalten.

## Hauptkomponenten

```cs
public class MqSpreadStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _buyProcessor;
	private QuotingProcessor _sellProcessor;
}
```

## Strategieparameter

Die Strategie erlaubt die Anpassung der folgenden Parameter:

- **PriceType** - Marktpreistyp für das Quoting (Standardwert Following)
- **PriceOffset** - Preisabstand zum Marktpreis
- **BestPriceOffset** - Mindestabweichung für die Aktualisierung der Quote (Standardwert 0.1 %)

## Initialisierung der Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) abonniert die Strategie Änderungen der Marktzeit:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Änderungen der Marktzeit für Quote-Aktualisierungen abonnieren
	Connector.CurrentTimeChanged += Connector_CurrentTimeChanged;
	Connector_CurrentTimeChanged(new TimeSpan());
}
```

## Verwaltung der Quoting-Prozessoren

Die Methode `Connector_CurrentTimeChanged` wird aufgerufen, wenn sich die Marktzeit ändert, und verwaltet die Erstellung sowie Aktualisierung der Quoting-Prozessoren:

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// Neue Prozessoren nur bei Nullposition und gestoppten aktuellen Prozessoren erstellen
	if (Position != 0)
		return;

	if (_buyProcessor != null && _buyProcessor.LeftVolume > 0)
		return;

	if (_sellProcessor != null && _sellProcessor.LeftVolume > 0)
		return;

	// Ressourcen vorhandener Prozessoren freigeben
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	// Verhalten für Market Quoting erstellen
	var buyBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	var sellBehavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// Prozessor für Kauf erstellen
	_buyProcessor = new QuotingProcessor(
		buyBehavior,
		Security,
		Portfolio,
		Sides.Buy,
		Volume,
		Volume, // Maximales Ordervolumen
		TimeSpan.Zero, // Kein Timeout
		this, // Strategy implementiert ISubscriptionProvider
		this, // Strategy implementiert IMarketRuleContainer
		this, // Strategy implementiert ITransactionProvider
		this, // Strategy implementiert ITimeProvider
		this, // Strategy implementiert IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Handelsberechtigung prüfen
		true, // Orderbuchpreise verwenden
		true  // Letzten Trade-Preis verwenden, wenn das Orderbuch leer ist
	)
	{
		Parent = this
	};

	// Prozessor für Verkauf erstellen
	_sellProcessor = new QuotingProcessor(
		sellBehavior,
		Security,
		Portfolio,
		Sides.Sell,
		Volume,
		Volume, // Maximales Ordervolumen
		TimeSpan.Zero, // Kein Timeout
		this, // Strategy implementiert ISubscriptionProvider
		this, // Strategy implementiert IMarketRuleContainer
		this, // Strategy implementiert ITransactionProvider
		this, // Strategy implementiert ITimeProvider
		this, // Strategy implementiert IMarketDataProvider
		IsFormedAndOnlineAndAllowTrading, // Handelsberechtigung prüfen
		true, // Orderbuchpreise verwenden
		true  // Letzten Trade-Preis verwenden, wenn das Orderbuch leer ist
	)
	{
		Parent = this
	};

	// Erstellung neuer Quoting-Prozessoren protokollieren
	this.AddInfoLog($"Created buy/sell spread at {CurrentTime}");

	// Ereignisse des Kaufprozessors für das Logging abonnieren
	_buyProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Buy order {order.TransactionId} registered at price {order.Price}");

	_buyProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Buy order failed: {fail.Error.Message}");

	_buyProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Buy trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

	_buyProcessor.Finished += isOk => {
		this.AddInfoLog($"Buy quoting finished with success: {isOk}");
		_buyProcessor?.Dispose();
		_buyProcessor = null;
	};

	// Ereignisse des Verkaufsprozessors für das Logging abonnieren
	_sellProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Sell order {order.TransactionId} registered at price {order.Price}");

	_sellProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Sell order failed: {fail.Error.Message}");

	_sellProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Sell trade executed: {trade.Trade.Volume} at {trade.Trade.Price}");

	_sellProcessor.Finished += isOk => {
		this.AddInfoLog($"Sell quoting finished with success: {isOk}");
		_sellProcessor?.Dispose();
		_sellProcessor = null;
	};

	// Beide Prozessoren starten
	_buyProcessor.Start();
	_sellProcessor.Start();
}
```

## Ressourcenfreigabe

In der Methode [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped) gibt die Strategie Ressourcen frei:

```cs
protected override void OnStopped()
{
	// Abonnement entfernen, um Speicherlecks zu verhindern
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// Prozessorressourcen freigeben
	_buyProcessor?.Dispose();
	_buyProcessor = null;

	_sellProcessor?.Dispose();
	_sellProcessor = null;

	base.OnStopped();
}
```

## Handelslogik

- Die Strategie reagiert auf Änderungen der Marktzeit.
- Bei Nullposition und gestoppten Prozessoren werden zwei neue Prozessoren erstellt:
  - Kaufprozessor (Buy)
  - Verkaufsprozessor (Sell)
- Beide Prozessoren sind mit demselben Volumen konfiguriert und verwenden dieselben Quoting-Einstellungen.
- Prozessoren erzeugen einen Spread im Markt, indem sie gleichzeitig Kauf- und Verkaufsorders platzieren.

## Funktionen

- Verwendet den modernen Quoting-Prozessor [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) mit [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior).
- Erzeugt einen Spread im Markt, indem gleichzeitig Kauf- und Verkaufsorders platziert werden.
- Arbeitet nur bei Nullposition und verhindert dadurch die Ansammlung unerwünschter Risiken.
- Unterstützt die Konfiguration verschiedener Quoting-Parameter (Preistyp, Abstand, Mindestabweichung).
- Enthält detailliertes Logging der Ereignisse des Quoting-Prozessors.
- Verwaltet Ressourcen korrekt beim Stoppen der Strategie und beim Erstellen neuer Prozessoren.
- Unterstützt die Arbeit mit verschiedenen Typen von Marktpreisen (Following, Best, Opposite usw.).

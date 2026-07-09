# Quoting-Strategie

## Überblick

`MqStrategy` ist eine Strategie, die einen Quoting-Mechanismus zur Verwaltung von Marktpositionen verwendet. Sie erstellt einen Quoting-Prozessor auf Basis der aktuellen Position, sodass sie adaptiv auf Änderungen der Marktbedingungen reagieren kann.

## Hauptkomponenten

```cs
public class MqStrategy : Strategy
{
	private readonly StrategyParam<MarketPriceTypes> _priceType;
	private readonly StrategyParam<Unit> _priceOffset;
	private readonly StrategyParam<Unit> _bestPriceOffset;

	private QuotingProcessor _quotingProcessor;
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
	Connector_CurrentTimeChanged(default);
}
```

## Verwaltung des Quoting-Prozessors

Die Methode `Connector_CurrentTimeChanged` wird aufgerufen, wenn sich die Marktzeit ändert, und verwaltet die Erstellung sowie Aktualisierung des Quoting-Prozessors:

```cs
private void Connector_CurrentTimeChanged(TimeSpan obj)
{
	// Neuen Prozessor nur erstellen, wenn der aktuelle gestoppt ist oder nicht existiert
	if (_quotingProcessor != null && _quotingProcessor.LeftVolume > 0)
		return;

	// Ressourcen des alten Prozessors freigeben, falls vorhanden
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	// Quoting-Seite anhand der aktuellen Position bestimmen
	var side = Position <= 0 ? Sides.Buy : Sides.Sell;

	// Neues Quoting-Verhalten erstellen
	var behavior = new MarketQuotingBehavior(
		PriceOffset,
		BestPriceOffset,
		PriceType
	);

	// Quoting-Volumen berechnen
	var quotingVolume = Volume + Math.Abs(Position);

	// Prozessor erstellen und initialisieren
	_quotingProcessor = new QuotingProcessor(
		behavior,
		Security,
		Portfolio,
		side,
		quotingVolume,
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

	// Prozessorereignisse für das Logging abonnieren
	_quotingProcessor.OrderRegistered += order =>
		this.AddInfoLog($"Auftrag {order.TransactionId} zum Preis {order.Price} registriert");

	_quotingProcessor.OrderFailed += fail =>
		this.AddInfoLog($"Auftrag fehlgeschlagen: {fail.Error.Message}");

	_quotingProcessor.OwnTrade += trade =>
		this.AddInfoLog($"Ausführung erfolgt: {trade.Trade.Volume} zu {trade.Trade.Price}");

	_quotingProcessor.Finished += isOk => {
		this.AddInfoLog($"Quoting erfolgreich abgeschlossen: {isOk}");
		_quotingProcessor?.Dispose();
		_quotingProcessor = null;
	};

	// Prozessor starten
	_quotingProcessor.Start();
}
```

## Ressourcenfreigabe

In der Methode [OnStopped](xref:StockSharp.Algo.Strategies.Strategy.OnStopped) gibt die Strategie Ressourcen frei:

```cs
protected override void OnStopped()
{
	// Abonnement entfernen, um Speicherlecks zu verhindern
	Connector.CurrentTimeChanged -= Connector_CurrentTimeChanged;

	// Ressourcen des aktuellen Prozessors freigeben, falls vorhanden
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	base.OnStopped();
}
```

## Handelslogik

- Die Strategie reagiert auf Änderungen der Marktzeit.
- Die Quoting-Richtung wird anhand der aktuellen Position bestimmt:
  - Wenn Position <= 0 ist, wird eine Kauf-Quote erstellt.
  - Wenn Position > 0 ist, wird eine Verkaufs-Quote erstellt.
- Das Quoting-Volumen wird als Basisvolumen plus absoluter Wert der aktuellen Position berechnet.
- Für das Quoting wird [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) mit [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) verwendet.

## Funktionen

- Verwendet den modernen Quoting-Prozessor statt älterer Quoting-Strategien.
- Reagiert adaptiv auf Positionsänderungen, indem die Quoting-Richtung geändert wird.
- Unterstützt die Konfiguration verschiedener Quoting-Parameter (Preistyp, Abstand, Mindestabweichung).
- Enthält detailliertes Logging der Ereignisse des Quoting-Prozessors.
- Verwaltet Ressourcen korrekt beim Stoppen der Strategie und beim Erstellen neuer Prozessoren.
- Unterstützt die Arbeit mit verschiedenen Typen von Marktpreisen (Following, Best, Opposite usw.).

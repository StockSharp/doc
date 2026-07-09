# Quoting-Algorithmus

## Überblick

Ein Quoting-Algorithmus ist ein Mechanismus, mit dem Orders automatisch am Markt platziert und aktualisiert werden können, um den bestmöglichen Ausführungspreis zu erreichen. Statt aggressive Market-Orders zu platzieren, verwendet Quoting Limit-Orders. Das hilft, Slippage zu minimieren und Handelskosten zu senken.

## Hauptkomponenten

StockSharp stellt zwei Schlüsselkomponenten für Quoting bereit:

1. **[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor)** -- der Hauptprozessor, der Marktdaten und Orderstatus analysiert, um Quoting-Aktionen zu empfehlen.

2. **[IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior)** -- eine Schnittstelle, die das Quoting-Verhalten definiert, einschließlich der Berechnung des optimalen Preises und der Entscheidung, ob Orders aktualisiert werden müssen.

## Beispiele für Strategien mit Quoting

Die Dokumentation enthält Beispiele von Strategien, die die Verwendung des Quoting-Mechanismus demonstrieren:

- **MqStrategy** -- Beispiel einer Strategie, die den Quoting-Mechanismus zur Verwaltung einer Position im Markt verwendet.
- **MqSpreadStrategy** -- Beispiel einer Strategie, die im Markt einen Spread erzeugt, indem gleichzeitig Kauf- und Verkaufsquotes platziert werden.
- **StairsCountertrendStrategy** -- Beispiel einer Gegentrendstrategie, die Quoting für einen präziseren Markteinstieg verwendet.

Diese Beispiele helfen zu verstehen, wie der Quoting-Mechanismus in eigene Handelsalgorithmen integriert werden kann.

## Quoting-Verhalten

StockSharp unterstützt verschiedene Quoting-Verhalten:

- **[MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior)** -- Quoting auf Basis des Marktpreises mit konfigurierbarem Abstand und Typ.
- **[BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior)** -- Quoting auf Basis des besten Preises mit konfigurierbarem Abstand.
- **[LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior)** -- Quoting zu einem festen Preis.
- **[BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior)** -- Quoting auf Basis des besten Preises nach Volumen.
- **[LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior)** -- Quoting auf Basis eines angegebenen Levels im Orderbuch.
- **[LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior)** -- Quoting auf Basis des Preises des letzten Trades.
- **[TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior)** -- Options-Quoting auf Basis des theoretischen Preises.
- **[VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior)** -- Options-Quoting auf Basis der Volatilität.

## Verwendung in Ihrer eigenen Strategie

### Schritt 1: Quoting-Verhalten erstellen

```csharp
// Verhalten für Market Quoting erstellen
var behavior = new MarketQuotingBehavior(
	new Unit(0.01m), // Preisabstand von der besten Quote
	new Unit(0.1m, UnitTypes.Percent), // Mindestabweichung für Quote-Aktualisierung
	MarketPriceTypes.Following // Marktpreistyp für Quoting
);
```

### Schritt 2: Prozessor erstellen und initialisieren

```csharp
// Quoting-Prozessor erstellen
_quotingProcessor = new QuotingProcessor(
	behavior,
	Security, // Instrument
	Portfolio, // Portfolio
	Sides.Buy, // Quoting-Richtung
	Volume, // Quoting-Volumen
	Volume, // Maximales Ordervolumen
	TimeSpan.Zero, // Kein Timeout
	this, // Strategy implementiert ISubscriptionProvider
	this, // Strategy implementiert IMarketRuleContainer
	this, // Strategy implementiert ITransactionProvider
	this, // Strategy implementiert ITimeProvider
	this, // Strategy implementiert IMarketDataProvider
	IsFormedAndOnlineAndAllowTrading, // Handelsberechtigung prüfen
	true, // Orderbuchpreise verwenden
	true  // Letzten Trade-Preis verwenden, wenn Orderbuch leer ist
)
{
	Parent = this
};
```

### Schritt 3: Prozessorereignisse abonnieren

```csharp
// Prozessorereignisse für Logging und Verarbeitung abonnieren
_quotingProcessor.OrderRegistered += order =>
	this.AddInfoLog($"Order {order.TransactionId} zum Preis {order.Price} registriert");

_quotingProcessor.OrderFailed += fail =>
	this.AddInfoLog($"Order fehlgeschlagen: {fail.Error.Message}");

_quotingProcessor.OwnTrade += trade =>
	this.AddInfoLog($"Trade ausgeführt: {trade.Trade.Volume} zu {trade.Trade.Price}");

_quotingProcessor.Finished += isOk => {
	this.AddInfoLog($"Quoting erfolgreich abgeschlossen: {isOk}");
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;
};
```

### Schritt 4: Prozessor starten

```csharp
// Prozessor starten
_quotingProcessor.Start();
```

### Schritt 5: Ressourcen freigeben

Vergessen Sie nicht, die Prozessorressourcen beim Stoppen der Strategie aufzuräumen:

```csharp
protected override void OnStopped()
{
	// Ressourcen des aktuellen Prozessors freigeben
	_quotingProcessor?.Dispose();
	_quotingProcessor = null;

	base.OnStopped();
}
```

## Vorteile der Verwendung

1. **Reduzierte Slippage** -- Quoting hilft, im Vergleich zu Market-Orders einen besseren Ausführungspreis zu erzielen.

2. **Konfigurationsflexibilität** -- verschiedene Quoting-Verhalten für unterschiedliche Marktsituationen.

3. **Automatische Aktualisierungen** -- der Prozessor verfolgt Marktänderungen automatisch und aktualisiert Orders bei Bedarf.

4. **Volle Kontrolle** -- Möglichkeit, Quoting-Parameter zu konfigurieren, einschließlich Preisabstand, Mindestabweichung und Marktpreistyp.

5. **Risikomanagement** -- Möglichkeit, ein Timeout zur Begrenzung der Ausführungszeit festzulegen.

## Praktische Anwendungsfälle

- **Market Making** -- Erzeugen von Liquidität im Markt durch Platzieren zweiseitiger Quotes.
- **Algorithmischer Handel** -- Verbesserung des Ausführungspreises in automatisierten Strategien.
- **Gegentrendhandel** -- präziserer Markteinstieg gegen den Trend.
- **Arbitrage** -- gleichzeitiges Quoting auf mehreren Märkten, um Preisunterschiede auszunutzen.

Die Implementierung des Quoting-Mechanismus in Ihrer Strategie kann die Orderausführung deutlich verbessern und ihre Effizienz erhöhen.

# Quoting-Referenz

## Überblick

Dieser Abschnitt enthält eine vollständige Referenz zur Architektur und zu den Komponenten des Quoting-Systems in StockSharp. Alle verfügbaren Quoting-Verhalten, Aktionstypen und Schnittstellen werden beschrieben. Eine grundlegende Einführung finden Sie unter [Quoting-Algorithmus](quoting.md).

## Architektur

### QuotingStrategy (Veraltet)

Die Klasse [QuotingStrategy](xref:StockSharp.Algo.Strategies.Quoting.QuotingStrategy) ist veraltet. Es wird empfohlen, stattdessen [QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) zu verwenden.

Hauptparameter der veralteten Klasse:

- `QuotingSide` -- Quoting-Richtung (Buy/Sell)
- `QuotingVolume` -- Quoting-Volumen
- `TimeOut` -- Ausführungs-Timeout
- `UseBidAsk` -- Orderbuchpreise verwenden
- `UseLastTradePrice` -- Preis des letzten Trades verwenden

### QuotingEngine

[QuotingEngine](xref:StockSharp.Algo.Strategies.Quoting.QuotingEngine) -- der funktionale Kern des Quoting-Systems. Er berechnet verbleibendes Volumen und Timeouts und gibt Aktionsempfehlungen ohne Seiteneffekte zurück.

### QuotingBehaviorAlgo

[QuotingBehaviorAlgo](xref:StockSharp.Algo.Strategies.Quoting.QuotingBehaviorAlgo) -- Implementierung der Schnittstelle `IPositionModifyAlgo` für algorithmische Positionsverwaltung.

Hauptmethoden:

| Methode | Beschreibung |
|---------|--------------|
| `UpdateMarketData(time, price, volume)` | Marktdaten aktualisieren |
| `UpdateOrderBook(depth)` | Orderbuch aktualisieren |
| `GetNextAction()` | Nächste Quoting-Aktion abrufen |

Unterstützt VWAP- und TWAP-Modi.

### QuotingProcessor

[QuotingProcessor](xref:StockSharp.Algo.Strategies.Quoting.QuotingProcessor) -- der Hauptprozessor zur Ausführung von Quoting innerhalb einer Strategie. Verwaltet den Order-Lebenszyklus: Platzierung, Änderung, Stornierung.

## QuotingAction-Typen

Der Prozessor gibt Aktionen vom Typ [QuotingAction](xref:StockSharp.Algo.Strategies.Quoting.QuotingAction) zurück:

| Typ | Beschreibung |
|-----|--------------|
| `None(reason)` | Keine Aktion erforderlich |
| `PlaceOrder(price, volume)` | Neue Order platzieren |
| `ModifyOrder(price, volume)` | Bestehende Order ändern |
| `CancelOrder()` | Order stornieren |
| `Finish(success, reason)` | Quoting beenden |

## Quoting-Verhalten

StockSharp stellt 10 Arten von Quoting-Verhalten bereit, die jeweils die Schnittstelle [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) implementieren:

| # | Klasse | Beschreibung | Schlüsselparameter |
|---|--------|--------------|--------------------|
| 1 | [BestByPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByPriceQuotingBehavior) | Bester Preis aus dem Orderbuch | BestPriceOffset |
| 2 | [LastTradeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LastTradeQuotingBehavior) | Preis des letzten Trades | BestPriceOffset |
| 3 | [LimitQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LimitQuotingBehavior) | Fester Limitpreis | LimitPrice |
| 4 | [MarketQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.MarketQuotingBehavior) | Marktpreis mit Abstand | PriceOffset, PriceType (Following/Opposite/Middle) |
| 5 | [VolatilityQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VolatilityQuotingBehavior) | Optionsvolatilität (Black-Scholes) | IVRange, Model |
| 6 | [TheorPriceQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TheorPriceQuotingBehavior) | Theoretischer Optionspreis | TheorPriceOffset |
| 7 | [BestByVolumeQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.BestByVolumeQuotingBehavior) | Kumuliertes Volumen im Orderbuch | VolumeExchange |
| 8 | [LevelQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.LevelQuotingBehavior) | Orderbuchtiefen-Level | Level (Range\<int\>), OwnLevel |
| 9 | [VWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.VWAPQuotingBehavior) | VWAP -- volumengewichteter Durchschnitt | BestPriceOffset |
| 10 | [TWAPQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.TWAPQuotingBehavior) | TWAP -- zeitgewichteter Durchschnitt | TimeInterval, PriceBufferSize (Standardwert 10) |

## IQuotingBehavior-Schnittstelle

Die Schnittstelle [IQuotingBehavior](xref:StockSharp.Algo.Strategies.Quoting.IQuotingBehavior) definiert zwei zentrale Methoden:

### CalculateBestPrice

Berechnet den besten Preis zum Platzieren einer Order:

```cs
decimal? CalculateBestPrice(
	Security security,
	IMarketDataProvider provider,
	Sides quotingDirection,
	decimal? bestBid,
	decimal? bestAsk,
	decimal? lastTradePrice,
	decimal? lastTradeVolume,
	IEnumerable<QuoteChange> bids,
	IEnumerable<QuoteChange> asks);
```

### NeedQuoting

Bestimmt, ob die aktuelle Order aktualisiert werden muss:

```cs
decimal? NeedQuoting(
	Security security,
	IMarketDataProvider provider,
	DateTimeOffset currentTime,
	decimal? currentPrice,
	decimal? currentVolume,
	decimal? newVolume,
	decimal? bestPrice);
```

Gibt `null` zurück, wenn keine Aktualisierung erforderlich ist, oder den neuen Preis zum Platzieren der Order.

## Verwendungsbeispiele

### Market Quoting mit Abstand

```cs
var behavior = new MarketQuotingBehavior
{
	PriceOffset = new Unit(2, UnitTypes.Absolute),
	PriceType = MarketPriceTypes.Following,
	BestPriceOffset = new Unit(0.5m),
};
```

### VWAP-Quoting

```cs
var behavior = new VWAPQuotingBehavior
{
	BestPriceOffset = new Unit(1),
};
```

### Quoting der Optionsvolatilität

```cs
var behavior = new VolatilityQuotingBehavior
{
	IVRange = new Range<decimal>(0.2m, 0.3m),
	Model = new BlackScholes(option, connector),
};
```

### Vollständiges Beispiel mit Prozessor

```cs
// Quoting-Verhalten auswählen
var behavior = new BestByPriceQuotingBehavior
{
	BestPriceOffset = new Unit(0.01m),
};

// Prozessor erstellen
var processor = new QuotingProcessor(
	behavior,
	Security,
	Portfolio,
	Sides.Buy,
	volume: 10,
	maxOrderVolume: 10,
	timeout: TimeSpan.FromMinutes(5),
	subscriptionProvider: this,
	ruleContainer: this,
	transactionProvider: this,
	timeProvider: this,
	marketDataProvider: this,
	isTradingAllowed: IsFormedAndOnlineAndAllowTrading,
	useBidAsk: true,
	useLastTradePrice: true)
{
	Parent = this,
};

processor.Finished += isOk =>
{
	this.AddInfoLog($"Quoting abgeschlossen: {isOk}");
	processor?.Dispose();
};

processor.Start();
```

## Siehe auch

- [Quoting-Algorithmus](quoting.md)
- [Quoting-Strategie](samples/mq.md)
- [Volatilitäts-Quoting](../options/volatility_trading.md)

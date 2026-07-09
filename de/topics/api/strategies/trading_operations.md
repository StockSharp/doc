# Handelsoperationen in Strategien

In StockSharp stellt die Klasse [Strategy](xref:StockSharp.Algo.Strategies.Strategy) verschiedene Methoden für die Arbeit mit Orders bereit. Dadurch lassen sich Handelsstrategien komfortabel implementieren.

## Methoden zum Platzieren von Orders

Es gibt mehrere Möglichkeiten, Orders in StockSharp-Strategien zu platzieren:

### 1. Verwendung von High-Level-Methoden

Am einfachsten ist die Verwendung integrierter Methoden, die eine Order in einem einzigen Aufruf erstellen und registrieren:

```cs
// Zum Marktpreis kaufen
BuyMarket(volume);

// Zum Marktpreis verkaufen
SellMarket(volume);

// Zum Limitpreis kaufen
BuyLimit(price, volume);

// Zum Limitpreis verkaufen
SellLimit(price, volume);

// Aktuelle Position zum Marktpreis schließen
ClosePosition();
```

Diese Methoden bieten maximale Einfachheit und Lesbarkeit des Codes. Sie führen automatisch Folgendes aus:
- Erstellen eines Order-Objekts mit den angegebenen Parametern
- Ausfüllen der erforderlichen Felder (Instrument, Portfolio usw.)
- Registrieren der Order im Handelssystem

### 2. Verwendung von CreateOrder + RegisterOrder

Ein flexiblerer Ansatz besteht darin, Erstellung und Registrierung von Orders zu trennen:

```cs
// Order-Objekt erstellen
var order = CreateOrder(Sides.Buy, price, volume);

// Zusätzliche Order-Einstellungen
order.Comment = "My special order";
order.TimeInForce = TimeInForce.MatchOrCancel;

// Order registrieren
RegisterOrder(order);
```

Die Methode [CreateOrder](xref:StockSharp.Algo.Strategies.Strategy.CreateOrder(StockSharp.Messages.Sides,System.Decimal,System.Nullable{System.Decimal})) erstellt ein initialisiertes Order-Objekt, das vor der Registrierung weiter angepasst werden kann.

### 3. Direktes Erstellen und Registrieren einer Order

Für maximale Kontrolle können Sie ein Order-Objekt direkt erstellen und registrieren:

```cs
// Order-Objekt direkt erstellen
var order = new Order
{
	Security = Security,
	Portfolio = Portfolio,
	Side = Sides.Buy,
	Type = OrderTypes.Limit,
	Price = price,
	Volume = volume,
	Comment = "Custom order"
};

// Order registrieren
RegisterOrder(order);
```

Weitere Details zur Arbeit mit Orders finden Sie im Abschnitt [Orderverwaltung](../orders_management.md).

## Verarbeitung von Order-Ereignissen

Nach dem Registrieren einer Order ist es wichtig, ihren Status zu verfolgen. In einer Strategie können Sie:

### 1. Ereignishandler verwenden

```cs
// Ereignis für empfangene Orders abonnieren
OrderReceived += OnOrderReceived;

// Ereignis für fehlgeschlagene Order-Registrierungen abonnieren
OrderRegisterFailed += OnOrderRegisterFailed;

private void OnOrderReceived(Order order)
{
	if (order.State == OrderStates.Done)
	{
		// Order ausgeführt - entsprechende Logik ausführen
	}
}

private void OnOrderRegisterFailed(OrderFail fail)
{
	// Fehler bei der Order-Registrierung verarbeiten
	LogError($"Order registration error: {fail.Error}");
}
```

### 2. Regeln für Orders verwenden

Ein leistungsfähigerer Ansatz ist die Verwendung von [Regeln](event_model.md) für Orders:

```cs
// Order erstellen
var order = BuyLimit(price, volume);

// Regel erstellen, die ausgelöst wird, wenn die Order ausgeführt wurde
order
	.WhenMatched(this)
	.Do(() => {
		// Aktionen nach Order-Ausführung
		LogInfo($"Order {order.TransactionId} executed");

		// Zum Beispiel eine Stop-Order platzieren
		var stopOrder = SellLimit(price * 0.95, volume);
	})
	.Apply(this);

// Regel zur Verarbeitung eines Registrierungsfehlers
order
	.WhenRegisterFailed(this)
	.Do(fail => {
		LogError($"Order registration error: {fail.Error}");
		// Möglicherweise mit anderen Parametern erneut versuchen
	})
	.Apply(this);
```

Detaillierte Beispiele zur Verwendung von Regeln mit Orders finden Sie im Abschnitt [Order-Regelbeispiele](event_model/samples/rule_order.md).

## Positionsverwaltung

Die Strategie stellt auch Methoden zur Positionsverwaltung bereit:

```cs
// Aktuelle Position abrufen
decimal currentPosition = Position;

// Aktuelle Position schließen
ClosePosition();

// Position mit Stop-Loss und Take-Profit schützen
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute),   // Take-Profit
	stopLoss: new Unit(20, UnitTypes.Absolute),     // Stop-Loss
	isStopTrailing: true,                        // Trailing-Stop
	useMarketOrders: true                        // Market-Orders verwenden
);
```

## Strategiezustand vor dem Handel

Vor der Ausführung von Handelsoperationen muss sichergestellt werden, dass sich die Strategie im richtigen Zustand befindet. StockSharp stellt mehrere Eigenschaften und Methoden bereit, um die Bereitschaft der Strategie zu prüfen:

### IsFormed-Eigenschaft

Die Eigenschaft [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) gibt an, ob alle in der Strategie verwendeten Indikatoren gebildet (aufgewärmt) sind. Standardmäßig wird geprüft, ob alle zur Sammlung [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) hinzugefügten Indikatoren den Zustand [IIndicator.IsFormed](xref:StockSharp.Algo.Indicators.IIndicator.IsFormed) = `true` haben.

Mehr über die Arbeit mit Indikatoren in einer Strategie finden Sie im Abschnitt [Indikatoren in Strategien](indicators.md).

### IsOnline-Eigenschaft

Die Eigenschaft [IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) zeigt an, ob sich die Strategie im Echtzeitmodus befindet. Sie wird erst dann `true`, wenn die Strategie gestartet ist und alle ihre Marktdatenabonnements in den Zustand [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) gewechselt sind.

Weitere Details zu Marktdatenabonnements in Strategien finden Sie im Abschnitt [Marktdatenabonnements in Strategien](subscriptions.md).

### TradingMode-Eigenschaft

Die Eigenschaft [TradingMode](xref:StockSharp.Algo.Strategies.Strategy.TradingMode) definiert den Handelsmodus der Strategie. Mögliche Werte:

- [StrategyTradingModes.Full](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Full) - alle Handelsoperationen sind erlaubt (Standardmodus)
- [StrategyTradingModes.Disabled](xref:StockSharp.Algo.Strategies.StrategyTradingModes.Disabled) - Handel ist vollständig deaktiviert
- [StrategyTradingModes.CancelOrdersOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.CancelOrdersOnly) - nur Order-Stornierung ist erlaubt
- [StrategyTradingModes.ReducePositionOnly](xref:StockSharp.Algo.Strategies.StrategyTradingModes.ReducePositionOnly) - nur Operationen zur Positionsreduzierung sind erlaubt

Diese Eigenschaft kann über Strategieparameter konfiguriert werden:

```cs
public SmaStrategy()
{
	_tradingMode = Param(nameof(TradingMode), StrategyTradingModes.Full)
					.SetDisplay("Handelsmodus", "Erlaubte Handelsoperationen", "Grundeinstellungen");
}
```

### Hilfsmethoden zur Zustandsprüfung

Für die bequeme Prüfung der Handelsbereitschaft der Strategie stellt StockSharp Hilfsmethoden bereit:

- [IsFormedAndOnline()](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnline) - prüft, ob die Strategie den Zustand `IsFormed = true` und `IsOnline = true` hat

- [IsFormedAndOnlineAndAllowTrading(StrategyTradingModes)](xref:StockSharp.Algo.Strategies.Strategy.IsFormedAndOnlineAndAllowTrading(StockSharp.Algo.Strategies.StrategyTradingModes)) - prüft, ob die Strategie gebildet ist, sich im Online-Modus befindet und über die erforderlichen Handelsberechtigungen verfügt

Die Methode `IsFormedAndOnlineAndAllowTrading` akzeptiert einen optionalen Parameter `required` vom Typ [StrategyTradingModes](xref:StockSharp.Algo.Strategies.StrategyTradingModes):

```cs
public bool IsFormedAndOnlineAndAllowTrading(StrategyTradingModes required = StrategyTradingModes.Full)
```

Mit diesem Parameter können Sie die Mindeststufe der Handelsberechtigungen angeben, die für eine bestimmte Operation erforderlich ist:

1. **StrategyTradingModes.Full** (Standardwert) - gibt nur dann `true` zurück, wenn sich die Strategie im vollständigen Handelsmodus befindet (`TradingMode = StrategyTradingModes.Full`). Wird für Operationen verwendet, die eine Position erhöhen können.

2. **StrategyTradingModes.ReducePositionOnly** - gibt `true` zurück, wenn sich die Strategie im vollständigen Handelsmodus oder ausschließlich im Modus zur Positionsreduzierung befindet. Wird für das Schließen oder teilweise Schließen von Positionen verwendet.

3. **StrategyTradingModes.CancelOrdersOnly** - gibt bei jedem aktiven Handelsmodus (außer `Disabled`) `true` zurück. Wird für Operationen zur Order-Stornierung verwendet.

Dadurch können Sie unterschiedliche Handelsoperationen abhängig vom aktuellen Handelsmodus selektiv erlauben oder verbieten:

```cs
// Für das Platzieren einer neuen Order, die eine Position erhöht, ist der vollständige Handelsmodus erforderlich
if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.Full))
{
	// Wir können beliebige Orders platzieren
	RegisterOrder(CreateOrder(Sides.Buy, price, volume));
}
// Zum Schließen einer Position reicht der Modus zur Positionsreduzierung aus
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly) && Position != 0)
{
	// Wir können nur die Position schließen
	ClosePosition();
}
// Zum Stornieren aktiver Orders reicht der Modus für Order-Stornierung aus
else if (IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
{
	// Wir können nur Orders stornieren
	CancelActiveOrders();
}
```

Diese Methode ermöglicht damit die Implementierung eines sicheren Zugriffskontrollmechanismus für Handelsfunktionen: Kritischere Operationen, etwa das Öffnen neuer Positionen, erfordern eine höhere Berechtigungsstufe, während weniger kritische Operationen wie das Stornieren von Orders auch in einem eingeschränkten Handelsmodus ausgeführt werden können.

Es ist gute Praxis, diese Methoden vor der Ausführung von Handelsoperationen zu verwenden:

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Prüfen, ob die Strategie gebildet ist und sich im Online-Modus befindet
	// und ob Handel erlaubt ist
	if (!IsFormedAndOnlineAndAllowTrading())
		return;

	// Handelslogik
	// ...
}
```

## Beispiel für Handelsoperationen

Unten sehen Sie ein Beispiel, das verschiedene Möglichkeiten zum Platzieren von Orders in einer Strategie und zur Verarbeitung ihrer Ausführung zeigt:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Kerzen abonnieren
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);

	// Regel zur Verarbeitung von Kerzen erstellen
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);

	Connector.Subscribe(subscription);
}

private void ProcessCandle(ICandleMessage candle)
{
	// Prüfen, ob die Strategie handelsbereit ist
	if (!this.IsFormedAndOnlineAndAllowTrading())
		return;

	// Beispielhafte Handelslogik auf Basis des Schlusskurses
	if (candle.ClosePrice > _previousClose * 1.01)
	{
		// Option 1: Verwendung einer High-Level-Methode
		var order = BuyLimit(candle.ClosePrice, Volume);

		// Regel zur Verarbeitung der Order-Ausführung erstellen
		order
			.WhenMatched(this)
			.Do(() => {
				// Wenn die Order ausgeführt wurde, Stop-Loss und Take-Profit setzen
				StartProtection(
					takeProfit: new Unit(50, UnitTypes.Absolute),
					stopLoss: new Unit(20, UnitTypes.Absolute)
				);
			})
			.Apply(this);
	}
	else if (candle.ClosePrice < _previousClose * 0.99)
	{
		// Option 2: Getrennte Erstellung und Registrierung
		var order = CreateOrder(Sides.Sell, candle.ClosePrice, Volume);
		RegisterOrder(order);

		// Alternative Verarbeitung über das Ereignis
		OrderReceived += (o) => {
			if (o == order && o.State == OrderStates.Done)
			{
				// Aktionen nach der Ausführung
			}
		};
	}

	_previousClose = candle.ClosePrice;
}
```

## Siehe auch

- [Orderverwaltung](../orders_management.md)
- [Order-Regeln](event_model/samples/rule_order.md)
- [Ereignismodell](event_model.md)
- [Positionsschutz](take_profit_and_stop_loss.md)

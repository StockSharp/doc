# Marktdatenabonnements in Strategien

In StockSharp verwenden Strategien einen Abonnementmechanismus, um Marktdaten zu empfangen. Dieser Ansatz ist die wichtigste und bevorzugte Methode zur Datenbeschaffung in Handelsstrategien.

## Grundlagen von Abonnements

Abonnements in Strategien basieren auf dem allgemeinen [StockSharp-Abonnementmechanismus](../market_data/subscriptions.md). Sie bieten eine zentralisierte und einheitliche Möglichkeit, alle Arten von Marktdaten zu erhalten.

## Erstellen eines Abonnements in einer Strategie

In der Methode [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) der Strategie können Sie ein Abonnement für die erforderlichen Daten erstellen und starten:

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);

	// Abonnement für 5-Minuten-Kerzen direkt über DataType erstellen
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);

	// Wenn zusätzliche Parameter erforderlich sind, können Sie das Abonnement konfigurieren
	subscription.From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(7));

	// Regel zur Verarbeitung eingehender Kerzen erstellen
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);

	// Abonnement starten
	Connector.Subscribe(subscription);
}
```

In diesem Beispiel wird ein Abonnement für 5-Minuten-Kerzen mit einem praktischen Konstruktor erstellt, der `DataType` und `Security` akzeptiert. Bei Bedarf können Sie zusätzlich Abonnementparameter konfigurieren, beispielsweise den Historienzeitraum.

## Vorteile von Abonnements in Strategien

Die Verwendung von Abonnements in Strategien hat im Vergleich zum direkten Abonnieren von Ereignissen von [Strategy.Connector](xref:StockSharp.Algo.Strategies.Strategy.Connector) mehrere Vorteile:

1. **Isolation** - jedes Abonnement arbeitet unabhängig, sodass verschiedene Datentypen für verschiedene Instrumente empfangen werden können, ohne sich gegenseitig zu beeinflussen. Dies schützt die Strategie außerdem davor, Daten zu erhalten, die für andere parallel laufende Strategien bestimmt sind. Bei direktem Abonnement von Connector-Ereignissen müssten Sie Daten zusätzlich filtern, um Informationen anderer Strategien auszuschließen.

2. **Zustandsverwaltung** - Abonnements haben klare Zustände ([SubscriptionStates](xref:StockSharp.Messages.SubscriptionStates)), mit denen genau bestimmt werden kann, ob historische Daten gerade empfangen werden oder das Abonnement bereits in den Online-Modus gewechselt ist.

3. **Automatische Steuerung des Strategiezustands** - die Strategie verfolgt automatisch den Zustand aller ihrer Abonnements und wechselt erst dann in den Online-Modus ([IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline)), wenn alle Abonnements online sind.

4. **Einheitlichkeit des Codes** - Abonnements verwenden einen einheitlichen Ansatz, unabhängig vom Typ der angeforderten Daten.

5. **Integration mit Regeln** - Abonnements lassen sich über Regeln leicht in das [Ereignismodell](event_model.md) der Strategie integrieren.

6. **Automatische Abonnementverwaltung** - wenn die Strategie stoppt, werden alle ihre Abonnements automatisch storniert, wodurch Ressourcen freigegeben werden.

7. **Unterstützung historischer Daten** - Möglichkeit, historische Daten vor dem Wechsel zu Echtzeitdaten zu laden.

## Überwachung von Abonnementzuständen

Die Strategie verfolgt automatisch den Zustand aller Abonnements, um ihren Betriebsmodus zu steuern:

```cs
private void CheckRefreshOnlineState()
{
	bool nowOnline = ProcessState == ProcessStates.Started;

	if (nowOnline)
		nowOnline = _subscriptions.CachedKeys
			.Where(s => !s.SubscriptionMessage.IsHistoryOnly())
			.All(s => s.State == SubscriptionStates.Online);

	// IsOnline-Zustand der Strategie aktualisieren
	IsOnline = nowOnline;
}
```

Die Eigenschaft [Strategy.IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) ist nur dann `true`, wenn alle Strategieabonnements in den Zustand [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) gewechselt sind. Dadurch erkennt die Strategie den Moment, in dem sie mit aktuellen Marktdaten arbeitet.

## Arten von Abonnements

In Strategien können Sie Abonnements für verschiedene Arten von Marktdaten verwenden:

```cs
// Abonnement für Kerzen
var candleSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	Security);

// Abonnement für Markttiefe
var depthSubscription = new Subscription(
	DataType.MarketDepth,
	Security);

// Abonnement für Tick-Trades
var tickSubscription = new Subscription(
	DataType.Ticks,
	Security);

// Abonnement für Level1 (bester Bid/Ask und andere Basisinformationen)
var level1Subscription = new Subscription(
	DataType.Level1,
	Security);
```

## Verarbeitung von Abonnementdaten über Regeln

Zur Verarbeitung von Daten, die über ein Abonnement eintreffen, wird empfohlen, [Regeln](event_model.md) zu verwenden:

```cs
// Abonnement für Kerzen
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), Security);

// Regel zur Verarbeitung eingehender Kerzen erstellen
Connector
	.WhenCandlesFinished(subscription)  // Regelaktivierung beim Empfang einer abgeschlossenen Kerze
	.Do(ProcessCandle)                   // Verarbeitungsmethode aufrufen
	.Apply(this);                        // Regel auf Strategie anwenden

// Abonnement starten
Connector.Subscribe(subscription);
```

Im obigen Beispiel wird eine Regel erstellt, die die Methode `ProcessCandle` beim Empfang jeder abgeschlossenen Kerze aufruft.

## Anfordern historischer Daten

Die Strategie setzt den Zeitraum für das Laden von Historie automatisch über die Eigenschaft [Strategy.HistorySize](xref:StockSharp.Algo.Strategies.Strategy.HistorySize):

```cs
// Zeitraum für das Laden von Historie auf 30 Tage setzen
strategy.HistorySize = TimeSpan.FromDays(30);
```

Beim Erstellen eines Abonnements setzt die Strategie automatisch den Parameter `From`, um Historie zu laden, sofern er nicht explizit angegeben wurde.

## Stornieren von Abonnements

Abonnements können manuell storniert werden, indem die Methode [UnSubscribe](xref:StockSharp.BusinessEntities.ISubscriptionProvider.UnSubscribe(StockSharp.BusinessEntities.Subscription)) aufgerufen wird:

```cs
// Abonnement stornieren
Connector.UnSubscribe(subscription);
```

Beim Stoppen der Strategie werden alle Abonnements automatisch storniert, wenn der Parameter [UnsubscribeOnStop](xref:StockSharp.Algo.Strategies.Strategy.UnsubscribeOnStop) auf `true` gesetzt ist (Standardwert).

## Siehe auch

- [Marktdatenabonnements](../market_data/subscriptions.md)
- [Ereignismodell](event_model.md)
- [Strategiekompatibilität mit Plattformen](compatibility.md)


# Orderzustände

Die StockSharp API bietet die Möglichkeit, Informationen über Orders über den integrierten Abonnementmechanismus zu empfangen. Wie bei Marktdaten verwenden Transaktionsinformationen einen einheitlichen Ansatz auf Basis von [Subscription](xref:StockSharp.BusinessEntities.Subscription).

## Orderbezogene Ereignisse

[Connector](xref:StockSharp.Algo.Connector) stellt folgende Ereignisse zur Verarbeitung von Orderinformationen bereit:

| Ereignis | Beschreibung |
|---------|----------|
| [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) | Ereignis zum Empfang von Orderinformationen |
| [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) | Ereignis für Fehler bei der Orderregistrierung |
| [OrderCancelFailReceived](xref:StockSharp.Algo.Connector.OrderCancelFailReceived) | Ereignis für Fehler bei der Orderstornierung |
| [OrderEditFailReceived](xref:StockSharp.Algo.Connector.OrderEditFailReceived) | Ereignis für Fehler bei der Orderänderung |
| [OwnTradeReceived](xref:StockSharp.Algo.Connector.OwnTradeReceived) | Ereignis zum Empfang von Informationen über eigene Trades |

## Enum OrderStates

Während ihrer Lebensdauer durchläuft eine Order folgende Zustände:

![OrderStates](../../../images/orderstates.png)

- [OrderStates.None](xref:StockSharp.Messages.OrderStates.None) - die Order wurde im Handelsalgorithmus erstellt, aber noch nicht zur Registrierung gesendet.
- [OrderStates.Pending](xref:StockSharp.Messages.OrderStates.Pending) - die Order wurde zur Registrierung gesendet ([RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)). Das System wartet auf die Bestätigung ihrer Annahme durch die Börse. Bei erfolgreicher Annahme wird das Ereignis [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived) ausgelöst, und die Order wechselt in den Zustand [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active). Auch die Eigenschaften [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) und [Order.ServerTime](xref:StockSharp.BusinessEntities.Order.ServerTime) werden initialisiert. Wenn die Order abgelehnt wird, wird das Ereignis [OrderRegisterFailReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderRegisterFailReceived) mit einer Fehlerbeschreibung ausgelöst, und die Order wechselt in den Zustand [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed).
- [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) - die Order ist an der Börse aktiv. Eine solche Order bleibt aktiv, bis ihr gesamtes Volumen [Order.Volume](xref:StockSharp.BusinessEntities.Order.Volume) ausgeführt wurde oder sie über [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order)) zwangsweise storniert wird. Wenn die Order teilweise ausgeführt wird, werden Ereignisse [OwnTradeReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OwnTradeReceived) über neue Trades für die platzierte Order ausgelöst, ebenso das Ereignis [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived), das eine Benachrichtigung über die Änderung des Orderrests [Order.Balance](xref:StockSharp.BusinessEntities.Order.Balance) übergibt. Letzteres Ereignis wird auch bei einer Orderstornierung ausgelöst.
- [OrderStates.Done](xref:StockSharp.Messages.OrderStates.Done) - die Order ist an der Börse nicht mehr aktiv (sie wurde vollständig ausgeführt oder storniert).
- [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) - die Order wurde aus irgendeinem Grund von der Börse (oder einem zwischengeschalteten System, zum Beispiel dem Serverteil der Handelsplattform) nicht angenommen.

## Automatische Abonnements

Standardmäßig erstellt [Connector](xref:StockSharp.Algo.Connector) beim Verbinden automatisch Abonnements für Transaktionsinformationen ([SubscriptionsOnConnect](xref:StockSharp.Algo.Connector.SubscriptionsOnConnect)). Dazu gehören Abonnements für:

- Orderinformationen
- Trade-Informationen
- Positionsinformationen
- Basissuche nach Instrumenten

Beispiel zur Verarbeitung eines Orderempfangsereignisses:

```cs
private void InitConnector()
{
	// Ereignis für Orderempfang abonnieren
	Connector.OrderReceived += OnOrderReceived;

	// Ereignis für Empfang eigener Trades abonnieren
	Connector.OwnTradeReceived += OnOwnTradeReceived;

	// Ereignis für Fehler bei der Orderregistrierung abonnieren
	Connector.OrderRegisterFailReceived += OnOrderRegisterFailed;
}

private void OnOrderReceived(Subscription subscription, Order order)
{
	// Empfangene Order verarbeiten
	_ordersWindow.OrderGrid.Orders.TryAdd(order);

	// Wichtig! Prüfen, ob die Order zum aktuellen Abonnement gehört,
	// um doppelte Verarbeitung zu vermeiden
	if (subscription == _myOrdersSubscription)
	{
		// Zusätzliche Verarbeitung für das konkrete Abonnement
		Console.WriteLine($"Auftrag: {order.TransactionId}, Status: {order.State}");
	}
}
```

## Manuelles Erstellen von Orderabonnements

In manchen Fällen müssen Sie Informationen über Orders explizit anfordern. Dafür können Sie separate Abonnements erstellen:

```cs
// Abonnement für Orders eines bestimmten Portfolios erstellen
var ordersSubscription = new Subscription(DataType.Transactions, portfolio)
{
	TransactionId = Connector.TransactionIdGenerator.GetNextId(),
};

// Handler für den Empfang von Orders
Connector.OrderReceived += (subscription, order) =>
{
	if (subscription == ordersSubscription)
	{
		Console.WriteLine($"Auftrag: {order.TransactionId}, Status: {order.State}, Portfolio: {order.Portfolio.Name}");
	}
};

// Abonnement starten
Connector.Subscribe(ordersSubscription);
```

## Orderstatus prüfen

Zur Bestimmung des aktuellen Zustands einer Order werden Erweiterungsmethoden verwendet:

```cs
// Aufträgetatus prüfen
Order order = ...; // empfangene Order

// Ist die Order storniert
bool isCanceled = order.IsCanceled();

// Ist die Order vollständig ausgeführt
bool isMatched = order.IsMatched();

// Ist die Order teilweise ausgeführt
bool isPartiallyMatched = order.IsMatchedPartially();

// Ist mindestens ein Teil der Order ausgeführt
bool isNotEmpty = order.IsMatchedEmpty();

// Ausgeführtes Volumen abrufen
decimal matchedVolume = order.GetMatchedVolume();
```

## Erweiterter Ansatz: Arbeit mit mehreren Abonnements

In komplexen Szenarien müssen Sie möglicherweise gleichzeitig mit mehreren Orderabonnements arbeiten. In diesem Fall ist es wichtig, Ereignisse korrekt zu behandeln, um Duplikate zu vermeiden:

```cs
private Subscription _portfolio1OrdersSubscription;
private Subscription _portfolio2OrdersSubscription;

private void RequestOrdersForDifferentPortfolios()
{
	// Abonnement für Orders des ersten Portfolios
	_portfolio1OrdersSubscription = new Subscription(DataType.Transactions, _portfolio1);

	// Abonnement für Orders des zweiten Portfolios
	_portfolio2OrdersSubscription = new Subscription(DataType.Transactions, _portfolio2);

	// Gemeinsamer Handler für den Empfang von Orders
	Connector.OrderReceived += OnMultipleSubscriptionOrderReceived;

	// Abonnements starten
	Connector.Subscribe(_portfolio1OrdersSubscription);
	Connector.Subscribe(_portfolio2OrdersSubscription);
}

private void OnMultipleSubscriptionOrderReceived(Subscription subscription, Order order)
{
	// Bestimmen, zu welchem Abonnement die Order gehört
	if (subscription == _portfolio1OrdersSubscription)
	{
		// Aufträge des ersten Portfolios verarbeiten
	}
	else if (subscription == _portfolio2OrdersSubscription)
	{
		// Aufträge des zweiten Portfolios verarbeiten
	}
}
```

> [!NOTE]
> Ein solcher erweiterter Ansatz mit mehreren Orderabonnements sollte nur in Ausnahmefällen verwendet werden, wenn der Standard-Abonnementmechanismus nicht ausreicht.

## Asynchrone Natur von Transaktionen

Das Senden von Transaktionen (Registrierung, Ersetzung oder Stornierung von Orders) erfolgt asynchron. Dadurch muss das Handelsprogramm nicht auf eine Bestätigung der Börse warten, sondern kann weiterarbeiten, was die Reaktion auf Änderungen der Marktsituation beschleunigt.

Um den Status einer Order zu verfolgen, müssen Sie die entsprechenden Ereignisse abonnieren:
- [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) für den Empfang von Orderstatusaktualisierungen
- [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) zur Behandlung von Registrierungsfehlern

## Siehe auch

- [Abonnements](../market_data/subscriptions.md)
- [Orderzustände](orders_states.md)
- [Neue Order erstellen](create_new_order.md)
- [Order stornieren](order_cancel.md)


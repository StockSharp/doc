# Adapter-Routing

StockSharp unterstützt gleichzeitige Verbindungen zu mehreren Börsen und Brokern. Das Routing-System (Basket-Routing) verwaltet, welche Nachrichten an welche Adapter geleitet werden, und gewährleistet einen transparenten Betrieb mit mehreren Verbindungen.

## Allgemeine Architektur

Bei Verwendung mehrerer Adapter erstellt der Connector automatisch einen Basket, der alle Verbindungen kombiniert. Der Router bestimmt, an welchen Adapter jede einzelne Nachricht geleitet werden soll -- Marktdatenabonnements, Transaktionen, Portfolioanfragen usw.

## AdapterRouter

Die Schnittstelle [IAdapterRouter](xref:StockSharp.Algo.IAdapterRouter) definiert die Logik zum Routen von Nachrichten zwischen Adaptern.

### Hauptmethoden

| Methode | Beschreibung |
|--------|-------------|
| `GetAdapters` | Gibt eine Liste der Adapter zurück, die für die Verarbeitung der gegebenen Nachricht geeignet sind |
| `GetSubscriptionAdaptersAsync` | Bestimmt asynchron die Adapter für Marktdatenabonnements |
| `GetPortfolioAdapter` | Gibt den an ein bestimmtes Portfolio gebundenen Adapter zurück |
| `TryGetOrderAdapter` | Findet den Adapter, über den eine Order registriert wurde |
| `SetSecurityAdapter` | Bindet ein Instrument an einen bestimmten Adapter |
| `SetPortfolioAdapter` | Bindet ein Portfolio an einen bestimmten Adapter |

### Routing-Prioritäten

Das System bestimmt den Ziel-Adapter in folgender Prioritätsreihenfolge:

1. **Explizite Angabe** -- wenn ein Adapter in der Nachricht über die Eigenschaft `message.Adapter` angegeben ist, wird dieser Adapter verwendet.
2. **Instrumentenbindung** -- Zuordnung, die über `SetSecurityAdapter` festgelegt wurde.
3. **Datentypbindung** -- Adapter, die für einen bestimmten Nachrichtentyp registriert sind.
4. **Filterung nach unterstütztem Typ** -- Adapter, die den gegebenen Nachrichtentyp unterstützen, werden ausgewählt.

### Konfiguration des Routings

```cs
var router = connector.Adapter.InnerAdapters;

// Bind instrument to adapter for receiving tick data
router.SetSecurityAdapter(
    secId,
    DataType.Ticks,
    binanceAdapter
);

// Bind portfolio to adapter for transactions
router.SetPortfolioAdapter(
    "MyPortfolio",
    interactiveBrokersAdapter
);
```

## Verwaltung von Verbindungen

### Verbindungsstatus

Jeder Adapter im Basket durchläuft die standardmäßigen Verbindungsstatus:

- **Disconnected** -- getrennt
- **Connecting** -- Verbindung wird hergestellt
- **Connected** -- verbunden
- **Disconnecting** -- Verbindung wird getrennt

Der Basket aggregiert die Status aller enthaltenen Adapter.

### Aggregationsparameter

Die Eigenschaft `ConnectDisconnectEventOnFirstAdapter` bestimmt, wann der Basket als verbunden gilt:

- `true` -- das Verbindungsereignis wird ausgelöst, wenn der **erste** Adapter sich verbindet (Standard). Ermöglicht den Arbeitsbeginn, ohne auf alle Verbindungen zu warten.
- `false` -- das Ereignis wird erst ausgelöst, nachdem sich **alle** Adapter verbunden haben.

```cs
// Wait for all adapters to connect
connector.Adapter.InnerAdapters.ConnectDisconnectEventOnFirstAdapter = false;

connector.Connected += () =>
{
    Console.WriteLine("All adapters connected");
};

connector.Connect();
```

## Übergeordnete und untergeordnete Abonnements

Bei der Arbeit mit mehreren Adaptern kann ein einzelnes Abonnement in mehrere untergeordnete Abonnements aufgeteilt werden, von denen jedes an seinen eigenen Adapter geleitet wird. Das System führt automatisch Folgendes aus:

- Erstellt untergeordnete Abonnements für jeden geeigneten Adapter
- Aggregiert Antworten, bevor das übergeordnete Abonnement benachrichtigt wird
- Behandelt Teilfehler (wenn ein Adapter das Abonnement nicht abschließen kann, arbeiten die anderen weiter)

### Beispiel für mehrere Börsen

```cs
// Adding adapters
connector.Adapter.InnerAdapters.Add(binanceAdapter);
connector.Adapter.InnerAdapters.Add(bybitAdapter);

connector.Connect();

// Subscribing to ticks -- will be automatically routed
// to all adapters supporting the given instrument
var subscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(subscription);
```

## Warteschlange für ausstehende Nachrichten

Wenn beim Senden einer Nachricht kein Adapter verbunden ist, wird die Nachricht in eine Warteschlange für ausstehende Nachrichten (`IPendingMessageState`) eingereiht. Sobald ein Adapter sich verbindet, werden alle angesammelten Nachrichten automatisch gesendet.

```cs
// Registering an order before connecting -- the order will be sent
// automatically after the connection is established
connector.RegisterOrder(order);
connector.Connect();
```

## Konfiguration mehrerer Verbindungen

### Programmatische Konfiguration

```cs
// Creating adapters
var binance = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
    Key = "<API_KEY>",
    Secret = "<API_SECRET>".Secure(),
};

var ib = new InteractiveBrokersMessageAdapter(connector.TransactionIdGenerator)
{
    Address = InteractiveBrokersMessageAdapter.DefaultAddress,
};

// Adding to the basket
connector.Adapter.InnerAdapters.Add(binance);
connector.Adapter.InnerAdapters.Add(ib);

// Configuring routing
connector.Adapter.InnerAdapters.SetPortfolioAdapter("BinancePortfolio", binance);
connector.Adapter.InnerAdapters.SetPortfolioAdapter("IBPortfolio", ib);

connector.Connect();
```

### Grafische Konfiguration

Für die visuelle Konfiguration von Verbindungen verwenden Sie die grafische Konfigurationskomponente. Details finden Sie im Abschnitt [Grafische Konfiguration](connectors/graphical_configuration.md).

## Order-Tracking

Der Router verfolgt automatisch, über welchen Adapter jede Order registriert wurde. Beim Empfang von Order-Updates (Statusänderungen, Trades) leitet das System diese über denselben Adapter:

```cs
// The order will be registered through the adapter bound to the portfolio
var order = new Order
{
    Security = security,
    Portfolio = portfolio,
    Side = Sides.Buy,
    Price = price,
    Volume = volume,
};

connector.RegisterOrder(order);

// Cancellation will go through the same adapter automatically
connector.CancelOrder(order);
```

## Siehe auch

- [Connectors](connectors.md)
- [Grafische Konfiguration](connectors/graphical_configuration.md)
- [Positionsverwaltung](positions.md)

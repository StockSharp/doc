# Lebenszyklus von Abonnements

Abonnements in StockSharp durchlaufen bestimmte Lebenszyklusphasen. Die Schnittstelle [ISubscriptionProvider](xref:StockSharp.BusinessEntities.ISubscriptionProvider) stellt Ereignisse bereit, mit denen jede Phase verfolgt werden kann.

## Lebenszyklus-Ereignisse

### SubscriptionStarted

```cs
event Action<Subscription> SubscriptionStarted;
```

Wird aufgerufen, wenn ein Abonnement erfolgreich gestartet wurde: Der Adapter hat die Anfrage akzeptiert und mit der Datenübertragung begonnen. Bei historischen Abonnements bedeutet dies, dass das Laden der Daten begonnen hat. Bei Live-Abonnements bedeutet es, dass der Server die Anfrage akzeptiert hat.

### SubscriptionOnline

```cs
event Action<Subscription> SubscriptionOnline;
```

Wird aufgerufen, wenn das Abonnement in den Echtzeitmodus gewechselt ist. Bei Live-Abonnements bedeutet dies, dass das historische Aufholen, sofern vorhanden, abgeschlossen ist und die Daten nun in Echtzeit eintreffen. Dieses Signal ist für Strategien wichtig, damit sie erkennen, dass Indikatoren "aufgewärmt" sind und der Handel beginnen kann.

### SubscriptionStopped

```cs
event Action<Subscription, Exception> SubscriptionStopped;
```

Wird aufgerufen, wenn das Abonnement beendet wurde. Der Parameter `Exception` enthält den Grund für das Beenden:
- `null` - normale Beendigung (Benutzer hat abbestellt oder historische Daten sind abgeschlossen).
- Ein Exception-Objekt - ein Fehler (Verbindungsverlust, serverseitiger Fehler usw.).

### SubscriptionFailed

```cs
event Action<Subscription, Exception, bool> SubscriptionFailed;
```

Wird bei einem Abonnementfehler aufgerufen. Der dritte Parameter `bool` gibt an, ob es sich um eine Subscribe-Operation (`true`) oder eine Unsubscribe-Operation (`false`) handelte.

## Ereignisreihenfolge

Typische Sequenz für ein Live-Abonnement:

1. `Subscribe(subscription)` aufrufen
2. `SubscriptionStarted` - Abonnement akzeptiert
3. Daten treffen ein (Kerzen, Orderbücher, Trades usw.)
4. `SubscriptionOnline` - Wechsel in den Echtzeitmodus
5. Weitere Daten treffen in Echtzeit ein
6. `UnSubscribe(subscription)` aufrufen oder Verbindungsverlust
7. `SubscriptionStopped` - Abonnement beendet

Bei einem historischen Abonnement (mit angegebenem Datumsbereich):

1. `Subscribe(subscription)` aufrufen
2. `SubscriptionStarted` - Abonnement akzeptiert
3. Historische Daten treffen ein
4. `SubscriptionStopped` mit `null` - alle Daten wurden empfangen

## SubscriptionsOnConnect

Die Eigenschaft [Connector.SubscriptionsOnConnect](xref:StockSharp.Algo.Connector) definiert die Menge der Abonnements, die bei der Verbindung automatisch gesendet werden:

```cs
ISet<Subscription> SubscriptionsOnConnect { get; }
```

Standardmäßig sind Abonnements für die Suche nach Instrumenten, Portfolios und Orders enthalten:

```cs
SubscriptionsOnConnect.Add(SecurityLookup);
SubscriptionsOnConnect.Add(PortfolioLookup);
SubscriptionsOnConnect.Add(OrderLookup);
```

Sie können eigene Abonnements hinzufügen, die bei jeder Verbindung automatisch gestartet werden:

```cs
// Automatisches Level1-Datenabonnement hinzufügen
var l1Sub = new Subscription(DataType.Level1, security);
connector.SubscriptionsOnConnect.Add(l1Sub);

// Automatische Ordersuche bei Verbindung entfernen
connector.SubscriptionsOnConnect.Remove(connector.OrderLookup);
```

## Verbindungsereignisse pro Adapter

Bei der Arbeit mit mehreren Verbindungen (mehreren Adaptern) sind Ereignisse hilfreich, die anzeigen, welcher konkrete Adapter verbunden oder getrennt wurde:

### ConnectedEx

```cs
event Action<IMessageAdapter> ConnectedEx;
```

Wird bei erfolgreicher Verbindung eines bestimmten Adapters aufgerufen. Der Parameter ist der Adapter, der das Ereignis ausgelöst hat.

### DisconnectedEx

```cs
event Action<IMessageAdapter> DisconnectedEx;
```

Wird beim Trennen eines bestimmten Adapters aufgerufen.

### ConnectionErrorEx

```cs
event Action<IMessageAdapter, Exception> ConnectionErrorEx;
```

Wird bei einem Verbindungsfehler eines bestimmten Adapters aufgerufen.

Zusätzlich sind die aggregierten Ereignisse `Connected`, `Disconnected` und `ConnectionError` verfügbar, die ohne Angabe eines bestimmten Adapters ausgelöst werden.

## Beispiel

```cs
private readonly Connector _connector = new();

public void SetupSubscriptionTracking()
{
    // Lebenszyklus des Abonnements verfolgen
    _connector.SubscriptionStarted += subscription =>
    {
        Console.WriteLine($"Subscription started: {subscription.DataType}, " +
            $"Security: {subscription.SecurityId}");
    };

    _connector.SubscriptionOnline += subscription =>
    {
        Console.WriteLine($"Subscription online: {subscription.DataType}");
    };

    _connector.SubscriptionStopped += (subscription, error) =>
    {
        if (error == null)
            Console.WriteLine($"Subscription completed: {subscription.DataType}");
        else
            Console.WriteLine($"Subscription interrupted: {subscription.DataType}, " +
                $"Error: {error.Message}");
    };

    // Einzelne Adapterverbindungen verfolgen
    _connector.ConnectedEx += adapter =>
    {
        Console.WriteLine($"Adapter connected: {adapter.Name}");
    };

    _connector.DisconnectedEx += adapter =>
    {
        Console.WriteLine($"Adapter disconnected: {adapter.Name}");
    };

    _connector.ConnectionErrorEx += (adapter, error) =>
    {
        Console.WriteLine($"Adapter connection error {adapter.Name}: {error.Message}");
    };

    // Verbinden
    _connector.Connect();

    // Nach der Verbindung ein Abonnement erstellen
    _connector.Connected += () =>
    {
        var subscription = new Subscription(DataType.Ticks, security);
        _connector.Subscribe(subscription);
    };
}
```

## Siehe auch

[Verbindung](../connectors.md)

# Gefiltertes Orderbuch

Ein gefiltertes Orderbuch ist ein spezialisiertes Werkzeug in StockSharp, mit dem Trader und automatisierte Strategien am Markt arbeiten können, während eigene Orders von der Betrachtung ausgeschlossen werden. Dies ist besonders wichtig, wenn mehrere Strategien parallel laufen, um Situationen zu verhindern, in denen eine Strategie beginnt, mit einer anderen zu "handeln", ohne zu erkennen, dass das Volumen im Orderbuch von einem anderen Marktteilnehmer oder aus Aktionen einer anderen parallel laufenden Strategie stammt.

## Vorteile des gefilterten Orderbuchs

- **Vermeidung von Selbsthandel:** Strategien führen keine Orders gegen sich selbst oder gegeneinander aus, wenn sie parallel laufen.
- **Reinheit der Analyse:** Strategien können Marktbedingungen ausschließlich auf Basis externer Orders analysieren, ohne Verzerrungen durch eigene Orders.
- **Ausführungseffizienz:** Hilft, die Qualität der Orderausführung zu verbessern, indem die Auswirkung eigener Orders auf den Marktpreis minimiert wird.

## Abonnementbeispiel

Der Ansatz zur Arbeit mit dem gefilterten Orderbuch verwendet dieselbe Methode wie das [Abonnieren eines regulären Orderbuchs](subscriptions.md), jedoch mit einem anderen Wert für [DataType](xref:StockSharp.Messages.DataType). Das folgende Beispiel veranschaulicht das Abonnement eines gefilterten Orderbuchs für ein bestimmtes Instrument:

1. **Abonnieren des Ereignisses für Orderbuchaktualisierungen:** [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived), um Orderbuchaktualisierungen zu empfangen. Dieses Ereignis wird sowohl für reguläre als auch für gefilterte Orderbücher verwendet.

    Prüfen Sie beim Verarbeiten des Ereignisses [Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType) im Objekt `subscription`, das dem Ereignis zugeordnet ist. Wenn [Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType) [DataType](xref:StockSharp.Messages.DataType.FilteredMarketDepth) ist, bedeutet dies, dass das empfangene Orderbuch gefiltert ist:

    ```cs
    connector.OrderBookReceived += (sender, subscription, orderBook) =>
    {
        if (subscription.DataType == DataType.FilteredMarketDepth)
        {
            // Verarbeitungslogik für das gefilterte Orderbuch
            Console.WriteLine($"Received filtered order book for {orderBook.SecurityId}.");
        }
    };
    ```

2. **Senden des Abonnements:** Erstellen Sie ein [Subscription](xref:StockSharp.BusinessEntities.Subscription)-Objekt und senden Sie es an den Connector:

    ```cs
    var subscription = new Subscription(DataType.FilteredMarketDepth, security);
    connector.Subscribe(subscription);

    // oder so
    //var subscription = connector.SubscribeFilteredMarketDepth(security);
    ```

## Fazit

Die Verwendung des gefilterten Orderbuchs in StockSharp stellt Tradern und Strategieentwicklern ein flexibles Werkzeug für die Marktanalyse bereit. Sie können damit unerwünschte Selbstinteraktion zwischen gleichzeitig laufenden Strategien vermeiden und Entscheidungen auf Basis von Marktdaten aus Orders vereinfachen.


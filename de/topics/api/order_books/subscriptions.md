# Abonnements

## Orderbuch abonnieren

Um das Orderbuch in StockSharp zu abonnieren, müssen Sie folgende Schritte ausführen:

1. Abonnieren Sie das Ereignis zum Empfang von Orderbüchern [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) und verarbeiten Sie Objekte des Interfaces [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage):

```cs
// Ereignishandler
private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// Hier können Sie Orderbuchdaten verarbeiten, zum Beispiel auf dem Bildschirm anzeigen oder in Ihrer Handelsstrategie verwenden
	Console.WriteLine($"Received order book for {orderBook.SecurityId}. Best buy price: {orderBook.GetBestBid()?.Price}, Best sell price: {orderBook.GetBestAsk()?.Price}");
}

// Ereignis abonnieren
connector.OrderBookReceived += OnOrderBookReceived;
```

Es ist wichtig, das Ereignis [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) **vor** dem Senden einer Abonnementanfrage für das Orderbuch zu abonnieren. Dadurch wird sichergestellt, dass keine Daten verpasst werden, falls Orderbücher sehr schnell nach dem Senden der Abonnementanfrage eintreffen.

2. Senden Sie eine Abonnementanfrage mit der Methode [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)):

```cs
var security = GetSecurity(); // Security-Objekt abrufen, das abonniert werden soll
				
// Orderbuch abonnieren
var subscription = new Subscription(DataType.MarketDepth, security);
connector.Subscribe(subscription);
```

## Orderbuch abbestellen

Um das Orderbuch abzubestellen, rufen Sie die Methode [Connector.UnSubscribe](xref:StockSharp.Algo.Connector.UnSubscribe(StockSharp.BusinessEntities.Subscription)) auf:

```cs
connector.UnSubscribe(subscription);
```

## Klarstellung zum Empfang von Orderbüchern

Bei der Arbeit mit dem Ereignis [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) ist es wichtig zu verstehen, dass die über dieses Ereignis eintreffenden Orderbücher bereits zusammengestellt und einsatzbereit sind. Das bedeutet: Unabhängig von der Art der Datenübertragung durch die Quelle - ob differenzielle Daten (nur Änderungen im Orderbuch) oder vollständige Snapshots des Orderbuchs - verarbeitet die StockSharp-Plattform diese Daten so, dass der Trader ein vollständiges und aktualisiertes Orderbuch erhält.

Die Plattform integriert Änderungen automatisch in das Orderbuch und aktualisiert dessen Inhalt auf den aktuellen Zustand, bevor das Ereignis [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) aufgerufen wird. Dadurch wird die Arbeit mit Daten vereinfacht, da Trader differenzielle Daten nicht selbst verarbeiten oder das Orderbuch aus aufeinanderfolgenden Snapshots zusammensetzen müssen. Sie können also sicher sein, dass die im Ereignishandler empfangenen Daten den neuesten Zustand des Orderbuchs zum Zeitpunkt des Ereignisses widerspiegeln.

Dies vereinfacht die Entwicklung von Handelsstrategien und Marktanalysen erheblich, da sich Trader direkt auf die Logik ihrer Strategien konzentrieren können, ohne Zeit mit den technischen Aspekten der Zusammenstellung und Verarbeitung von Orderbuchdaten zu verbringen.

## Verwendungsbeispiel

Beispiele für die Verwendung des Orderbuchs sind im Projekt *Samples\/01\_Basic\/02\_MarketDepths* auf [GitHub](https://github.com/StockSharp/StockSharp/) oder im StockSharp-API-Archiv verfügbar, das über den [Installer](../../installer.md) bezogen werden kann. Diese Beispiele bieten praktische Darstellungen der Verbindung zu einem Handelssystem, des Abonnierens eines gefilterten Orderbuchs und der Verarbeitung empfangener Daten. Sie können als guter Ausgangspunkt für die Entwicklung eigener Handelsstrategien dienen.

## Siehe auch

[Subscriptions](../market_data/subscriptions.md)


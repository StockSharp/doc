# Dünnes Orderbuch

Ein Sparse Order Book ist eine Darstellung des Orderbuchs, die alle möglichen Preisniveaus anzeigt, einschließlich solcher, auf denen aktuell keine aktiven Orders vorhanden sind. Dieser Ansatz ermöglicht Tradern, die "Lücken" zwischen Orders visuell zu bewerten, also Preisniveaus ohne Kauf- oder Verkaufsorders, und gibt Einblick in mögliche Widerstands- oder Unterstützungsniveaus.

## Warum ein Sparse Order Book verwenden

Die Verwendung eines Sparse Order Book hat mehrere Vorteile:

1. **Visualisierung von Lücken:** Preisniveaus mit fehlenden Orders lassen sich leichter erkennen, was auf mögliche Einstiegs- oder Ausstiegspunkte hinweisen kann.
2. **Liquiditätsanalyse:** Eine klare Darstellung der Orderverteilung hilft, die Liquidität des Instruments auf verschiedenen Preisniveaus einzuschätzen.
3. **Strategische Planung:** Das Verständnis der Orderbuchstruktur ermöglicht eine genauere Planung von Handelsoperationen unter Berücksichtigung möglicher "Leerstellen" in der Liquidität.

## Erstellen eines Sparse Order Book

Für die Arbeit mit einem gruppierten Orderbuch muss zuerst der Empfang über [Abonnements](subscriptions.md) eingerichtet und anschließend die Erweiterungsmethode [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32)) aufgerufen werden. Die Methode nimmt folgende Parameter:

- `priceRange` - die Preisdifferenz, bis zu der Niveaus erweitert werden müssen.
- `priceStep` - der Preisschritt des Handelsinstruments. Er wird verwendet, wenn `priceRange` eine geringere Genauigkeit auf Preisniveaus als `priceStep` hat und die erhaltenen Preise auf den Preisschritt des Instruments gerundet werden müssen.

```cs
// Es wird angenommen, dass orderBook ein aus StockSharp erhaltenes IOrderBookMessage-Objekt ist
var sparseDepth = orderBook.Sparse(priceRange, priceStep);

// Jetzt enthält sparseDepth eine Darstellung des ursprünglichen Orderbuchs,
// in der alle möglichen Preisniveaus berücksichtigt werden, einschließlich solcher ohne Orders.
```

In diesem Beispiel wird [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32)) verwendet, um ein Sparse Order Book zu erstellen, das die Anzeige aller Preisniveaus ermöglicht, auch solcher ohne aktive Orders. Dies kann nützlich sein, um mögliche "leere" Niveaus zu analysieren, die als Unterstützungs- oder Widerstandsniveaus dienen können.


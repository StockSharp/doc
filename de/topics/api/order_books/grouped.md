# Gruppiertes Orderbuch

Zusätzlich zum [dünnen Orderbuch](sparse.md) kann es nützlich sein, ein gruppiertes Orderbuch zu verwenden, in dem Orders über breitere Preisbereiche aggregiert werden, um die Analyse zu vereinfachen und allgemeine Trends in Nachfrage und Angebot zu erkennen.

Vorteile eines gruppierten Orderbuchs:

- **Vereinfachte Analyse:** Die Aggregation von Orderdaten erleichtert die Wahrnehmung des Gesamtbilds des Marktes.
- **Trendidentifikation:** Wichtige Preisniveaus, auf denen der Großteil der Orders konzentriert ist, lassen sich leichter erkennen.

## Implementierung eines gruppierten Orderbuchs:

Für die Arbeit mit einem gruppierten Orderbuch muss zunächst der Empfang über [Abonnements](subscriptions.md) eingerichtet und anschließend die Erweiterungsmethode [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)) aufgerufen werden:

```cs
// Orderbuchdaten mit einem Preisaggregationsschritt gruppieren, zum Beispiel 0,5 Preiseinheiten
var groupedDepth = orderBook.Group(0.5);

// groupedDepth enthält nun ein Orderbuch, in dem Orders
// nach Preisniveaus mit dem angegebenen Aggregationsschritt gruppiert sind.
```

Die Methode [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)) ermöglicht die Aggregation von Orders im Buch über größere Preisniveaus. Dadurch wird die visuelle Marktanalyse vereinfacht und die Identifikation der wichtigsten Nachfrage- und Angebotsniveaus unterstützt, ohne jede einzelne Preisänderung analysieren zu müssen.

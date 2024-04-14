# Grouped Order Book

In addition to the [sparse order book](sparse.md), it can be useful to use a grouped order book, where orders are aggregated over broader price ranges to simplify analysis and identify general trends in demand and supply.

Advantages of a grouped order book:

- **Simplified analysis:** Aggregating order data simplifies the perception of the overall market picture.
- **Trend identification:** It's easier to identify key price levels where the majority of orders are concentrated.

## Implementation of a grouped order book:

To work with a grouped order book, it's necessary to first set up reception through [subscriptions](subscriptions.md), and then call the extension method [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)):

```cs
// Grouping order book data with a price aggregation step, for example, 0.5 units of price
var groupedDepth = orderBook.Group(0.5);

// groupedDepth now contains an order book in which orders are grouped
// by price levels with the specified aggregation step.
```

The [Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)) method allows for the aggregation of orders in the book over larger price levels, simplifying visual market analysis and helping to identify the main levels of demand and supply without the need to analyze every individual price change.
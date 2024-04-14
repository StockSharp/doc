# Sparse Order Book

A sparse order book is a presentation of the order book that displays all possible price levels, including those without any active orders at the moment. This approach allows traders to visually assess the "gaps" between orders, i.e., price levels where there are no buy or sell orders, giving an insight into potential resistance or support levels.

## Why Use a Sparse Order Book

Using a sparse order book has several advantages:

1. **Visualization of gaps:** It's easier to identify price levels with missing orders, which can indicate potential entry or exit points.
2. **Liquidity analysis:** A clear representation of order distribution helps assess the instrument's liquidity at different price levels.
3. **Strategic planning:** Understanding the structure of the order book allows for more accurate trading operations planning, considering potential "voids" in liquidity.

## Creating a Sparse Order Book

To work with a grouped order book, you first need to set up reception through [subscriptions](subscriptions.md), and then call the extension method [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal})). The method takes the following parameters:

- `priceRange` - the price difference up to which levels need to be expanded.
- `priceStep` - the trading instrument's price step. It is used in case `priceRange` has a lower precision on price levels than `priceStep`, and it is necessary to round the obtained prices to the instrument's price step.

```cs
// It is assumed that orderBook is an IOrderBookMessage object obtained from StockSharp
var sparseDepth = orderBook.Sparse(priceRange, priceStep);

// Now, sparseDepth contains a representation of the original order book,
// where all possible price levels are considered, including those without any orders.
```

In this example, [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal})) is used to create a sparse order book, which allows for displaying all price levels, even those without any active orders. This can be useful for analyzing potential "empty" levels that may serve as support or resistance levels.
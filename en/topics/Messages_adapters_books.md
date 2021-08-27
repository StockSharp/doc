# Order books (incremental and regular)

If an external trading system sends full order books (the order book is sent in full with each change), you should send it as a message:

```cs
// getting glasses from the trading system
private void SessionOnOrderBook(string pair, OrderBook book)
{
		SendOutMessage(new QuoteChangeMessage
		{
			SecurityId = pair.ToStockSharp(),
			Bids = book.Bids.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			Asks = book.Asks.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			ServerTime = book.Time,
		});
}
```

If an external trading system sends incremental order books (only changes in price levels are sent, not the entire order book), the logic of both building an order book snapshot (if it is not sent) and returning order book changes should be written in the adapter. To do this, you need to use the [QuoteChangeMessage.State](xref:StockSharp.Messages.QuoteChangeMessage.State) property: 

```cs
// get a snapshot of the glass from the trading system
private void SessionOnOrderBookSnapshot(string pair, OrderBook book)
{
		SendOutMessage(new QuoteChangeMessage
		{
			SecurityId = pair.ToStockSharp(),
			Bids = book.Bids.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			Asks = book.Asks.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			ServerTime = book.Time,
			State = QuoteChangeStates.SnapshotComplete, // <- specify that the current message is a snapshot,
			// and you need to reset the state of the glass with a new snapshot
		});
}
```

For sending incremental messages, the code is similar, but the order book change sign is set. If the [QuoteChange.Volume](xref:StockSharp.Messages.QuoteChange.Volume).Volume value is equal to 0, then this is a sign for removing the price level: 

```cs
// we get the changes of the order book of the order book
private void SessionOnOrderBookIncrement(string pair, OrderBook book)
{
		SendOutMessage(new QuoteChangeMessage
		{
			SecurityId = pair.ToStockSharp(),
			Bids = book.Bids.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(), // <- with zero volume, quotes are interpreted as deleted
			Asks = book.Asks.Select(e => new QuoteChange(e.Price, e.Size)).ToArray(),
			ServerTime = book.Time,
			State = QuoteChangeStates.Increment, // <- specify that the current message is incremental
		});
}
```

The last step is to override the [IMessageAdapter.IsSupportOrderBookIncrements](xref:StockSharp.Messages.IMessageAdapter.IsSupportOrderBookIncrements) property, which will indicate that the [OrderBookIncrementMessageAdapter](xref:StockSharp.Algo.OrderBookIncrementMessageAdapter) should be added to the adapter chain when connecting (see [Adapters chain](Messages_adapters_chain.md) for details): 

```cs
public partial class MyOwnMessageAdapter : MessageAdapter
{
	// ...
	
	/// <inheritdoc />
	public override bool IsSupportOrderBookIncrements => true;
}
```

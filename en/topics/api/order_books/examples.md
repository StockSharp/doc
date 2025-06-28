# Examples with the Order Book

## Getting the Best Prices

To get the best prices from the order book, it's important to focus on the first elements of the lists for buy orders ([Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids)) and sell orders ([Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks)), as these represent the most favorable available prices for transactions:

```cs
var bestBid = orderBook.Bids.FirstOrDefault();
var bestAsk = orderBook.Asks.FirstOrDefault();

if (bestBid != null)
{
	Console.WriteLine($"Best buy price: {bestBid.Price}");
}

if (bestAsk != null)
{
	Console.WriteLine($"Best sell price: {bestAsk.Price}");
}
```

Or use the ready-made extension methods [GetBestBid](xref:StockSharp.Messages.Extensions.GetBestBid(StockSharp.Messages.IOrderBookMessage)) and [GetBestAsk](xref:StockSharp.Messages.Extensions.GetBestAsk(StockSharp.Messages.IOrderBookMessage)):

```cs
var bestBid = orderBook.GetBestBid();
var bestAsk = orderBook.GetBestAsk();

if (bestBid != null)
{
	Console.WriteLine($"Best buy price: {bestBid.Price}, volume: {bestBid.Volume}");
}
else
{
	Console.WriteLine("No best buy orders.");
}

if (bestAsk != null)
{
	Console.WriteLine($"Best sell price: {bestAsk.Price}, volume: {bestAsk.Volume}");
}
else
{
	Console.WriteLine("No best sell orders.");
}
```

## Analyzing Order Book Depth

To analyze the depth of the order book, you can iterate through the items in the [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) and [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks) lists, starting from the beginning of the list. This provides an overview of the distribution of orders at different price levels and helps identify potential support and resistance levels:

```cs
foreach (var bid in orderBook.Bids)
{
	Console.WriteLine($"Buy price: {bid.Price}, volume: {bid.Volume}");
}

foreach (var ask in orderBook.Asks)
{
	Console.WriteLine($"Sell price: {ask.Price}, volume: {ask.Volume}");
}
```

## Searching for Volumes in the Order Book

An algorithm for searching for significant volumes in the order book helps identify levels where large orders accumulate. This can indicate the interest of major players and serve as an additional signal when making trading decisions.

Algorithm:

1. Determine a volume threshold that will be considered significant.
2. Iterate through the orders in the [Bids](xref:StockSharp.Messages.IOrderBookMessage.Bids) and [Asks](xref:StockSharp.Messages.IOrderBookMessage.Asks) list, comparing the volume of each order against the threshold value.
3. Record the price levels where orders with volume above the threshold were found.

```cs
double significantVolumeThreshold = 10000; // Example of a threshold value

Console.WriteLine("Significant volumes in the order book:");

foreach (var bid in orderBook.Bids)
{
	if (bid.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Buy: Price {bid.Price}, volume {bid.Volume}");
	}
}

foreach (var ask in orderBook.Asks)
{
	if (ask.Volume >= significantVolumeThreshold)
	{
		Console.WriteLine($"Sell: Price {ask.Price}, volume {ask.Volume}");
	}
}
```

This algorithm helps highlight levels with significant volumes, which may play a key role in market price movements.
# Subscriptions

## Subscribing to the Order Book

To subscribe to the order book in StockSharp, you need to perform the following steps:

1. Subscribe to the event for receiving order books [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) and handle the [IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) interface objects:

```cs
private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// Here you can process the order book data, for example, display it on the screen or use it in your trading strategy
	Console.WriteLine($"Received order book for {orderBook.SecurityId}. Best buy price: {orderBook.GetBestBid()?.Price}, Best sell price: {orderBook.GetBestAsk()?.Price}");
}
```

2. Initiate the subscription using the [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe) method:

```cs
var security = GetSecurity(); // Get the Security object you want to subscribe to
                
// subscribe to the order book
var subscription = new Subscription(DataType.MarketDepth, security);
connector.Subscribe(subscription);

// or like this
//var subscription = connector.SubscribeMarketDepth(security);
```

## Unsubscribing from the Order Book

To unsubscribe from the order book, call the [Connector.UnSubscribe](xref:StockSharp.Algo.Connector.UnSubscribe) method:

```cs
connector.UnSubscribe(subscription);
```

## Clarification on Receiving Order Books

When working with the [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) event, it's important to understand that the order books coming through this event are already compiled and ready for use. This means that regardless of the method of data transmission by the source - whether differential data (only changes in the order book) or full snapshots of the order book - the StockSharp platform processes this data in such a way that the trader receives a complete and updated order book.

The platform automatically integrates changes into the order book, updating its contents to the current state before calling the [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) event. This simplifies working with data, as traders do not need to independently process differential data or compile the order book from consecutive snapshots. Thus, you can be confident that the data received in the event handler reflects the latest state of the order book at the time of the event.

This significantly simplifies the development of trading strategies and market analysis, as traders can focus directly on the logic of their strategies, without spending time on the technical aspects of compiling and processing order book data.

## See Also

[Subscriptions](API_ConnectorsSubscriptions.md)
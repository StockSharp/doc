# Filtered Order Book

A filtered order book is a specialized tool in StockSharp that allows traders and automated strategies to operate in the market, excluding their own orders from consideration. This is critically important when using multiple strategies in parallel to prevent situations where one strategy starts "trading" with another, not realizing that the volume in the order book comes from another market participant or is the result of actions by another strategy running in parallel.

## Advantages of the Filtered Order Book

- **Avoidance of self-trading:** Strategies will not execute orders against themselves or each other when run in parallel.
- **Purity of analysis:** Allows strategies to analyze market conditions based solely on external orders, without distortions caused by their own orders.
- **Execution efficiency:** Helps improve the quality of order execution by minimizing the impact on the market price by one's own orders.

## Subscription Example

The approach to working with the filtered order book uses the same method as [subscribing to a regular order book](subscriptions.md), but with a different [DataType](xref:StockSharp.Messages.DataType) value. Below is an example illustrating the subscription to a filtered order book for a specific instrument:

1. **Subscribing to the order book update event:** [Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) to receive order book updates. This event is used for both the regular and the filtered order book.

    When processing the event, check the [Subscription.DataType](xref:StockSharp.Algo.Subscription.DataType) in the `subscription` object associated with the event. If [Subscription.DataType](xref:StockSharp.Algo.Subscription.DataType) is [DataType](xref:StockSharp.Messages.DataType.FilteredMarketDepth), it indicates that the received order book is filtered:

    ```cs
    connector.OrderBookReceived += (sender, subscription, orderBook) =>
    {
        if (subscription.DataType == DataType.FilteredMarketDepth)
        {
            // Handling logic for the filtered order book
            Console.WriteLine($"Received filtered order book for {orderBook.SecurityId}.");
        }
    };
    ```

2. **Sending the subscription:** Form a [Subscription](xref:StockSharp.Algo.Subscription) object and send it to the connector:

    ```cs
    var subscription = new Subscription(DataType.FilteredMarketDepth, security);
    connector.Subscribe(subscription);
    
    // or like this
    //var subscription = connector.SubscribeFilteredMarketDepth(security);
    ```

## Conclusion

Using the filtered order book in StockSharp provides traders and strategy developers with a flexible tool for market analysis, allowing them to avoid unwanted self-interaction between simultaneously running strategies and simplifying decision-making based on market order data.
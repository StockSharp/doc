# 筛选订单簿

过滤订单簿是StockSharp中的一种专用工具，它允许交易者和自动化策略在市场中操作，同时将自己的订单排除在考虑之外。在同时使用多种策略时，这一点尤为重要，以防出现一种策略在“不知道订单簿中的成交量来自其他市场参与者或是另一并行运行策略的操作结果”的情况下，开始与另一策略“交易”的情况。

## 过滤订单簿的优势

- **避免自我交易：** 当策略并行运行时，不会对自身或彼此执行订单。
- **分析的纯净性：** 允许策略仅基于外部订单分析市场状况，而不受自身订单造成的扭曲影响。
- **执行效率：** 通过将自身订单对市场价格的影响降到最低，帮助提高订单执行的质量。

## 订阅示例

处理过滤订单簿的方法与[订阅常规订单簿](subscriptions.md)的方法相同，但使用不同的[数据类型](xref:StockSharp.Messages.DataType)值。下面是一个示例，说明如何订阅特定合约的过滤订单簿：

1. **订阅订单簿更新事件：**[Connector.OrderBookReceived](xref:StockSharp.Algo.Connector.OrderBookReceived) 接收订单簿更新。此事件用于常规和过滤后的订单簿。

    处理事件时，请检查与该事件关联的 `subscription` 对象中的 [Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType)。如果 [Subscription.DataType](xref:StockSharp.Messages.SubscriptionBase`1.DataType) 为 [DataType](xref:StockSharp.Messages.DataType.FilteredMarketDepth)，则表示接收到的订单簿是过滤后的订单簿：

    ```cs
    connector.OrderBookReceived += (sender, subscription, orderBook) =>
    {
        if (subscription.DataType == DataType.FilteredMarketDepth)
        {
            // 过滤订单簿的处理逻辑
            Console.WriteLine($"Received filtered order book for {orderBook.SecurityId}.");
        }
    };
    ```

2. **发送订阅：** 创建一个 [Subscription](xref:StockSharp.BusinessEntities.Subscription) 对象并将其发送到连接器：

    ```cs
    var subscription = new Subscription(DataType.FilteredMarketDepth, security);
    connector.Subscribe(subscription);
    
    // 或这样
    //var subscription = connector.SubscribeFilteredMarketDepth(security);
    ```

## 结论

在 StockSharp 中使用过滤后的订单簿为交易者和策略开发者提供了一个灵活的市场分析工具，使他们能够避免同时运行的策略之间的不必要自我交互，并简化基于市场订单数据的决策。

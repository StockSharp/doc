# 分组订单簿

除了[稀疏订单簿](sparse.md)之外，使用分组订单簿也可能有用，在分组订单簿中，订单在更广泛的价格区间内被聚合，以简化分析并识别供需的一般趋势。

分组订单簿的优点：

- **简化分析：** 聚合订单数据可以简化对整体市场情况的认知。
- **趋势识别：** 更容易识别大多数订单集中所在的关键价格水平。

## 分组订单簿的实现：

要使用分组订单簿，首先需要通过[订阅](subscriptions.md)设置接收，然后调用扩展方法[Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal))：

```cs
// 按价格聚合步长对订单簿数据分组，例如 0.5 个价格单位
var groupedDepth = orderBook.Group(0.5);

// groupedDepth 现在包含一个订单已分组的订单簿
// 按指定聚合步长的价格层级分组。
```

[Group](xref:StockSharp.Messages.Extensions.Group(StockSharp.Messages.IOrderBookMessage,System.Decimal)) 方法允许在更大的价格水平上对订单簿中的订单进行聚合，从而简化市场的可视化分析，并有助于识别主要的供需水平，而无需分析每一个单独的价格变动。
# 稀疏订单簿

稀疏的订单簿是一种订单簿的展示方式，它显示所有可能的价格水平，包括那些当前没有任何活跃订单的价格。这种方法允许交易者直观地评估订单之间的“空隙”，即没有买入或卖出订单的价格水平，从而洞察潜在的阻力或支撑水平。

## 为什么使用稀疏订单簿

使用稀疏订单簿有几个优点：

1. **缺口可视化：** 更容易识别缺少订单的价格水平，这可以指示潜在的进出点。
2. **流动性分析：** 清晰的订单分布展示有助于评估该交易品种在不同价格水平的流动性。
3. **策略规划：**了解订单簿的结构可以更准确地进行交易操作计划，同时考虑流动性中潜在的“空白”.

## 创建稀疏订单簿

要使用分组订单簿，您首先需要通过[订阅](subscriptions.md)配置接收，然后调用扩展方法 [Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32))。该方法接受以下参数：

- `priceRange` - 价格差异需要扩展到的水平。
- `priceStep` - 交易品种的价格步长。当 `priceRange` 在价格水平上的精度低于 `priceStep` 且需要将获得的价格四舍五入到交易品种的价格步长时使用。

```cs
// 假定 orderBook 是从 StockSharp 获取的 IOrderBookMessage 对象
var sparseDepth = orderBook.Sparse(priceRange, priceStep);

// 现在 sparseDepth 包含原始订单簿的表示，
// 其中考虑所有可能的价格层级，包括没有任何订单的层级。
```

在这个例子中，[Sparse](xref:StockSharp.Messages.Extensions.Sparse(StockSharp.Messages.IOrderBookMessage,System.Decimal,System.Nullable{System.Decimal},System.Int32)) 被用来创建一个稀疏的订单簿，这允许显示所有价格级别，即使那些没有任何活跃订单的价格级别也能显示。这对于分析可能作为支撑或阻力位的“空”级别可能很有用。

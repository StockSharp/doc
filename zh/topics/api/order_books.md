# 订单簿

## 描述

订单簿（a.k.a. 市场深度）是关于特定交易品种当前买卖订单的信息，按价格等级组织。在StockSharp中，订单簿提供了供需数据，从而可以进行实时市场分析。

## 结构

[订单簿](xref:StockSharp.Messages.IOrderBookMessage) 包含两份订单列表：

- 买入订单，按价格降序排列 - [买单](xref:StockSharp.Messages.IOrderBookMessage.Bids)。
- 卖出订单，按价格升序排列 - [卖盘](xref:StockSharp.Messages.IOrderBookMessage.Asks)。

每个订单都包括价格和数量。

## 使用

订单簿数据用于：

- 识别具有最大订单量的价格水平，这可能表明潜在的支撑或阻力水平。
- 评估交易品种的市场流动性。
- 根据对订单簿变化的分析来开发交易策略。

## 数据检索

在 StockSharp 中，订阅订单簿数据并接收更新是通过相应的 [API 方法](order_books/subscriptions.md) 完成的。
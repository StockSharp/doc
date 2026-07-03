# 交易编号

在处理订单时，主要标识符是 [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId)，而不是 [Order.Id](xref:StockSharp.BusinessEntities.Order.Id)。这样做是因为 [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) 由交易所生成。因此，在执行 [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** 方法后的一段时间里，[Order.Id](xref:StockSharp.BusinessEntities.Order.Id) 可能尚未初始化。因此，在发送交易后，交易程序会立即生成 [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId)。

[Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId) 由 [IdGenerator](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs) 类自动生成。这是一个抽象类，有两个标准实现：

- [IncrementalIdGenerator](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs#L28) - 默认安装。它会将数字增加1。初始值通过 [IncrementalIdGenerator.Current](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs#L42) 属性设置，默认情况下，该值等于自当天开始以来的毫秒数。
- [MillisecondIdGenerator](https://github.com/StockSharp/Ecng/blob/master/Common/IdGenerator.cs#L93)。它生成交易编号，该编号等于生成器创建时起的毫秒数。

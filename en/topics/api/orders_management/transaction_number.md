# Transaction number

When working with orders the main identifier is [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId), not [Order.Id](xref:StockSharp.BusinessEntities.Order.Id). This is done because the [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) generated by the exchange. Because of this, immediately after the execution the [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** method, for some time the [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) can not be initialized. Therefore, at once after the transaction sending, the trading program generates [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId). 

[Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId) automatically generated by the [IdGenerator](xref:Ecng.Common.IdGenerator) class. This is an abstract class that has two standard implementations: 

- [IncrementalIdGenerator](xref:Ecng.Common.IncrementalIdGenerator) \- installed by default. It increases the number by 1. The initial value set through the [IncrementalIdGenerator.Current](xref:Ecng.Common.IncrementalIdGenerator.Current), property, and by default, the value is equal to the number of milliseconds since the start of the day. 
- [MillisecondIdGenerator](xref:Ecng.Common.MillisecondIdGenerator). It generates the transaction number, which equal to the number of milliseconds from the time of generator creating. 
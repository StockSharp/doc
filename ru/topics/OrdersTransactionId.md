# Номер транзакции

При работе с заявками главным идентификатором является [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId), а не [Order.Id](xref:StockSharp.BusinessEntities.Order.Id). Это сделано по причине того, что [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) генерируется биржей. Из\-за этого сразу после выполнения метода [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder) еще какое\-то время может не быть инициализирован [Order.Id](xref:StockSharp.BusinessEntities.Order.Id). Поэтому сразу после отправки транзакции торговая программа генерирует [Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId). 

[Order.TransactionId](xref:StockSharp.BusinessEntities.Order.TransactionId) генерируется автоматически классом [IdGenerator](xref:Ecng.Common.IdGenerator). Это абстрактный класс, который стандартно имеет две реализации: 

- [IncrementalIdGenerator](xref:Ecng.Common.IncrementalIdGenerator) \- установлен по умолчанию. Увеличивает номер на 1. Первоначальное значение выставляется через свойство [IncrementalIdGenerator.Current](xref:Ecng.Common.IncrementalIdGenerator.Current), и по умолчанию равно количеству миллисекунд с начала дня. 
- [MillisecondIdGenerator](xref:Ecng.Common.MillisecondIdGenerator). Генерирует номер транзакции, равный количеству миллисекунд с времени создания генератора. 

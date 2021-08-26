# Номер транзакции

При работе с заявками главным идентификатором является [Order.TransactionId](../api/StockSharp.BusinessEntities.Order.TransactionId.html), а не [Order.Id](../api/StockSharp.BusinessEntities.Order.Id.html). Это сделано по причине того, что [Order.Id](../api/StockSharp.BusinessEntities.Order.Id.html) генерируется биржей. Из\-за этого сразу после выполнения метода [Connector.RegisterOrder](../api/StockSharp.Algo.Connector.RegisterOrder.html) еще какое\-то время может не быть инициализирован [Order.Id](../api/StockSharp.BusinessEntities.Order.Id.html). Поэтому сразу после отправки транзакции торговая программа генерирует [Order.TransactionId](../api/StockSharp.BusinessEntities.Order.TransactionId.html). 

[Order.TransactionId](../api/StockSharp.BusinessEntities.Order.TransactionId.html) генерируется автоматически классом [IdGenerator](../api/Ecng.Common.IdGenerator.html). Это абстрактный класс, который стандартно имеет две реализации: 

- [IncrementalIdGenerator](../api/Ecng.Common.IncrementalIdGenerator.html)

   \- установлен по умолчанию. Увеличивает номер на 1. Первоначальное значение выставляется через свойство 

  [IncrementalIdGenerator.Current](../api/Ecng.Common.IncrementalIdGenerator.Current.html)

  , и по умолчанию равно количеству миллисекунд с начала дня. 
- [MillisecondIdGenerator](../api/Ecng.Common.MillisecondIdGenerator.html)

  . Генерирует номер транзакции, равный количеству миллисекунд с времени создания генератора. 

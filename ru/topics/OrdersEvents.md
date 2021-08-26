# Получение информации по заявкам

Ниже перечислены события [IConnector](../api/StockSharp.BusinessEntities.IConnector.html) связанные с заявками:

- [NewOrder](../api/StockSharp.Algo.Connector.NewOrder.html)

   \- событие появления новой заявок. 
- [OrderCancelFailed](../api/StockSharp.Algo.Connector.OrderCancelFailed.html)

   \- событие ошибки отмены заявки. 
- [OrderChanged](../api/StockSharp.Algo.Connector.OrderChanged.html)

   \- событие изменения состояния заявки (снята, удовлетворена). 
- [OrderRegisterFailed](../api/StockSharp.Algo.Connector.OrderRegisterFailed.html)

   \- событие ошибки регистрации заявки. 
- [NewStopOrder](../api/StockSharp.Algo.Connector.NewStopOrder.html)

   \- событие появления новой стоп\-заявки. 
- [StopOrderCancelFailed](../api/StockSharp.Algo.Connector.StopOrderCancelFailed.html)

   \- событие ошибки отмены стоп\-заявки. 
- [StopOrderChanged](../api/StockSharp.Algo.Connector.StopOrderChanged.html)

   \- событие изменения состояния стоп\-заявки. 
- [StopOrderRegisterFailed](../api/StockSharp.Algo.Connector.StopOrderRegisterFailed.html)

   \- событие ошибки регистрации стоп\-заявки. 

Отправка транзакций (регистрация, замена или снятие заявок) идёт в асинхронном режиме. Асинхронный режим позволяет торговой программе не дожидаться подтверждения биржей принятия транзакции, продолжив дальше выполнять работу. Это сокращает время простоя программы, и увеличивает скорость реагирования на изменения ситуации на рынке. 

Чтобы узнать в программе, когда биржа присвоила заявке [Order.Id](../api/StockSharp.BusinessEntities.Order.Id.html), необходимо подписаться на событие [Connector.OrderChanged](../api/StockSharp.Algo.Connector.OrderChanged.html) (или для стоп\-заявок [Connector.StopOrderChanged](../api/StockSharp.Algo.Connector.StopOrderChanged.html)). Для определения неудачной регистрации используется событие [Connector.OrderRegisterFailed](../api/StockSharp.Algo.Connector.OrderRegisterFailed.html) (или для стоп\-заявок [Connector.StopOrderRegisterFailed](../api/StockSharp.Algo.Connector.StopOrderRegisterFailed.html)). 

> [!CAUTION]
> Если при старте приложения из шлюза были переданы ранее зарегистрированные заявки, то все они передаются через события [NewOrder](../api/StockSharp.BusinessEntities.ITransactionProvider.NewOrder.html), независимо от их состояния (кроме состояния [Failed](../api/StockSharp.Messages.OrderStates.Failed.html)). Это сделано потому, что событие [NewOrder](../api/StockSharp.Algo.Connector.NewOrder.html) отражает факт появления новых заявок в программе, а не событие успешной регистрации заявки. 

## См. также

[Номер транзакции](OrdersTransactionId.md)

# Получение информации по заявкам

Ниже перечислены события [IConnector](xref:StockSharp.BusinessEntities.IConnector) связанные с заявками:

- [Connector.NewOrder](xref:StockSharp.Algo.Connector.NewOrder) \- событие появления новой заявок. 
- [Connector.OrderCancelFailed](xref:StockSharp.Algo.Connector.OrderCancelFailed) \- событие ошибки отмены заявки. 
- [Connector.OrderChanged](xref:StockSharp.Algo.Connector.OrderChanged) \- событие изменения состояния заявки (снята, удовлетворена). 
- [Connector.OrderRegisterFailed](xref:StockSharp.Algo.Connector.OrderRegisterFailed) \- событие ошибки регистрации заявки. 
- [Connector.NewStopOrder](xref:StockSharp.Algo.Connector.NewStopOrder) \- событие появления новой стоп\-заявки. 
- [Connector.StopOrderCancelFailed](xref:StockSharp.Algo.Connector.StopOrderCancelFailed) \- событие ошибки отмены стоп\-заявки. 
- [Connector.StopOrderChanged](xref:StockSharp.Algo.Connector.StopOrderChanged) \- событие изменения состояния стоп\-заявки. 
- [Connector.StopOrderRegisterFailed](xref:StockSharp.Algo.Connector.StopOrderRegisterFailed) \- событие ошибки регистрации стоп\-заявки. 

Отправка транзакций (регистрация, замена или снятие заявок) идёт в асинхронном режиме. Асинхронный режим позволяет торговой программе не дожидаться подтверждения биржей принятия транзакции, продолжив дальше выполнять работу. Это сокращает время простоя программы, и увеличивает скорость реагирования на изменения ситуации на рынке. 

Чтобы узнать в программе, когда биржа присвоила заявке [Order.Id](xref:StockSharp.BusinessEntities.Order.Id), необходимо подписаться на событие [Connector.OrderChanged](xref:StockSharp.Algo.Connector.OrderChanged) (или для стоп\-заявок [Connector.StopOrderChanged](xref:StockSharp.Algo.Connector.StopOrderChanged)). Для определения неудачной регистрации используется событие [Connector.OrderRegisterFailed](xref:StockSharp.Algo.Connector.OrderRegisterFailed) (или для стоп\-заявок [Connector.StopOrderRegisterFailed](xref:StockSharp.Algo.Connector.StopOrderRegisterFailed)). 

> [!CAUTION]
> Если при старте приложения из шлюза были переданы ранее зарегистрированные заявки, то все они передаются через события [ITransactionProvider.NewOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.NewOrder), независимо от их состояния (кроме состояния [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed)). Это сделано потому, что событие [Connector.NewOrder](xref:StockSharp.Algo.Connector.NewOrder) отражает факт появления новых заявок в программе, а не событие успешной регистрации заявки. 

## См. также

[Номер транзакции](transaction_number.md)

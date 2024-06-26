# Состояния заявок

Заявка во время своей жизни проходит следующие состояния:

![OrderStates](../../../images/orderstates.png)

- [OrderStates.None](xref:StockSharp.Messages.OrderStates.None) \- заявка была создана в роботе и еще не была отправлена на регистрацию. 
- [OrderStates.Pending](xref:StockSharp.Messages.OrderStates.Pending) \- заявка была отправлена на регистрацию ([ITransactionProvider.RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)**) и для нее было вызвано событие [ITransactionProvider.NewOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.NewOrder). Для заявки ожидается подтверждение ее принятия от биржи. В случае успеха принятия будет вызвано событие [ITransactionProvider.OrderChanged](xref:StockSharp.BusinessEntities.ITransactionProvider.OrderChanged), и заявка будет переведена в состояние [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active). Также будут проинициализированы свойства [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) и [Order.Time](xref:StockSharp.BusinessEntities.Order.Time). В случае отвержения заявки будет вызвано событие [ITransactionProvider.OrderRegisterFailed](xref:StockSharp.BusinessEntities.ITransactionProvider.OrderRegisterFailed) с описанием ошибки, и заявка будет переведена в состояние [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed). 
- [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) \- заявка активна на бирже. Такая заявка будет активна до тех пор, пока не исполнится весь ее выставленный объем [Order.Volume](xref:StockSharp.BusinessEntities.Order.Volume), или она не будет снята принудительно через [ITransactionProvider.CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)**. Если заявка исполняется частично, то вызываются события [ITransactionProvider.NewMyTrade](xref:StockSharp.BusinessEntities.ITransactionProvider.NewMyTrade) о новых сделках по выставленной заявке, а так же событие [ITransactionProvider.OrderChanged](xref:StockSharp.BusinessEntities.ITransactionProvider.OrderChanged), где передается уведомление об изменении баланса по заявке [Order.Balance](xref:StockSharp.BusinessEntities.Order.Balance). Последнее событие будет выведено и в случае отмены заявки.
- [OrderStates.Done](xref:StockSharp.Messages.OrderStates.Done) \- заявка более не активна на бирже (была полностью исполнена или снята). 
- [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) \- заявка не была принята биржей (или промежуточной системой, как, например, серверная часть торговой платформы) по какой\-либо причине. 

Для того, чтобы узнать, в каком торговом состоянии находится заявка (какой объем реализован, была ли полностью удовлетворена заявка и т.д.) необходимо использовать методы [Extensions.IsCanceled](xref:StockSharp.Messages.Extensions.IsCanceled(StockSharp.Messages.IOrderMessage))**(**[StockSharp.Messages.IOrderMessage](xref:StockSharp.Messages.IOrderMessage) order **)**, [Extensions.IsMatchedEmpty](xref:StockSharp.Messages.Extensions.IsMatchedEmpty(StockSharp.Messages.IOrderMessage))**(**[StockSharp.Messages.IOrderMessage](xref:StockSharp.Messages.IOrderMessage) order **)**, [Extensions.IsMatchedPartially](xref:StockSharp.Messages.Extensions.IsMatchedPartially(StockSharp.Messages.IOrderMessage))**(**[StockSharp.Messages.IOrderMessage](xref:StockSharp.Messages.IOrderMessage) order **)**, [Extensions.IsMatched](xref:StockSharp.Messages.Extensions.IsMatched(StockSharp.Messages.IOrderMessage))**(**[StockSharp.Messages.IOrderMessage](xref:StockSharp.Messages.IOrderMessage) order **)** и [Extensions.GetMatchedVolume](xref:StockSharp.Messages.Extensions.GetMatchedVolume(StockSharp.Messages.IOrderMessage))**(**[StockSharp.Messages.IOrderMessage](xref:StockSharp.Messages.IOrderMessage) order **)**:

```cs
// любая заявка
Order order = ....
// отменена ли
Console.WriteLine(order.IsCanceled());
// исполнилась ли полностью
Console.WriteLine(order.IsMatched());
// исполнилась ли частично
Console.WriteLine(order.IsMatchedPartially());
// исполнилась ли хотя бы одна часть заявки 
Console.WriteLine(order.IsMatchedEmpty());
// получить реализованный объем
Console.WriteLine(order.GetMatchedVolume());
```

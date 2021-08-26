# Состояния заявок

Заявка во время своей жизни проходит следующие состояния:

![OrderStates](~/images/OrderStates.png)

- [None](../api/StockSharp.Messages.OrderStates.None.html)

   \- заявка была создана в роботе и еще не была отправлена на регистрацию. 
- [Pending](../api/StockSharp.Messages.OrderStates.Pending.html)

   \- заявка была отправлена на регистрацию (

  [ITransactionProvider.RegisterOrder](../api/StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder.html)

  ) и для нее было вызвано событие 

  [ITransactionProvider.NewOrder](../api/StockSharp.BusinessEntities.ITransactionProvider.NewOrder.html)

  . Для заявки ожидается подтверждение ее принятия от биржи. В случае успеха принятия будет вызвано событие 

  [ITransactionProvider.OrderChanged](../api/StockSharp.BusinessEntities.ITransactionProvider.OrderChanged.html)

  , и заявка будет переведена в состояние 

  [Active](../api/StockSharp.Messages.OrderStates.Active.html)

  . Также будут проинициализированы свойства 

  [Order.Id](../api/StockSharp.BusinessEntities.Order.Id.html)

   и 

  [Order.Time](../api/StockSharp.BusinessEntities.Order.Time.html)

  . В случае отвержения заявки будет вызвано событие 

  [ITransactionProvider.OrderRegisterFailed](../api/StockSharp.BusinessEntities.ITransactionProvider.OrderRegisterFailed.html)

   с описанием ошибки, и заявка будет переведена в состояние 

  [Failed](../api/StockSharp.Messages.OrderStates.Failed.html)

  . 
- [Active](../api/StockSharp.Messages.OrderStates.Active.html)

   \- заявка активна на бирже. Такая заявка будет активна до тех пор, пока не исполнится весь ее выставленный объем 

  [Order.Volume](../api/StockSharp.BusinessEntities.Order.Volume.html)

  , или она не будет снята принудительно через 

  [ITransactionProvider.CancelOrder](../api/StockSharp.BusinessEntities.ITransactionProvider.CancelOrder.html)

  . Если заявка исполняется частично, то вызываются события 

  [ITransactionProvider.NewMyTrade](../api/StockSharp.BusinessEntities.ITransactionProvider.NewMyTrade.html)

   о новых сделках по выставленной заявке, а так же событие 

  [ITransactionProvider.OrderChanged](../api/StockSharp.BusinessEntities.ITransactionProvider.OrderChanged.html)

  , где передается уведомление об изменении баланса по заявке 

  [Order.Balance](../api/StockSharp.BusinessEntities.Order.Balance.html)

  . Последнее событие будет выведено и в случае отмены заявки.
- [Done](../api/StockSharp.Messages.OrderStates.Done.html)

   \- заявка более не активна на бирже (была полностью исполнена или снята). 
- [Failed](../api/StockSharp.Messages.OrderStates.Failed.html)

   \- заявка не была принята биржей (или промежуточной системой, как, например, серверная часть торговой платформы) по какой\-либо причине. 

Для того, чтобы узнать, в каком торговом состоянии находится заявка (какой объем реализован, была ли полностью удовлетворена заявка и т.д.) необходимо использовать методы [IsCanceled](../api/StockSharp.Algo.TraderHelper.IsCanceled.html), [IsMatchedEmpty](../api/StockSharp.Algo.TraderHelper.IsMatchedEmpty.html), [IsMatchedPartially](../api/StockSharp.Algo.TraderHelper.IsMatchedPartially.html), [IsMatched](../api/StockSharp.Algo.TraderHelper.IsMatched.html) и [GetMatchedVolume](../api/StockSharp.Algo.TraderHelper.GetMatchedVolume.html):

```cs
\/\/ любая заявка
Order order \= ....
\/\/ отменена ли
Console.WriteLine(order.IsCanceled());
\/\/ исполнилась ли полностью
Console.WriteLine(order.IsMatched());
\/\/ исполнилась ли частично
Console.WriteLine(order.IsMatchedPartially());
\/\/ исполнилась ли хотя бы одна часть заявки 
Console.WriteLine(order.IsMatchedEmpty());
\/\/ получить реализованный объем
Console.WriteLine(order.GetMatchedVolume());
```

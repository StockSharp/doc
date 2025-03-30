# Ордер Лог

Полный журнал заявок – сервис биржи ММВБ\-РТС, который позволяет получать список всех торговых транзакций, принятых торговой системой в текущую торговую сессию с указанием текущего статуса заявок (поставлена\/удалена) и изменений параметров транзакции (частичных исполнений, передвижений заявки). Также в журнале отображается запись о сделке с указанием номера сведенной в данную сделку заявки.

Для того чтобы начать получать данные по ордер лог, надо подписаться на событие [OrderLogReceived](xref:StockSharp.Algo.ISubscriptionProvider.OrderLogReceived):

```cs
private Connector _connector;
...
_connector.OrderLogReceived += OnOrderLogReceived;
...
private void OnOrderLogReceived(Subscription subscription, IOrderLogMessage ol)
{
	Console.WriteLine(ol);                
}
		
```

Сопоставление данных ордер лога из документации «Шлюз ФОРТС Plaza\-2» и [S\#](../../../../api.md):

| Шлюз ФОРТС Plaza\-2 | StockSharp                                                                                                                                                                                                                                                                       |
| ------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| replID              | OrderLogItem.Order.TransactionId                                                                                                                                                                                                                                                 |
| replRev             | отсутствует                                                                                                                                                                                                                                                                      |
| replAct             | отсутствует                                                                                                                                                                                                                                                                      |
| id\_ord             | OrderLogItem.Order.Id                                                                                                                                                                                                                                                            |
| sess\_id            | отсутствует                                                                                                                                                                                                                                                                      |
| moment              | OrderLogItem.Order.Time                                                                                                                                                                                                                                                          |
| status              | OrderLogItem.Order.OrderStatus                                                                                                                                                                                                                                                   |
| action              | - 0 \- Заявка удалена: OrderLogItem.Trade \=\= null and OrderLogItem.Order.State \=\= OrderStates.Done
- 1 \- Заявка добавлена: OrderLogItem.Trade \=\= null and OrderLogItem.Order.State \!\= OrderStates.Done
- 2 \- Заявка сведена в сделку: OrderLogItem.Trade \!\= null
 |
| isin\_id            | OrderLogItem.Order.Security                                                                                                                                                                                                                                                      |
| dir                 | OrderLogItem.Order.Direction                                                                                                                                                                                                                                                     |
| price               | OrderLogItem.Order.Price                                                                                                                                                                                                                                                         |
| amount              | OrderLogItem.Order.Volume                                                                                                                                                                                                                                                        |
| amount\_rest        | OrderLogItem.Order.Balance                                                                                                                                                                                                                                                       |
| id\_deal            | OrderLogItem.Trade.Id                                                                                                                                                                                                                                                            |
| deal\_price         | OrderLogItem.Trade.Price                                                                                                                                                                                                                                                         |

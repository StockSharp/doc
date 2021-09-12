# Создать новую заявку

Для создания новой заявки необходимо создать объект [Order](xref:StockSharp.BusinessEntities.Order), который содержит информацию о заявке и зарегистрировать его на бирже. В дальнейшем, если требуется работа с заявкой (например, отменить ее или изменить), то необходимо использовать именно этот объект [Order](xref:StockSharp.BusinessEntities.Order). Для регистрации заявок на бирже предусмотрен метод [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order**)** который отправляет заявку на сервер.

В примере ниже показано создание заявки и регистрация ее на бирже:

```cs
	var order = new Order
    {
        // устанавливается тип заявки, в данном примере лимитный
        Type = OrderTypes.Limit,
        // устанавливается портфель для исполнения заявки
        Portfolio = Portfolio.SelectedPortfolio,
        // устанавливается объём заявки
        Volume = Volume.Text.To<decimal>(),
        // устанавливается цена заявки
        Price = Price.Text.To<decimal>(),
        // устанавливается инструмент
        Security = Security,
        // устанавливается направление заявки, в данном примере покупка
        Direction = Sides.Buy,
    };
	//Метод RegisterOrder отправляет заявку на сервер
  _connector.RegisterOrder(order);
    
```

## См. также

[Снятие заявок](OrdersCancel.md)

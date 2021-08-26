# Create new order

To create a new order, you should create an [Order](../api/StockSharp.BusinessEntities.Order.html), object that contains information about the order and register it on the exchange. Further, if you want to work with the order (for example, to cancel it or change it), then this [Order](../api/StockSharp.BusinessEntities.Order.html). object should be used. To register orders on the exchange, the [RegisterOrder](../api/StockSharp.Algo.Connector.RegisterOrder.html) method is provided which sends the order to the server.

The example below shows the creation of an order and its registration on the exchange:

```cs
	var order \= new Order
    {
        Type \= OrderTypes.Limit,
        Portfolio \= Portfolio.SelectedPortfolio,
        Volume \= Volume.Text.To\<decimal\>(),
        Price \= Price.Text.To\<decimal\>(),
        Security \= Security,
        Direction \= Sides.Buy,
    };
    \_connector.RegisterOrder(order);
    
```

## Recommended content

[Order cancel](OrdersCancel.md)

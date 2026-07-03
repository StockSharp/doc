# Crear nueva orden

Para crear una nueva orden, debe crear un objeto [Order](xref:StockSharp.BusinessEntities.Order) que contiene información sobre la orden y registrarlo en el exchange. Además, si desea trabajar con la orden (por ejemplo, cancelarla o cambiarla), debe usar este objeto [Order](xref:StockSharp.BusinessEntities.Order). Para registrar órdenes en el exchange, use el método [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)**, que envía la orden al servidor.

El siguiente ejemplo muestra la creación de una orden y su registro en el exchange:

```cs
	var order = new Order
	{
		Type = OrderTypes.Limit,
		Portfolio = Portfolio.SelectedPortfolio,
		Volume = Volume.Text.To<decimal>(),
		Price = Price.Text.To<decimal>(),
		Security = Security,
		Direction = Sides.Buy,
	};
	_connector.RegisterOrder(order);
	
```

## Contenido recomendado

[Cancelación de orden](order_cancel.md)

# Reemplazo de órdenes

El reemplazo de órdenes al crear algoritmos de negociación es un método más avanzado que cancelarlas y registrarlas de nuevo. Para reemplazar la orden debe llamar al método [Connector.ReRegisterOrder](xref:StockSharp.Algo.Connector.ReRegisterOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) oldOrder, [StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) newOrder **)**.

Como resultado del reemplazo de una orden se crea un nuevo objeto [Order](xref:StockSharp.BusinessEntities.Order), que contiene la información de la orden antigua más la parte modificada. Posteriormente, si desea trabajar con la orden modificada (por ejemplo, cancelarla o cambiarla de nuevo), debe usar este nuevo objeto [Order](xref:StockSharp.BusinessEntities.Order).

El siguiente ejemplo muestra cómo "mover" la orden al mejor precio:

```cs
if (registeredOrder.Security.BestBid != null && registeredOrder.Security.BestAsk != null)
{
	// registeredOrder - orden registrada correctamente.
	var newOrder = registeredOrder.Clone();
	// cambiar el precio para que sea el mejor del libro de órdenes
	newOrder.Price = (registeredOrder.Direction == Sides.Buy ? registeredOrder.Security.BestBid : registeredOrder.Security.BestAsk).Price;
	// enviar solicitud para reemplazar nuestra orden con el nuevo precio
	_connector.ReRegisterOrder(registeredOrder, newOrder);
}
```

## Contenido recomendado

[Cancelación de órdenes](order_cancel.md)

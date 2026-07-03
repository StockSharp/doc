# Crear nueva orden stop

Para crear una nueva orden stop, necesita crear un objeto [Order](xref:StockSharp.BusinessEntities.Order) que contiene información sobre la orden y registrarlo en el exchange.

A diferencia de una orden normal, para una orden stop debe especificar la propiedad [Order.Type](xref:StockSharp.BusinessEntities.Order.Type) como [OrderTypes.Conditional](xref:StockSharp.Messages.OrderTypes.Conditional) y establecer la propiedad [Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition) con las condiciones de orden necesarias.

Además, si necesita trabajar con la orden (por ejemplo, cancelarla o cambiarla), debe usar este objeto [Order](xref:StockSharp.BusinessEntities.Order). Para registrar órdenes en el exchange, se proporciona el método [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)**, que envía una orden al servidor.

```cs
Connector Connector = new Connector();		
...   
private void StopOrder_Click(object sender, RoutedEventArgs e)
{
	var order = new Order
	{
		Security = SecurityEditor.SelectedSecurity,
		Portfolio = PortfolioEditor.SelectedPortfolio,
		Price = decimal.Parse(TextBoxPrice.Text),
		Volume = decimal.Parse(TextBoxVolumePrice.Text),
		Direction = Sides.Buy,
		Type = OrderTypes.Conditional,
		Condition = new FixOrderCondition()
		{
			Type = FixOrderConditionTypes.StopLimit,
			StopLimitPrice = decimal.Parse(TextBoxStopLimitPrice.Text),
		}
	};
	Connector.RegisterOrder(order);
}
...
							
```

Cada conexión tiene su propia implementación de la clase [OrderCondition](xref:StockSharp.Messages.OrderCondition), ya que cada conexión tiene sus propias características únicas. Por ejemplo, para [Kucoin](../connectors/crypto_exchanges/kucoin.md) es [KucoinOrderCondition](xref:StockSharp.Kucoin.KucoinOrderCondition), etc. 

## Contenido recomendado

[Estados de órdenes](orders_states.md)

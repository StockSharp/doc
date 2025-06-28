# Create new stop order

To create a new stop order, you need to create an [Order](xref:StockSharp.BusinessEntities.Order), object that contains information about the order and register it on the exchange.

Unlike a regular order, for a stop order, you need to specify the [Order.Type](xref:StockSharp.BusinessEntities.Order.Type) property as [OrderTypes.Conditional](xref:StockSharp.Messages.OrderTypes.Conditional) and set the [Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition) property with the necessary order conditions.

Further, if you need to work with the order (for example, cancel it or change it), then this [Order](xref:StockSharp.BusinessEntities.Order). object shall be used. To register orders on the exchange, the [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** method is provided, which sends an order to the server.

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

Each connection has its own implementation of the [OrderCondition](xref:StockSharp.Messages.OrderCondition) class, since each connection has its own unique features. For example, for [Kucoin](../connectors/crypto_exchanges/kucoin.md) it is [KucoinOrderCondition](xref:StockSharp.Kucoin.KucoinOrderCondition), etc. 

## Recommended content

[Orders states](orders_states.md)

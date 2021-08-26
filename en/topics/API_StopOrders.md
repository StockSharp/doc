# Create new stop order

To create a new stop order, you need to create an [Order](../api/StockSharp.BusinessEntities.Order.html), object that contains information about the order and register it on the exchange.

Unlike a regular order, for a stop order, you need to specify the [Order.Type](../api/StockSharp.BusinessEntities.Order.Type.html) property as [Conditional](../api/StockSharp.Messages.OrderTypes.Conditional.html) and set the [Order.Condition](../api/StockSharp.BusinessEntities.Order.Condition.html) property with the necessary order conditions.

Further, if you need to work with the order (for example, cancel it or change it), then this [Order](../api/StockSharp.BusinessEntities.Order.html). object shall be used. To register orders on the exchange, the [RegisterOrder](../api/StockSharp.Algo.Connector.RegisterOrder.html) method is provided, which sends an order to the server.

```cs
Connector Connector \= new Connector();		
...   
private void StopOrder\_Click(object sender, RoutedEventArgs e)
{
	var order \= new Order
	{
		Security \= SecurityEditor.SelectedSecurity,
		Portfolio \= PortfolioEditor.SelectedPortfolio,
		Price \= decimal.Parse(TextBoxPrice.Text),
		Volume \= decimal.Parse(TextBoxVolumePrice.Text),
		Direction \= Sides.Buy,
        Type \= OrderTypes.Conditional,
        Condition \= new FixOrderCondition()
        {
            Type \= FixOrderConditionTypes.StopLimit,
            StopLimitPrice \= decimal.Parse(TextBoxStopLimitPrice.Text),
        }
	};
	Connector.RegisterOrder(order);
}
...
							
```

Each connection has its own implementation of the [OrderCondition](../api/StockSharp.Messages.OrderCondition.html) class, since each connection has its own unique features. For example, for [Kucoin](Kucoin.md) it is [KucoinOrderCondition](../api/StockSharp.Kucoin.KucoinOrderCondition.html), etc. 

## Recommended content

[Getting orders information](OrdersEvents.md)

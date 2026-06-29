# 创建新的止损单

要创建一个新的止损订单，您需要创建一个包含订单信息的[Order](xref:StockSharp.BusinessEntities.Order)对象，并在交易所注册它。

与普通订单不同，对于止损订单，您需要将[Order.Type](xref:StockSharp.BusinessEntities.Order.Type)属性指定为[OrderTypes.Conditional](xref:StockSharp.Messages.OrderTypes.Conditional)，并设置[Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition)属性以包含必要的订单条件。

此外，如果您需要处理该订单（例如，取消或更改它），则应使用此 [Order](xref:StockSharp.BusinessEntities.Order) 对象。为了在交易所注册订单，提供了 [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order)**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) 订单 **)** 方法，该方法将订单发送到服务器。

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

每个连接都有其自身对 [OrderCondition](xref:StockSharp.Messages.OrderCondition) 类的实现，因为每个连接都有其独特的特性。例如，对于 [Kucoin](../connectors/crypto_exchanges/kucoin.md) 来说，它是 [KucoinOrderCondition](xref:StockSharp.Kucoin.KucoinOrderCondition) 等。

## 推荐内容

[订单状态](orders_states.md)

# Criar nova ordem stop

Para criar uma nova ordem stop, é necessário criar um objeto [Order](xref:StockSharp.BusinessEntities.Order) que contenha informação sobre a ordem e registá-lo na bolsa.

Ao contrário de uma ordem normal, numa ordem stop é necessário especificar a propriedade [Order.Type](xref:StockSharp.BusinessEntities.Order.Type) como [OrderTypes.Conditional](xref:StockSharp.Messages.OrderTypes.Conditional) e definir a propriedade [Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition) com as condições de ordem necessárias.

Depois, se precisar de trabalhar com a ordem (por exemplo, cancelá-la ou alterá-la), este objeto [Order](xref:StockSharp.BusinessEntities.Order) deve ser usado. Para registar ordens na bolsa, é fornecido o método [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)**, que envia uma ordem para o servidor.

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

Cada ligação tem a sua própria implementação da classe [OrderCondition](xref:StockSharp.Messages.OrderCondition), uma vez que cada ligação tem as suas próprias funcionalidades específicas. Por exemplo, para [Kucoin](../connectors/crypto_exchanges/kucoin.md), é [KucoinOrderCondition](xref:StockSharp.Kucoin.KucoinOrderCondition), etc.

## Conteúdo recomendado

[Estados das ordens](orders_states.md)

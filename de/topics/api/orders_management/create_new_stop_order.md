# Neue Stop-Order erstellen

Um eine neue Stop-Order zu erstellen, müssen Sie ein [Order](xref:StockSharp.BusinessEntities.Order)-Objekt erstellen, das Informationen über die Order enthält, und es an der Börse registrieren.

Anders als bei einer regulären Order müssen Sie für eine Stop-Order die Eigenschaft [Order.Type](xref:StockSharp.BusinessEntities.Order.Type) auf [OrderTypes.Conditional](xref:StockSharp.Messages.OrderTypes.Conditional) setzen und die Eigenschaft [Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition) mit den erforderlichen Orderbedingungen festlegen.

Wenn Sie später mit der Order arbeiten müssen (zum Beispiel sie stornieren oder ändern), sollte dieses [Order](xref:StockSharp.BusinessEntities.Order)-Objekt verwendet werden. Zur Registrierung von Orders an der Börse steht die Methode [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** bereit, die eine Order an den Server sendet.

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

Jede Verbindung hat ihre eigene Implementierung der Klasse [OrderCondition](xref:StockSharp.Messages.OrderCondition), da jede Verbindung eigene Besonderheiten besitzt. Für [Kucoin](../connectors/crypto_exchanges/kucoin.md) ist dies beispielsweise [KucoinOrderCondition](xref:StockSharp.Kucoin.KucoinOrderCondition) usw.

## Empfohlene Inhalte

[Orderzustände](orders_states.md)


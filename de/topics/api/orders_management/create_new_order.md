# Neue Order erstellen

Um eine neue Order zu erstellen, sollten Sie ein [Order](xref:StockSharp.BusinessEntities.Order)-Objekt erstellen, das Informationen über die Order enthält, und es an der Börse registrieren. Wenn Sie später mit der Order arbeiten möchten (zum Beispiel sie stornieren oder ändern), sollte dieses [Order](xref:StockSharp.BusinessEntities.Order)-Objekt verwendet werden. Zur Registrierung von Orders an der Börse verwenden Sie die Methode [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)**, die die Order an den Server sendet.

Das folgende Beispiel zeigt die Erstellung einer Order und ihre Registrierung an der Börse:

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

## Empfohlene Inhalte

[Order cancel](order_cancel.md)


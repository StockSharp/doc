# Orders ersetzen

Das Ersetzen von Orders beim Erstellen von Handelsalgorithmen ist eine fortgeschrittenere Methode als das Stornieren und erneute Registrieren. Um eine Order zu ersetzen, rufen Sie die Methode [Connector.ReRegisterOrder](xref:StockSharp.Algo.Connector.ReRegisterOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) oldOrder, [StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) newOrder **)** auf.

Als Ergebnis der Orderersetzung wird ein neues [Order](xref:StockSharp.BusinessEntities.Order)-Objekt erstellt, das die Informationen der alten Order plus den geänderten Teil enthält. Wenn Sie anschließend mit der geänderten Order arbeiten möchten (zum Beispiel sie stornieren oder erneut ändern), müssen Sie dieses neue [Order](xref:StockSharp.BusinessEntities.Order)-Objekt verwenden.

Das folgende Beispiel zeigt, wie die Order auf den besten Preis "verschoben" wird:

```cs
if (registeredOrder.Security.BestBid != null && registeredOrder.Security.BestAsk != null)
{
	// registeredOrder - erfolgreich registrierte Order.
	var newOrder = registeredOrder.Clone();
	// Preis auf den besten Wert im Orderbuch ändern
	newOrder.Price = (registeredOrder.Direction == Sides.Buy ? registeredOrder.Security.BestBid : registeredOrder.Security.BestAsk).Price;
	// Anfrage senden, um unsere Order durch eine Order mit neuem Preis zu ersetzen
	_connector.ReRegisterOrder(registeredOrder, newOrder);
}
```

## Empfohlene Inhalte

[Orders cancel](order_cancel.md)


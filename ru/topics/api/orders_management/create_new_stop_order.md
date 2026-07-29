# Создать новую стоп заявку

Для создания новой стоп заявки необходимо создать объект [Order](xref:StockSharp.BusinessEntities.Order), который содержит информацию о заявке и зарегистрировать его на бирже.

В отличие от обычной заявки для стоп заявки необходимо указать свойство [Order.Type](xref:StockSharp.BusinessEntities.Order.Type) как [OrderTypes.Conditional](xref:StockSharp.Messages.OrderTypes.Conditional) и задать свойство [Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition) с необходимыми условиями заявки.

В дальнейшем, если требуется работа с заявкой (например, отменить ее или изменить), то необходимо использовать именно этот объект [Order](xref:StockSharp.BusinessEntities.Order). Для регистрации заявок на бирже предусмотрен метод [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** который отправляет заявку на сервер.

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
		Side = Sides.Buy,
		Type = OrderTypes.Conditional,
		Condition = new FixOrderCondition
		{
			Type = FixStopOrderTypes.StopLoss,
			StopLoss = decimal.Parse(TextBoxStopLossPrice.Text),
		}
	};
	Connector.RegisterOrder(order);
}
...
							
```

Для каждого подключения используется собственная реализация класса [OrderCondition](xref:StockSharp.Messages.OrderCondition), учитывающая особенности конкретного протокола или биржи. В этом примере для FIX-подключения применяется [FixOrderCondition](xref:StockSharp.Fix.FixOrderCondition), а тип стоп-заявки задаётся значением `FixStopOrderTypes.StopLoss`.

## См. также

[Состояния заявок](orders_states.md)

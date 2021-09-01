# Создать новую стоп заявку

Для создания новой стоп заявки необходимо создать объект [Order](xref:StockSharp.BusinessEntities.Order), который содержит информацию о заявке и зарегистрировать его на бирже.

В отличии от обычной заявки для стоп заявки необходимо указать свойство [Order.Type](xref:StockSharp.BusinessEntities.Order.Type) как [Conditional](xref:StockSharp.Messages.OrderTypes.Conditional) и задать свойство [Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition) с необходимыми условиями заявки.

В дальнейшем, если требуется работа с заявкой (например, отменить ее или изменить), то необходимо использовать именно этот объект [Order](xref:StockSharp.BusinessEntities.Order). Для регистрации заявок на бирже предусмотрен метод [RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order)) который отправляет заявку на сервер.

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
        Condition = new QuikOrderCondition()
        {
            Type = QuikOrderConditionTypes.StopLimit,
            StopLimitPrice = decimal.Parse(TextBoxStopLimitPrice.Text),
        }
	};
	Connector.RegisterOrder(order);
}
...
							
```

Для каждого подключения есть собственная реализация класса [OrderCondition](xref:StockSharp.Messages.OrderCondition) так как каждое подключение имеет свои уникальные особенности. Например, для [QUIK](Quik.md) это [QuikOrderCondition](xref:StockSharp.Quik.QuikOrderCondition) , для [KuCoin](Kucoin.md) это [KucoinOrderCondition](xref:StockSharp.Kucoin.KucoinOrderCondition) и т. д. 

## См. также

[Получение информации по заявкам](OrdersEvents.md)

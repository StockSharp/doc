# Criar nova ordem

Para criar uma nova ordem, deve criar um objeto [Order](xref:StockSharp.BusinessEntities.Order) que contenha informação sobre a ordem e registá-lo na bolsa. Depois, se pretender trabalhar com a ordem (por exemplo, cancelá-la ou alterá-la), deve usar este objeto [Order](xref:StockSharp.BusinessEntities.Order). Para registar ordens na bolsa, use o método [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)**, que envia a ordem para o servidor.

O exemplo abaixo mostra a criação de uma ordem e o seu registo na bolsa:

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

## Conteúdo recomendado

[Cancelamento de ordem](order_cancel.md)

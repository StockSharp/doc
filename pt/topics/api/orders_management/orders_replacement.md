# Substituição de ordens

Substituir ordens ao criar algoritmos de negociação é um método mais avançado do que cancelá-las e registá-las novamente. Para substituir a ordem, deve chamar o método [Connector.ReRegisterOrder](xref:StockSharp.Algo.Connector.ReRegisterOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) oldOrder, [StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) newOrder **)**.

Como resultado da substituição da ordem, é criado um novo objeto [Order](xref:StockSharp.BusinessEntities.Order), que contém a informação da ordem antiga mais a parte alterada. Posteriormente, se pretender trabalhar com a ordem alterada (por exemplo, cancelá-la ou alterá-la novamente), deve usar este novo objeto [Order](xref:StockSharp.BusinessEntities.Order).

O exemplo seguinte mostra como "mover" a ordem para o melhor preço:

```cs
if (registeredOrder.Security.BestBid != null && registeredOrder.Security.BestAsk != null)
{
	// registeredOrder — ordem registrada com sucesso.
	var newOrder = registeredOrder.Clone();
	// alterar o preço para ser o melhor no livro de ofertas
	newOrder.Price = (registeredOrder.Direction == Sides.Buy ? registeredOrder.Security.BestBid : registeredOrder.Security.BestAsk).Price;
	// enviar solicitação para substituir nossa ordem pelo novo preço
	_connector.ReRegisterOrder(registeredOrder, newOrder);
}
```

## Conteúdo recomendado

[Cancelamento de ordens](order_cancel.md)

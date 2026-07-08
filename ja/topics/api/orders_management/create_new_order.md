# 新しい注文の作成

新しい注文を作成するには、注文に関する情報を含む [Order](xref:StockSharp.BusinessEntities.Order) オブジェクトを作成し、それを取引所に登録する必要があります。その後、その注文を操作したい場合 (たとえば、キャンセルまたは変更する場合) は、この [Order](xref:StockSharp.BusinessEntities.Order) オブジェクトを使用する必要があります。取引所に注文を登録するには、注文をサーバーへ送信する [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** メソッドを使用します。

次の例は、注文の作成と取引所への登録を示しています。

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

## 推奨コンテンツ

[注文のキャンセル](order_cancel.md)


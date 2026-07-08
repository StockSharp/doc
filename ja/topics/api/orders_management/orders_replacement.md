# 注文の差し替え

取引アルゴリズムを作成する際、注文の差し替えは、注文をキャンセルして再度登録するよりも高度な方法です。注文を差し替えるには、[Connector.ReRegisterOrder](xref:StockSharp.Algo.Connector.ReRegisterOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) oldOrder, [StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) newOrder **)** メソッドを呼び出す必要があります。

注文の差し替えの結果として、古い注文情報に変更された部分を加えた新しい [Order](xref:StockSharp.BusinessEntities.Order) オブジェクトが作成されます。その後、変更後の注文を操作したい場合 (たとえば、キャンセルまたは再度変更する場合) は、この新しい [Order](xref:StockSharp.BusinessEntities.Order) オブジェクトを使用する必要があります。

次の例は、最良価格へ注文を「移動」する方法を示しています。

```cs
if (registeredOrder.Security.BestBid != null && registeredOrder.Security.BestAsk != null)
{
	// registeredOrder - 正常に登録された注文。
	var newOrder = registeredOrder.Clone();
	// 価格を板上の最良価格に変更します
	newOrder.Price = (registeredOrder.Direction == Sides.Buy ? registeredOrder.Security.BestBid : registeredOrder.Security.BestAsk).Price;
	// 注文を新しい価格に差し替えるリクエストを送信します
	_connector.ReRegisterOrder(registeredOrder, newOrder);
}
```

## 推奨コンテンツ

[注文のキャンセル](order_cancel.md)

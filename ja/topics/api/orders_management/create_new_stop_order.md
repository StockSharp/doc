# 新しいストップ注文の作成

新しいストップ注文を作成するには、注文に関する情報を含む [Order](xref:StockSharp.BusinessEntities.Order) オブジェクトを作成し、それを取引所に登録する必要があります。

通常の注文とは異なり、ストップ注文では [Order.Type](xref:StockSharp.BusinessEntities.Order.Type) プロパティに [OrderTypes.Conditional](xref:StockSharp.Messages.OrderTypes.Conditional) を指定し、必要な注文条件を設定した [Order.Condition](xref:StockSharp.BusinessEntities.Order.Condition) プロパティを設定する必要があります。

その後、その注文を操作する必要がある場合 (たとえば、キャンセルまたは変更する場合) は、この [Order](xref:StockSharp.BusinessEntities.Order) オブジェクトを使用します。取引所に注文を登録するために、注文をサーバーへ送信する [Connector.RegisterOrder](xref:StockSharp.Algo.Connector.RegisterOrder(StockSharp.BusinessEntities.Order))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order **)** メソッドが用意されています。

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

各接続には、それぞれ固有の機能があるため、[OrderCondition](xref:StockSharp.Messages.OrderCondition) クラスの独自の実装があります。たとえば、[Kucoin](../connectors/crypto_exchanges/kucoin.md) では [KucoinOrderCondition](xref:StockSharp.Kucoin.KucoinOrderCondition) などです。

## 推奨コンテンツ

[注文状態](orders_states.md)


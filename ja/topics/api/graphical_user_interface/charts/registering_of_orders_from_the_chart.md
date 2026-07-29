# チャートからの注文登録

S# ではチャートから注文を登録できます。この機能を有効にするには、[Chart.OrderCreationMode](xref:StockSharp.Xaml.Charting.Chart.OrderCreationMode) プロパティを **"True"** に設定する必要があります。既定では無効になっています。

![API GUI チャートからの取引](../../../../images/api_gui_trading_from_chart.png)

買い注文は、**Ctrl + 左マウスボタン** の組み合わせで登録されます。

売り注文は、**Ctrl + 右マウスボタン** の組み合わせで登録されます。

生成された注文は、`CreateOrder` イベントで受け取れます。

```cs
ChartPanel.CreateOrder += (chartArea,order) =>
{
	order.Portfolio = _portfolio;
	order.Security = _security;
	order.Volume = 1;
	
	_connector.RegisterOrder(order);
};
```

登録された注文は、注文を表示するための特殊な要素 [ChartActiveOrdersElement](xref:StockSharp.Xaml.Charting.ChartActiveOrdersElement) として表示されます。

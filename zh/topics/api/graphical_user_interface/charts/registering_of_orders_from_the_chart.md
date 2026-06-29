# 从图表注册订单

S# 允许从图表注册订单，要激活此功能，您需要将 [Chart.OrderCreationMode](xref:StockSharp.Xaml.Charting.Chart.OrderCreationMode) 属性设置为 **"True"**，默认情况下该功能是禁用的。

![从图表使用 API GUI 交易](../../../../images/api_gui_trading_from_chart.png)

购买订单将使用 **Ctrl + 左键单击** 组合来注册。

出售订单将使用 **Ctrl + 右键单击** 组合来注册。

生成的订单可以通过新订单创建事件进行拦截。

```cs
ChartPanel.CreateOrder += (chartArea,order) =>
{
	order.Portfolio = _portfolio;
	order.Security = _security;
	order.Volume = 1;
	
	_connector.RegisterOrder(order);
};
```

已注册的订单将作为用于显示订单的特殊元素 [ChartActiveOrdersElement](xref:StockSharp.Xaml.Charting.ChartActiveOrdersElement) 显示。

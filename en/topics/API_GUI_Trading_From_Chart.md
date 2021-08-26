# Registering of orders from the chart

S\# allows to register orders from the chart, to activate this feature, you need to set the [OrderCreationMode](../api/StockSharp.Xaml.Charting.Chart.OrderCreationMode.html) property to **"True"**, it is disabled by default.

![API GUI Trading from chart](~/images/API_GUI_Trading_from_chart.png)

Orders for purchase will be registered using the **Ctrl + Left Mouse Button** combination.

Orders for sale will be registered using the **Ctrl + Right Mouse Button** combination.

The resulting order can be intercepted through the event of a new order creation.

```cs
ChartPanel.CreateOrder +\= (chartArea,order) \=\>
{
	order.Portfolio \= \_portfolio;
	order.Security \= \_security;
	order.Volume \= 1;
	
	\_connector.RegisterOrder(order);
};
```

Registered orders will be displayed as a special element for displaying orders [ChartActiveOrdersElement](../api/StockSharp.Xaml.Charting.ChartActiveOrdersElement.html).

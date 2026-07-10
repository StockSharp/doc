# Registro de órdenes desde el gráfico

S# permite registrar órdenes desde el gráfico. Para activar esta función, debe establecer la propiedad [Chart.OrderCreationMode](xref:StockSharp.Xaml.Charting.Chart.OrderCreationMode) en **"True"**; está deshabilitada de forma predeterminada.

![Trading desde el gráfico en API GUI](../../../../images/api_gui_trading_from_chart.png)

Las órdenes de compra se registrarán usando la combinación **Ctrl + botón izquierdo del ratón**.

Las órdenes de venta se registrarán usando la combinación **Ctrl + botón derecho del ratón**.

La orden resultante se puede interceptar mediante el evento de creación de una nueva orden.

```cs
ChartPanel.CreateOrder += (chartArea,order) =>
{
	order.Portfolio = _portfolio;
	order.Security = _security;
	order.Volume = 1;
	
	_connector.RegisterOrder(order);
};
```

Las órdenes registradas se mostrarán como un elemento especial para mostrar órdenes [ChartActiveOrdersElement](xref:StockSharp.Xaml.Charting.ChartActiveOrdersElement).

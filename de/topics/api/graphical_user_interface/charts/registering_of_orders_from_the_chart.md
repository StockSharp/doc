# Registrierung von Orders aus dem Diagramm

S# ermöglicht das Registrieren von Orders aus dem Diagramm. Um diese Funktion zu aktivieren, müssen Sie die Eigenschaft [Chart.OrderCreationMode](xref:StockSharp.Xaml.Charting.Chart.OrderCreationMode) auf **"True"** setzen; standardmäßig ist sie deaktiviert.

![API GUI Trading from chart](../../../../images/api_gui_trading_from_chart.png)

Kauforders werden mit der Tastenkombination **Ctrl + Left Mouse Button** registriert.

Verkaufsorders werden mit der Tastenkombination **Ctrl + Right Mouse Button** registriert.

Die resultierende Order kann über das Ereignis zum Erstellen einer neuen Order abgefangen werden.

```cs
ChartPanel.CreateOrder += (chartArea,order) =>
{
	order.Portfolio = _portfolio;
	order.Security = _security;
	order.Volume = 1;
	
	_connector.RegisterOrder(order);
};
```

Registrierte Orders werden als spezielles Element zur Anzeige von Orders dargestellt: [ChartActiveOrdersElement](xref:StockSharp.Xaml.Charting.ChartActiveOrdersElement).

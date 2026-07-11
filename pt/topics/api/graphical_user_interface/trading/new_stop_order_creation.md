# Criação de nova ordem stop

[OrderConditionalWindow](xref:StockSharp.Xaml.OrderConditionalWindow) - a janela para criar uma ordem condicional.

![Captura de tela de Criação de nova ordem stop](../../../../images/gui_orderconditionalwindow.png)

**Propriedades principais**

- [OrderConditionalWindow.Portfolios](xref:StockSharp.Xaml.OrderConditionalWindow.Portfolios) - a lista de portfólios.
- [OrderConditionalWindow.SecurityProvider](xref:StockSharp.Xaml.OrderConditionalWindow.SecurityProvider) - fornecedor de informações sobre instrumentos.
- [OrderConditionalWindow.MarketDataProvider](xref:StockSharp.Xaml.OrderConditionalWindow.MarketDataProvider) - fornecedor de dados de mercado.
- [OrderConditionalWindow.Adapter](xref:StockSharp.Xaml.OrderConditionalWindow.Adapter) - adaptador de mensagens.
- [OrderConditionalWindow.Order](xref:StockSharp.Xaml.OrderConditionalWindow.Order) - a ordem criada.

Abaixo está um fragmento de código que mostra a sua utilização. O exemplo de código foi retirado de *Samples\/InteractiveBrokers\/SampleIB*.

```cs
...
private readonly Connector _connector = new Connector();
...
private void NewStopOrderClick(object sender, RoutedEventArgs e)
{
	var wnd = new OrderConditionalWindow
	{
		Order = new Order
		{
			Security = SecurityPicker.SelectedSecurity,
			Type = OrderTypes.Conditional,
			ExpiryDate = DateTime.Today
		},
		SecurityProvider = _connector,
		MarketDataProvider = _connector,
		Portfolios = new PortfolioDataSource(_connector),
		Adapter = _connector.Adapter
	};
	if (wnd.ShowModal(this))
		_connector.RegisterOrder(wnd.Order);
}
						
	  				
```

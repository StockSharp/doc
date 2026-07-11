# Janela de definições de comissão

[CommissionWindow](xref:StockSharp.Xaml.CommissionWindow) - Uma janela especial para definir as regras de cobrança de comissão.

![Captura de tela de Janela de definições de comissão](../../../images/api_comissionwindow.png)

Abaixo está um exemplo do código para chamar uma janela para definir as regras de cobrança de comissão.

```cs
		private void RiskButton_OnClick(object sender, RoutedEventArgs e)
		{
			var wnd = new CommissionWindow();
			wnd.Rules.AddRange(Strategy.RiskManager.Rules.Select(r => r.Clone()));
			if (!wnd.ShowModal(this))
				return;
			Strategy.RiskManager.Rules.Clear();
			Strategy.RiskManager.Rules.AddRange(wnd.Rules);
		}
	  				
```

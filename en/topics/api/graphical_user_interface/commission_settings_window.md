# Commission settings window

[CommissionWindow](xref:StockSharp.Xaml.CommissionWindow) - A special window for setting the rules for charging a commission.

![API ComissionWindow](../../../images/api_comissionwindow.png)

Below is an example of the code to call a window for setting the rules for charging a commission.

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

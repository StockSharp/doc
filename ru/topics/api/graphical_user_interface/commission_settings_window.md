# Окно настройки комиссии

[CommissionWindow](xref:StockSharp.Xaml.CommissionWindow) \- Специальное окно для настройки правил взимания комиссии. 

![API ComissionWindow](../../../images/api_comissionwindow.png)

Ниже приведен пример кода вызова окна настройки правил взимания комиссии. 

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

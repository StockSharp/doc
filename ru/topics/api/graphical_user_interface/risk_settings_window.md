# Окно настройки рисков

[AlertSettingsWindow](xref:StockSharp.Alerts.AlertSettingsWindow) \- Специальное окно для настройки контроля рисков. 

![API GUI RiskWindow](../../../images/api_gui_riskwindow.png)

Ниже приведен пример кода вызова окна настройки контроля рисков для стратегии. 

```cs
		private void RiskButton_OnClick(object sender, RoutedEventArgs e)
		{
			var wnd = new RiskWindow();
			wnd.Rules.AddRange(Strategy.RiskManager.Rules.Select(r => r.Clone()));
			if (!wnd.ShowModal(this))
				return;
			Strategy.RiskManager.Rules.Clear();
			Strategy.RiskManager.Rules.AddRange(wnd.Rules);
		}
	  				
```

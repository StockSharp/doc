# Окно настройки рисков

[AlertSettingsWindow](../api/StockSharp.Alerts.AlertSettingsWindow.html) \- Специальное окно для настройки контроля рисков. 

![API GUI RiskWindow](../images/API_GUI_RiskWindow.png)

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

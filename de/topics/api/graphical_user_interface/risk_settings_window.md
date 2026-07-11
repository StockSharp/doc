# Fenster für Risikoeinstellungen

[AlertSettingsWindow](xref:StockSharp.Alerts.AlertSettingsWindow) - ein spezielles Fenster zum Konfigurieren der Risikokontrolle.

![Fenster für Risikoeinstellungen Bildschirmfoto](../../../images/api_gui_riskwindow.png)

Nachfolgend sehen Sie ein Codebeispiel zum Aufrufen des Fensters für Risikokontrolleinstellungen der Strategie.

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

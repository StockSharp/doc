# Ventana de configuración de riesgo

[AlertSettingsWindow](xref:StockSharp.Alerts.AlertSettingsWindow) - Ventana especial para configurar el control de riesgos. 

![Captura de Ventana de configuración de riesgo](../../../images/api_gui_riskwindow.png)

A continuación se muestra un ejemplo del código para llamar a la ventana de configuración de control de riesgos para la estrategia. 

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


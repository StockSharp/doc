# Adapterinitialisierung BarChart

Der folgende Code zeigt, wie der [BarChartMessageAdapter](xref:StockSharp.BarChart.BarChartMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) uebergeben wird.

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new BarChartMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

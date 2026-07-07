# Inicialização do adaptador BarChart

O código abaixo demonstra como inicializar o [BarChartMessageAdapter](xref:StockSharp.BarChart.BarChartMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

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

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

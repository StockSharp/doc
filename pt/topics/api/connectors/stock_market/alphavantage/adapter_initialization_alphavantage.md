# Inicialização do adaptador AlphaVantage

O código abaixo demonstra como inicializar o [AlphaVantageMessageAdapter](xref:StockSharp.AlphaVantage.AlphaVantageMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new AlphaVantageMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your Token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
			
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

# Inicialização do adaptador Alpaca

O código abaixo demonstra como inicializar o [AlpacaMessageAdapter](xref:StockSharp.Alpaca.AlpacaMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new AlpacaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),

	// descomente para o modo sandbox
	//IsDemo = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

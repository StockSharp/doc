# Inicialização do adaptador Oanda

O código abaixo demonstra como inicializar o [OandaMessageAdapter](xref:StockSharp.Oanda.OandaMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new OandaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<O seu token>".To<SecureString>(),
	IsDemo = true, // Demo
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

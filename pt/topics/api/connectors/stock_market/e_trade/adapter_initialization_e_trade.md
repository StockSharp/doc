# Inicialização do adaptador E\*TRADE

O código abaixo demonstra como inicializar o [ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new ETradeMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerSecret = "<O seu segredo>".To<SecureString>(),
	ConsumerKey = "<A sua chave>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

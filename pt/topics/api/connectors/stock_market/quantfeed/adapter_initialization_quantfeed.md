# Inicialização do adaptador QuantFEED

O código abaixo demonstra como inicializar o [QuantFeedMessageAdapter](xref:StockSharp.QuantHouse.QuantFeedMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new QuantFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<O seu login>",
	Password = "<A sua palavra-passe>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

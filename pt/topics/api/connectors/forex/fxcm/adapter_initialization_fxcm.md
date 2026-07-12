# Inicialização do adaptador FXCM

O código abaixo demonstra como inicializar o [FxcmMessageAdapter](xref:StockSharp.Fxcm.FxcmMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FxcmMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<O seu login>",
	Password = "<A sua palavra-passe>".To<SecureString>(),
	Address = "<O seu endereço>".To<Uri>(),
	IsDemo = true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

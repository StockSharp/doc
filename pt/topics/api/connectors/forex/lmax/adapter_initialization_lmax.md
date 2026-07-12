# Inicialização do adaptador LMAX

O código abaixo demonstra como inicializar o [LmaxMessageAdapter](xref:StockSharp.LMAX.LmaxMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new LmaxMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<O seu login>",
	Password = "<A sua palavra-passe>".To<SecureString>(),
	IsDemo = true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

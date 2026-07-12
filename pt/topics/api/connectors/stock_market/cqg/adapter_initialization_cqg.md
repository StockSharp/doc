# Inicialização do adaptador CQG

O código abaixo demonstra como inicializar o [CqgComMessageAdapter](xref:StockSharp.Cqg.Com.CqgComMessageAdapter) e o [CqgContinuumMessageAdapter](xref:StockSharp.Cqg.Continuum.CqgContinuumMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

1. **CQG COM**, ligação através do **CQG Integrated Client** local:

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgComMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<O seu login>",
	Password = "<A sua palavra-passe>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

2. **CQG Continuum**, ligação direta ao servidor:

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new CqgContinuumMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<O seu login>",
	Password = "<A sua palavra-passe>".To<SecureString>(),
	Address = "<Address>".To<IPAddress>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

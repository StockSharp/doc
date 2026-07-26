> [!CAUTION]
> **A API de negociação da IEX usada por este conector não está mais disponível. O conector não funciona; a documentação é mantida apenas para referência.**

# Inicialização do adaptador IEX

O código abaixo demonstra como inicializar o [IEXMessageAdapter](xref:StockSharp.IEX.IEXMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new IEXMessageAdapter(Connector.TransactionIdGenerator)
{
	Token  = "<O seu token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

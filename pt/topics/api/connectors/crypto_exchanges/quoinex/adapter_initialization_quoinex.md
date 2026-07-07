> [!NOTE]
> QUOINEX foi renomeada para Liquid, que encerrou em 2022. Esta documentação é preservada para referência histórica.

# Inicialização do adaptador Quoinex

O código abaixo demonstra como inicializar o [QuoinexMessageAdapter](xref:StockSharp.Quoinex.QuoinexMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new QuoinexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)


> [!WARNING]
> Esta bolsa foi encerrada permanentemente (~2023 - encerrada). Este conector já não está operacional. A documentação é preservada para referência histórica.

# Inicialização do adaptador Gopax

O código abaixo demonstra como inicializar o [GopaxMessageAdapter](xref:StockSharp.Gopax.GopaxMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GopaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)


> [!WARNING]
> Esta bolsa foi encerrada permanentemente (maio de 2023 - encerrada). Este conector já não está operacional. A documentação é preservada para referência histórica.

# Inicialização do adaptador Hotbit

O código abaixo demonstra como inicializar o [HotbitMessageAdapter](xref:StockSharp.Hotbit.HotbitMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new HotbitMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)


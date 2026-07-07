> [!WARNING]
> Esta bolsa foi encerrada permanentemente (dezembro de 2020 — comprometida e encerrada). Este conector já não está operacional. A documentação é preservada para referência histórica.

# Inicialização do adaptador Livecoin

O código abaixo demonstra como inicializar o [LiveCoinMessageAdapter](xref:StockSharp.LiveCoin.LiveCoinMessageAdapter) e passá-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new LiveCoinMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

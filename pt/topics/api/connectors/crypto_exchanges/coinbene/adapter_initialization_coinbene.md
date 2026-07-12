> [!WARNING]
> Esta bolsa encerrou permanentemente as operações (~2021 — encerrada). Este conector já não está operacional. A documentação é preservada para referência histórica.

# Inicialização do adaptador CoinBene

O código abaixo demonstra como inicializar o [CoinBeneMessageAdapter](xref:StockSharp.CoinBene.CoinBeneMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new CoinBeneMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

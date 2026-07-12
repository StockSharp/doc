> [!WARNING]
> Esta bolsa foi encerrada permanentemente (julho de 2017 — apreendida). Este conector já não está operacional. A documentação é preservada para referência histórica.

# Inicialização do adaptador WEX (BTC-e)

O código abaixo demonstra como inicializar o [BtceMessageAdapter](xref:StockSharp.Btce.BtceMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new BtceMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

> [!CAUTION]
> **A bolsa FatBTC encerrou as operações. O conector não funciona mais; a documentação é mantida apenas para referência.**

# Inicialização do adaptador FatBTC

O código abaixo demonstra como inicializar o [FatBtcMessageAdapter](xref:StockSharp.FatBTC.FatBtcMessageAdapter) e passá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new FatBtcMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

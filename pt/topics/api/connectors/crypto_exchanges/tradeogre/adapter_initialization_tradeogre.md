# Inicialização do adaptador TradeOgre

O código abaixo demonstra como inicializar o [TradeOgreMessageAdapter](xref:StockSharp.TradeOgre.TradeOgreMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new TradeOgreMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)


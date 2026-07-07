> [!NOTE]
> GDAX foi renomeada para Coinbase Pro e, mais tarde, para Coinbase Advanced Trade. Esta documentação é preservada para referência histórica.

# Inicialização do adaptador GDAX

O código abaixo demonstra como inicializar o [GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)


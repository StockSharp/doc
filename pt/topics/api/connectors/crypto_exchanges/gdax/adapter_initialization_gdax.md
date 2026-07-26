> [!CAUTION]
> **O serviço GDAX não está mais disponível. Seu sucessor, Coinbase Pro, também foi descontinuado; para a Coinbase, use o conector Coinbase atual. Este conector não funciona; a documentação é mantida apenas para referência.**

# Inicialização do adaptador GDAX

O código abaixo demonstra como inicializar o [GdaxMessageAdapter](xref:StockSharp.Gdax.GdaxMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();				
			...				
			var messageAdapter = new GdaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...	
							
```

## Conteúdo recomendado

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)


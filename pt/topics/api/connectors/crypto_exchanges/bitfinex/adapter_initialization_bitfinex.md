# Inicialização do adaptador Bitfinex

O código abaixo demonstra como inicializar o [BitfinexMessageAdapter](xref:StockSharp.Bitfinex.BitfinexMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitfinexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

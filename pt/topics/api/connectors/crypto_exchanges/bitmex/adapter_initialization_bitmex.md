> [!CAUTION]
> **A bolsa BitMEX será encerrada em 23 de setembro de 2026; novos cadastros já foram suspensos. Após o encerramento, o conector deixará de funcionar.**

# Inicialização do adaptador BitMEX

O código abaixo demonstra como inicializar o [BitmexMessageAdapter](xref:StockSharp.Bitmex.BitmexMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitmexMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

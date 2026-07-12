> [!WARNING]
> Esta bolsa encerrou permanentemente as operações (~2021 — encerrada). Este conector não está mais operacional. A documentação é mantida para referência histórica.

# Inicialização do adaptador BitZ

O código abaixo demonstra como inicializar o [BitZMessageAdapter](xref:StockSharp.BitZ.BitZMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitZMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

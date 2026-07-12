> [!NOTE]
> A BitMax foi renomeada para AscendEX. Esta documentação é mantida para referência histórica.

# Inicialização do adaptador BitMax

O código abaixo demonstra como inicializar o [BitMaxMessageAdapter](xref:StockSharp.BitMax.BitMaxMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BitMaxMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<A sua chave de API>".To<SecureString>(),
				Secret = "<O seu segredo de API>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

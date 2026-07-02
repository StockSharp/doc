> [!WARNING]
> Esta bolsa encerrou permanentemente as operações (~2021 — encerrada). Este conector não está mais operacional. A documentação é mantida para referência histórica.

# Inicialização do adaptador BW

O código abaixo demonstra como inicializar o [BWMessageAdapter](xref:StockSharp.BW.BWMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
			Connector Connector = new Connector();
			...
			var messageAdapter = new BWMessageAdapter(Connector.TransactionIdGenerator)
			{
				Key = "<Your API Key>".To<SecureString>(),
				Secret = "<Your API Secret>".To<SecureString>(),
			};
			Connector.Adapter.InnerAdapters.Add(messageAdapter);
			...

```

## Conteúdo recomendado

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

# Inicialização do adaptador: Mirae Asset Sharekhan

O código seguinte mostra como inicializar [MiraeSharekhanMessageAdapter](xref:StockSharp.MiraeSharekhan.MiraeSharekhanMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MiraeSharekhanMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	ApiKey = "<valor>",
	VendorKey = "<valor>",
	CustomerId = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

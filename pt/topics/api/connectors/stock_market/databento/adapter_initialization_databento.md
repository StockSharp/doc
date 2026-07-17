# Inicialização do adaptador: Databento

O código seguinte mostra como inicializar [DatabentoMessageAdapter](xref:StockSharp.Databento.DatabentoMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DatabentoMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<valor>".ToSecureString(),
	Dataset = "<valor>",
	LiveAddress = "<valor>",
	HistoricalAddress = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

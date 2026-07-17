# Inicialização do adaptador: LSEG Real-Time

O código seguinte mostra como inicializar [LsegRealTimeMessageAdapter](xref:StockSharp.LsegRealTime.LsegRealTimeMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LsegRealTimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	Secret = "<valor>".ToSecureString(),
	Address = "<valor>",
	StandbyAddress = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

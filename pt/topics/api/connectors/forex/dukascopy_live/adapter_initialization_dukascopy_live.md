# Inicialização do adaptador: DukasCopy Live

O código seguinte mostra como inicializar [DukasCopyLiveMessageAdapter](xref:StockSharp.DukasCopyLive.DukasCopyLiveMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DukasCopyLiveMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	Login = "<valor>",
	BridgeJarPath = "<valor>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

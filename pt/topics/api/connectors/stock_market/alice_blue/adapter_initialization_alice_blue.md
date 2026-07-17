# Inicialização do adaptador: Alice Blue

O código seguinte mostra como inicializar [AliceBlueMessageAdapter](xref:StockSharp.AliceBlue.AliceBlueMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new AliceBlueMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	UserId = "<valor>",
	ClientId = "<valor>",
	DeviceId = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

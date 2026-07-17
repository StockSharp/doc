# Inicialização do adaptador: QMT

O código seguinte mostra como inicializar [QmtMessageAdapter](xref:StockSharp.Qmt.QmtMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QmtMessageAdapter(Connector.TransactionIdGenerator)
{
	GatewayToken = "<valor>".ToSecureString(),
	GatewayHost = "<valor>",
	GatewayPort = 10,
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

# Inicialização do adaptador: Zerodha Kite Connect

O código seguinte mostra como inicializar [ZerodhaMessageAdapter](xref:StockSharp.Zerodha.ZerodhaMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ZerodhaMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiSecret = "<valor>".ToSecureString(),
	Token = "<valor>".ToSecureString(),
	RequestToken = "<valor>".ToSecureString(),
	ApiKey = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

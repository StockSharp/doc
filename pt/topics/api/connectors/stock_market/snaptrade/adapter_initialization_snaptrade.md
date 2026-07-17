# Inicialização do adaptador: SnapTrade

O código seguinte mostra como inicializar [SnapTradeMessageAdapter](xref:StockSharp.SnapTrade.SnapTradeMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SnapTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<valor>".ToSecureString(),
	UserSecret = "<valor>".ToSecureString(),
	ClientId = "<valor>",
	UserId = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

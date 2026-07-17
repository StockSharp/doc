# Inicialização do adaptador: Flattrade

O código seguinte mostra como inicializar [FlattradeMessageAdapter](xref:StockSharp.Flattrade.FlattradeMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FlattradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	UserId = "<valor>",
	AccountId = "<valor>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

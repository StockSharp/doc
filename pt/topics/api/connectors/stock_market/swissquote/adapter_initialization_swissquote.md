# Inicialização do adaptador: Swissquote OpenWealth

O código seguinte mostra como inicializar [SwissquoteMessageAdapter](xref:StockSharp.Swissquote.SwissquoteMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SwissquoteMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	CustomerId = "<valor>",
	SafekeepingAccountId = "<valor>",
	CashAccountId = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

# Inicialização do adaptador: lemon.markets

O código seguinte mostra como inicializar [LemonMarketsMessageAdapter](xref:StockSharp.LemonMarkets.LemonMarketsMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LemonMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<valor>".ToSecureString(),
	AccountId = "<valor>",
	SecuritiesAccountId = "<valor>",
	DataPrivacyPrincipal = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

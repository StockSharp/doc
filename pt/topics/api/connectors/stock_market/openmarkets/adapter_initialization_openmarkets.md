# Inicialização do adaptador: OpenMarkets

O código seguinte mostra como inicializar [OpenMarketsMessageAdapter](xref:StockSharp.OpenMarkets.OpenMarketsMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new OpenMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientSecret = "<valor>".ToSecureString(),
	ClientId = "<valor>",
	AccountCode = "<valor>",
	DataSource = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

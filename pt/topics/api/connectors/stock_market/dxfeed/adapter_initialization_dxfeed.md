# Inicialização do adaptador: dxFeed

O código seguinte mostra como inicializar [DxFeedMessageAdapter](xref:StockSharp.DxFeed.DxFeedMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DxFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	Address = "<valor>",
	MarketDepthSources = "<valor>",
	AggregationPeriod = TimeSpan.FromSeconds(10),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

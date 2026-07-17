# Inicialização do adaptador: kabu Station

O código seguinte mostra como inicializar [KabuStationMessageAdapter](xref:StockSharp.KabuStation.KabuStationMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KabuStationMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiPassword = "<valor>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

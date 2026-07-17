# Inicialização do adaptador: Capital Futures

O código seguinte mostra como inicializar [CapitalFuturesMessageAdapter](xref:StockSharp.CapitalFutures.CapitalFuturesMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CapitalFuturesMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	SdkPath = "<valor>",
	Login = "<valor>",
	Account = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

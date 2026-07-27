# Inicialização do adaptador: DukasCopy JForex

O código seguinte mostra como inicializar [DukasCopyJForexMessageAdapter](xref:StockSharp.DukasCopyJForex.DukasCopyJForexMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DukasCopyJForexMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	Login = "<valor>",
	BridgeJarPath = "<valor>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

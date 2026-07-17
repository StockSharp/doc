# Inicialização do adaptador: Bloomberg BLPAPI and EMSX

O código seguinte mostra como inicializar [BloombergMessageAdapter](xref:StockSharp.Bloomberg.BloombergMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BloombergMessageAdapter(Connector.TransactionIdGenerator)
{
	SdkPath = "<valor>",
	EmsxService = "<valor>",
	Broker = "<valor>",
	ServerAddress = "<valor>".To<EndPoint>(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

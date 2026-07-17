# Inicialização do adaptador: Tecnologias de negociação

O código seguinte mostra como inicializar [TradingTechnologiesMessageAdapter](xref:StockSharp.TradingTechnologies.TradingTechnologiesMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradingTechnologiesMessageAdapter(Connector.TransactionIdGenerator)
{
	AppSecretKey = "<valor>".ToSecureString(),
	SdkPath = "<valor>",
	IsBinaryProtocol = true,
	IsOptionsEnabled = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

# Inicialização do adaptador: Sierra Chart DTC

O código seguinte mostra como inicializar [SierraChartDtcMessageAdapter](xref:StockSharp.SierraChartDtc.SierraChartDtcMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SierraChartDtcMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	Login = "<valor>",
	TradeAccount = "<valor>",
	TargetHost = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

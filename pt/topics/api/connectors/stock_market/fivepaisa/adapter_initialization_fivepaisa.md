# Inicialização do adaptador: 5paisa Xstream

O código seguinte mostra como inicializar [FivePaisaMessageAdapter](xref:StockSharp.FivePaisa.FivePaisaMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FivePaisaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	AppKey = "<valor>",
	ClientCode = "<valor>",
	AlgoId = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

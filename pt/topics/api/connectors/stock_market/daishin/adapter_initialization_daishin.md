# Inicialização do adaptador: Daishin CYBOS Plus

O código seguinte mostra como inicializar [DaishinMessageAdapter](xref:StockSharp.Daishin.DaishinMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DaishinMessageAdapter(Connector.TransactionIdGenerator)
{
	Account = "<valor>",
	IsTradingEnabled = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Substitua os valores de exemplo pelos parâmetros emitidos ou configurados para a sua conta.

## Consulte também

[Janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md)

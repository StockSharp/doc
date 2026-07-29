# Inicialização do adaptador do Histórico Gate.io

O código abaixo demonstra como inicializar o [GateIOHistoryMessageAdapter](xref:StockSharp.GateIOHistory.GateIOHistoryMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new();
...
var messageAdapter = new GateIOHistoryMessageAdapter(connector.TransactionIdGenerator)
{
	CheckDates = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de definições da ligação](../../../graphical_user_interface/connection_settings_window.md)

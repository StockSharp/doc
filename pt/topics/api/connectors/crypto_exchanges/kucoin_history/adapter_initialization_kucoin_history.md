# Inicialização do adaptador do Histórico Kucoin

O código abaixo demonstra como inicializar o [KucoinHistoryMessageAdapter](xref:StockSharp.KucoinHistory.KucoinHistoryMessageAdapter) e adicioná-lo ao [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new();
...
var messageAdapter = new KucoinHistoryMessageAdapter(connector.TransactionIdGenerator)
{
	CheckDates = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Conteúdo recomendado

[Janela de definições da ligação](../../../graphical_user_interface/connection_settings_window.md)

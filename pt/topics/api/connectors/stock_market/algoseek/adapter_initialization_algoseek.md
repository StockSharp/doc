# Inicialização do adaptador AlgoSeek

O código abaixo demonstra como inicializar o [AlgoSeekMessageAdapter](xref:StockSharp.AlgoSeek.AlgoSeekMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AlgoSeekMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

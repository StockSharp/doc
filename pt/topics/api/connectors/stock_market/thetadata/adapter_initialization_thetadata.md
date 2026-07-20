# Inicialização do adaptador ThetaData

O código abaixo demonstra como inicializar o [ThetaDataMessageAdapter](xref:StockSharp.ThetaData.ThetaDataMessageAdapter) e enviá-lo para o [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ThetaDataMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Ver também

[Janela de definições de ligação](../../../graphical_user_interface/connection_settings_window.md)

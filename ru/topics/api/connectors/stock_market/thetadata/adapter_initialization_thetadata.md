# Инициализация адаптера ThetaData

Код ниже демонстрирует как инициализировать [ThetaDataMessageAdapter](xref:StockSharp.ThetaData.ThetaDataMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ThetaDataMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

# Инициализация адаптера Simba

Код ниже демонстрирует как инициализировать [SimbaMessageAdapter](xref:StockSharp.Simba.SimbaMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var connector = new Connector();
...
var messageAdapter = new SimbaMessageAdapter(connector.TransactionIdGenerator)
{
    DialectType = DialectTypes.Spectra,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

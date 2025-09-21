# Инициализация адаптера Bybit History

Код ниже демонстрирует, как инициализировать [BybitHistoryMessageAdapter](xref:StockSharp.BybitHistory.BybitHistoryMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BybitHistoryMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

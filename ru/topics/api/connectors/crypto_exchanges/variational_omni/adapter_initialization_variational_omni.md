# Инициализация адаптера Variational Omni

Код ниже демонстрирует как инициализировать [VariationalOmniMessageAdapter](xref:StockSharp.VariationalOmni.VariationalOmniMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new VariationalOmniMessageAdapter(Connector.TransactionIdGenerator)
{
	Endpoint = "<Ваше значение>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

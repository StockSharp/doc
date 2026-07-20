# Инициализация адаптера X Open Hub

Код ниже демонстрирует, как инициализировать [XOpenHubMessageAdapter](xref:StockSharp.XOpenHub.XOpenHubMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new XOpenHubMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Ваше значение>",
	Password = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

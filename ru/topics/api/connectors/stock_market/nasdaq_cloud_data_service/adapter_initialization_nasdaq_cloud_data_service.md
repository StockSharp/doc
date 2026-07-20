# Инициализация адаптера Nasdaq Cloud Data Service

Код ниже демонстрирует как инициализировать [NasdaqCloudDataServiceMessageAdapter](xref:StockSharp.NasdaqCloudDataService.NasdaqCloudDataServiceMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new NasdaqCloudDataServiceMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Ваше значение>",
	Password = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

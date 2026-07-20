# Инициализация адаптера uSMART OpenAPI

Код ниже демонстрирует как инициализировать [UsmartMessageAdapter](xref:StockSharp.Usmart.UsmartMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new UsmartMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<Ваше значение>".To<SecureString>(),
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

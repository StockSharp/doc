# Инициализация адаптера Phillip POEMS

Код ниже демонстрирует как инициализировать [PhillipPoemsMessageAdapter](xref:StockSharp.PhillipPoems.PhillipPoemsMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PhillipPoemsMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Ваше значение>",
	ClientSecret = "<Ваше значение>".To<SecureString>(),
	ApiKey = "<Ваше значение>".To<SecureString>(),
	AccessToken = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

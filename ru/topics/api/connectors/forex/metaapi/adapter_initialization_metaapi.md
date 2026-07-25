# Инициализация адаптера: MetaApi

Следующий код создаёт [MetaApiMessageAdapter](xref:StockSharp.MetaApi.MetaApiMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MetaApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените примеры токеном и идентификатором счёта, развёрнутого в MetaApi.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

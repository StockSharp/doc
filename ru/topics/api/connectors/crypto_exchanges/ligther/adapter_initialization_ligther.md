# Инициализация адаптера Ligther

Код ниже демонстрирует, как инициализировать [LigtherMessageAdapter](xref:StockSharp.Ligther.LigtherMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new LigtherMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваш API Key>".To<SecureString>(),
	Secret = "<Ваш API Secret>".To<SecureString>(),
	AccountIndex = 0,
	ApiKeyIndex = 0,
	Section = LigtherSections.Derivatives,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

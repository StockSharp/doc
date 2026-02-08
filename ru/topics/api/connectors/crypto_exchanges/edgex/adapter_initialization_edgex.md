# Инициализация адаптера edgeX

Код ниже демонстрирует, как инициализировать [EdgeXMessageAdapter](xref:StockSharp.EdgeX.EdgeXMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new EdgeXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ваш API Key>".To<SecureString>(),
	Secret = "<Ваш API Secret>".To<SecureString>(),
	ClearingAccount = "<Ваш Clearing Account>",
	Passphrase = "<Ваш Passphrase>".To<SecureString>(),
	Section = EdgeXSections.Derivatives,
	EnableSpotSection = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

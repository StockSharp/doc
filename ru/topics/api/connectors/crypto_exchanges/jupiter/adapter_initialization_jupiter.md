# Инициализация адаптера Jupiter

Код ниже демонстрирует как инициализировать [JupiterMessageAdapter](xref:StockSharp.Jupiter.JupiterMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new JupiterMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ваше значение>".To<SecureString>(),
	WalletAddress = "<Ваше значение>",
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

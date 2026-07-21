# Инициализация адаптера Synthetix

Код ниже демонстрирует как инициализировать [SynthetixMessageAdapter](xref:StockSharp.Synthetix.SynthetixMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new SynthetixMessageAdapter(Connector.TransactionIdGenerator)
{
	SubAccountId = "<Ваше значение>",
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

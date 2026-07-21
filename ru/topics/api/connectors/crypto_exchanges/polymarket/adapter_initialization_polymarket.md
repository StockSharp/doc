# Инициализация адаптера Polymarket

Код ниже демонстрирует как инициализировать [PolymarketMessageAdapter](xref:StockSharp.Polymarket.PolymarketMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PolymarketMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Ваше значение>",
	ApiSecret = "<Ваше значение>".To<SecureString>(),
	Passphrase = "<Ваше значение>".To<SecureString>(),
	SignerAddress = "<Ваше значение>",
	FunderAddress = "<Ваше значение>",
	PrivateKey = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

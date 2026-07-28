# Инициализация адаптера: CoinCatch

Следующий код создаёт [CoinCatchMessageAdapter](xref:StockSharp.CoinCatch.CoinCatchMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinCatchMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш API-ключ>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
	Passphrase = "<Ваша кодовая фраза API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_coincatch.md).

## См. также

[Настройки коннектора](configuration_coincatch.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

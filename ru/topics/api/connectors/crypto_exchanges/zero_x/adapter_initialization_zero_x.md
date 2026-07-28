# Инициализация адаптера: 0x

Следующий код создаёт [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ZeroXMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Ваш API-ключ>".To<SecureString>(),
	WalletAddress = "<Адрес вашего кошелька>",
	PrivateKey = "<Ваш закрытый ключ>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_zero_x.md).

## См. также

[Настройки коннектора](configuration_zero_x.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

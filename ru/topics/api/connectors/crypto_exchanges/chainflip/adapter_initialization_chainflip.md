# Инициализация адаптера: Chainflip

Следующий код создаёт [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ChainflipMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Адрес вашего кошелька EVM>",
	PrivateKey = "<Ваш закрытый ключ EVM>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите данные кошелька, целевые адреса и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_chainflip.md).

## См. также

[Настройки коннектора](configuration_chainflip.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

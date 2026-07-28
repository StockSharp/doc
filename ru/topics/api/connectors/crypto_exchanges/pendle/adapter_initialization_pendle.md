# Инициализация адаптера: Pendle

Следующий код создаёт [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new PendleMessageAdapter(connector.TransactionIdGenerator)
{
	Chain = PendleChains.Ethereum,
	WalletAddress = "<Адрес вашего кошелька>",
	PrivateKey = "<Ваш закрытый ключ>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите данные кошелька и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_pendle.md).

## См. также

[Настройки коннектора](configuration_pendle.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

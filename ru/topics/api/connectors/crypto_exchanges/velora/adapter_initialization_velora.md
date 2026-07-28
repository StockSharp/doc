# Инициализация адаптера: Velora

Следующий код создаёт [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new VeloraMessageAdapter(connector.TransactionIdGenerator)
{
	Partner = "<Идентификатор партнёра>",
	WalletAddress = "<Адрес вашего кошелька>",
	PrivateKey = "<Ваш закрытый ключ>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_velora.md).

## См. также

[Настройки коннектора](configuration_velora.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

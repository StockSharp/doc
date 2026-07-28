# Инициализация адаптера: Dexalot

Следующий код создаёт [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexalotMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Адрес вашего кошелька>",
	PrivateKey = "<Ваш закрытый ключ>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите данные кошелька и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_dexalot.md).

## См. также

[Настройки коннектора](configuration_dexalot.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

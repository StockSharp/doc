# Инициализация адаптера: STON.fi

Следующий код создаёт [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new StonFiMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Адрес вашего кошелька TON>",
	Mnemonic = "<Ваша мнемоническая фраза из 24 слов>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите данные кошелька и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_stonfi.md).

## См. также

[Настройки коннектора](configuration_stonfi.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

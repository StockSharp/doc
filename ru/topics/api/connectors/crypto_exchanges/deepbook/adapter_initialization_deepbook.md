# Инициализация адаптера: DeepBook

Следующий код создаёт [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DeepBookMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Адрес вашего кошелька>",
	PrivateKey = "<Ваш закрытый ключ>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_deepbook.md).

## См. также

[Настройки коннектора](configuration_deepbook.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

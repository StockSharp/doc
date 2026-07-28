# Инициализация адаптера: Tokocrypto

Следующий код создаёт [TokocryptoMessageAdapter](xref:StockSharp.Tokocrypto.TokocryptoMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new TokocryptoMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш API-ключ>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_tokocrypto.md).

## См. также

[Настройки коннектора](configuration_tokocrypto.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

# Инициализация адаптера: SSI

Следующий код создаёт [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SSIMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш ключ API>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
	ClientId = "<Ваш идентификатор клиента>",
	PrivateKey = "<Ваш закрытый ключ RSA>".To<SecureString>(),
	Otp = "<Текущий одноразовый код>".To<SecureString>(),
	Account = "<Номер вашего счёта>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_ssi.md).

## См. также

[Настройки коннектора](configuration_ssi.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

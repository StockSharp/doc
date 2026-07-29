# Инициализация адаптера: m.Stock

Следующий код создаёт [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new MStockMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш ключ API>".To<SecureString>(),
	ClientCode = "<Ваш клиентский код>",
	Password = "<Ваш пароль>".To<SecureString>(),
	Otp = "<Текущий одноразовый код>".To<SecureString>(),
	UseTotp = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_mstock.md).

## См. также

[Настройки коннектора](configuration_mstock.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

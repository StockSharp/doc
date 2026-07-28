# Инициализация адаптера: Settrade

Следующий код создаёт [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SettradeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш ключ API>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
	AppCode = "<Код вашего приложения>",
	BrokerId = "<Идентификатор вашего брокера>",
	Account = "<Номер вашего счёта>",
	Pin = "<Ваш торговый ПИН-код>".To<SecureString>(),
	AccountType = SettradeAccountTypes.Equity,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные, тип счёта и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_settrade.md).

## См. также

[Настройки коннектора](configuration_settrade.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

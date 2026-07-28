# Инициализация адаптера: XRPL DEX

Следующий код создаёт [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new XrplMessageAdapter(connector.TransactionIdGenerator)
{
	Account = "<Адрес вашего счёта XRPL>",
	Seed = "<Ваша секретная фраза семейства>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите данные счёта и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_xrpl.md).

## См. также

[Настройки коннектора](configuration_xrpl.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

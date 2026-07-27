# Инициализация адаптера: Primary

Следующий код создаёт [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PrimaryMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_primary.md).

## См. также

[Настройки коннектора](configuration_primary.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

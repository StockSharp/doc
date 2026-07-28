# Инициализация адаптера: NovaDAX

Следующий код создаёт [NovaDaxMessageAdapter](xref:StockSharp.NovaDax.NovaDaxMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new NovaDaxMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ваш API-ключ>".To<SecureString>(),
	Secret = "<Ваш секрет API>".To<SecureString>(),
	AccountId = "<Идентификатор вашего счёта>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_novadax.md).

## См. также

[Настройки коннектора](configuration_novadax.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

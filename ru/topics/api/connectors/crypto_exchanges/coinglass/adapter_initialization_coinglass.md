# Инициализация адаптера: CoinGlass

Следующий код создаёт [CoinGlassMessageAdapter](xref:StockSharp.CoinGlass.CoinGlassMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinGlassMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ваш токен доступа>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_coinglass.md).

## См. также

[Настройки коннектора](configuration_coinglass.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

# Инициализация адаптера: Coinmetro

Следующий код создаёт [CoinmetroMessageAdapter](xref:StockSharp.Coinmetro.CoinmetroMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinmetroMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ваш токен доступа>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_coinmetro.md).

## См. также

[Настройки коннектора](configuration_coinmetro.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

# Инициализация адаптера: CoinPaprika

Следующий код создаёт [CoinPaprikaMessageAdapter](xref:StockSharp.CoinPaprika.CoinPaprikaMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinPaprikaMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ваш токен доступа>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_coinpaprika.md).

## См. также

[Настройки коннектора](configuration_coinpaprika.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

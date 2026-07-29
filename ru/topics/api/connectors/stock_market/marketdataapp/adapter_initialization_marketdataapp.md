# Инициализация адаптера: MarketData.app

Следующий код создаёт [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new MarketDataAppMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ваш токен API>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Укажите параметры доступа и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_marketdataapp.md).

## См. также

[Настройки коннектора](configuration_marketdataapp.md)

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

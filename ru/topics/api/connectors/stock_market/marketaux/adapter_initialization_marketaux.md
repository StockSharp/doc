# Инициализация адаптера: Marketaux

Следующий код создаёт [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MarketauxMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_marketaux.md).

## См. также

[Настройки коннектора](configuration_marketaux.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

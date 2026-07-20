# Инициализация адаптера Goldman Sachs Marquee

Код ниже демонстрирует как инициализировать [MarqueeMessageAdapter](xref:StockSharp.Marquee.MarqueeMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MarqueeMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Ваше значение>",
	ClientSecret = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

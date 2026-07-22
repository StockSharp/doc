# Инициализация адаптера CoinGecko

Код ниже демонстрирует как инициализировать [CoinGeckoMessageAdapter](xref:StockSharp.CoinGecko.CoinGeckoMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinGeckoMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ваше значение>".To<SecureString>(),
	QuoteCurrency = "<Ваше значение>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

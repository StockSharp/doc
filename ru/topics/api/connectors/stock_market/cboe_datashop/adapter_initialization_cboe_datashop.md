# Инициализация адаптера Cboe DataShop / LiveVol

Код ниже демонстрирует как инициализировать [CboeDataShopMessageAdapter](xref:StockSharp.CboeDataShop.CboeDataShopMessageAdapter) и передать его в [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CboeDataShopMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Ваше значение>",
	Password = "<Ваше значение>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

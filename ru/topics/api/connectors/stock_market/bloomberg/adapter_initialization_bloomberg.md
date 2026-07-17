# Инициализация адаптера: Bloomberg BLPAPI and EMSX

В следующем коде показано, как инициализировать [BloombergMessageAdapter](xref:StockSharp.Bloomberg.BloombergMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BloombergMessageAdapter(Connector.TransactionIdGenerator)
{
	SdkPath = "<значение>",
	EmsxService = "<значение>",
	Broker = "<значение>",
	ServerAddress = "<значение>".To<EndPoint>(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

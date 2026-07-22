# Инициализация адаптера SBE

Следующий код инициализирует [CryBroSBEMessageAdapter](xref:StockSharp.CryBro.SBE.CryBroSBEMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CryBroSBEMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "127.0.0.1:5002".To<EndPoint>(),
	SenderCompId = "<login>",
	TargetCompId = "StockSharp",
	Password = "<password>".ToSecureString(),
	IsSupportNativeCandles = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Клиент и сервер должны использовать совместимые идентификаторы и версии схемы SBE.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

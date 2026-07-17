# Инициализация адаптера: Fubon Neo

В следующем коде показано, как инициализировать [FubonNeoMessageAdapter](xref:StockSharp.FubonNeo.FubonNeoMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FubonNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<значение>".ToSecureString(),
	ApiKey = "<значение>".ToSecureString(),
	CertificatePassword = "<значение>".ToSecureString(),
	SdkPath = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

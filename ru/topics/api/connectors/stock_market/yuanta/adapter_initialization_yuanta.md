# Инициализация адаптера: Yuanta SPARK

В следующем коде показано, как инициализировать [YuantaMessageAdapter](xref:StockSharp.Yuanta.YuantaMessageAdapter) и добавить его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new YuantaMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<значение>".ToSecureString(),
	CertificatePassword = "<значение>".ToSecureString(),
	SdkPath = "<значение>",
	Account = "<значение>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Замените значения в примере параметрами, выданными или настроенными для вашего счёта.

## См. также

[Окно настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

# Инициализация адаптера: HDFC Securities

Следующий код создаёт [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new HdfcMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestToken = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_hdfc_securities.md).

## См. также

[Настройки коннектора](configuration_hdfc_securities.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

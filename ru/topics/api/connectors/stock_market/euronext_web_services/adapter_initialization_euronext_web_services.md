# Инициализация адаптера: Euronext Web Services

Следующий код создаёт [EuronextWebServicesMessageAdapter](xref:StockSharp.EuronextWebServices.EuronextWebServicesMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EuronextWebServicesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_euronext_web_services.md).

## См. также

[Настройки коннектора](configuration_euronext_web_services.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

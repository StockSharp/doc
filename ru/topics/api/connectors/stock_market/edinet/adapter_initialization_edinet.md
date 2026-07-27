# Инициализация адаптера: EDINET

Следующий код создаёт [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EdinetMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_edinet.md).

## См. также

[Настройки коннектора](configuration_edinet.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

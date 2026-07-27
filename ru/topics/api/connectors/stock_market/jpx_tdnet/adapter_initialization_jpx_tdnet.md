# Инициализация адаптера: JPX TDnet

Следующий код создаёт [JpxTdnetMessageAdapter](xref:StockSharp.JpxTdnet.JpxTdnetMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new JpxTdnetMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_jpx_tdnet.md).

## См. также

[Настройки коннектора](configuration_jpx_tdnet.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

# Инициализация адаптера: Quiver Quantitative

Следующий код создаёт [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuiverQuantMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_quiver_quant.md).

## См. также

[Настройки коннектора](configuration_quiver_quant.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

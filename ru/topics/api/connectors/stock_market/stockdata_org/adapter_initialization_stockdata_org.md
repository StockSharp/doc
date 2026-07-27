# Инициализация адаптера: StockData.org

Следующий код создаёт [StockDataOrgMessageAdapter](xref:StockSharp.StockDataOrg.StockDataOrgMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new StockDataOrgMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_stockdata_org.md).

## См. также

[Настройки коннектора](configuration_stockdata_org.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

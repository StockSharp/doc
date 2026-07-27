# Инициализация адаптера: Financial Datasets

Следующий код создаёт [FinancialDatasetsMessageAdapter](xref:StockSharp.FinancialDatasets.FinancialDatasetsMessageAdapter) и добавляет его в [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinancialDatasetsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Укажите учётные данные и остальные обязательные свойства, описанные на странице [Настройки коннектора](configuration_financial_datasets.md).

## См. также

[Настройки коннектора](configuration_financial_datasets.md)

[окне настройки подключений](../../../graphical_user_interface/connection_settings_window.md)

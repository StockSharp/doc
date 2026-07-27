# Adapter initialization: Financial Datasets

The following code initializes [FinancialDatasetsMessageAdapter](xref:StockSharp.FinancialDatasets.FinancialDatasetsMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinancialDatasetsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_financial_datasets.md) page.

## See also

[Connector configuration](configuration_financial_datasets.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

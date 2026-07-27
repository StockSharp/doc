# Adapter initialization: StockData.org

The following code initializes [StockDataOrgMessageAdapter](xref:StockSharp.StockDataOrg.StockDataOrgMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new StockDataOrgMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_stockdata_org.md) page.

## See also

[Connector configuration](configuration_stockdata_org.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

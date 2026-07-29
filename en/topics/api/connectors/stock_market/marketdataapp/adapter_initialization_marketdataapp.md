# Adapter initialization: MarketData.app

The following code initializes [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new MarketDataAppMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Your API token>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_marketdataapp.md) page.

## See also

[Connector configuration](configuration_marketdataapp.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

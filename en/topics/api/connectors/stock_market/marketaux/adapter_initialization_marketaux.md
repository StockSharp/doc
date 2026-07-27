# Adapter initialization: Marketaux

The following code initializes [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MarketauxMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_marketaux.md) page.

## See also

[Connector configuration](configuration_marketaux.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

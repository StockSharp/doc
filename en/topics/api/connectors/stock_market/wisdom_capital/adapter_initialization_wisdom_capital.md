# Adapter initialization: Wisdom Capital

The following code initializes [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new WisdomCapitalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	MarketDataKey = "<key>".ToSecureString(),
	MarketDataSecret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_wisdom_capital.md) page.

## See also

[Connector configuration](configuration_wisdom_capital.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

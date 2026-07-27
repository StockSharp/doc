# Adapter initialization: Toss Securities

The following code initializes [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TossSecuritiesMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_toss_securities.md) page.

## See also

[Connector configuration](configuration_toss_securities.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

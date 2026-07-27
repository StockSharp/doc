# Adapter initialization: Tradejini

The following code initializes [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradejiniMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<key>".ToSecureString(),
	Password = "<secret>".ToSecureString(),
	TwoFactorCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_tradejini.md) page.

## See also

[Connector configuration](configuration_tradejini.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

# Adapter initialization: STON.fi

The following code initializes [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new StonFiMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Your TON wallet address>",
	Mnemonic = "<Your 24-word mnemonic>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the wallet credentials and any other required properties described on the [Connector configuration](configuration_stonfi.md) page.

## See also

[Connector configuration](configuration_stonfi.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

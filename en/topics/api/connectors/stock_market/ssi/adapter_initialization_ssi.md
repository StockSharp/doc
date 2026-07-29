# Adapter initialization: SSI

The following code initializes [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SSIMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
	ClientId = "<Your client identifier>",
	PrivateKey = "<Your RSA private key>".To<SecureString>(),
	Otp = "<Current OTP>".To<SecureString>(),
	Account = "<Your account number>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_ssi.md) page.

## See also

[Connector configuration](configuration_ssi.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

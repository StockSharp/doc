# Adapter initialization: B3 UP2DATA

The following code initializes [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new B3Up2DataMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_b3_up2data.md) page.

## See also

[Connector configuration](configuration_b3_up2data.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

# Zhongtai XTP adapter initialization

The following code demonstrates how to initialize [XtpMessageAdapter](xref:StockSharp.Xtp.XtpMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Login>",
	Password = "<Password>".ToSecureString(),
	ClientId = 1,
	QuoteAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6001),
	TransactionAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6002),
	Protocol = XtpProtocols.Tcp,
	SoftwareKey = "<Software key>",
	SoftwareVersion = "1.0",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

# CTP adapter initialization

The following code demonstrates how to initialize [CtpMessageAdapter](xref:StockSharp.Ctp.CtpMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Login>",
	Password = "<Password>".ToSecureString(),
	BrokerId = "<Broker ID>",
	InvestorId = "<Investor ID>",
	MarketDataAddress = "tcp://<Market-data address>",
	TraderAddress = "tcp://<Trader address>",
	AppId = "<Application ID>",
	AuthCode = "<Authentication code>".ToSecureString(),
	ProductionMode = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

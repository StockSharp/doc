# Tradovate adapter initialization

The following code demonstrates how to initialize [TradovateMessageAdapter](xref:StockSharp.Tradovate.TradovateMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradovateMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<user name>",
	Password = "<password>".ToSecureString(),
	ClientId = "<API client ID>",
	Secret = "<API client secret>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = "<stable device ID>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set `IsDemo` to `false` to connect to the live environment.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

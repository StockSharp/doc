# Questrade adapter initialization

The following code demonstrates how to initialize [QuestradeMessageAdapter](xref:StockSharp.Questrade.QuestradeMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QuestradeMessageAdapter(Connector.TransactionIdGenerator)
{
	RefreshToken = "<Refresh token>".ToSecureString(),
	Account = "<Account>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

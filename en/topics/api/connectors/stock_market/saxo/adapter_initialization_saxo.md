# Saxo OpenAPI adapter initialization

The following code demonstrates how to initialize [SaxoMessageAdapter](xref:StockSharp.Saxo.SaxoMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SaxoMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<Access token>".ToSecureString(),
	RefreshToken = "<Refresh token>".ToSecureString(),
	ClientId = "<Client ID>",
	ClientSecret = "<Client secret>".ToSecureString(),
	RedirectUri = "<Redirect URI>",
	AccountKey = "<Account key>",
	Environment = SaxoEnvironments.Simulation,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)


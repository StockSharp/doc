# Adapter initialization: MetaApi

The following code initializes [MetaApiMessageAdapter](xref:StockSharp.MetaApi.MetaApiMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MetaApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the token and the identifier of the account deployed in MetaApi.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

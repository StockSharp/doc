# Adapter initialization Ligther

The code below demonstrates how to initialize [LigtherMessageAdapter](xref:StockSharp.Ligther.LigtherMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new LigtherMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
	AccountIndex = 0,
	ApiKeyIndex = 0,
	Section = LigtherSections.Derivatives,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

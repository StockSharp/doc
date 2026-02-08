# Adapter initialization Paradex

The code below demonstrates how to initialize [ParadexMessageAdapter](xref:StockSharp.Paradex.ParadexMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ParadexMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
	StarknetAccount = "<Your Starknet Account>",
	StarknetPrivateKey = "<Your Starknet Private Key>".To<SecureString>(),
	Section = ParadexSections.Derivatives,
	AuthPath = "/v1/auth",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

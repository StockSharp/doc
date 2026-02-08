# Adapter initialization Aster

The code below demonstrates how to initialize [AsterMessageAdapter](xref:StockSharp.Aster.AsterMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AsterMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
	Section = AsterSections.Derivatives,
	DerivativesProtocolMode = AsterDerivativesProtocolModes.Legacy,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

# Adapterinitialisierung edgeX

Der folgende Code zeigt, wie der [EdgeXMessageAdapter](xref:StockSharp.EdgeX.EdgeXMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new EdgeXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
	ClearingAccount = "<Your Clearing Account>",
	Passphrase = "<Your Passphrase>".To<SecureString>(),
	Section = EdgeXSections.Derivatives,
	EnableSpotSection = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

# Adapterinitialisierung edgeX

Der folgende Code zeigt, wie der [EdgeXMessageAdapter](xref:StockSharp.EdgeX.EdgeXMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new EdgeXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
	ClearingAccount = "<Ihr Clearing-Konto>",
	Passphrase = "<Ihre Passphrase>".To<SecureString>(),
	Section = EdgeXSections.Derivatives,
	EnableSpotSection = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

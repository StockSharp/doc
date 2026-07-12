# Paradex-Adapter initialisieren

Der folgende Code zeigt, wie [ParadexMessageAdapter](xref:StockSharp.Paradex.ParadexMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ParadexMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
	StarknetAccount = "<Ihr Starknet-Konto>",
	StarknetPrivateKey = "<Ihr privater Starknet-Schlüssel>".To<SecureString>(),
	Section = ParadexSections.Derivatives,
	AuthPath = "/v1/auth",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)


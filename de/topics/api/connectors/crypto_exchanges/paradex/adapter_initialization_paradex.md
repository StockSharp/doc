# Paradex-Adapter initialisieren

Der folgende Code zeigt, wie [ParadexMessageAdapter](xref:StockSharp.Paradex.ParadexMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

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

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)


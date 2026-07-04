# Initialisierung des Gate.io-Adapters

Der folgende Code zeigt, wie der [GateIOMessageAdapter](xref:StockSharp.GateIO.GateIOMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GateIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

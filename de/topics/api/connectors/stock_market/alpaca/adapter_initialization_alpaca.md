# Adapterinitialisierung Alpaca

Der folgende Code zeigt, wie der [AlpacaMessageAdapter](xref:StockSharp.Alpaca.AlpacaMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) uebergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AlpacaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),

	// für den Sandbox-Modus auskommentieren
	//IsDemo = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

# Adapterinitialisierung Alpaca

Der folgende Code zeigt, wie der [AlpacaMessageAdapter](xref:StockSharp.Alpaca.AlpacaMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AlpacaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Geheimnis>".To<SecureString>(),

	// für den Sandbox-Modus auskommentieren
	//IsDemo = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

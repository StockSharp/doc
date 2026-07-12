# Adapter-Initialisierung PolygonIO

Der folgende Code zeigt, wie der [PolygonIOMessageAdapter](xref:StockSharp.PolygonIO.PolygonIOMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PolygonIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ihr Token>".To<SecureString>(),
	ConnectionType = PolygonIOConnectionTypes.History, // Verbindung für REST-Datenquellen
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

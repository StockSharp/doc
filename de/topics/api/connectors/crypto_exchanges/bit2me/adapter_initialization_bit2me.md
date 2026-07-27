# Initialisierung des Bit2Me-Adapters

Der folgende Code zeigt, wie der [Bit2MeMessageAdapter](xref:StockSharp.Bit2Me.Bit2MeMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector connector = new Connector();
...
var messageAdapter = new Bit2MeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Lassen Sie `Key` und `Secret` weg, wenn nur öffentliche Marktdaten benötigt werden. REST- und WebSocket-Adressen lassen sich über `RestEndpoint` und `WebSocketEndpoint` ändern.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

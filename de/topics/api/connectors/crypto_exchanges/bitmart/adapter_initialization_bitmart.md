# Adapter-Initialisierung Bitmart

Der folgende Code zeigt, wie man den [BitmartMessageAdapter](xref:StockSharp.Bitmart.BitmartMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitmartMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Geheimnis>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

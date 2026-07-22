# Initialisierung des ProBit-Global-Adapters

Der folgende Code zeigt, wie der [ProBitMessageAdapter](xref:StockSharp.ProBit.ProBitMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector connector = new Connector();
...
var messageAdapter = new ProBitMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihre OAuth-Client-ID>".To<SecureString>(),
	Secret = "<Ihr OAuth-Client-Geheimnis>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Lassen Sie `Key` und `Secret` weg, wenn nur öffentliche Marktdaten benötigt werden.

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

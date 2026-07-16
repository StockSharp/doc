# Initialisierung des Moomoo-Adapters

Der folgende Code zeigt, wie [MoomooMessageAdapter](xref:StockSharp.Moomoo.MoomooMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new MoomooMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Loopback, 11111),
	Password = "<Passwort>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)


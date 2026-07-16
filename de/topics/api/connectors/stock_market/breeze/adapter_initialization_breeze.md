# Initialisierung des ICICI Direct Breeze-Adapters

Der folgende Code zeigt, wie [BreezeMessageAdapter](xref:StockSharp.Breeze.BreezeMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new BreezeMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<API-Schlüssel>",
	SecretKey = "<Geheimer Schlüssel>".ToSecureString(),
	ApiSession = "<API-Sitzung>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)


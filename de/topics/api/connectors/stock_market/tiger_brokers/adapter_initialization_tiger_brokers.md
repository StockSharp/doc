# Initialisierung des Tiger Brokers-Adapters

Der folgende Code zeigt, wie [TigerBrokersMessageAdapter](xref:StockSharp.TigerBrokers.TigerBrokersMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new TigerBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	TigerId = "<Tiger-Kennung>",
	Account = "<Konto>",
	License = TigerLicenses.Singapore,
	PrivateKey = "<Privater Schlüssel>".ToSecureString(),
	Token = "<Token>".ToSecureString(),
	AutoGrabPermission = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)


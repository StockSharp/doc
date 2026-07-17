# Initialisierung des Questrade-Adapters

Der folgende Code zeigt, wie [QuestradeMessageAdapter](xref:StockSharp.Questrade.QuestradeMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new QuestradeMessageAdapter(Connector.TransactionIdGenerator)
{
	RefreshToken = "<Aktualisierungstoken>".ToSecureString(),
	Account = "<Konto>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Ersetzen Sie die Beispielwerte durch die für Ihr Konto ausgegebenen Zugangsdaten und Serveradressen.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

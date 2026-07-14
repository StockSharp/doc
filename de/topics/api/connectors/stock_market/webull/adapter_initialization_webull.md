# Initialisierung des Webull-Adapters

Der folgende Code zeigt, wie der [WebullMessageAdapter](xref:StockSharp.Webull.WebullMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new WebullMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Anwendungsschlüssel>".ToSecureString(),
	Secret = "<Anwendungsgeheimnis>".ToSecureString(),
	Token = "<Zugriffstoken>".ToSecureString(),
	Account = "<Kontokennung>",
	IsDemo = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Die Einstellungen `Token` und `Account` können entfallen, wenn sie nicht erforderlich sind.

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

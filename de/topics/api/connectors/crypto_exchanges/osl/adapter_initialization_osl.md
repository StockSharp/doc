# Initialisierung des OSL Global-Adapters

Der folgende Code zeigt, wie man den [OSLMessageAdapter](xref:StockSharp.OSL.OSLMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OSLMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr Wert>".To<SecureString>(),
	Secret = "<Ihr Wert>".To<SecureString>(),
	Passphrase = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

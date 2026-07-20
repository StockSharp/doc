# Initialisierung des Crypto.com-Adapters

Der folgende Code zeigt, wie man den [CryptoComMessageAdapter](xref:StockSharp.Crypto.com.CryptoComMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CryptoComMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Ihr Wert>".To<SecureString>(),
	Secret = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

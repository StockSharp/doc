# Initialisierung des QuickSwap-Adapters

Der folgende Code zeigt, wie man den [QuickSwapMessageAdapter](xref:StockSharp.QuickSwap.QuickSwapMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new QuickSwapMessageAdapter(Connector.TransactionIdGenerator)
{
	GraphApiKey = "<Ihr Wert>".To<SecureString>(),
	WalletAddress = "<Ihr Wert>",
	PrivateKey = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

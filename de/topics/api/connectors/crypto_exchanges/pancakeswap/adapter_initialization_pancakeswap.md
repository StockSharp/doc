# Initialisierung des PancakeSwap-Adapters

Der folgende Code zeigt, wie man den [PancakeSwapMessageAdapter](xref:StockSharp.PancakeSwap.PancakeSwapMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PancakeSwapMessageAdapter(Connector.TransactionIdGenerator)
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

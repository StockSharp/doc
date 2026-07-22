# Initialisierung des CoinAPI-Adapters

Der folgende Code zeigt, wie man den [CoinApiMessageAdapter](xref:StockSharp.CoinApi.CoinApiMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

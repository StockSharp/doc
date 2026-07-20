# Match-Trader-Adapter initialisieren

Der folgende Code zeigt, wie der [MatchTraderMessageAdapter](xref:StockSharp.MatchTrader.MatchTraderMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MatchTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Ihr Wert>",
	Password = "<Ihr Wert>".To<SecureString>(),
	AccountId = "<Ihr Wert>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

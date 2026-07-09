# Adapterinitialisierung E\*TRADE

Der folgende Code zeigt, wie der [ETradeMessageAdapter](xref:StockSharp.ETrade.ETradeMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ETradeMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerSecret = "<Your Secret>".To<SecureString>(),
	ConsumerKey = "<Your Key>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

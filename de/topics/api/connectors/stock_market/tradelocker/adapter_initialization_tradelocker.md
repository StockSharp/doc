# Adapterinitialisierung TradeLocker

Der folgende Code zeigt, wie der [TradeLockerMessageAdapter](xref:StockSharp.TradeLocker.TradeLockerMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TradeLockerMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Ihr Wert>",
	Password = "<Ihr Wert>".To<SecureString>(),
	AccountId = "<Ihr Wert>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

# Adapterinitialisierung S&P Global Commodity Insights

Der folgende Code zeigt, wie der [SpGlobalMessageAdapter](xref:StockSharp.SpGlobal.SpGlobalMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new SpGlobalMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Ihr Wert>",
	Password = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

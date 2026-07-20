# Adapterinitialisierung Goldman Sachs Marquee

Der folgende Code zeigt, wie der [MarqueeMessageAdapter](xref:StockSharp.Marquee.MarqueeMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MarqueeMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Ihr Wert>",
	ClientSecret = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

# LMAX-Adapter initialisieren

Der folgende Code zeigt, wie der [LmaxMessageAdapter](xref:StockSharp.LMAX.LmaxMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new LmaxMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	IsDemo = true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

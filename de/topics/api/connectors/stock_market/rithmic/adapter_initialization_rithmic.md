# Rithmic Adapter-Initialisierung

Der folgende Code zeigt, wie der [RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new RithmicMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	CertFile = "<Path to certificate file>",
	Server = RithmicServers.Real,
	//Server = RithmicServers.Test,
	//Server = RithmicServers.Simulator,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

Eine alternative und bequemere Möglichkeit ist die Erweiterungsmethode `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<RithmicMessageAdapter>(a =>
{
	a.UserName = "<Your Login>";
	a.Password = "<Your Password>".To<SecureString>();
	a.CertFile = "<Path to certificate file>";
	a.Server = RithmicServers.Real;
});
```

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

# Rithmic Adapter-Initialisierung

Der folgende Code zeigt, wie der [RithmicMessageAdapter](xref:StockSharp.Rithmic.RithmicMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new RithmicMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Ihr Login>",
	Password = "<Ihr Passwort>".To<SecureString>(),
	CertFile = "<Pfad zur Zertifikatsdatei>",
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
	a.UserName = "<Ihr Login>";
	a.Password = "<Ihr Passwort>".To<SecureString>();
	a.CertFile = "<Pfad zur Zertifikatsdatei>";
	a.Server = RithmicServers.Real;
});
```

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

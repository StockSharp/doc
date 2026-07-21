# Initialisierung des Talos-Adapters

Der folgende Code zeigt, wie man den [TalosMessageAdapter](xref:StockSharp.Talos.TalosMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TalosMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Ihr Wert>".To<EndPoint>(),
	SenderCompId = "<Ihr Wert>",
	TargetCompId = "<Ihr Wert>",
	Login = "<Ihr Wert>",
	Password = "<Ihr Wert>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

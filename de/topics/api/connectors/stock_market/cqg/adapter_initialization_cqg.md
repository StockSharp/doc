# Adapterinitialisierung CQG

Der folgende Code zeigt, wie der [CqgComMessageAdapter](xref:StockSharp.Cqg.Com.CqgComMessageAdapter) und der [CqgContinuumMessageAdapter](xref:StockSharp.Cqg.Continuum.CqgContinuumMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben werden.

1. **CQG COM**, Verbindung über den lokalen **CQG Integrated Client**:

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CqgComMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Ihr Login>",
	Password = "<Ihr Passwort>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

2. **CQG Continuum**, direkte Verbindung zum Server:

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CqgContinuumMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<Ihr Login>",
	Password = "<Ihr Passwort>".To<SecureString>(),
	Address = "<Address>".To<IPAddress>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

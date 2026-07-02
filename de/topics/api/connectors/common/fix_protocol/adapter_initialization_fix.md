# FIX-Adapterinitialisierung

Der folgende Code zeigt, wie [FixMessageAdapter](xref:StockSharp.Fix.FixMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FixMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your Login>",
	Password = "<Your Password>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

Eine alternative und komfortablere Möglichkeit ist die Verwendung der Erweiterungsmethode `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<FixMessageAdapter>(a =>
{
	a.Login = "<Your Login>";
	a.Password = "<Your Password>".To<SecureString>();
	a.Address = "<Address>".To<EndPoint>();
});
```

## Siehe auch

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

# Adapterinitialisierung Interactive Brokers

Der folgende Code zeigt, wie der [InteractiveBrokersMessageAdapter](xref:StockSharp.InteractiveBrokers.InteractiveBrokersMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Your Address>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

Eine alternative und bequemere Möglichkeit ist die Verwendung der Erweiterungsmethode `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<InteractiveBrokersMessageAdapter>(a =>
{
	a.Address = "<Your Address>".To<EndPoint>();
});
```

## Siehe auch

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

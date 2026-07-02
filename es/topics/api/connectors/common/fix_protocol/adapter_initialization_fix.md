# Inicialización del adaptador FIX

El código a continuación muestra cómo inicializar [FixMessageAdapter](xref:StockSharp.Fix.FixMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

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

Una forma alternativa y más conveniente es usar el método de extensión `AddAdapter<T>()`:

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

## Vea también

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

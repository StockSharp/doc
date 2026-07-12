# Inicialización del adaptador Interactive Brokers

El siguiente código muestra cómo inicializar [InteractiveBrokersMessageAdapter](xref:StockSharp.InteractiveBrokers.InteractiveBrokersMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Su dirección>".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

Una forma alternativa y más cómoda es usar el método de extensión `AddAdapter<T>()`:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<InteractiveBrokersMessageAdapter>(a =>
{
	a.Address = "<Su dirección>".To<EndPoint>();
});
```

## Ver también

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

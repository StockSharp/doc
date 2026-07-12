# Inicialización del adaptador OpenECry

El siguiente código muestra cómo inicializar [OpenECryMessageAdapter](xref:StockSharp.OpenECry.OpenECryMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new OpenECryMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Su usuario>",
	Password = "<Su contraseña>".To<SecureString>(),
	Address = "<Address>".To<EndPoint>(),
	EnableOECLogging = true,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

# Inicialización del adaptador FXCM

El siguiente código muestra cómo inicializar [FxcmMessageAdapter](xref:StockSharp.Fxcm.FxcmMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new FxcmMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Su usuario>",
	Password = "<Su contraseña>".To<SecureString>(),
	Address = "<Su dirección>".To<Uri>(),
	IsDemo = true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

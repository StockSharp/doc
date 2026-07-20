# Inicialización del adaptador Cboe DataShop / LiveVol

El siguiente código muestra cómo inicializar [CboeDataShopMessageAdapter](xref:StockSharp.CboeDataShop.CboeDataShopMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CboeDataShopMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Su valor>",
	Password = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

# Inicialización del adaptador Paradex

El siguiente código muestra cómo inicializar [ParadexMessageAdapter](xref:StockSharp.Paradex.ParadexMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ParadexMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Su clave API>".To<SecureString>(),
	Secret = "<Su secreto API>".To<SecureString>(),
	StarknetAccount = "<Su cuenta de Starknet>",
	StarknetPrivateKey = "<Su clave privada de Starknet>".To<SecureString>(),
	Section = ParadexSections.Derivatives,
	AuthPath = "/v1/auth",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

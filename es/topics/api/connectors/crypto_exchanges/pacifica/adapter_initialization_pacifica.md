# Inicialización del adaptador Pacifica

El siguiente código muestra cómo inicializar [PacificaMessageAdapter](xref:StockSharp.Pacifica.PacificaMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PacificaMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Su valor>",
	PrivateKey = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)

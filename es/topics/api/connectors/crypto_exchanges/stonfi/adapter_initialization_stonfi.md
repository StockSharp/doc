# Inicialización del adaptador: STON.fi

El siguiente código inicializa [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new StonFiMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<La dirección de su monedero TON>",
	Mnemonic = "<Su frase mnemónica de 24 palabras>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Indique los datos del monedero y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_stonfi.md).

## Véase también

[Configuración del conector](configuration_stonfi.md)

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)

# Inicialización del adaptador: B3 UP2DATA

El siguiente código inicializa [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter) y lo agrega a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new B3Up2DataMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Indique las credenciales y las demás propiedades necesarias descritas en la página de [Configuración del conector](configuration_b3_up2data.md).

## Véase también

[Configuración del conector](configuration_b3_up2data.md)

[ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)

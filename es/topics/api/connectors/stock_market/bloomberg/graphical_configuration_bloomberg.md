# Configuración gráfica: Bloomberg BLPAPI and EMSX

En todos los productos StockSharp, la conexión se configura en la [ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md).

- `ServerAddress` - dirección del servicio. Valor predeterminado: `new DnsEndPoint("localhost", 8194)`.
- `SdkPath` - ruta de un archivo o directorio local.
- `IsEmsxEnabled` - interruptor que controla el comportamiento del conector.
- `EmsxService` - parámetro de conexión. Valor predeterminado: `//blp/emapisvc`.
- `Broker` - parámetro de conexión.

## Véase también

[Conectores](../../../connectors.md)

[Configuración gráfica](../../graphical_configuration.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)

[Creación de un conector propio](../../creating_own_connector.md)

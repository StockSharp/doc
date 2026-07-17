# Configuración gráfica: LSEG Real-Time

En todos los productos StockSharp, la conexión se configura en la [ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md).

- `AuthenticationMode` - modo u opción del conector.
- `Address` - dirección del servicio.
- `StandbyAddress` - dirección del servicio.
- `IsHotStandby` - interruptor que controla el comportamiento del conector.
- `Login` - identificador de cuenta o cliente.
- `Password` - credencial de autenticación.
- `ClientId` - identificador de cuenta o cliente.
- `Secret` - credencial de autenticación.
- `ApplicationId` - identificador de cuenta o cliente. Valor predeterminado: `256`.
- `Service` - parámetro de conexión. Valor predeterminado: `ELEKTRON_DD`.
- `Region` - parámetro de conexión. Valor predeterminado: `us-east-1`.
- `Position` - parámetro de conexión.
- `Scope` - parámetro de conexión. Valor predeterminado: `trapi.streaming.pricing.read`.
- `AuthUrl` - dirección del servicio.
- `DiscoveryUrl` - dirección del servicio. Valor predeterminado: `https://api.refinitiv.com/streaming/pricing/v1/`.

## Véase también

[Conectores](../../../connectors.md)

[Configuración gráfica](../../graphical_configuration.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)

[Creación de un conector propio](../../creating_own_connector.md)

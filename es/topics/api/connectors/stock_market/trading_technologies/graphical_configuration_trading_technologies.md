# Configuración gráfica: Tecnologías de negociación

En todos los productos StockSharp, la conexión se configura en la [ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md).

- `SdkPath` - ruta de un archivo o directorio local.
- `AppSecretKey` - credencial de autenticación.
- `Environment` - modo u opción del conector. Valor predeterminado: `TradingTechnologiesEnvironments.ProdSim`.
- `InitializationTimeout` - intervalo de tiempo. Valor predeterminado: `5000`.
- `MarketDepth` - parámetro numérico del conector. Valor predeterminado: `20`.
- `IsBinaryProtocol` - interruptor que controla el comportamiento del conector. Valor predeterminado: `true`.
- `IsOptionsEnabled` - interruptor que controla el comportamiento del conector. Valor predeterminado: `true`.

## Véase también

[Conectores](../../../connectors.md)

[Configuración gráfica](../../graphical_configuration.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)

[Creación de un conector propio](../../creating_own_connector.md)

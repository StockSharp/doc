# Configuración gráfica: OpenMarkets

En todos los productos StockSharp, la conexión se configura en la [ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md).

- `ClientId` - identificador de cuenta o cliente.
- `ClientSecret` - credencial de autenticación.
- `AccountCode` - identificador de cuenta o cliente.
- `IsTest` - interruptor que controla el comportamiento del conector.
- `DataSource` - parámetro de conexión. Valor predeterminado: `OpenMarketsExtensions.DefaultDataSource`.
- `DefaultExchange` - parámetro de conexión. Valor predeterminado: `OpenMarketsExtensions.DefaultExchange`.
- `DefaultDestination` - parámetro de conexión. Valor predeterminado: `OpenMarketsExtensions.DefaultExchange`.
- `OrderGiver` - parámetro de conexión.
- `OrderTaker` - parámetro de conexión.
- `DefaultPriceMultiplier` - parámetro numérico del conector. Valor predeterminado: `0.01m`.
- `DepthPollingInterval` - intervalo de tiempo. Valor predeterminado: `TimeSpan.FromSeconds(2)`.

## Véase también

[Conectores](../../../connectors.md)

[Configuración gráfica](../../graphical_configuration.md)

[Guardar y cargar la configuración](../../save_and_load_settings.md)

[Creación de un conector propio](../../creating_own_connector.md)

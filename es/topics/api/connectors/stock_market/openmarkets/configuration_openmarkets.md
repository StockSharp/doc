# Configuración del conector: OpenMarkets

Obtenga las credenciales del proveedor e indique los parámetros de conexión.

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

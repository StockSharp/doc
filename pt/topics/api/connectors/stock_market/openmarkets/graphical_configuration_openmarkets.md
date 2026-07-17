# Configuração gráfica: OpenMarkets

Em todos os produtos StockSharp, a conexão é configurada na [janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md).

- `ClientId` - identificador da conta ou do cliente.
- `ClientSecret` - credencial de autenticação.
- `AccountCode` - identificador da conta ou do cliente.
- `IsTest` - interruptor que controla o comportamento do conector.
- `DataSource` - parâmetro de conexão. Valor predefinido: `OpenMarketsExtensions.DefaultDataSource`.
- `DefaultExchange` - parâmetro de conexão. Valor predefinido: `OpenMarketsExtensions.DefaultExchange`.
- `DefaultDestination` - parâmetro de conexão. Valor predefinido: `OpenMarketsExtensions.DefaultExchange`.
- `OrderGiver` - parâmetro de conexão.
- `OrderTaker` - parâmetro de conexão.
- `DefaultPriceMultiplier` - parâmetro numérico do conector. Valor predefinido: `0.01m`.
- `DepthPollingInterval` - intervalo de tempo. Valor predefinido: `TimeSpan.FromSeconds(2)`.

## Consulte também

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Guardar e carregar configurações](../../save_and_load_settings.md)

[Criar um conector próprio](../../creating_own_connector.md)

# Configuração do conector: OpenMarkets

Obtenha as credenciais junto do fornecedor e indique os parâmetros de conexão.

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

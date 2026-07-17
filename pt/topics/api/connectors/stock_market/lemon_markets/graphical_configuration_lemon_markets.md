# Configuração gráfica: lemon.markets

Em todos os produtos StockSharp, a conexão é configurada na [janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md).

- `ApiKey` - credencial de autenticação.
- `IsDemo` - interruptor que controla o comportamento do conector. Valor predefinido: `true`.
- `AccountId` - identificador da conta ou do cliente.
- `SecuritiesAccountId` - identificador da conta ou do cliente.
- `DataPrivacyPrincipal` - parâmetro de conexão.
- `DataPrivacyJustification` - parâmetro de conexão. Valor predefinido: `app_usage-stocksharp`.
- `PersonId` - identificador da conta ou do cliente.
- `DefaultFeeAmount` - parâmetro numérico do conector.
- `IsAppropriatenessConsentAccepted` - interruptor que controla o comportamento do conector.
- `PollingInterval` - intervalo de tempo. Valor predefinido: `TimeSpan.FromSeconds(10)`.

## Consulte também

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Guardar e carregar configurações](../../save_and_load_settings.md)

[Criar um conector próprio](../../creating_own_connector.md)

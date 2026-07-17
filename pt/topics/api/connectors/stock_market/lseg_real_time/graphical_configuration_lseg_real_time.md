# Configuração gráfica: LSEG Real-Time

Em todos os produtos StockSharp, a conexão é configurada na [janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md).

- `AuthenticationMode` - modo ou opção do conector.
- `Address` - endereço do serviço.
- `StandbyAddress` - endereço do serviço.
- `IsHotStandby` - interruptor que controla o comportamento do conector.
- `Login` - identificador da conta ou do cliente.
- `Password` - credencial de autenticação.
- `ClientId` - identificador da conta ou do cliente.
- `Secret` - credencial de autenticação.
- `ApplicationId` - identificador da conta ou do cliente. Valor predefinido: `256`.
- `Service` - parâmetro de conexão. Valor predefinido: `ELEKTRON_DD`.
- `Region` - parâmetro de conexão. Valor predefinido: `us-east-1`.
- `Position` - parâmetro de conexão.
- `Scope` - parâmetro de conexão. Valor predefinido: `trapi.streaming.pricing.read`.
- `AuthUrl` - endereço do serviço.
- `DiscoveryUrl` - endereço do serviço. Valor predefinido: `https://api.refinitiv.com/streaming/pricing/v1/`.

## Consulte também

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Guardar e carregar configurações](../../save_and_load_settings.md)

[Criar um conector próprio](../../creating_own_connector.md)

# Configuração gráfica: Tecnologias de negociação

Em todos os produtos StockSharp, a conexão é configurada na [janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md).

- `SdkPath` - caminho para um ficheiro ou diretório local.
- `AppSecretKey` - credencial de autenticação.
- `Environment` - modo ou opção do conector. Valor predefinido: `TradingTechnologiesEnvironments.ProdSim`.
- `InitializationTimeout` - intervalo de tempo. Valor predefinido: `5000`.
- `MarketDepth` - parâmetro numérico do conector. Valor predefinido: `20`.
- `IsBinaryProtocol` - interruptor que controla o comportamento do conector. Valor predefinido: `true`.
- `IsOptionsEnabled` - interruptor que controla o comportamento do conector. Valor predefinido: `true`.

## Consulte também

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Guardar e carregar configurações](../../save_and_load_settings.md)

[Criar um conector próprio](../../creating_own_connector.md)

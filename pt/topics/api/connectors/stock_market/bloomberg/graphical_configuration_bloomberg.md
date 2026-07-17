# Configuração gráfica: Bloomberg BLPAPI and EMSX

Em todos os produtos StockSharp, a conexão é configurada na [janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md).

- `ServerAddress` - endereço do serviço. Valor predefinido: `new DnsEndPoint("localhost", 8194)`.
- `SdkPath` - caminho para um ficheiro ou diretório local.
- `IsEmsxEnabled` - interruptor que controla o comportamento do conector.
- `EmsxService` - parâmetro de conexão. Valor predefinido: `//blp/emapisvc`.
- `Broker` - parâmetro de conexão.

## Consulte também

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Guardar e carregar configurações](../../save_and_load_settings.md)

[Criar um conector próprio](../../creating_own_connector.md)

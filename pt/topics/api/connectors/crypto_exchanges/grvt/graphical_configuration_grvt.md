# Configuração gráfica do GRVT

Para todos os produtos [S#](../../../../api.md), a configuração gráfica da conexão é realizada na [janela de configurações de conexão](../../../graphical_user_interface/connection_settings_window.md):

- `Key` - chave da API para autenticação.
- `Secret` - segredo da API para autenticação.
- `SubAccountId` - identificador da subconta de negociação.
- `Environment` - ambiente do serviço: rede de produção ou de teste.
- `EdgeEndpoint` - ponto de acesso da API de autenticação.
- `MarketDataEndpoint` - ponto de acesso da API de dados de mercado.
- `TradingEndpoint` - ponto de acesso da API de negociação.
- `MarketWebSocketEndpoint` - ponto de acesso WebSocket de dados de mercado.
- `TradingWebSocketEndpoint` - ponto de acesso WebSocket de negociação.
- `SnapshotInterval` - intervalo dos instantâneos de dados de mercado, em milissegundos.
- `MarketDepth` - número de níveis solicitados do livro de ofertas.

## Veja também

[Conectores](../../../connectors.md)

[Configuração gráfica](../../graphical_configuration.md)

[Criando Seu Próprio Conector](../../creating_own_connector.md)

[Salvar e carregar configurações](../../save_and_load_settings.md)

# SET Market Data

O **conector SET Market Data** conecta o StockSharp a um serviço profissional de dados e análises de mercado. Ele traduz dados e operações específicos do provedor para o modelo unificado de mensagens do StockSharp, permitindo usar as mesmas assinaturas e fluxos em diferentes mercados.

## Principais recursos

- Cobertura típica: ações.
- Pesquisa de instrumentos e dados de referência do provedor.
- Dados de mercado, empresas, registros, divulgações e referência suportados pelo provedor.
- Dados de mercado suportados pelo adaptador: cotações de nível 1 e livros de ordens.
- Assinaturas em tempo real pelo fluxo de dados do provedor.
- Este adaptador é destinado ao acesso a dados e não encaminha ordens.
- Transportes, sessões e formatos específicos do provedor ficam ocultos atrás da API padrão do StockSharp.

## Uso típico

Use-o para alimentar gráficos, armazenamento de mercado, análises, pesquisas e testes de estratégias com dados do provedor.

Instrumentos, profundidade de dados, permissões de negociação, limites e disponibilidade dependem de SET Market Data, do plano de API e da conta conectada.

## Veja também

[Configuração do conector](set_market_data/configuration_set_market_data.md)

[Configuração gráfica](set_market_data/graphical_configuration_set_market_data.md)

[Inicialização do adaptador](set_market_data/adapter_initialization_set_market_data.md)

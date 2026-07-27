# SEC API

O **conector SEC API** conecta o StockSharp a um serviço de dados financeiros e informações de referência. Ele traduz dados específicos do provedor para o modelo unificado de mensagens do StockSharp, permitindo usar as mesmas assinaturas e fluxos com diferentes fontes de dados.

## Principais recursos

- Cobertura típica: ações e dados de referência de emissores.
- Pesquisa de instrumentos e dados de referência do provedor.
- Dados de mercado, empresas, registros, divulgações e referência suportados pelo provedor.
- Dados de mercado suportados pelo adaptador: notícias financeiras e divulgações financeiras.
- Solicitações de dados históricos para gráficos, análises e testes de estratégias.
- Assinaturas em tempo real pelo fluxo de dados do provedor.
- Este adaptador é destinado ao acesso a dados e não encaminha ordens.
- Transportes, sessões e formatos específicos do provedor ficam ocultos atrás da API padrão do StockSharp.

## Uso típico

Use-o para dados mestres de valores mobiliários, monitoramento de divulgações, pesquisa de emissores, fluxos de conformidade e análise histórica.

Instrumentos, profundidade de dados, permissões de negociação, limites e disponibilidade dependem de SEC API, do plano de API e da conta conectada.

## Veja também

[Configuração do conector](sec_api/configuration_sec_api.md)

[Configuração gráfica](sec_api/graphical_configuration_sec_api.md)

[Inicialização do adaptador](sec_api/adapter_initialization_sec_api.md)

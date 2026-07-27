# Financial Datasets

O **conector Financial Datasets** conecta o StockSharp a um serviço profissional de dados e análises de mercado. Ele traduz dados e operações específicos do provedor para o modelo unificado de mensagens do StockSharp, permitindo usar as mesmas assinaturas e fluxos em diferentes mercados.

## Principais recursos

- Cobertura típica: ações.
- Pesquisa de instrumentos e dados de referência do provedor.
- Dados de mercado, empresas, registros, divulgações e referência suportados pelo provedor.
- Dados de mercado suportados pelo adaptador: cotações de nível 1, velas, notícias financeiras e divulgações financeiras.
- Solicitações de dados históricos para gráficos, análises e testes de estratégias.
- Assinaturas em tempo real pelo fluxo de dados do provedor.
- Este adaptador é destinado ao acesso a dados e não encaminha ordens.
- Transportes, sessões e formatos específicos do provedor ficam ocultos atrás da API padrão do StockSharp.

## Uso típico

Use-o para alimentar gráficos, armazenamento de mercado, análises, pesquisas e testes de estratégias com dados do provedor.

Instrumentos, profundidade de dados, permissões de negociação, limites e disponibilidade dependem de Financial Datasets, do plano de API e da conta conectada.

## Veja também

[Configuração do conector](financial_datasets/configuration_financial_datasets.md)

[Configuração gráfica](financial_datasets/graphical_configuration_financial_datasets.md)

[Inicialização do adaptador](financial_datasets/adapter_initialization_financial_datasets.md)

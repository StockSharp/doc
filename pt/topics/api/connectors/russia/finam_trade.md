# API de negociação da Finam

O **conector da API de negociação da Finam** liga aplicações StockSharp às contas de corretagem e aos dados de mercado fornecidos pela Finam. Instrumentos, cotações, ordens, negócios e o estado da carteira são convertidos para o modelo unificado de mensagens do StockSharp.

## Principais recursos

- Pesquisa de ações, títulos, moedas, fundos, futuros e opções disponíveis na Finam.
- Cotações de nível 1, livros de ofertas, negócios públicos e velas por período.
- Consulta de velas históricas e assinaturas de mercado em tempo real.
- Envio de ordens a mercado, limitadas, stop e stop-limit, além de cancelamento.
- Atualizações do estado das ordens, negócios próprios, saldos em dinheiro e posições.
- Troca automática do segredo da API por um token de sessão de curta duração.
- Endereços REST e WebSocket configuráveis para gateways compatíveis e ambientes de teste.

## Uso típico

O conector pode ser usado em robôs de negociação, terminais, monitores de carteira e serviços de gerenciamento de ordens que precisem de uma única interface StockSharp para dados de mercado e negociação na Finam.

É necessário um segredo da API de negociação da Finam. Uma conta pode ser selecionada explicitamente; se o identificador ficar vazio, o conector usa a primeira conta disponível para o token. Os instrumentos seguem o formato Finam `ticker@MIC`. Mercados, profundidade do histórico, dados em tempo real, permissões de negociação e limites de requisição dependem da conta e das condições do serviço Finam.

## Veja também

[Configuração do conector](finam_trade/configuration_finam_trade.md)

[Configuração gráfica](finam_trade/graphical_configuration_finam_trade.md)

[Inicialização do adaptador](finam_trade/adapter_initialization_finam_trade.md)

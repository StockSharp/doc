# StocksTrader

O **StocksTrader** conecta o StockSharp à API REST oficial do StocksTrader.

O conector oferece descoberta de contas demo e reais, consulta periódica do estado da conta, pesquisa de instrumentos e os instantâneos mais recentes de compra, venda e último preço. A negociação abrange ordens a mercado, limitadas e stop, modificação e cancelamento de ordens pendentes, alteração de stop-loss e take-profit das posições abertas, fechamento de posições e histórico de ordens e negócios.

O StocksTrader não possui API de fluxo de dados de mercado, portanto uma solicitação de nível 1 retorna o instantâneo mais recente disponível e é concluída imediatamente, enquanto ordens, negócios e o estado da conta são consultados periodicamente.

Antes de conectar, crie um token bearer no terminal web do StocksTrader.

## Veja também

[Configuração do conector](stocks_trader/configuration_stocks_trader.md)

[Configuração gráfica](stocks_trader/graphical_configuration_stocks_trader.md)

[Inicialização do adaptador](stocks_trader/adapter_initialization_stocks_trader.md)

[Documentação oficial da API StocksTrader](https://api-doc.stockstrader.com/)

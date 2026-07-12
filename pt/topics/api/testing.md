# Testes históricos/Emulação

As estratégias escritas usando [Strategy](xref:StockSharp.Algo.Strategies.Strategy) podem ser testadas em três modos:

1. Teste com [Dados históricos](testing/historical_data.md). Com este tipo de dados, podem ser realizadas tanto a análise de mercado para encontrar padrões como a otimização dos parâmetros da estratégia.
2. Teste com [Dados aleatórios](testing/random_data.md). Uma ferramenta conveniente para testes iniciais da estratégia, para identificar erros nos algoritmos. Ou para testes automatizados executados de acordo com uma agenda.
3. Teste com [simulador](testing/simulator.md) baseado em dados recebidos de uma ligação real ao sistema de negociação (por exemplo, de [OpenECry](connectors/stock_market/openecry.md)), mas sem o registo real de ordens (a execução é emulada com base nos livros de ordens recebidos).

Ao usar todos os três modos, a ênfase máxima está no facto de o código da estratégia, escrito usando [Strategy](xref:StockSharp.Algo.Strategies.Strategy), não ser alterado ao alternar entre negociação real, testes e novamente negociação real. Isto é conseguido através da implementação da interface principal [IConnector](xref:StockSharp.BusinessEntities.IConnector), que é uma porta de entrada para o sistema de negociação. A forma como a interface é usada já foi mostrada na secção [API](../api.md). Em modo de teste, não será o sistema de negociação real a atuar como sistema de negociação, mas sim a emulação (dependendo do modo selecionado). Portanto, o código da estratégia nunca saberá se está a negociar com uma bolsa real ou com a emulação.


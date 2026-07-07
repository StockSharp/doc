# Estratégias no StockSharp

## Introdução

StockSharp fornece uma infraestrutura poderosa para criar, testar e executar estratégias de negociação. A base para desenvolver estratégias de negociação algorítmica é a classe base [Strategy](xref:StockSharp.Algo.Strategies.Strategy), que fornece um conjunto de funções e abstrações padrão para trabalhar com dados de mercado, executar operações de negociação e analisar resultados.

## Navegação

### Noções Básicas de Estratégias

- [Subscrições de Dados de Mercado em Estratégias](strategies/subscriptions.md) - Guia detalhado sobre a utilização de subscrições de dados de mercado em estratégias. Explica a criação e configuração de subscrições, a gestão do seu ciclo de vida e o acompanhamento do seu estado.

- [Indicadores em Estratégias](strategies/indicators.md) - Informação sobre o trabalho com indicadores de análise técnica em estratégias. Abrange a adição de indicadores a uma estratégia, o controlo da sua formação e a sua utilização na lógica de negociação.

- [Operações de Negociação em Estratégias](strategies/trading_operations.md) - Guia para executar operações de negociação em estratégias. Descreve métodos para criar e enviar ordens, fechar posições e acompanhar o seu estado.

- [Proteção de Posição](strategies/take_profit_and_stop_loss.md) - Descrição dos mecanismos de proteção de posições abertas usando Take Profit e Stop Loss. Examina abordagens locais e de servidor para proteção de posições.

- [Parâmetros de Estratégia](strategies/parameters.md) - Guia para trabalhar com parâmetros de estratégia através de [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1). Descreve como criar parâmetros configuráveis, definir a sua apresentação na GUI e utilizá-los na otimização.

- [Logging em Estratégias](strategies/logging.md) - Guia para usar o mecanismo de logging em estratégias para acompanhar e depurar o desempenho do algoritmo.

### Funcionalidades Avançadas

- [Compatibilidade da Estratégia com Plataformas](strategies/compatibility.md) - Recomendações para criar estratégias compatíveis com várias plataformas StockSharp: [Designer](../designer.md), [Shell](../shell.md), [Runner](../runner.md) e testes na cloud.

- [APIs de Alto Nível em Estratégias](strategies/high_level_api.md) - Descrição de métodos de alto nível para simplificar o trabalho com subscrições, indicadores, gráficos e proteção de posições. Explica como escrever código mais limpo ao focar-se na lógica de negociação.

- [Trabalhar com Gráficos em Estratégias](strategies/chart.md) - Guia para visualizar dados da estratégia num gráfico. Explica como aceder ao gráfico, criar áreas, adicionar elementos e renderizar dados.

- [Guardar e Carregar Definições](strategies/settings_saving_and_loading.md) - Descrição do mecanismo para guardar e carregar definições da estratégia através dos métodos [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) e [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)).

- [Carregamento de Estado](strategies/orders_and_trades_loading.md) - Guia para carregar ordens e negócios previamente executados numa estratégia, por exemplo, ao reiniciar uma estratégia durante uma sessão de negociação.

- [Arredondamento de Preços](strategies/shrink_price.md) - Guia para arredondar corretamente preços em estratégias usando o método [ShrinkPrice](xref:StockSharp.BusinessEntities.EntitiesExtensions.ShrinkPrice(StockSharp.BusinessEntities.Security,System.Decimal)).

- [Tipo Unit](strategies/unit_type.md) - Descrição do tipo de dados [Unit](xref:StockSharp.Messages.Unit) para simplificar operações aritméticas sobre quantidades como percentagens, pontos ou pips.

- [Modelo de Eventos](strategies/event_model.md) - Explicação do modelo de eventos de estratégia baseado em [IMarketRule](xref:StockSharp.Algo.IMarketRule). Abrange a criação de regras para reagir a eventos de mercado, combinar condições e gerir ciclos de vida das regras.

## Começar o Desenvolvimento de Estratégias

Para começar a desenvolver a sua própria estratégia, recomenda-se:

1. Familiarizar-se com os fundamentos do trabalho com estratégias para compreender os princípios gerais das estratégias no StockSharp.

2. Estudar a secção [Subscrições de Dados de Mercado em Estratégias](strategies/subscriptions.md) para compreender o mecanismo de receção e processamento de dados de mercado.

3. Rever a secção [Indicadores em Estratégias](strategies/indicators.md) para compreender o trabalho com indicadores de análise técnica.

4. Explorar a secção [Operações de Negociação em Estratégias](strategies/trading_operations.md) para compreender os mecanismos das operações de negociação.

5. Examinar a secção [Parâmetros de Estratégia](strategies/parameters.md) para aprender sobre mecanismos de configuração de estratégias.

6. Conhecer a secção [APIs de Alto Nível em Estratégias](strategies/high_level_api.md) para simplificar o código da estratégia usando funções de alto nível integradas.

## Teste de Estratégias

StockSharp fornece vários métodos para testar estratégias:

- **Teste em Dados Históricos** - Permite avaliar a eficácia da estratégia em dados históricos.
- **Otimização de Parâmetros** - Ajuda a encontrar valores ideais para os parâmetros da estratégia.
- **Teste em Conta Virtual** - Permite verificar o desempenho da estratégia em modo de tempo real sem arriscar fundos reais.

Descrições detalhadas dos métodos de teste e da avaliação de desempenho de estratégias podem ser encontradas na secção [Testes](../api/testing.md).

# Documentação da API

## Visão geral

A API do StockSharp, também conhecida como API S#, é um kit de desenvolvimento de software (SDK) para construir aplicações de negociação como [Designer](designer.md), [Terminal](terminal.md) e outras ferramentas de negociação personalizadas. Ela fornece a infraestrutura central para dados de mercado, roteamento de ordens, execução de estratégias, testes e componentes de interface do utilizador para negociação.

## Recursos

- **Scripting de estratégias**: A API do StockSharp permite que os utilizadores escrevam e executem estratégias de negociação diretamente no [Designer](designer/strategies/using_code.md). As estratégias podem ser desenvolvidas, testadas e implantadas usando C#, F# ou Python.

- **Ferramentas de análise**: A API integra-se com o Hydra para [análise detalhada de dados de mercado](hydra/analytics.md). Ela suporta processamento de dados, armazenamento e fluxos de trabalho analíticos personalizados.

- **Desenvolvimento de aplicações personalizadas**: Os desenvolvedores podem usar a API do StockSharp para construir [soluções de negociação](api/examples.md) independentes, em vez de depender apenas do scripting de aplicações integrado.

- **Conectores e controlos gráficos**: A API inclui muitos [Conectores](api/connectors.md) para dados de mercado em tempo real e acesso à negociação. Ela também fornece componentes personalizáveis de [interface gráfica do utilizador](api/graphical_user_interface.md) para construir plataformas de negociação profissionais.

## Arquitetura

A API do StockSharp é construída em torno da modularidade e da [extensibilidade](api/connectors/creating_own_connector.md). Os desenvolvedores podem estendê-la com plugins e módulos adicionais sem alterar o sistema central. Essa arquitetura ajuda a construir aplicações de negociação escaláveis e de fácil manutenção.

## Código aberto

O núcleo da API do StockSharp é de código aberto. O código-fonte está disponível no GitHub, para que os desenvolvedores possam estudá-lo, modificá-lo e contribuir com melhorias.

## Repositório GitHub

O código-fonte oficial da API do StockSharp está disponível no repositório GitHub:

[Repositório GitHub do StockSharp](https://github.com/stocksharp/stocksharp)

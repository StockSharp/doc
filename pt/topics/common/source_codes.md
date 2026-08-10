# Códigos-fonte

O código aberto do [S#](../api.md) está dividido entre vários repositórios. O [repositório principal do StockSharp](https://github.com/StockSharp/StockSharp) contém o modelo de mensagens, as entidades de negócio, as abstrações comuns dos conectores, os algoritmos, as ferramentas de teste e outras bases da plataforma. As implementações de conectores específicas de cada fornecedor não são armazenadas no repositório principal.

Todos os conectores abertos específicos de cada fornecedor são mantidos em [StockSharp\/Connectors](https://github.com/StockSharp/Connectors). Cada conector é um projeto .NET independente, e o repositório inclui `Connectors.slnx` para os compilar em conjunto.

O motor independente de gráficos para navegador e o conjunto de gráficos para terminais web são mantidos em [StockSharp\/JS-Charts](https://github.com/StockSharp/JS-Charts). Consulte [Gráficos JavaScript](../api/graphical_user_interface/charts/javascript_charts.md).

[Instruções para usar o GitHub](https://docs.github.com/pt/get-started/start-your-journey/hello-world)

Lista de componentes disponíveis com código-fonte:

- Classes comuns para criar as suas próprias ligações.
- Formato do armazenamento de dados de mercado.
- Simulador de negociação.
- Simulador histórico.
- Indicadores (mais de 140) de análise técnica.
- Algoritmos para calcular lucro/prejuízo, deslizamento e atraso.
- Algoritmos para construir velas de qualquer período, bem como velas não baseadas no tempo (tick, range, etc.).
- Registo.
- Importação e exportação.

Os códigos-fonte de todos os componentes fechados, bem como dos programas prontos a usar, estão disponíveis mediante compra. Para mais informações sobre o custo dos códigos-fonte, consulte [Custo do código-fonte](https://stocksharp.com/pt/store/?groups=22).

## Conteúdo recomendado

[Instruções de instalação](../api/setup.md)

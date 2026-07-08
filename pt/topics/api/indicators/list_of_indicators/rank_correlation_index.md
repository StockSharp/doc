# Rank Correlation Index

O **Rank Correlation Index (RCI)** é um oscilador baseado no coeficiente de correlação de postos de Spearman. Compara os postos dos preços
com os postos temporais dentro da janela móvel e mostra quão próximo o movimento recente está de uma sequência perfeitamente ascendente ou descendente.

Use a classe [RankCorrelationIndex](xref:StockSharp.Algo.Indicators.RankCorrelationIndex) para aceder ao indicador.

## Cálculo

1. Atribua a cada ponto de dados dentro da janela **Length** um posto temporal (1 para o valor mais antigo, `Length` para o mais recente).
2. Ordene os preços por valor (1 para o preço mais baixo, `Length` para o mais alto).
3. Calcule a diferença `d = RankTime - RankPrice` para cada barra.
4. Aplique a fórmula de Spearman:  
   `RCI = 1 - (6 × Σ d²) / (Length × (Length² - 1))`.

Quando multiplicado por 100, o indicador varia entre -100 e +100.

## Parâmetros

- **Length** - tamanho da janela para o procedimento de ordenação.

## Interpretação

- **RCI ≈ +100** - sequência perfeitamente ascendente (forte tendência de alta).
- **RCI ≈ -100** - sequência perfeitamente descendente (forte tendência de baixa).
- **RCI perto de 0** - mercado aleatório ou lateral.
- Divergências entre o preço e o RCI alertam para possíveis inversões.

O indicador é útil para avaliar tendências de curto prazo e detetar pontos de viragem, especialmente quando combinado com ferramentas de momentum.

![indicator_rank_correlation_index](../../../../images/indicator_rank_correlation_index.png)

## Ver também

[Momentum](momentum.md)
[RoC](roc.md)
[RSI](rsi.md)

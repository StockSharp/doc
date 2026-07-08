# HurstExponent

**Hurst Exponent** é uma medida estatística usada para avaliar a tendência de uma série temporal para seguir uma tendência ou regressar à média.

Para utilizar o indicador, é necessário usar a classe [HurstExponent](xref:StockSharp.Algo.Indicators.HurstExponent).

## Descrição

O expoente de Hurst (H) varia entre 0 e 1 e descreve a memória de longo prazo de uma série:
- **H > 0.5** indica comportamento tendencial persistente.
- **H < 0.5** mostra uma tendência para reversão à média.
- **H ≈ 0.5** corresponde a um passeio aleatório.

Pode ajudar a estimar a eficiência do mercado e revelar ciclos ou tendências emergentes.

## Parâmetros

- **Length** - o número de barras usadas no cálculo.

## Cálculo

Um método comum baseia-se na abordagem de intervalo reescalado (R/S):
1. Para cada janela de `Length` pontos, calcular os desvios acumulados em relação à média.
2. Encontrar o intervalo `R` como a diferença entre o desvio acumulado máximo e mínimo.
3. Calcular o desvio padrão `S` da janela.
4. Avaliar o intervalo reescalado `R/S`.
5. Estimar `H` como a inclinação de `log(R/S)` em relação a `log(Length)`.

Valores mais elevados do expoente sugerem comportamento tendencial mais forte, enquanto valores mais baixos implicam maior reversão à média.

![indicator_hurst_exponent](../../../../images/indicator_hurst_exponent.png)

## Ver Também

[Fractal Adaptive Moving Average](fractal_adaptive_moving_average.md)
[MMI](market_meanness_index.md)

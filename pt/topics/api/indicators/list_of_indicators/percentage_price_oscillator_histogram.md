# Percentage Price Oscillator Histogram

O **Percentage Price Oscillator Histogram (PPOH)** apresenta a distância entre a linha PPO e a sua linha de sinal como um histograma, ajudando os traders a avaliar imediatamente o equilíbrio do momentum.

Para usar o indicador, use a classe [PercentagePriceOscillatorHistogram](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorHistogram).

## Descrição

O histograma PPO deriva do Percentage Price Oscillator (PPO) padrão. Em vez de desenhar tanto a linha PPO como a linha de sinal, visualiza a diferença entre ambas como barras em torno do nível zero. Barras positivas indicam que a linha PPO está acima da linha de sinal (momentum bullish), enquanto barras negativas mostram que a linha PPO está abaixo da linha de sinal (momentum bearish).

O histograma reage rapidamente a alterações no spread entre o PPO e a linha de sinal, tornando-o adequado para detetar mudanças precoces na força da tendência ou identificar divergências de momentum.

## Cálculo

1. Calcular a linha PPO e a sua linha de sinal usando os períodos pretendidos.
2. Subtrair a linha de sinal da linha PPO para obter o valor do histograma.

```
Histogram = PPO - Signal
```

Valores acima de zero destacam pressão bullish, enquanto valores abaixo de zero refletem pressão bearish. A velocidade com que as barras do histograma se expandem ou contraem fornece pistas sobre a aceleração ou desaceleração do momentum.

## Interpretação

- **Cruzamentos da linha zero.** A passagem acima de zero confirma que a linha PPO cruzou acima da linha de sinal, sugerindo uma mudança bullish. A queda abaixo de zero indica um cruzamento bearish.
- **Surtos de momentum.** O crescimento rápido de barras positivas sugere reforço do momentum bullish; barras em contração apontam para enfraquecimento da força e uma possível reversão.
- **Divergências.** A divergência entre a ação do preço e o histograma pode alertar os traders para potencial esgotamento da tendência antes de este se tornar visível nos gráficos de preço.

![indicator_percentage_price_oscillator_histogram](../../../../images/indicator_percentage_price_oscillator_histogram.png)

## Ver também

- [Percentage Price Oscillator](percentage_price_oscillator.md)
- [Percentage Price Oscillator Signal](percentage_price_oscillator_signal.md)


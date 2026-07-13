# Histograma do PPO

O **Histograma do PPO (PPOH)** apresenta a distância entre a linha PPO e a sua linha de sinal como um histograma, ajudando os operadores a avaliar imediatamente o equilíbrio do momentum.

Para usar o indicador, use a classe [PercentagePriceOscillatorHistogram](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorHistogram).

## Descrição

O histograma PPO deriva do PPO padrão. Em vez de desenhar tanto a linha PPO como a linha de sinal, visualiza a diferença entre ambas como barras em torno do nível zero. Barras positivas indicam que a linha PPO está acima da linha de sinal (momentum de alta), enquanto barras negativas mostram que a linha PPO está abaixo da linha de sinal (momentum de baixa).

O histograma reage rapidamente a alterações no spread entre o PPO e a linha de sinal, tornando-o adequado para detetar mudanças precoces na força da tendência ou identificar divergências de momentum.

## Cálculo

1. Calcular a linha PPO e a sua linha de sinal usando os períodos pretendidos.
2. Subtrair a linha de sinal da linha PPO para obter o valor do histograma.

```
Histogram = PPO - sinal
```

Valores acima de zero destacam pressão de alta, enquanto valores abaixo de zero refletem pressão de baixa. A velocidade com que as barras do histograma se expandem ou contraem fornece pistas sobre a aceleração ou desaceleração do momentum.

## Interpretação

- **Cruzamentos da linha zero.** A passagem acima de zero confirma que a linha PPO cruzou acima da linha de sinal, sugerindo uma mudança de alta. A queda abaixo de zero indica um cruzamento de baixa.
- **Surtos de momentum.** O crescimento rápido de barras positivas sugere reforço do momentum de alta; barras em contração apontam para enfraquecimento da força e uma possível reversão.
- **Divergências.** A divergência entre a ação do preço e o histograma pode alertar os operadores para potencial esgotamento da tendência antes de este se tornar visível nos gráficos de preço.

![Gráfico do indicador Histograma do PPO](../../../../images/indicator_percentage_price_oscillator_histogram.png)

## Ver também

- [PPO](percentage_price_oscillator.md)
- [Sinal do PPO](percentage_price_oscillator_signal.md)

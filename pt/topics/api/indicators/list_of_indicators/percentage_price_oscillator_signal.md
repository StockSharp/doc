# Sinal do PPO

O indicador **Sinal do PPO (PPOS)** complementa o PPO padrão ao adicionar a linha de sinal associada, que é normalmente usada para filtrar trades.

Para usar o indicador, use a classe [PercentagePriceOscillatorSignal](xref:StockSharp.Algo.Indicators.PercentagePriceOscillatorSignal).

## Descrição

O oscilador percentual de preço (PPO) mede a diferença percentual entre duas médias móveis exponenciais (EMAs). A versão de sinal foca-se em suavizar a linha PPO com uma EMA adicional, ajudando os traders a reagir apenas a mudanças mais persistentes no momentum.

O indicador é formado pelos seguintes componentes:

1. **Linha PPO** - a diferença percentual entre as EMAs rápida e lenta.
2. **Linha de sinal** - uma EMA calculada a partir da linha PPO (9 períodos por predefinição).

Quando a linha PPO cruza acima da linha de sinal, sugere aumento do momentum de alta; o cruzamento abaixo sinaliza reforço do momentum de baixa. Permanecer acima ou abaixo da linha de sinal pode confirmar a força da tendência predominante.

## Cálculo

1. Calcular as EMAs rápida e lenta da série de preços selecionada.
2. Calcular a linha PPO como a distância percentual entre as EMAs rápida e lenta.
3. Suavizar a linha PPO com uma EMA para obter a linha de sinal.

```
FastEMA = EMA(Price, ShortPeriod)
SlowEMA = EMA(Price, LongPeriod)
PPO = ((FastEMA - SlowEMA) / SlowEMA) * 100
Signal = EMA(PPO, SignalPeriod)
```

## Interpretação

- **Cruzamentos de sinal.** Um sinal de alta ocorre quando a linha PPO cruza a linha de sinal a partir de baixo; o cruzamento oposto aponta para momentum de baixa.
- **Confirmação da tendência.** Manter-se acima da linha de sinal confirma uma tendência ascendente, enquanto permanecer abaixo suporta uma tendência descendente.
- **Divergências.** A divergência entre a ação do preço e a linha PPO enquanto interage com a linha de sinal pode antecipar reversões.

![indicator_percentage_price_oscillator_signal](../../../../images/indicator_percentage_price_oscillator_signal.png)

## Ver também

- [PPO](percentage_price_oscillator.md)
- [Histograma do PPO](percentage_price_oscillator_histogram.md)

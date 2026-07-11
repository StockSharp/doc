# SW

**onda senoidal (SW)** é um indicador técnico que utiliza a função matemática seno para identificar padrões cíclicos no movimento do preço. O indicador procura identificar e antecipar oscilações periódicas do mercado.

Para utilizar o indicador, é necessário usar a classe [SineWave](xref:StockSharp.Algo.Indicators.SineWave).

## Descrição

O indicador onda senoidal baseia-se na ideia de que os movimentos do mercado têm uma natureza cíclica e podem ser modelados através de funções sinusoidais. Este indicador é particularmente útil em mercados que se movem lateralmente ou que apresentam flutuações cíclicas previsíveis.

Características principais do indicador:
- Ajuda a identificar potenciais pontos de inversão do mercado
- Permite determinar a posição atual no ciclo
- Pode ser utilizado para prever movimentos futuros do preço

Sinais do indicador:
- Potencial compra quando a linha da onda sinusoidal atinge um mínimo e começa a virar para cima
- Potencial venda quando a linha atinge um máximo e começa a virar para baixo

## Parâmetros

- **Length** - período do ciclo da onda sinusoidal, que define o comprimento do ciclo em barras de preço.

## Cálculo

O cálculo do indicador onda senoidal baseia-se na utilização da função seno e na determinação do ciclo dominante no movimento do preço:

1. Determinar o ciclo dominante através de análise espectral ou de outro método de identificação de ciclos.

2. Aplicar a função seno para modelar o ciclo identificado:
   ```
   SineWave(t) = A * sin(2π * t / Length + φ)
   ```
   onde:
   - A - amplitude (altura da onda)
   - t - tempo ou barra atual
   - Length - comprimento do ciclo
   - φ - desfasamento de fase para alinhar a onda sinusoidal com o ciclo real do preço

3. Adicionalmente, pode ser calculado um indicador adiantado, que antecipa a onda sinusoidal principal em um quarto de ciclo:
   ```
   Lead(t) = A * sin(2π * t / Length + φ + π/2)
   ```

O indicador pode incluir componentes adicionais, como uma linha de tendência ou um filtro, para melhorar a precisão dos sinais.

![IndicatorSineWave](../../../../images/indicator_sine_wave.png)

## Ver também

[Ciclo de tendência de Schaff](schaff_trend_cycle.md)

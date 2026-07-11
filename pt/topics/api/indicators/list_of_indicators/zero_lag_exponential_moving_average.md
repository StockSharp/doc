# ZLEMA

**Média móvel exponencial de atraso zero (ZLEMA)** é uma versão modificada da média móvel exponencial (EMA), desenvolvida por John Ehlers. A ZLEMA foi concebida para eliminar ou reduzir significativamente o atraso inerente às médias móveis tradicionais.

Para usar o indicador, é necessário usar a classe [ZeroLagExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ZeroLagExponentialMovingAverage).

## Descrição

A média móvel exponencial de atraso zero (ZLEMA) foi criada para resolver o principal problema da maioria das médias móveis - o atraso do sinal. As médias móveis tradicionais ficam atrasadas em relação aos movimentos de preço devido à janela temporal usada no seu cálculo. A ZLEMA minimiza este atraso usando um mecanismo de correção baseado na diferença entre o preço atual e o preço no passado.

Principais vantagens da ZLEMA:
- Reação mais rápida a alterações de preço
- Menos atraso em comparação com médias móveis tradicionais
- Preservação do efeito de suavização característico da EMA

A ZLEMA pode ser usada para:
- Determinar a direção da tendência
- Encontrar pontos de entrada e saída
- Identificar níveis de suporte e resistência
- Criar sistemas de negociação baseados em cruzamentos

## Parâmetros

- **Length** - período de cálculo que determina o grau de suavização (semelhante ao período na EMA).

## Cálculo

O cálculo da ZLEMA baseia-se na eliminação do atraso através de previsão e inclui os seguintes passos:

1. Calcular o atraso como metade do período:
   ```
   lag = (Length - 1) / 2
   ```

2. Calcular o preço "sem tendência":
   ```
   preço sem tendência = 2 * Price - Price[lag]
   ```
   Este é um passo-chave que permite "olhar em frente" e eliminar o atraso.

3. Aplicar suavização exponencial ao preço sem tendência:
   ```
   k = 2 / (Length + 1)
   ZLEMA = k * preço sem tendência + (1 - k) * ZLEMA[previous]
   ```

O resultado é uma média móvel que acompanha o preço muito mais de perto do que uma EMA normal com o mesmo período, mantendo o efeito de suavização.

![IndicatorZeroLagExponentialMovingAverage](../../../../images/indicator_zero_lag_exponential_moving_average.png)

## Ver também

[EMA](ema.md)
[DEMA](dema.md)
[TEMA](tema.md)
[T3MA](t3_moving_average.md)

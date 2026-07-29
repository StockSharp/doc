# Histograma MACD

**Convergência/Divergência de Médias Móveis (MACD)** é um indicador de momentum que mostra a relação entre duas médias móveis do preço de um instrumento, apresentado como um histograma.

Para utilizar o indicador, deve ser usada a classe [MovingAverageConvergenceDivergenceHistogram](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergenceHistogram).

São usadas três médias móveis exponenciais com períodos diferentes para calcular o indicador. A média móvel rápida com um período mais curto (EMA_s) é subtraída da média móvel lenta com um período mais longo (EMA_l). A linha MACD é construída a partir dos valores obtidos.

MACD = EMA_s(P) − EMA_l(P)

Os períodos predefinidos são 12 e 26. Esta linha é então suavizada por uma terceira média móvel exponencial (EMA_a), normalmente com um período de 9, resultando na chamada linha de sinal MACD (Signal).

Signal = EMA_a(EMA_s(P) − EMA_l(P))

Estas duas curvas resultantes representam o MACD linear normal. Além disso, a linha zero, relativamente à qual as curvas flutuam, é normalmente marcada na janela do indicador.

Ao construir o histograma MACD, as barras do histograma mostram a diferença entre a linha de sinal e a linha MACD, simplificando ainda mais a perceção do indicador.

![Gráfico do indicador Histograma MACD](../../../../images/indicatormovingaverageconvergencedivergencehistogram.png)

## Ver Também

[MACD com linha de sinal](macd_with_signal_line.md)

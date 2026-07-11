# KAMA

**Média móvel adaptativa de Kaufman (AMA, KAMA, AMkA)** foi desenvolvida por Perry Kaufman para considerar o ruído e a volatilidade do mercado. O indicador KAMA pode ser usado para identificar pontos de inversão no tempo, determinar a tendência geral e filtrar movimentos de preços.

Para utilizar o indicador, deve ser usada a classe [KaufmanAdaptiveMovingAverage](xref:StockSharp.Algo.Indicators.KaufmanAdaptiveMovingAverage).

##### Definições do Indicador.

- Fast MA - a constante de suavização rápida;
- Slow MA - a constante de suavização lenta;
- Period - o período da média móvel de Kaufman.

![IndicatorKaufmanAdaptiveMovingAverage](../../../../images/indicatorkaufmanadaptivemovingaverage.png)

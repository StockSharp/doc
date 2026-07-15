# SuperTrend

**O indicador SuperTrend** é um indicador seguidor de tendência baseado no intervalo verdadeiro médio (ATR). Ajuda a identificar a direção atual da tendência e possíveis pontos de inversão.

Para utilizar o indicador, deve ser usada a classe [SuperTrend](xref:StockSharp.Algo.Indicators.SuperTrend).

## Descrição

O SuperTrend é construído com o preço médio e o valor do ATR. A linha do indicador muda de acima do preço para abaixo (e vice-versa) quando a tendência muda. Desta forma, o SuperTrend destaca visualmente a tendência atual até que o preço cruze a linha do indicador.

## Parâmetros

- **período ATR** - o período utilizado para o cálculo do ATR.
- **Multiplicador** - o fator que define a distância a que a linha é deslocada em relação ao preço médio.

## Cálculo

1. Calcular o ATR ao longo do período escolhido.
2. Calcular dois limites:
   ```
   UpperBand = (High + Low) / 2 + Multiplier * ATR
   LowerBand = (High + Low) / 2 - Multiplier * ATR
   ```
3. O SuperTrend inicialmente é igual a uma das bandas, dependendo da tendência atual.
4. Se o preço de fecho cruzar a linha SuperTrend, a direção da tendência muda e a linha passa para o lado oposto.

![Gráfico do indicador SuperTrend](../../../../images/indicator_supertrend.png)

## Ver também

[ATR](atr.md)
[SAR parabólico](parabolic_sar.md)

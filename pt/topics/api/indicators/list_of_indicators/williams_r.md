# %R

**%R de Williams (%R, intervalo percentual de Williams)** é um indicador de momentum que oscila entre 0 e -100 e apresenta níveis de sobrecompra e sobrevenda.

Para usar o indicador, deve ser usada a classe [WilliamsR](xref:StockSharp.Algo.Indicators.WilliamsR).
##### Cálculo

A fórmula para calcular o indicador intervalo percentual de Williams é semelhante à usada para calcular o Oscilador estocástico:

%R = - (MAX(HIGH(i - n)) - CLOSE(i)) / (MAX(HIGH(i - n)) - MIN(LOW(i - n))) * 100

onde:

CLOSE(i) - preço de fecho de hoje;
MAX(HIGH(i - n)) - o máximo mais alto dos últimos n períodos;
MIN(LOW(i - n)) - o mínimo mais baixo dos últimos n períodos.

O valor de n é definido como parâmetro do indicador.

![IndicatorWilliamsR](../../../../images/indicatorwilliamsr.png)

## Ver também

[ZigZag](zigzag.md)

# Momentum

O indicador **Momentum** mede a magnitude da alteração de preço de um instrumento financeiro durante um determinado período. Indica momentos de sobrecompra e sobrevenda quando a curva atinge valores máximos ou mínimos. Adicionar uma média móvel suavizada ao indicador melhora a interpretação das alterações de tendência.

Para utilizar o indicador, deve ser usada a classe [Momentum](xref:StockSharp.Algo.Indicators.Momentum).
##### Cálculo

Momentum é definido como o rácio entre o preço de hoje e o preço de n períodos atrás:

MOMENTUM = CLOSE(i) / CLOSE(i - n) * 100

onde:
CLOSE(i) — o preço de fecho da barra actual;
CLOSE(i - n) — o preço de fecho de n barras atrás.


![IndicatorMomentum](../../../../images/indicatormomentum.png)

## Ver Também

[Índice de fluxo monetário](money_flow_index.md)


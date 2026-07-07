# Elder Ray

O **Elder Ray Index** é um indicador composto de Alexander Elder que combina uma média móvel exponencial com os osciladores Bull Power
e Bear Power. Visualiza o equilíbrio entre compradores e vendedores e ajuda a identificar quando um dos lados perde o controlo.

Use a classe [ElderRay](xref:StockSharp.Algo.Indicators.ElderRay) para aceder ao indicador.

## Componentes

O indicador devolve uma estrutura [ElderRayValue](xref:StockSharp.Algo.Indicators.ElderRayValue) que contém:

- **EMA** - a média móvel exponencial de base dos preços de fecho;
- **Bull Power** - a distância entre o máximo da barra e a EMA;
- **Bear Power** - a distância entre o mínimo da barra e a EMA.

## Parâmetros

Elder Ray herda as definições de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** - período da EMA;
- **Alpha** - coeficiente de suavização, quando configurado diretamente.

## Interpretação

- **Bull Power > 0** juntamente com uma EMA ascendente confirma uma tendência ascendente.
- **Bear Power < 0** com uma EMA descendente confirma uma tendência descendente.
- A diminuição do Bull Power em preços ascendentes ou o aumento do Bear Power em preços descendentes formam divergências e avisam sobre reversões.
- Cruzamentos da linha zero do Bull Power ou Bear Power marcam a mudança no controlo do mercado.

As decisões de trading são tomadas analisando a EMA e ambos os osciladores simultaneamente. Por exemplo, aparece uma oportunidade de compra quando
a EMA está a subir, o Bear Power recupera de um novo mínimo e o Bull Power rompe acima de zero.

![indicator_elder_ray](../../../../images/indicator_elder_ray.png)

## Ver também

[Bull Power](bull_power.md)
[Bear Power](bear_power.md)
[ExponentialMovingAverage](ema.md)

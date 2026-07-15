# Raios de Elder

O **índice Elder-Ray** é um indicador composto de Alexander Elder que combina uma média móvel exponencial com os osciladores de força compradora
e força vendedora. Visualiza o equilíbrio entre compradores e vendedores e ajuda a identificar quando um dos lados perde o controlo.

Use a classe [ElderRay](xref:StockSharp.Algo.Indicators.ElderRay) para aceder ao indicador.

## Componentes

O indicador devolve uma estrutura [ElderRayValue](xref:StockSharp.Algo.Indicators.ElderRayValue) que contém:

- **EMA** - a média móvel exponencial de base dos preços de fecho;
- **força compradora** - a distância entre o máximo da barra e a EMA;
- **força vendedora** - a distância entre o mínimo da barra e a EMA.

## Parâmetros

Raios de Elder herda as definições de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Período** - período da EMA;
- **Alfa** - coeficiente de suavização, quando configurado diretamente.

## Interpretação

- **força compradora > 0** juntamente com uma EMA ascendente confirma uma tendência ascendente.
- **força vendedora < 0** com uma EMA descendente confirma uma tendência descendente.
- A diminuição da força compradora em preços ascendentes ou o aumento da força vendedora em preços descendentes formam divergências e avisam sobre reversões.
- Cruzamentos da linha zero da força compradora ou força vendedora marcam a mudança no controlo do mercado.

As decisões de negociação são tomadas analisando a EMA e ambos os osciladores simultaneamente. Por exemplo, aparece uma oportunidade de compra quando
a EMA está a subir, o força vendedora recupera de um novo mínimo e o força compradora rompe acima de zero.

![Gráfico do indicador Raios de Elder](../../../../images/indicator_elder_ray.png)

## Ver também

[força compradora](bull_power.md)
[força vendedora](bear_power.md)
[Média móvel exponencial](ema.md)

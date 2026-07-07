# ADL

﻿# ADL

**Accumulation/Distribution Line (ADL)** é um indicador de volume desenvolvido por Mark Chaikin. O indicador avalia a relação entre oferta e procura no mercado analisando a correlação entre preço e volume.

Para usar o indicador, é necessário usar a classe [AccumulationDistributionLine](xref:StockSharp.Algo.Indicators.AccumulationDistributionLine).

## Descrição

A Accumulation/Distribution Line é um indicador cumulativo que usa volume e preço para determinar se um instrumento está numa fase de acumulação (compra) ou distribuição (venda).

O indicador ADL ajuda a confirmar uma tendência ou a avisar sobre a sua potencial reversão:
- Se o preço estiver a subir e o ADL estiver a cair, isto pode sinalizar fraqueza numa tendência ascendente.
- Se o preço estiver a cair e o ADL estiver a subir, isto pode indicar uma potencial reversão de uma tendência descendente.

## Cálculo

O cálculo da Accumulation/Distribution Line ocorre em dois passos:

**1. Cálculo do multiplicador de volume (CLV - Close Location Value):**
```
CLV = ((Close - Low) - (High - Close)) / (High - Low)
```

**2. Cálculo do ADL:**
```
ADL = Previous ADL Value + CLV * Volume
```

Onde:
- Close - preço de fecho do período
- Low - preço mínimo do período
- High - preço máximo do período
- Volume - volume de negociação do período

Se (High - Low) for igual a zero, CLV é definido como zero.

![IndicatorAccumulationDistributionLine](../../../../images/indicator_accumulation_distribution_line.png)

## Ver também

[OBV](on_balance_volume.md)

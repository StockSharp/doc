# força compradora

﻿# força compradora

**força compradora** é a contraparte altista dentro do sistema Elder-ray. Mede a força com que os compradores empurram os preços acima de uma média móvel exponencial (EMA), comparando o máximo da barra com o preço médio.

Use a classe [BullPower](xref:StockSharp.Algo.Indicators.BullPower) para trabalhar com este indicador.

## Descrição

O indicador usa a fórmula:

`força compradora = High − EMA`.

- Valores positivos confirmam pressão compradora e sustentam uma tendência de subida.
- Valores em queda em direção a zero ou abaixo de zero sinalizam enfraquecimento dos compradores.
- Picos extremos podem anteceder correções, especialmente quando a EMA está apontada para baixo.

## Parâmetros

força compradora herda os seus parâmetros de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** — período da EMA.
- **Alpha** (opcional) — coeficiente de suavização, quando aplicável.

## Utilização

- força compradora a subir em conjunto com uma EMA a subir confirma a força da tendência.
- O preço a fazer novos máximos sem leituras mais altas de força compradora forma divergência baixista.
- Combine Bull e força vendedora com a EMA do preço para avaliar a estrutura completa do [raios de Elder](elder_ray.md).

![indicator_bull_power](../../../../images/indicator_bull_power.png)

## Ver também

[força vendedora](bear_power.md)
[raios de Elder](elder_ray.md)
[Média móvel exponencial](ema.md)

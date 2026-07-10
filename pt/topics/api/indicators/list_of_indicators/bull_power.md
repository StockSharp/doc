# Bull Power

﻿# Bull Power

**Bull Power** é a contraparte altista dentro do sistema Elder-ray. Mede a força com que os compradores empurram os preços acima de uma média móvel exponencial (EMA), comparando o máximo da barra com o preço médio.

Use a classe [BullPower](xref:StockSharp.Algo.Indicators.BullPower) para trabalhar com este indicador.

## Descrição

O indicador usa a fórmula:

`Bull Power = High − EMA`.

- Valores positivos confirmam pressão compradora e sustentam uma tendência de subida.
- Valores em queda em direção a zero ou abaixo de zero sinalizam enfraquecimento dos compradores.
- Picos extremos podem anteceder correções, especialmente quando a EMA está apontada para baixo.

## Parâmetros

Bull Power herda os seus parâmetros de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** — período da EMA.
- **Alpha** (opcional) — coeficiente de suavização, quando aplicável.

## Utilização

- Bull Power a subir em conjunto com uma EMA a subir confirma a força da tendência.
- O preço a fazer novos máximos sem leituras mais altas de Bull Power forma divergência baixista.
- Combine Bull e Bear Power com a EMA do preço para avaliar a estrutura completa do [Elder Ray](elder_ray.md).

![indicator_bull_power](../../../../images/indicator_bull_power.png)

## Ver também

[Bear Power](bear_power.md)
[Elder Ray](elder_ray.md)
[Média móvel exponencial](ema.md)

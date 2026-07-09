# BV

﻿# BV

**Balance Volume (BV)** é um indicador técnico que acompanha a acumulação e distribuição do volume de negociação com base em alterações de preço.

Para usar o indicador, é necessário usar a classe [BalanceVolume](xref:StockSharp.Algo.Indicators.BalanceVolume).

## Descrição

O indicador Balance Volume (BV) foi concebido para analisar a relação entre a alteração de preço e o volume de negociação. Ajuda os traders a determinar como as alterações de volume correspondem ao movimento do preço, o que pode indicar a força ou fraqueza da tendência atual.

A ideia principal do BV é que o volume deve confirmar a direção do preço. Se o preço subir com volume crescente, isto indica uma tendência ascendente forte. Pelo contrário, se o preço cair com volume crescente, isto sugere uma tendência descendente forte.

O indicador BV é especialmente útil para:
- Confirmar a força da tendência atual
- Identificar potenciais reversões de tendência
- Detetar divergências entre preço e volume
- Determinar níveis de acumulação e distribuição

## Cálculo

O cálculo do indicador Balance Volume baseia-se na comparação do preço de fecho com o preço de fecho anterior e na ponderação do volume de negociação:

```
Se Close > Previous Close:
	BV = Previous BV + Volume
Se Close < Previous Close:
	BV = Previous BV - Volume
Se Close = Previous Close:
	BV = Previous BV
```

Onde:
- Close - preço de fecho atual
- Previous Close - preço de fecho anterior
- Volume - volume de negociação atual
- Previous BV - valor anterior do indicador Balance Volume

## Interpretação

- **BV a subir com aumento do preço** - confirmação de uma tendência ascendente, indica forte interesse comprador
- **BV a cair com diminuição do preço** - confirmação de uma tendência descendente, indica forte interesse vendedor
- **BV a subir com preço estável ou em queda** - potencial acumulação, pode anteceder uma reversão ascendente
- **BV a cair com preço estável ou em subida** - potencial distribuição, pode anteceder uma reversão descendente
- **Divergência entre BV e preço** - aviso de uma possível reversão de tendência:
  - Se o preço subir enquanto o BV cai, uma reversão descendente rápida pode estar iminente
  - Se o preço cair enquanto o BV sobe, uma reversão ascendente rápida pode estar iminente

![indicator_balance_volume](../../../../images/indicator_balance_volume.png)

## Ver também

[OBV](on_balance_volume.md)
[ADL](accumulation_distribution_line.md)
[ChaikinMoneyFlow](chaikin_money_flow.md)
[ForceIndex](force_index.md)

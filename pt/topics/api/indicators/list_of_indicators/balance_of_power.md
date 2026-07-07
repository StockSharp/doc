# BOP

﻿# BOP

**Balance of Power (BOP)** é um indicador concebido para medir a força dos bulls (compradores) em relação aos bears (vendedores), avaliando a capacidade dos compradores para elevar o preço do mínimo até ao máximo.

Para usar o indicador, é necessário usar a classe [BalanceOfPower](xref:StockSharp.Algo.Indicators.BalanceOfPower).

## Descrição

O indicador Balance of Power (BOP) apresenta o equilíbrio de forças entre compradores e vendedores no mercado. Baseia-se na suposição de que, numa tendência, compradores (bulls) ou vendedores (bears) conseguem controlar o preço ao longo da sessão. Ao comparar a diferença entre os preços de fecho e abertura com todo o intervalo de preço (high-low), o indicador permite avaliar quem domina atualmente o mercado.

O BOP ajuda os traders a:
- Determinar a direção e a força da tendência atual
- Identificar potenciais pontos de reversão
- Detetar divergências entre o preço e o indicador
- Determinar níveis de sobrecompra e sobrevenda

## Cálculo

A fórmula para calcular o indicador Balance of Power (BOP) é bastante simples:

```
BOP = (Close - Open) / (High - Low)
```

Onde:
- Close - preço de fecho
- Open - preço de abertura
- High - preço mais alto do período
- Low - preço mais baixo do período

Se (High - Low) for zero, o BOP é definido como zero para evitar divisão por zero.

O BOP é muitas vezes adicionalmente suavizado usando uma média móvel para reduzir a volatilidade e melhorar a legibilidade do sinal.

## Interpretação

- **Valores positivos de BOP** (acima de zero) indicam que os compradores (bulls) estão a controlar o mercado, o que pode sinalizar uma tendência ascendente.
- **Valores negativos de BOP** (abaixo de zero) indicam que os vendedores (bears) estão a controlar o mercado, o que pode sinalizar uma tendência descendente.
- **Cruzamento da linha zero** pode ser considerado um sinal de uma potencial alteração da direção da tendência.
- **Valores extremos** (fortemente positivos ou fortemente negativos) podem indicar condições de sobrecompra ou sobrevenda no mercado.
- **Divergências** entre BOP e preço podem sinalizar uma possível reversão de tendência:
  - Se o preço estiver a subir e o BOP estiver a cair, isto pode ser um aviso de enfraquecimento da tendência ascendente.
  - Se o preço estiver a cair e o BOP estiver a subir, isto pode indicar um potencial fim da tendência descendente.

![indicator_balance_of_power](../../../../images/indicator_balance_of_power.png)

## Ver também

[BalanceOfMarketPower](balance_of_market_power.md)
[ForceIndex](force_index.md)
[ADL](accumulation_distribution_line.md)
[OBV](on_balance_volume.md)

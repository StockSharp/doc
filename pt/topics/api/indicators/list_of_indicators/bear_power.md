# Bear Power

﻿# Bear Power

**Bear Power** faz parte do sistema Elder-ray de Alexander Elder e mostra a força dos vendedores em comparação com uma média móvel exponencial (EMA). Mede até que ponto os mínimos intradiários caem abaixo do preço médio e destaca momentos em que os vendedores perdem controlo.

Use a classe [BearPower](xref:StockSharp.Algo.Indicators.BearPower) para aceder ao indicador.

## Descrição

O indicador é calculado como a diferença entre o mínimo da barra e o valor da EMA:

`Bear Power = Low − EMA`.

- Leituras negativas confirmam pressão vendedora.
- Valores a subir em direção a zero ou acima de zero indicam enfraquecimento dos vendedores e uma possível reversão altista.
- Fundos profundos antecedem frequentemente recuperações, especialmente durante vendas em pânico.

## Parâmetros

Bear Power herda a configuração de [ExponentialMovingAverage](xref:StockSharp.Algo.Indicators.ExponentialMovingAverage):

- **Length** — período da EMA.
- **Alpha** (opcional) — coeficiente de suavização se a EMA estiver configurada desta forma.

## Utilização

- Procure reversões quando Bear Power vira para cima após um mínimo extremo enquanto a EMA começa a subir.
- O cruzamento da linha zero pode confirmar uma alteração da tendência predominante.
- Combine Bear Power com [Bull Power](bull_power.md) e a EMA do preço para construir o indicador [Elder Ray](elder_ray.md) completo.

![indicator_bear_power](../../../../images/indicator_bear_power.png)

## Ver também

[Bull Power](bull_power.md)
[Elder Ray](elder_ray.md)
[ExponentialMovingAverage](ema.md)

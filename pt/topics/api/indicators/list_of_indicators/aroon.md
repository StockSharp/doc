# Aroon

﻿# Aroon

O **Aroon Indicator** é um indicador técnico desenvolvido por Tushar Chande em 1995 para identificar alterações de tendência e a força da tendência. O nome "Aroon" vem de uma palavra em sânscrito que significa "amanhecer de uma nova era".

Para usar o indicador, é necessário usar a classe [Aroon](xref:StockSharp.Algo.Indicators.Aroon).

## Descrição

O Aroon Indicator é composto por duas linhas:
- **Aroon Up** - mede a força de uma tendência ascendente
- **Aroon Down** - mede a força de uma tendência descendente

O Aroon ajuda a determinar:
- O início de uma nova tendência
- A força da tendência atual
- Consolidação e movimento lateral
- Potenciais reversões de tendência

O indicador é especialmente útil para identificar fases iniciais da formação de uma nova tendência e para determinar períodos de consolidação.

## Parâmetros

O indicador tem os seguintes parâmetros:
- **Length** - período de cálculo (normalmente são usados 14-25 períodos)

## Cálculo

O cálculo do Aroon Indicator baseia-se na determinação do tempo (número de períodos) decorrido desde que foram atingidos os preços máximo e mínimo dentro do período especificado:

1. Aroon Up é calculado usando a fórmula:
   ```
   Aroon Up = ((Length - Periods since high) / Length) * 100
   ```

2. Aroon Down é calculado usando a fórmula:
   ```
   Aroon Down = ((Length - Periods since low) / Length) * 100
   ```

Onde:
- Length - período selecionado
- "Periods since high" - número de períodos desde que foi atingido o preço mais alto dentro do período Length
- "Periods since low" - número de períodos desde que foi atingido o preço mais baixo dentro do período Length

Ambas as linhas Aroon oscilam entre 0 e 100:
- Um valor de 100 significa que o máximo/mínimo foi atingido no período mais recente
- Um valor de 0 significa que o máximo/mínimo foi atingido há Length períodos

## Interpretação

- **Tendência ascendente forte**: Aroon Up está próximo de 100 e Aroon Down está próximo de 0
- **Tendência descendente forte**: Aroon Down está próximo de 100 e Aroon Up está próximo de 0
- **Movimento lateral**: ambas as linhas movem-se paralelamente entre si em níveis baixos
- **Potencial reversão de tendência**: cruzamento das linhas Aroon Up e Aroon Down
- **Consolidação**: ambas as linhas oscilam em torno de 50

![indicator_aroon](../../../../images/indicator_aroon.png)

## Ver também

[ADX](adx.md)
[DMI](dmi.md)

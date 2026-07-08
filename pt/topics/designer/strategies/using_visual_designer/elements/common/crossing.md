# Cruzamento

![Designer Crossing 00](../../../../../../images/designer_crossing_00.png)

Este elemento é usado para acompanhar a posição de dois valores um em relação ao outro. Por exemplo, para determinar o momento de cruzamento entre duas linhas.

A comparação é feita relativamente aos valores nos dois sockets **Up** e **Down**.

## Sockets de entrada

- **Up** - valores que permitem comparação (por exemplo, um valor numérico, um valor de indicador, etc.).
- **Down** - valores que permitem comparação (por exemplo, um valor numérico, um valor de indicador, etc.).

## Sockets de saída

- **Flag** - true se **Up** for maior do que **Down**; caso contrário, false.

![Designer Crossing 01](../../../../../../images/designer_crossing_01.png)

Um exemplo de utilização do bloco Crossing para acompanhar os cruzamentos de dois [indicadores SMA](../../../../../api/indicators/list_of_indicators/sma.md). São usados dois blocos Crossing, e cada um emite true separadamente dependendo de quando a SMA longa é maior do que a curta e de quando é menor.

## Ver também

[Atraso de valor](delay_value.md)

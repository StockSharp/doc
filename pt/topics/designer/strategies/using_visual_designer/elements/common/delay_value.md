# Atraso de valor

![Designer atraso 00](../../../../../../images/designer_delay_00.png)

Este componente é usado para atrasar a transmissão de um valor durante um número especificado de iterações.

## Sockets de entrada

- **Acionador** - Um sinal (qualquer valor exceto `False`) que inicializa o contador interno para iniciar a contagem decrescente do atraso.
- **Entrada** - Qualquer valor de entrada (exceto [velas inacabadas](../data_sources/candles.md) ou [valores não finais de indicadores](indicator.md)) que diminui o contador interno. Quando o contador chega a zero, é desativado e o socket de saída é ativado. Se o contador não tiver sido ativado pelo **Acionador**, os valores de entrada são ignorados.

## Sockets de saída

- **Sinal** - Emite um sinal quando o contador chega a zero, indicando o fim do atraso.

## Parâmetros

- **Duração** - Especifica a duração do atraso em iterações.

![Designer atraso 01](../../../../../../images/designer_delay_01.png)

## Ver também

- [Comparação](comparison.md)

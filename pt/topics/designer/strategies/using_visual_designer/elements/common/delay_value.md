# Value Delay

![Designer Delay 00](../../../../../../images/designer_delay_00.png)

Este componente é usado para atrasar a transmissão de um valor durante um número especificado de iterações.

## Sockets de entrada

- **Trigger** - Um sinal (qualquer valor exceto `False`) que inicializa o contador interno para iniciar a contagem decrescente do atraso.
- **Input** - Qualquer valor de entrada (exceto [velas inacabadas](../data_sources/candles.md) ou [valores não finais de indicadores](indicator.md)) que diminui o contador interno. Quando o contador chega a zero, é desativado e o socket de saída é ativado. Se o contador não tiver sido ativado pelo **Trigger**, os valores de entrada são ignorados.

## Sockets de saída

- **Signal** - Emite um sinal quando o contador chega a zero, indicando o fim do atraso.

## Parâmetros

- **Duration** - Especifica a duração do atraso em iterações.

![Designer Delay 01](../../../../../../images/designer_delay_01.png)

## Ver também

- [Comparison](comparison.md)

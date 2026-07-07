# Elementos compostos

Ao compor esquemas, existem frequentemente conjuntos de elementos que formam uma funcionalidade completa e que podem ser usados em diferentes esquemas ou várias vezes no mesmo esquema com valores de propriedades diferentes. Esses conjuntos de elementos podem ser colocados num elemento composto separado, que depois será usado como qualquer cubo comum.

Um elemento composto é um esquema normal que é guardado\/carregado\/editado como qualquer esquema de estratégia.

Quando adiciona um elemento composto a um esquema, todos os parâmetros não ligados de todos os cubos internos são automaticamente adicionados a ele. Os parâmetros não ligados na entrada dos cubos são adicionados como entrada, e os parâmetros não ligados na saída são adicionados como saída. Cada parâmetro adicionado recebe o mesmo nome do elemento de origem e do respetivo parâmetro. Além disso, para este elemento, são adicionadas as propriedades de todos os elementos para os quais a propriedade **Parameters** foi especificada.

Vamos considerar a utilização de elementos compostos no exemplo da estratégia de cruzamento de médias móveis, que ilustra a utilização do elemento composto [Crossing](elements/common/crossing.md) várias vezes. A estratégia pode abrir uma posição longa quando a média móvel curta cruza a média móvel longa de baixo para cima, e uma posição curta quando a média móvel curta cruza a média móvel longa de cima para baixo. O esquema da parte da estratégia de cruzamento de médias móveis onde é determinado o momento do cruzamento das médias móveis é apresentado na figura abaixo:

![Designer Creating a composite elements 00](../../../../images/designer_creating_composite_elements_00.png)

Como o cruzamento de médias móveis difere apenas na sua possível direção (a curta cruza de cima para baixo ou de baixo para cima), a parte do esquema que determina o momento do cruzamento pode ser extraída para um elemento composto separado. Quando adiciona este elemento ao esquema, especifica as propriedades que definem o algoritmo de cruzamento das médias móveis. O esquema do elemento composto pelo qual o cruzamento é determinado é apresentado na figura abaixo:

![Designer Crossing 01](../../../../images/designer_crossing_01.png)

O diagrama do elemento composto é constituído por elementos simples e baseia-se na memorização dos valores atuais (Prev In 1 e Prev In 2) e na comparação entre pares de valores atuais (CurrComparison) e anteriores (PrevComparison). Como cada um dos valores de entrada é usado em dois elementos do diagrama, os elementos de [Combination](elements/common/combination.md) (In 1, In 2) são colocados na entrada do elemento composto, permitindo dividir uma entrada em dois elementos e passar o valor de entrada para os elementos [Comparison](elements/common/comparison.md) e [Prev value](elements/common/prev_value.md). Quando chega um novo valor à entrada, os valores atuais são comparados e um novo valor é passado para o elemento [Prev value](elements/common/prev_value.md), a partir do qual é passado o valor anterior da entrada atual; em seguida, os valores anteriores são comparados. Se ambas as condições forem cumpridas, o que é verificado usando a [Logical condition](elements/common/logical_condition.md) And, então o valor da flag levantada é passado para a saída do elemento composto, podendo ser usado como disparador de uma ação posterior.

Para os cubos CurrComparison e PrevComparison, a flag **Parameters** do grupo de propriedades **Common** está definida. Por isso, as propriedades destes cubos foram incluídas nas propriedades do elemento composto [Crossing](elements/common/crossing.md), que serão posteriormente especificadas ao usar um elemento composto no esquema da estratégia.

![Designer Crossing 00](../../../../images/designer_crossing_00.png)

## Conteúdo recomendado

[Display candles on chart](schema_samples/display_candles_on_chart.md)

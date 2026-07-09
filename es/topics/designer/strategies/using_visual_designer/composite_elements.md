# Elementos compuestos

Al componer esquemas, a menudo hay conjuntos de elementos que forman una funcionalidad completa y pueden usarse en distintos esquemas o muchas veces dentro de un mismo esquema con distintos valores de propiedades. Estos conjuntos de elementos pueden colocarse en un elemento compuesto separado, que luego se usará como cualquier cubo normal.

Un elemento compuesto es un esquema normal que se guarda\/carga\/edita igual que cualquier esquema de estrategia.

Al añadir un elemento compuesto a un esquema, todos los parámetros no conectados de todos los cubos internos se añaden automáticamente a él. Los parámetros no conectados en la entrada de los cubos se añaden como entrada, y los parámetros no conectados en la salida se añaden como salida. Cada parámetro añadido se nombra igual que el elemento fuente y su parámetro. Además, para este elemento se añaden las propiedades de todos los elementos para los que se especificó la propiedad **Parámetros**.

Consideraremos el uso de elementos compuestos con el ejemplo de la estrategia de cruce de medias móviles, que ilustra el uso del elemento compuesto [Cruce](elements/common/crossing.md) varias veces. La estrategia puede abrir una posición larga cuando la media móvil corta cruza la larga de abajo hacia arriba, y una posición corta cuando la media móvil corta cruza la larga de arriba hacia abajo. El esquema de la parte de la estrategia de cruce de medias móviles donde se determina el momento del cruce se muestra en la siguiente figura:

![Designer Creating a composite elements 00](../../../../images/designer_creating_composite_elements_00.png)

Como el cruce de medias móviles difiere solo por su posible dirección (la corta cruza de arriba abajo o de abajo arriba), la parte del esquema que determina el momento del cruce puede extraerse a un elemento compuesto separado. Al añadir este elemento al esquema, se especifican las propiedades que definen el algoritmo de cruce de medias móviles. El esquema del elemento compuesto por el que se determina el cruce se muestra en la siguiente figura:

![Designer Crossing 01](../../../../images/designer_crossing_01.png)

El diagrama del elemento compuesto consta de elementos simples y se basa en memorizar los valores actuales (Prev In 1 y Prev In 2) y comparar entre sí los pares de valores actuales (CurrComparison) y anteriores (PrevComparison). Como cada uno de los valores de entrada se usa en dos elementos del diagrama, los elementos [Combinación](elements/common/combination.md) (In 1, In 2) se colocan en la entrada del elemento compuesto; permiten dividir una entrada en dos elementos y pasar el valor de entrada a los elementos [Comparación](elements/common/comparison.md) y [Valor anterior](elements/common/prev_value.md). Cuando llega un nuevo valor a la entrada, se comparan los valores actuales y se pasa un nuevo valor al elemento [Valor anterior](elements/common/prev_value.md), desde el cual se pasa el valor anterior para la entrada actual; luego se comparan los valores anteriores. Si se cumplen ambas condiciones, lo que se comprueba mediante la condición And [Condición lógica](elements/common/logical_condition.md), entonces el valor de la bandera activada se pasa a la salida del elemento compuesto, que puede usarse como disparador para una acción posterior.

Para los cubos CurrComparison y PrevComparison, se establece la bandera **Parámetros** del grupo de propiedades **Común**. Por lo tanto, las propiedades de estos cubos se tomaron en las propiedades del elemento compuesto [Cruce](elements/common/crossing.md), que se especificarán posteriormente al usar un elemento compuesto en el esquema de la estrategia.

![Designer Crossing 00](../../../../images/designer_crossing_00.png)

## Contenido recomendado

[Mostrar velas en el gráfico](schema_samples/display_candles_on_chart.md)

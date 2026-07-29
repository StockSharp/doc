# Mostrar velas en el gráfico

Para mostrar velas de un instrumento en un gráfico, puede usarse el siguiente esquema:

![Designer Visualización de las velas en el gráfico 00](../../../../../images/designer_conclusion_of_candles_on_chart_00.png)

Para el cubo [Variable](../elements/data_sources/variable.md), se selecciona el tipo de dato **Instrumento**. Si el instrumento no se especifica, pero se establece la bandera **Parámetros** del grupo de propiedades **Común**, se tomará de la estrategia y se pasará al cubo [Velas](../elements/data_sources/candles.md). Para el cubo [Velas](../elements/data_sources/candles.md), se especifica la configuración para construir velas de 5 minutos y pasar solo velas completamente formadas.

Para el cubo [Gráfico](../elements/common/chart.md), se añadió un elemento gráfico con tipo de vela, para el cual se añadió automáticamente el parámetro de entrada.

Después de añadir los elementos gráficos necesarios al panel del gráfico, se añade la conexión entre los elementos [Velas](../elements/data_sources/candles.md) y [Gráfico](../elements/common/chart.md), mediante la cual las velas construidas se envían al gráfico.

## Contenido recomendado

[Obtener el mejor precio para un instrumento](get_best_price_for_instrument.md)

# Mostrar velas en el gráfico

Para mostrar velas de un instrumento en un gráfico, puede usarse el siguiente esquema:

![Designer The conclusion of the candles on the chart 00](../../../../../images/designer_conclusion_of_candles_on_chart_00.png)

Para el cubo [Variable](../elements/data_sources/variable.md), se selecciona el tipo de dato **Instrument**. Si el instrumento no se especifica, pero se establece la bandera **Parameters** del grupo de propiedades **Common**, se tomará de la estrategia y se pasará al cubo [Velas](../elements/data_sources/candles.md). Para el cubo [Velas](../elements/data_sources/candles.md), se especifican la configuración para construir velas de 5 minutos y pasar solo velas completamente formadas.

Para el cubo [Gráfico](../elements/common/chart.md), se añadió un elemento gráfico con tipo de vela, para el cual se añadió automáticamente el parámetro de entrada.

Después de añadir los elementos gráficos necesarios al panel de gráfico, se añade la conexión de los elementos [Velas](../elements/data_sources/candles.md) y [Gráfico](../elements/common/chart.md), a través de la cual las velas construidas se pasarán para su salida al gráfico.

## Contenido recomendado

[Obtener el mejor precio para un instrumento](get_best_price_for_instrument.md)

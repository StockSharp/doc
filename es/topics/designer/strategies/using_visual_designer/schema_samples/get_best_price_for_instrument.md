# Obtener el mejor precio para un instrumento

Para registrar una orden de compra al mejor precio actual del instrumento, puede usarse el siguiente esquema:

![Designer Obtener las mejores cotizaciones para la herramienta 00](../../../../../images/designer_get_best_quote_for_instrument_00.png)

Para el cubo [Variable](../elements/data_sources/variable.md), se selecciona el tipo de dato **Instrumento**. Si el instrumento no se especifica, pero se establece la bandera **Parámetros** del grupo de propiedades **Común**, se tomará de la estrategia y se pasará al cubo [Libro de órdenes](../elements/market_depths/order_book.md). El cubo [Libro de órdenes](../elements/market_depths/order_book.md), después de recibir el instrumento actual desde la variable, pasa por el parámetro de salida los cambios del libro de órdenes para el instrumento seleccionado. Al recibir cambios del libro de órdenes, el cubo [Convertidor](../elements/converters/converter.md) elige de ellos el valor actual del mejor precio de compra.

## Contenido recomendado

[Obtener posición actual](get_current_position.md)

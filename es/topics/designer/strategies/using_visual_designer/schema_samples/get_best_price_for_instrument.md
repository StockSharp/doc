# Obtener el mejor precio para un instrumento

Para registrar una orden de compra al mejor precio actual del instrumento, puede usarse el siguiente esquema:

![Designer Get the best rates for the tool 00](../../../../../images/designer_get_best_quote_for_instrument_00.png)

Para el cubo [Variable](../elements/data_sources/variable.md), se selecciona el tipo de dato **Instrument**. Si el instrumento no se especifica, pero se establece la bandera **Parameters** del grupo de propiedades **Common**, se tomará de la estrategia y se pasará al cubo [Order book](../elements/market_depths/order_book.md). El cubo [Order book](../elements/market_depths/order_book.md), después de recibir el instrumento actual desde la variable, pasa por el parámetro de salida los cambios del libro de órdenes para el instrumento seleccionado. Al recibir cambios del libro de órdenes, el cubo [Converter](../elements/converters/converter.md) elige de ellos el valor actual del mejor precio de compra.

## Contenido recomendado

[Obtener posición actual](get_current_position.md)

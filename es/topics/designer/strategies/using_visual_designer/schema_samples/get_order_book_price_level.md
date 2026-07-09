# Obtener nivel de precio del libro de órdenes

Para obtener la línea de compra requerida del libro de órdenes, puede usarse el siguiente esquema:

![Designer Event model 00](../../../../../images/designer_event_model_00.png)

Para el cubo [Variable](../elements/data_sources/variable.md), se selecciona el tipo de dato **Instrumento**. Si el instrumento no se especifica, pero se establece la bandera **Parámetros** del grupo de propiedades **Común**, se tomará de la estrategia. Para el cubo [Convertidor](../elements/converters/converter.md), se seleccionan el tipo de datos y el campo correspondiente de la colección de cubos para Bids de compra. El cubo indexador obtiene el elemento requerido de la colección de mejores precios de compra. Para obtener cierto valor de precio o volumen en un nivel, puede usar el cubo [Convertidor](../elements/converters/converter.md).

## Contenido recomendado

[Galería](../../../strategy_gallery.md)

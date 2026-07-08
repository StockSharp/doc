# Modelo de eventos

El enfoque de construcción de esquemas en [Designer](../../../designer.md) se basa en la generación y el posterior procesamiento de eventos. Al construir una estrategia, no se sabe cuándo ocurrirá un evento de cambio de datos de mercado, pero puede suscribirse a este evento y procesarlo correspondientemente.

Cada cubo de [Designer](../../../designer.md) que tiene un parámetro de salida es el generador del evento. Y los cubos que tienen un parámetro de entrada pueden suscribirse al evento generado por el parámetro saliente. Suscribirse a un evento no es más que crear una línea de conexión entre dos cubos.

Por ejemplo, el cubo [Libro de órdenes](elements/market_depths/order_book.md) genera un evento de cambio del libro de órdenes. No se sabe de antemano cuándo ocurrirá un cambio. Al crear una línea de conexión entre el cubo [Libro de órdenes](elements/market_depths/order_book.md) y el cubo [Convertidor](elements/converters/converter.md), se realiza una suscripción al cambio del libro de órdenes para su posterior procesamiento con el cubo [Convertidor](elements/converters/converter.md), etc.:

![Designer Event model 00](../../../../images/designer_event_model_00.png)

## Contenido recomendado

[Primera estrategia](first_strategy.md)
